using Blazor.WinOld.Components;

namespace Blazor.WinOld;

public interface IDialogService
{
    Task<bool?> ShowMessageBox(DialogOptions options);
}

