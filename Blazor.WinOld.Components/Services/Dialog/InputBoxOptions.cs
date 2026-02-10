using Blazor.WinOld.Components;

namespace Blazor.WinOld.Components;

/// <summary>
/// Options for configuring a WinOldInputBox
/// </summary>
public record InputBoxOptions
{
    public Appearance Appearance { get; init; } = Appearance.Win98;
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public string? Value { get; set; } = string.Empty;
    public string OkButtonText { get; init; } = "OK";
    public string CancelButtonText { get; init; } = "Cancel";
}
