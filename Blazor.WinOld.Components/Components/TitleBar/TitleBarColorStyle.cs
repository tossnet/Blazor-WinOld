namespace Blazor.WinOld.Components;

/// <summary>
/// Builds the inline styles that apply a custom title bar color (Win31 / Win10) to a dialog host,
/// by overriding the theme's CSS variables.
/// </summary>
internal static class TitleBarColorStyle
{
    /// <summary>Style for the title bar element. The Win31 variable is also used by other controls,
    /// so it is overridden on the bar only, not on the whole window.</summary>
    public static string? ForTitleBar(Appearance appearance, string? color)
    {
        if (string.IsNullOrWhiteSpace(color))
            return null;

        return appearance switch
        {
            Appearance.Win31 => $"--dlg-win31-title-bar-background-color-active: {color}",
            Appearance.Win10 => $"--dlg-win10-title-bar-background-color-active: {color}",
            _ => null
        };
    }

    /// <summary>Style for the window container: in Win10 the border follows the title bar color.</summary>
    public static string? ForWindow(Appearance appearance, string? color)
    {
        if (string.IsNullOrWhiteSpace(color))
            return null;

        return appearance == Appearance.Win10
            ? $"--dlg-win10-title-bar-border-color-active: {color}"
            : null;
    }
}
