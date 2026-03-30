using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

/// <summary>
/// Options for configuring a WinOldDialog
/// </summary>
public record DialogOptions
{
    public Appearance Appearance { get; init; } = Appearance.Win98;
    public string Title { get; init; } = string.Empty;
    public string OkButtonText { get; init; } = "OK";
    public string CancelButtonText { get; init; } = string.Empty;

    /// <summary>
    /// Content to render inside the dialog body.
    /// </summary>
    public RenderFragment? ChildContent { get; init; }

    /// <summary>
    /// Optional fixed width of the dialog window (e.g. "400px", "50vw").
    /// If null, the dialog sizes itself to its content.
    /// </summary>
    public string? Width { get; init; }

    /// <summary>
    /// Optional fixed height of the dialog window (e.g. "300px", "80vh").
    /// If null, the dialog sizes itself to its content.
    /// </summary>
    public string? Height { get; init; }
}