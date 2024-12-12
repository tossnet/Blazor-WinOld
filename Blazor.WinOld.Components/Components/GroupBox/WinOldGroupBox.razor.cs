using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldGroupBox : WinOldComponentBase
{
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; }


    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; }

    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    /// </summary>
    private string GetComponentClass()
    {
        return Appearance switch
        {
            Appearance.Win7 => "fieldset-win-7",
            Appearance.WinXP => "fieldset-win-xp",
            Appearance.Win98 => "fieldset-win-98",
            _ => "fieldset-win-98"
        };
    }
}
