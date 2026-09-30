using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldNavPaneGroup : WinOldComponentBase
{
    private bool _expanded;
    private bool _lastExpandedParameter;

    [CascadingParameter]
    public WinOldNavPane? Pane { get; set; }

    /// <summary>
    /// Title shown in the group header.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// CSS class(es) for a header icon from any icon library (e.g. "bi bi-star").
    /// Ignored when <see cref="IconTemplate"/> is set.
    /// </summary>
    [Parameter]
    public string? IconCssClass { get; set; }

    /// <summary>
    /// Custom header icon (SVG, img, Blazor component…). Takes priority over <see cref="IconCssClass"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? IconTemplate { get; set; }

    /// <summary>
    /// Items of the group.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Whether the group is expanded. Bindable. In accordion mode, the first expanded group wins.
    /// </summary>
    [Parameter]
    public bool Expanded { get; set; } = true;

    /// </summary>
    [Parameter]
    public EventCallback<bool> ExpandedChanged { get; set; }

    /// <summary>
    /// When false, the header can't be clicked and the group stays expanded (ignored in accordion mode).
    /// </summary>
    [Parameter]
    public bool Collapsible { get; set; } = true;

    private bool IsAccordion => Pane?.IsAccordion == true;

    private bool IsPaneCollapsed => Pane?.Collapsed == true;

    private bool HasIcon => IconTemplate is not null || !string.IsNullOrEmpty(IconCssClass);

    private bool IsDisabled => ParentDisabled == true || Disabled;

    private bool IsClickable => !IsPaneCollapsed && (IsAccordion || Collapsible);

    /// <summary>
    /// Whether the items are currently shown. In compact mode, every item is shown since headers are hidden.
    /// </summary>
    public bool IsExpanded => IsPaneCollapsed
        || (IsAccordion ? Pane!.IsGroupOpen(this) : !Collapsible || _expanded);

    /// </summary>
    protected override void OnInitialized()
    {
        _expanded = Expanded;
        _lastExpandedParameter = Expanded;
        Pane?.RegisterGroup(this);
    }

    /// </summary>
    protected override void OnParametersSet()
    {
        if (_lastExpandedParameter == Expanded) return;

        _lastExpandedParameter = Expanded;
        _expanded = Expanded;
        if (Expanded && IsAccordion) _ = Pane!.OpenGroupAsync(this, notify: false);
    }

    /// </summary>
    internal async Task NotifyExpandedChangedAsync(bool expanded)
    {
        _expanded = expanded;

        // Unbound, the parameter keeps its old value on the next render: it must not be seen as a change.
        if (!ExpandedChanged.HasDelegate) return;

        _lastExpandedParameter = expanded;
        await ExpandedChanged.InvokeAsync(expanded);
    }

    /// </summary>
    private async Task ToggleAsync()
    {
        if (IsAccordion)
        {
            await Pane!.OpenGroupAsync(this);
        }
        else if (Collapsible)
        {
            await NotifyExpandedChangedAsync(!_expanded);
        }
    }

    /// </summary>
    private string GetGroupClass()
    {
        var cls = "navpane-group " + (Pane?.EffectiveAppearance ?? global::Blazor.WinOld.Components.Appearance.Win10) switch
        {
            global::Blazor.WinOld.Components.Appearance.DOS => "navpane-group-dos",
            global::Blazor.WinOld.Components.Appearance.Win31 => "navpane-group-31",
            global::Blazor.WinOld.Components.Appearance.Win98 => "navpane-group-98",
            global::Blazor.WinOld.Components.Appearance.WinXP => "navpane-group-xp",
            global::Blazor.WinOld.Components.Appearance.Win7 => "navpane-group-7",
            _ => "navpane-group-10"
        };
        cls += IsExpanded ? " navpane-group-expanded" : " navpane-group-collapsed";
        if (IsAccordion) cls += " navpane-group-accordion";
        if (IsPaneCollapsed) cls += " navpane-group-compact";
        if (IsDisabled) cls += " navpane-group-disabled";
        if (IsTouch) cls += " win-touch";
        return string.IsNullOrEmpty(Class) ? cls : $"{cls} {Class}";
    }

    /// </summary>
    public void Dispose()
    {
        Pane?.UnregisterGroup(this);
    }
}
