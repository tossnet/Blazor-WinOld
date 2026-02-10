using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.WinOld.Components;

/// <summary>
/// WinOldDialog component - A Windows-style dialog box
/// </summary>
/// <remarks>
/// ⚠️ This component is experimental and subject to change in future releases.
/// </remarks>
[Experimental("BWOLD001")]
public partial class WinOldDialog : WinOldComponentBase
{
    [Parameter]
    public DialogOptions Options { get; set; } = new DialogOptions();

    private void HandleOk()
    {
        // TODO: Implement dialog logic
    }
}
