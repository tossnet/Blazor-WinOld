using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldLabel : WinOldComponentBase
{
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; } = Appearance.Win10;

    /// </summary>
    private string GetComponentClass()
    {
        var cls = Appearance switch
        {
            Appearance.DOS => "lbl-dos",
            Appearance.Win31 => "lbl-win-31",
            Appearance.Win98 => "lbl-win-98",
            Appearance.WinXP => "lbl-win-xp",
            Appearance.Win7 => "lbl-win-7",
            Appearance.Win10 => "lbl-win-10",
            _ => "lbl-win-10"
        };
        if (IsTouch) cls += " win-touch";
        return cls;
    }
}

