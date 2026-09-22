namespace Blazor.WinOld.Components;

/// <summary>
/// Options for configuring a WinOldInputBox
/// </summary>
public record InputBoxOptions
{
    public Appearance Appearance { get; init; } = Appearance.Win98;
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public object? Value { get; set; } = string.Empty;
    public string OkButtonText { get; init; } = "OK";
    public string CancelButtonText { get; init; } = "Cancel";

    // Proprerty for numbers
    public string? Min { get; init; }
    public string? Max { get; init; }
    public string? Step { get; init; }

    //
    public InputBoxType InputType { get; init; } = InputBoxType.Text;

    /// <summary>
    /// DOS appearance only: background color of the input box. When null, it defaults to standard gray.
    /// </summary>
    public DosColor? DosColor { get; init; }

    /// <summary>
    /// DOS appearance only: Color of the OK/Cancel buttons. When null, it adapts to the background
    /// (green on a gray box, gray on a colored one).
    /// </summary>
    public DosColor? DosButtonColor { get; init; }
}

public enum InputBoxType
{
    Text,
    Integer,
    Decimal,
    Double
}
