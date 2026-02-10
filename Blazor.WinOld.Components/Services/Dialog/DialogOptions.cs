using System.Diagnostics.CodeAnalysis;

namespace Blazor.WinOld.Components;

/// <summary>
/// Options for configuring a WinOldDialog
/// </summary>
/// <remarks>
/// ⚠️ This API is experimental and subject to change in future releases.
/// </remarks>
[Experimental("BWOLD001")]
public record DialogOptions
{
    public Appearance Appearance { get; init; } = Appearance.Win98;
    public string Title { get; init; } = string.Empty;
    public string OkButtonText { get; init; } = "OK";
    public string CancelButtonText { get; init; } = string.Empty;
}