using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldOptionButton : WinOldComponentBase
{
    /// </summary>
    [CascadingParameter(Name = "RadioGroup")]
    public virtual string RadioGroup { get; set; }


    /// </summary>
    [CascadingParameter(Name = "Appearance")]
    public virtual Appearance Appearance { get; set; }

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
            Appearance.Win7 => "opt-win-7",
            Appearance.WinXP => "opt-win-xp",
            Appearance.Win98 => "opt-win-98",
            _ => "opt-win-98"
        };
    }
}

