using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldFrame : WinOldComponentBase
{
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; } = Appearance.Win10;

    /// </summary>
    [Parameter]
    public Border Border { get; set; } = Border.Default;

    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    /// </summary>
    private string GetComponentClass() =>
        Border == Border.None
            ? string.Empty
            : Appearance switch
            {
                Appearance.DOS => "frame-dos",
                Appearance.Win31 => "frame-win-31",
                Appearance.Win98 => "frame-win-98",
                Appearance.WinXP => "frame-win-xp",
                Appearance.Win7 => "frame-win-7",
                Appearance.Win10 => "frame-win-10",
                _ => "frame-win-10"
            };
}
