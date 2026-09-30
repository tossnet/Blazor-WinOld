using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.WinOld.Components;

public partial class WinOldNavPane : WinOldComponentBase
{
    private readonly List<WinOldNavPaneGroup> _groups = new();
    private WinOldNavPaneGroup? _openGroup;
    private ElementReference _rootRef;
    private IJSObjectReference? _module;
    private IJSObjectReference? _scrollHandle;
    private bool _scrollActive;

    [Inject] private IJSRuntime JS { get; set; } = default!;

    /// <summary>
    /// Content of the pane: <see cref="WinOldNavPaneGroup"/>, <see cref="WinOldNavPaneItem"/>
    /// and <see cref="WinOldNavPaneSeparator"/> instances.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Fixed area under the title (e.g. a search box). Hidden while the pane is <see cref="Collapsed"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? HeaderContent { get; set; }

    /// <summary>
    /// Fixed area at the bottom of the pane (e.g. account, settings). May contain <see cref="WinOldNavPaneItem"/>s.
    /// </summary>
    [Parameter]
    public RenderFragment? FooterContent { get; set; }

    /// <summary>
    /// Title of the pane, shown in its header and used as accessible label.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Visual style. When unset, falls back to the Appearance of the hosting <see cref="WinOldWindow"/>,
    /// then to <see cref="Blazor.WinOld.Components.Appearance.Win10"/>.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance? Appearance { get; set; }

    /// </summary>
    [CascadingParameter(Name = "WindowAppearance")]
    public Appearance? WindowAppearance { get; set; }

    /// <summary>
    /// How groups expand. <see cref="NavPaneExpandMode.Auto"/> gives an accordion for Win98
    /// (Office 97 Outlook bar) and independent groups for the other appearances.
    /// </summary>
    [Parameter]
    public NavPaneExpandMode ExpandMode { get; set; } = NavPaneExpandMode.Auto;

    /// <summary>
    /// Compact mode: only the icons are shown, in a narrow rail. Bindable.
    /// </summary>
    [Parameter]
    public bool Collapsed { get; set; }

    /// </summary>
    [Parameter]
    public EventCallback<bool> CollapsedChanged { get; set; }

    /// <summary>
    /// Shows the hamburger button toggling <see cref="Collapsed"/>. Defaults to true for Win10 only.
    /// </summary>
    [Parameter]
    public bool? ShowToggle { get; set; }

    /// <summary>
    /// Accordion only: replaces the scrollbar of the open group with small arrow buttons, shown only when
    /// items are hidden on their side (Office 97 Outlook bar). Defaults to true for Win98 only.
    /// </summary>
    [Parameter]
    public bool? ShowScrollButtons { get; set; }

    /// <summary>
    /// Width of the expanded pane, as any CSS length. When unset, each appearance uses its own default.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Value of the selected item (see <see cref="WinOldNavPaneItem.Value"/>). Bindable.
    /// </summary>
    [Parameter]
    public string? SelectedValue { get; set; }

    /// </summary>
    [Parameter]
    public EventCallback<string?> SelectedValueChanged { get; set; }

    /// <summary>
    /// Raised when any item of the pane is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<WinOldNavPaneItem> OnItemClick { get; set; }

    /// <summary>
    /// Appearance actually rendered, inherited by the children that don't set their own.
    /// </summary>
    public Appearance EffectiveAppearance => Appearance ?? WindowAppearance ?? global::Blazor.WinOld.Components.Appearance.Win10;

    /// </summary>
    public bool IsAccordion => ExpandMode switch
    {
        NavPaneExpandMode.Single => true,
        NavPaneExpandMode.Multiple => false,
        _ => EffectiveAppearance == global::Blazor.WinOld.Components.Appearance.Win98
    };

    /// </summary>
    internal bool HasScrollButtons => IsAccordion
        && (ShowScrollButtons ?? EffectiveAppearance == global::Blazor.WinOld.Components.Appearance.Win98);

    private bool IsToggleVisible => ShowToggle ?? EffectiveAppearance == global::Blazor.WinOld.Components.Appearance.Win10;

    /// </summary>
    internal void RegisterGroup(WinOldNavPaneGroup group)
    {
        if (_groups.Contains(group)) return;
        _groups.Add(group);

        // Accordion: the first group asking to be expanded wins, otherwise the first group.
        if (_openGroup is null || (!_openGroup.Expanded && group.Expanded))
        {
            var replaced = _openGroup is not null;
            _openGroup = group;
            if (replaced) StateHasChanged();
        }
    }

    /// </summary>
    internal void UnregisterGroup(WinOldNavPaneGroup group)
    {
        _groups.Remove(group);
        if (_openGroup == group)
        {
            _openGroup = _groups.FirstOrDefault();
            StateHasChanged();
        }
    }

    /// </summary>
    internal bool IsGroupOpen(WinOldNavPaneGroup group) => _openGroup == group;

    /// <summary>
    /// Accordion only: expands <paramref name="group"/> and collapses the previously expanded one.
    /// </summary>
    internal async Task OpenGroupAsync(WinOldNavPaneGroup group, bool notify = true)
    {
        if (_openGroup == group) return;

        var previous = _openGroup;
        _openGroup = group;
        StateHasChanged();

        if (!notify) return;
        if (previous is not null) await previous.NotifyExpandedChangedAsync(false);
        await group.NotifyExpandedChangedAsync(true);
    }

    /// </summary>
    internal async Task SelectAsync(WinOldNavPaneItem item)
    {
        if (SelectedValue != item.EffectiveValue)
        {
            SelectedValue = item.EffectiveValue;
            await SelectedValueChanged.InvokeAsync(SelectedValue);
        }
        await OnItemClick.InvokeAsync(item);
        StateHasChanged();
    }

    /// </summary>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (HasScrollButtons && !_scrollActive)
        {
            // Set before the await: guards against a second OnAfterRenderAsync firing
            // (and starting a second observer) before the import/init below completes.
            _scrollActive = true;
            _module ??= await JS.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorWinOld/js/navpane.js");
            _scrollHandle = await _module.InvokeAsync<IJSObjectReference>("initNavPaneScroll", _rootRef);
        }
        else if (!HasScrollButtons && _scrollActive)
        {
            _scrollActive = false;
            await DisposeScrollHandleAsync();
        }
    }

    private async Task DisposeScrollHandleAsync()
    {
        if (_scrollHandle is not null)
        {
            await _scrollHandle.InvokeVoidAsync("dispose");
            await _scrollHandle.DisposeAsync();
            _scrollHandle = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            await DisposeScrollHandleAsync();
            if (_module is not null)
                await _module.DisposeAsync();
        }
        catch (JSDisconnectedException)
        {
            // Circuit already gone (Blazor Server): nothing left to clean up on the browser side.
        }
    }

    /// </summary>
    private async Task ToggleCollapsed()
    {
        Collapsed = !Collapsed;
        await CollapsedChanged.InvokeAsync(Collapsed);
    }

    /// </summary>
    private string GetComponentClass()
    {
        var cls = EffectiveAppearance switch
        {
            global::Blazor.WinOld.Components.Appearance.DOS => "navpane-dos",
            global::Blazor.WinOld.Components.Appearance.Win31 => "navpane-win-31",
            global::Blazor.WinOld.Components.Appearance.Win98 => "navpane-win-98",
            global::Blazor.WinOld.Components.Appearance.WinXP => "navpane-win-xp",
            global::Blazor.WinOld.Components.Appearance.Win7 => "navpane-win-7",
            _ => "navpane-win-10"
        };
        if (IsAccordion) cls += " navpane-accordion";
        if (Collapsed) cls += " navpane-collapsed";
        if (ParentDisabled == true || Disabled) cls += " navpane-disabled";
        if (IsTouch) cls += " win-touch";
        return cls;
    }

    /// </summary>
    private string? GetStyle()
    {
        var style = !Collapsed && !string.IsNullOrWhiteSpace(Width) ? $"width: {Width};" : null;
        return string.IsNullOrEmpty(Style) ? style : $"{style} {Style}";
    }
}
