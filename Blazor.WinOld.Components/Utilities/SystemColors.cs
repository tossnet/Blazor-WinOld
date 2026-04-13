using System.Drawing;

namespace Blazor.WinOld.Components;

public static class SystemColors
{
    /// <summary>
    /// Sets or gets the current appearance (win98, winxp, win7, etc.).
    /// </summary>
    public static Appearance Appearance { get; set; }

    /// <summary>
    /// Returns the desktop color based on the appearance.
    /// </summary>
    public static Color Desktop
    {
        get
        {
            return Appearance switch
            {
                Appearance.Win98 => Color.FromArgb(14, 113 , 116),
                Appearance.WinXP => Color.FromArgb(90, 125, 220),
                Appearance.Win7 => Color.FromArgb(106, 183, 231),
                Appearance.Win10 => Color.FromArgb(12, 60, 120),
                _ => Color.White  
            };
        }
    }

    /// <summary>
    /// Returns the title bar color based on the appearance.
    /// </summary>
    public static Color TitleBar
    {
        get
        {
            return Appearance switch
            {
                Appearance.Win98 => Color.FromArgb(0, 0, 128),
                Appearance.WinXP => Color.FromArgb(0, 80, 238),   
                Appearance.Win7 => Color.FromArgb(65, 118 , 178),
                Appearance.Win10 => Color.FromArgb(0, 120, 212),
                _ => Color.DarkGray
            };
        }
    }
    /// <summary>
    /// </summary>
    public static Color Form
    {
        get
        {
            return Appearance switch
            {
                Appearance.Win98 => Color.FromArgb(192, 192, 192),
                Appearance.WinXP => Color.FromArgb(236, 233, 216),
                Appearance.Win7 => Color.FromArgb(240, 240, 240),
                Appearance.Win10 => Color.FromArgb(240, 240, 240),
                _ => Color.DarkGray
            };
        }
    }
}
