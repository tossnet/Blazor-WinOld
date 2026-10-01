using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

/// <summary>
/// Semantic heading (h1 to h6) styled like the titles of the chosen Windows version.
/// </summary>
public partial class WinOldHeading : WinOldComponentBase
{
    /// <summary>
    /// Text of the heading.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Visual style of the heading.
    /// </summary>
    [Parameter]
    public Appearance Appearance { get; set; } = Appearance.Win10;

    /// <summary>
    /// Heading level, from 1 (h1) to 6 (h6). Default is 2 (h2).
    /// </summary>
    [Parameter]
    public int Level { get; set; } = 2;

    private bool IsDisabled => ParentDisabled == true || Disabled;

    private int GetLevel() => Math.Clamp(Level, 1, 6);

    /// </summary>
    private string GetComponentClass()
    {
        var cls = Appearance switch
        {
            Appearance.DOS => "hd-dos",
            Appearance.Win31 => "hd-win-31",
            Appearance.Win98 => "hd-win-98",
            Appearance.WinXP => "hd-win-xp",
            Appearance.Win7 => "hd-win-7",
            Appearance.Win10 => "hd-win-10",
            _ => "hd-win-10"
        };
        cls = $"hd-win hd-{GetLevel()} {cls}";
        if (IsTouch) cls += " win-touch";
        if (!string.IsNullOrWhiteSpace(Class)) cls += $" {Class}";
        return cls;
    }
}
