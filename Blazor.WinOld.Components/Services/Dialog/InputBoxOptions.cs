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
}

public enum InputBoxType
{
    Text,
    Integer,
    Decimal,
    Double
}
