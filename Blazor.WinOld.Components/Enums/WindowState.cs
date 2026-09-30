using System.ComponentModel;

namespace Blazor.WinOld.Components;

public enum WindowState
{
    /// <summary>Normal size.</summary>
    [Description("normal")]
    Normal,

    /// <summary>Fills the screen.</summary>
    [Description("maximized")]
    Maximized,
}
