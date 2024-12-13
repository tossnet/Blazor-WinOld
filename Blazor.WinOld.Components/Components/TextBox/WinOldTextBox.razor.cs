using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.WinOld.Components;

public partial class WinOldTextBox : WinOldComponentBase
{
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; }

    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;


    /// </summary>
    private Guid ElementId { get; set; } = Guid.NewGuid();


    /// </summary>
    private string GetComponentClass()
    {
        return Appearance switch
        {
            Appearance.Win7 => "txt-win-7",
            Appearance.WinXP => "txt-win-xp",
            Appearance.Win98 => "txt-win-98",
            _ => "txt-win-98"
        };
    }
}
