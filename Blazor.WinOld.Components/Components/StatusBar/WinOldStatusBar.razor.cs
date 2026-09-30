using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldStatusBar : WinOldComponentBase
{
    /// <summary>
    /// Panels of the bar (typically <see cref="WinOldStatusBarPanel"/> instances).
    /// Whatever their number and widths, together they always fill the whole width of the bar.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Visual style. When unset, falls back to the Appearance of the <see cref="WinOldWindow"/>
    /// hosting the bar in its <c>StatusBar</c> slot, then to <see cref="Blazor.WinOld.Components.Appearance.Win10"/>.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance? Appearance { get; set; }

    /// </summary>
    [CascadingParameter(Name = "WindowAppearance")]
    public Appearance? WindowAppearance { get; set; }

    /// <summary>
    /// Appearance actually rendered, inherited by child panels that don't set their own.
    /// </summary>
    public Appearance EffectiveAppearance => Appearance ?? WindowAppearance ?? global::Blazor.WinOld.Components.Appearance.Win10;

    /// </summary>
    private string GetComponentClass()
    {
        var cls = EffectiveAppearance switch
        {
            global::Blazor.WinOld.Components.Appearance.DOS => "statusbar-dos",
            global::Blazor.WinOld.Components.Appearance.Win31 => "statusbar-win-31",
            global::Blazor.WinOld.Components.Appearance.Win98 => "statusbar-win-98",
            global::Blazor.WinOld.Components.Appearance.WinXP => "statusbar-win-xp",
            global::Blazor.WinOld.Components.Appearance.Win7 => "statusbar-win-7",
            global::Blazor.WinOld.Components.Appearance.Win10 => "statusbar-win-10",
            _ => "statusbar-win-10"
        };
        if (ParentDisabled == true || Disabled) cls += " statusbar-disabled";
        if (IsTouch) cls += " win-touch";
        return cls;
    }
}
