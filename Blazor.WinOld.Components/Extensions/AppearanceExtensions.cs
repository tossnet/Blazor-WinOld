namespace Blazor.WinOld.Components;

public static class AppearanceExtensions
{
    /// <summary>
    /// Returns the CSS class that styles the scrollbars of a scrollable element for the given appearance.
    /// </summary>
    public static string ToScrollbarClass(this Appearance appearance)
    {
        return appearance switch
        {
            Appearance.DOS => "scroll-win-dos",
            Appearance.Win31 => "scroll-win-31",
            Appearance.Win98 => "scroll-win-98",
            Appearance.WinXP => "scroll-win-xp",
            Appearance.Win7 => "scroll-win-7",
            _ => "scroll-win-10"
        };
    }
}
