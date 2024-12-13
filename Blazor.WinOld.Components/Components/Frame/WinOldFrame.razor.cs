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
    public Appearance Appearance { get; set; }

    /// </summary>
    [Parameter]
    public Border Border { get; set; } = Border.Default;

    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    /// </summary>
    private string GetComponentClass()
    {
        string style = string.Empty;

        switch (Appearance)
        {
            case Appearance.WinXP:
                style = "frame-win-xp";
                break;
            case Appearance.Win7:
                style = "frame-win-7";
                break;
            case Appearance.Win98:
                style = "frame-win-98";
                break;
        }

        if (Border == Border.None)
        {
            style = string.Empty;
        }

        return style;
    }
}
