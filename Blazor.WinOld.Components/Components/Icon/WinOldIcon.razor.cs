using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldIcon : WinOldComponentBase
{
    /// <summary>
    /// System icon to display. <see cref="Icon.None"/> renders nothing.
    /// </summary>
    [Parameter]
    public Icon Icon { get; set; } = Icon.None;

    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; } = Appearance.Win10;

    /// <summary>
    /// Width and height of the icon, in pixels.
    /// </summary>
    [Parameter]
    public int Size { get; set; } = 32;

    /// <summary>
    /// Tooltip and accessible name. When null, the icon is decorative (hidden from screen readers).
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>DOS has no icon bitmap.</summary>
    private string GetIconClass()
    {
        string? appearance = Appearance switch
        {
            Appearance.DOS => null,
            Appearance.Win31 => "31",
            Appearance.Win98 => "98",
            Appearance.WinXP => "xp",
            Appearance.Win7 => "7",
            Appearance.Win10 => "10",
            _ => "10"
        };

        if (appearance is null)
            return string.Empty;

        return Icon switch
        {
            Icon.Alert => $"icon-alert-win-{appearance}",
            Icon.Critical => $"icon-critical-win-{appearance}",
            Icon.Information => $"icon-info-win-{appearance}",
            Icon.Question => $"icon-question-win-{appearance}",
            _ => string.Empty
        };
    }
}
