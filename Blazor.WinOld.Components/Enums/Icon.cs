using System.ComponentModel;

namespace Blazor.WinOld.Components;

public enum Icon
{
    /// <summary />
    [Description("None")]
    None,

    /// <summary />
    [Description("Information")]
    Information,

    /// <summary />
    [Description("Question")]
    Question,

    /// <summary />
    [Description("Alert")]
    Alert,

    /// <summary />
    [Description("Critical")]
    Critical,
}