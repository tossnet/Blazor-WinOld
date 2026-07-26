using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldToolbarSeparator : WinOldComponentBase
{
    [CascadingParameter]
    public WinOldToolbar? RootToolbar { get; set; }

    /// <summary>
    /// Visual style. When unset, falls back to the ancestor <see cref="WinOldToolbar"/>'s
    /// <c>Appearance</c>, then to <see cref="Blazor.WinOld.Components.Appearance.Win10"/>.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance? Appearance { get; set; }

    private Appearance EffectiveAppearance => Appearance ?? RootToolbar?.Appearance ?? global::Blazor.WinOld.Components.Appearance.Win10;

    /// </summary>
    private string GetComponentClass()
    {
        return EffectiveAppearance switch
        {
            global::Blazor.WinOld.Components.Appearance.DOS => "toolbar-separator-dos",
            global::Blazor.WinOld.Components.Appearance.Win98 => "toolbar-separator-win-98",
            global::Blazor.WinOld.Components.Appearance.WinXP => "toolbar-separator-win-xp",
            global::Blazor.WinOld.Components.Appearance.Win7 => "toolbar-separator-win-7",
            global::Blazor.WinOld.Components.Appearance.Win10 => "toolbar-separator-win-10",
            _ => "toolbar-separator-win-10"
        };
    }
}
