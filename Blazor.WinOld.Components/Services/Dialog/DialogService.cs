using Blazor.WinOld.Components;

namespace Blazor.WinOld;

public class DialogService : IDialogService
{
    private readonly IServiceProvider _serviceProvider;

    public DialogService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// </summary>
    private event Func<DialogOptions, Task<bool?>>? OnShowMessageBox;

    /// </summary>
    internal void Register(Func<DialogOptions, Task<bool?>> handler)
    {
        OnShowMessageBox += handler;
    }

    /// </summary>
    public async Task<bool?> ShowMessageBox(DialogOptions options)
    {
        if (OnShowMessageBox is not null)
        {
            return await OnShowMessageBox.Invoke(options);
        }

        return null;
    }
}
