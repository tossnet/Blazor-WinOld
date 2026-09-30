using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldStatusBarPanel : WinOldComponentBase
{
    [CascadingParameter]
    public WinOldStatusBar? RootStatusBar { get; set; }

    /// <summary>
    /// Text of the panel. Ignored when <see cref="ChildContent"/> is set.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// <summary>
    /// Free content of the panel. Takes priority over <see cref="Text"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// CSS class(es) for an icon from any icon library (e.g. "bi bi-globe").
    /// Ignored when <see cref="IconTemplate"/> is set.
    /// </summary>
    [Parameter]
    public string? IconCssClass { get; set; }

    /// <summary>
    /// Custom icon content (SVG, img, Blazor component…).
    /// Takes priority over <see cref="IconCssClass"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? IconTemplate { get; set; }

    /// <summary>
    /// Width of the panel, as any CSS length ("120px", "25%", "12ch"…). When unset, the panel
    /// shares the remaining space equally with the other panels without a width. When every
    /// panel of the bar has a width, the last one stretches so the bar is always filled.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Horizontal alignment of the panel content.
    /// </summary>
    [Parameter]
    public StatusBarPanelAlign Align { get; set; } = StatusBarPanelAlign.Left;

    /// <summary>
    /// Visual style. When unset, falls back to the ancestor <see cref="WinOldStatusBar"/>'s
    /// appearance, then to <see cref="Blazor.WinOld.Components.Appearance.Win10"/>.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance? Appearance { get; set; }

    private Appearance EffectiveAppearance => Appearance ?? RootStatusBar?.EffectiveAppearance ?? global::Blazor.WinOld.Components.Appearance.Win10;

    private bool IsFixed => !string.IsNullOrWhiteSpace(Width);

    private string GetPanelClass()
    {
        var cls = "statusbar-panel " + EffectiveAppearance switch
        {
            global::Blazor.WinOld.Components.Appearance.DOS => "statusbar-panel-dos",
            global::Blazor.WinOld.Components.Appearance.Win31 => "statusbar-panel-31",
            global::Blazor.WinOld.Components.Appearance.Win98 => "statusbar-panel-98",
            global::Blazor.WinOld.Components.Appearance.WinXP => "statusbar-panel-xp",
            global::Blazor.WinOld.Components.Appearance.Win7 => "statusbar-panel-7",
            _ => "statusbar-panel-10"
        };

        cls += IsFixed ? " statusbar-panel-fixed" : " statusbar-panel-auto";
        cls += Align switch
        {
            StatusBarPanelAlign.Center => " statusbar-panel-center",
            StatusBarPanelAlign.Right => " statusbar-panel-right",
            _ => string.Empty
        };

        return string.IsNullOrEmpty(Class) ? cls : $"{cls} {Class}";
    }

    private string? GetPanelStyle()
    {
        var style = IsFixed ? $"flex-basis: {Width};" : null;
        return string.IsNullOrEmpty(Style) ? style : $"{style} {Style}";
    }
}
