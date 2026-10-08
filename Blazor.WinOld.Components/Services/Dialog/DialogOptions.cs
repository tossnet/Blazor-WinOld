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
    /// DOS appearance only: background color of the dialog box. When null, it is the standard gray.
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

    /// <summary>When true, a maximize button is rendered in the dialog's title bar.</summary>
    public bool MaxButton { get; init; } = false;

    /// <summary>Initial state of the dialog when it opens. Default: Normal.</summary>
    public WindowState WindowState { get; init; } = WindowState.Normal;

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

    /// <summary>Enlarges the title bar and its buttons for touch screens.</summary>
    public bool TouchMode { get; init; }
}