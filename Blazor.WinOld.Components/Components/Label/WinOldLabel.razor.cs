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
    public Appearance Appearance { get; set; }

    /// </summary>
    private string GetComponentClass()
    {
        return Appearance switch
        {
            Appearance.Win7 => "lbl-win-7",
            Appearance.WinXP => "lbl-win-xp",
            Appearance.Win98 => "lbl-win-98",
            _ => "lbl-win-98"
        };
    }
}

