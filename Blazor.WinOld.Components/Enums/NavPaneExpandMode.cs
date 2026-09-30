using System.ComponentModel;

namespace Blazor.WinOld.Components;

public enum NavPaneExpandMode
{
    /// <summary>
    /// Depends on the appearance: <see cref="Single"/> for Win98 (Office 97 Outlook bar), <see cref="Multiple"/> otherwise.
    /// </summary>
    [Description("auto")]
    Auto,

    /// <summary>
    /// Accordion: exactly one group is expanded at a time.
    /// </summary>
    [Description("single")]
    Single,

    /// <summary>
    /// Each group expands and collapses independently.
    /// </summary>
    [Description("multiple")]
    Multiple,
}
