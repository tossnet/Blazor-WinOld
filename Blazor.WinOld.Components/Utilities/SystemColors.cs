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
                Appearance.Win7 => Color.White, 
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
                _ => Color.DarkGray
            };
        }
    }
}
