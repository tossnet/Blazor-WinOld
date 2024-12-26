namespace Blazor.WinOld.Components;

public record DialogOptions
{
    public Appearance Appearance { get; init; } = Appearance.Win98;
    public string Title { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
    public Icon Icon { get; init; } = Icon.None;
    public string OkButtonText { get; init; } = "OK";
    public string CancelButtonText { get; init; } = string.Empty;
}