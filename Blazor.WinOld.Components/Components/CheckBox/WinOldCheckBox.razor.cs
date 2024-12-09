using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.WinOld.Components;

public partial class WinOldCheckBox : WinOldComponentBase
{
    /// <summary>
    /// Disables the form control, ensuring it doesn't participate in form submission.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }


    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; }


    /// </summary>
    [Parameter]
    public bool Checked { get; set; } 

    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    private Guid ElementId { get; set; } = Guid.NewGuid();
}
