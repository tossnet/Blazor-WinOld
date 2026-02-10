using Blazor.WinOld.Components;
using System.Diagnostics.CodeAnalysis;

namespace Blazor.WinOld;

public interface IDialogService
{
    /// <summary>
    /// Shows a message box with the specified options
    /// </summary>
    Task<bool?> ShowMessageBox(MessageBoxOptions options);


    /// <summary>
    /// Shows a input box with the specified options
    /// </summary>
    Task<string?> ShowInputBox(InputBoxOptions options);

    /// <summary>
    /// Shows a dialog with the specified options
    /// </summary>
    /// <remarks>
    /// ⚠️ This API is experimental and subject to change in future releases.
    /// </remarks>
    [Experimental("BWOLD001")]
    Task<bool?> ShowDialog(DialogOptions options);
}

