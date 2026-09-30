using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.WinOld.Components;

public partial class WinOldNavPaneItem : WinOldComponentBase
{
    [Inject]
    private NavigationManager Navigation { get; set; } = default!;

    [CascadingParameter]
    public WinOldNavPane? Pane { get; set; }

    /// <summary>
    /// Text of the item. Ignored when <see cref="ChildContent"/> is set.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Free content of the item. Takes priority over <see cref="Text"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// CSS class(es) for an icon from any icon library (e.g. "bi bi-inbox").
    /// Ignored when <see cref="IconTemplate"/> is set.
    /// </summary>
    [Parameter]
    public string? IconCssClass { get; set; }

    /// <summary>
    /// Custom icon content (SVG, img, Blazor component…). Takes priority over <see cref="IconCssClass"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? IconTemplate { get; set; }

    /// <summary>
    /// Navigation target. When set, the item is a link and is active whenever the current URL matches it.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// How the current URL is compared to <see cref="Href"/>, like <see cref="NavLink.Match"/>.
    /// </summary>
    [Parameter]
    public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;

    /// <summary>
    /// Value written to <see cref="WinOldNavPane.SelectedValue"/> on click. Defaults to <see cref="Href"/>, then <see cref="Text"/>.
    /// Items without <see cref="Href"/> are active when this value equals the pane's SelectedValue.
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// Short counter or tag shown next to the text (e.g. unread count).
    /// </summary>
    [Parameter]
    public string? Badge { get; set; }

    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// </summary>
    public string? EffectiveValue => Value ?? Href ?? Text;

    private bool IsDisabled => ParentDisabled == true || Disabled;

    private bool IsPaneCollapsed => Pane?.Collapsed == true;

    // In compact mode the text is hidden, so it becomes the tooltip.
    private string? Tooltip => IsPaneCollapsed ? Text : null;

    /// </summary>
    public bool IsActive => Href is not null
        ? IsUrlActive()
        : EffectiveValue is not null && EffectiveValue == Pane?.SelectedValue;

    /// </summary>
    protected override void OnInitialized()
    {
        Navigation.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        if (Href is not null) InvokeAsync(StateHasChanged);
    }

    private bool IsUrlActive()
    {
        var target = Navigation.ToAbsoluteUri(Href).AbsoluteUri.TrimEnd('/');
        var current = Navigation.Uri;
        var cut = current.IndexOfAny(['?', '#']);
        if (cut >= 0) current = current[..cut];
        current = current.TrimEnd('/');

        if (string.Equals(current, target, StringComparison.OrdinalIgnoreCase)) return true;

        return Match == NavLinkMatch.Prefix
            && current.StartsWith(target + "/", StringComparison.OrdinalIgnoreCase);
    }

    /// </summary>
    private async Task HandleClickAsync(MouseEventArgs args)
    {
        if (IsDisabled) return;

        await OnClick.InvokeAsync(args);
        if (Pane is not null) await Pane.SelectAsync(this);
    }

    /// </summary>
    private string GetItemClass()
    {
        var cls = "navpane-item " + (Pane?.EffectiveAppearance ?? global::Blazor.WinOld.Components.Appearance.Win10) switch
        {
            global::Blazor.WinOld.Components.Appearance.DOS => "navpane-item-dos",
            global::Blazor.WinOld.Components.Appearance.Win31 => "navpane-item-31",
            global::Blazor.WinOld.Components.Appearance.Win98 => "navpane-item-98",
            global::Blazor.WinOld.Components.Appearance.WinXP => "navpane-item-xp",
            global::Blazor.WinOld.Components.Appearance.Win7 => "navpane-item-7",
            _ => "navpane-item-10"
        };
        if (IsActive) cls += " navpane-item-active";
        if (IsPaneCollapsed) cls += " navpane-item-compact";
        if (IsDisabled) cls += " navpane-item-disabled";
        if (IsTouch) cls += " win-touch";
        return string.IsNullOrEmpty(Class) ? cls : $"{cls} {Class}";
    }

    /// </summary>
    public void Dispose()
    {
        Navigation.LocationChanged -= OnLocationChanged;
    }
}
