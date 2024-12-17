using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldOptionButtonGroup :  WinOldComponentBase
{
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    public string Name { get; set; } = string.Empty;

    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; }
}
