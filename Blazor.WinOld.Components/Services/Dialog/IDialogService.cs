using Blazor.WinOld.Components;

namespace Blazor.WinOld;

public interface IDialogService
{
    /// <summary>
    /// Shows a message box with the specified options
    /// </summary>
    Task<bool?> ShowMessageBox(MessageBoxOptions options);


    /// <summary>
    /// Shows an input box with the specified options
    /// </summary>
    Task<object?> ShowInputBoxCore(InputBoxOptions options);

    /// <summary>
    /// Shows a dialog with the specified options
    /// </summary>
    Task<bool?> ShowDialog(DialogOptions options);
}