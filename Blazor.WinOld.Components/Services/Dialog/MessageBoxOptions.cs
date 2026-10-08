namespace Blazor.WinOld.Components;

/// <summary>
/// Options for configuring a WinOldMessageBox
/// </summary>
public record MessageBoxOptions
{
    public Appearance Appearance { get; init; } = Appearance.Win98;
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public Icon Icon { get; init; } = Icon.None;
    public string OkButtonText { get; init; } = "OK";
    public string CancelButtonText { get; init; } = string.Empty;

    /// <summary>
    /// DOS appearance only: background color of the message box. When null, it follows <see cref="Icon"/>
    /// (None = gray, Information = green, Question = cyan, Alert = brown, Critical = red).
    /// </summary>
    public DosColor? DosColor { get; init; }

    /// <summary>
    /// DOS appearance only: color of the buttons. When null, it adapts to the background
    /// (green on a gray box, gray on a colored one).
    /// </summary>
    public DosColor? DosButtonColor { get; init; }

    /// <summary>
    /// Win31 / Win10 only: color of the title bar (any CSS color, e.g. "#2D7D46").
    /// In Win10 the window border follows it. When null, the theme color is used.
    /// </summary>
    public string? TitleBarColor { get; init; }

    /// <summary>Enlarges the title bar and its buttons for touch screens.</summary>
    public bool TouchMode { get; init; }
}

// BREAKING CHANGE NOTICE:
// The alias "DialogOptions" has been removed to avoid naming conflicts.
// Please use "MessageBoxOptions" instead for message boxes.
// For the new Dialog component, use "DialogOptions".

