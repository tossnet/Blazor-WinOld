using Blazor.WinOld.Components;
using System.Diagnostics.CodeAnalysis;

#pragma warning disable BWOLD001 // Suppress experimental warning for internal implementation

namespace Blazor.WinOld;

public class DialogService : IDialogService
{
    private readonly IServiceProvider _serviceProvider;

    public DialogService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    // MessageBox events and methods
    private event Func<MessageBoxOptions, Task<bool?>>? OnShowMessageBox;

    internal void Register(Func<MessageBoxOptions, Task<bool?>> handler)
    {
        OnShowMessageBox += handler;
    }

    public async Task<bool?> ShowMessageBox(MessageBoxOptions options)
    {
        if (OnShowMessageBox is not null)
        {
            return await OnShowMessageBox.Invoke(options);
        }

        return null;
    }


    // InputBox events and methods
    private event Func<InputBoxOptions, Task<string?>>? OnShowInputBox;

    internal void RegisterInputBox(Func<InputBoxOptions, Task<string?>> handler)
    {
        OnShowInputBox += handler;
    }

    public async Task<string?> ShowInputBox(InputBoxOptions options)
    {
        if (OnShowInputBox is not null)
        {
            return await OnShowInputBox.Invoke(options);
        }

        return null;
    }


    // Dialog events and methods
    private event Func<DialogOptions, Task<bool?>>? OnShowDialog;

    [Experimental("BWOLD001")]
    internal void RegisterDialog(Func<DialogOptions, Task<bool?>> handler)
    {
        OnShowDialog += handler;
    }

    [Experimental("BWOLD001")]
    public async Task<bool?> ShowDialog(DialogOptions options)
    {
        if (OnShowDialog is not null)
        {
            return await OnShowDialog.Invoke(options);
        }

        return null;
    }
}

#pragma warning restore BWOLD001
