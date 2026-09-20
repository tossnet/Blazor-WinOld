using System.ComponentModel;

namespace Blazor.WinOld.Components;

/// <summary>
/// Color of a DOS-styled component: a button, or the background of a message box (subset of the CGA/VGA palette).
/// </summary>
public enum DosColor
{
    /// <summary>Standard gray.</summary>
    [Description("default")]
    Default,

    /// <summary />
    [Description("white")]
    White,

    /// <summary />
    [Description("yellow")]
    Yellow,

    /// <summary />
    [Description("green")]
    Green,

    /// <summary />
    [Description("red")]
    Red,

    /// <summary />
    [Description("blue")]
    Blue,

    /// <summary />
    [Description("cyan")]
    Cyan,

    /// <summary />
    [Description("magenta")]
    Magenta,

    /// <summary />
    [Description("brown")]
    Brown,
}
