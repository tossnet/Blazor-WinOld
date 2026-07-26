using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldToolbar : WinOldComponentBase
{
    /// <summary>
    /// Toolbar buttons (typically <see cref="WinOldToolbarButton"/> instances).
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; } = Appearance.Win10;

    /// <summary>
    /// <see cref="ToolbarOverflowMode.Wrap"/> (default) wraps to multiple lines and always shows labels.
    /// <see cref="ToolbarOverflowMode.Collapse"/> stays on a single line and hides button labels
    /// (icon + tooltip only) once the toolbar's own width drops below <see cref="CollapseWidth"/>.
    /// </summary>
    [Parameter]
    public ToolbarOverflowMode Mode { get; set; } = ToolbarOverflowMode.Wrap;

    /// <summary>
    /// Width in pixels, below which button labels are hidden. Only used when <see cref="Mode"/> is
    /// <see cref="ToolbarOverflowMode.Collapse"/>. Not a universal value — depends on button count,
    /// label length and the active <see cref="Appearance"/>'s padding.
    /// </summary>
    [Parameter]
    public int CollapseWidth { get; set; } = 700;

    /// <summary>
    /// Default <c>Flat</c> value inherited by child <see cref="WinOldToolbarButton"/> instances
    /// that don't set their own — buttons show no background/border until hovered or pressed.
    /// </summary>
    [Parameter]
    public bool Flat { get; set; } = false;

    /// <summary>
    /// Default <c>IconOnly</c> value inherited by child <see cref="WinOldToolbarButton"/> instances
    /// that don't set their own — buttons never show a label (icon + tooltip only) and shrink to
    /// fit the icon, with no minimum width/height.
    /// </summary>
    [Parameter]
    public bool IconOnly { get; set; } = false;

    private readonly string _containerName = $"toolbar-{Guid.NewGuid():N}";

    private string? RootStyle => Mode == ToolbarOverflowMode.Collapse
        ? $"container-name:{_containerName};{Style}"
        : Style;

    /// </summary>
    private string GetComponentClass()
    {
        var baseClass = Appearance switch
        {
            Appearance.DOS => "toolbar-dos",
            Appearance.Win98 => "toolbar-win-98",
            Appearance.WinXP => "toolbar-win-xp",
            Appearance.Win7 => "toolbar-win-7",
            Appearance.Win10 => "toolbar-win-10",
            _ => "toolbar-win-10"
        };

        var modeClass = Mode == ToolbarOverflowMode.Collapse ? "toolbar-collapse-win" : "toolbar-wrap-win";
        return $"{baseClass} {modeClass}";
    }
}
