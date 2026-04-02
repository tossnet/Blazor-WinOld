using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldDialogHost : WinOldComponentBase
{
    [Inject] private IDialogService? DialogService { get; set; } = default!;

    private ElementReference _windowRef;
    private ElementReference _titleBarRef;
    private DraggableWindow _draggable = default!;

    private DialogOptions Options { get; set; } = new DialogOptions();
    private TaskCompletionSource<bool?>? Tcs { get; set; }
    private bool IsVisible { get; set; }

    protected override void OnInitialized()
    {
        if (DialogService is DialogService service)
        {
            service.RegisterDialog(ShowDialog);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsVisible)
            await _draggable.InitAsync(_windowRef, _titleBarRef);
    }

    private Task<bool?> ShowDialog(DialogOptions options)
    {
        Options = options;
        Tcs = new TaskCompletionSource<bool?>();
        IsVisible = true;
        StateHasChanged();

        return Tcs.Task;
    }

    private void HandleOk()
    {
        Tcs?.SetResult(true);
        Close();
    }

    private void HandleCancel()
    {
        Tcs?.SetResult(false);
        Close();
    }

    private void Close()
    {
        IsVisible = false;
        _ = _draggable.ResetAsync();
        StateHasChanged();
    }

    private string GetDialogClass() => Options.Appearance switch
    {
        Appearance.Win7 => "dlg-win-7",
        Appearance.WinXP => "dlg-win-xp",
        Appearance.Win98 => "dlg-win-98",
        Appearance.Win10 => "dlg-win-10",
        _ => "dlg-win-10"
    };

    private string GetTitleBarClass() => Options.Appearance switch
    {
        Appearance.Win7 => "title-bar-win-7",
        Appearance.WinXP => "title-bar-win-xp",
        Appearance.Win98 => "title-bar-win-98",
        Appearance.Win10 => "title-bar-win-10",
        _ => "title-bar-win-10"
    };

    private string GetTitleBarTextClass() => Options.Appearance switch
    {
        Appearance.Win7 => "title-bar-text-win-7",
        Appearance.WinXP => "title-bar-text-win-xp",
        Appearance.Win98 => "title-bar-text-win-98",
        Appearance.Win10 => "title-bar-text-win-10",
        _ => "title-bar-text-win-98"
    };

    private string GetTitleBarControlsClass() => Options.Appearance switch
    {
        Appearance.Win7 => "title-bar-controls-win-7",
        Appearance.WinXP => "title-bar-controls-win-xp",
        Appearance.Win98 => "title-bar-controls-win-98",
        Appearance.Win10 => "title-bar-controls-win-10",
        _ => "title-bar-controls-win-10"
    };

    private string GetDialogBodyClass() => Options.Appearance switch
    {
        Appearance.Win7 => "dlg-body-win-7",
        Appearance.WinXP => "dlg-body-win-xp",
        Appearance.Win98 => "dlg-body-win-98",
        Appearance.Win10 => "dlg-body-win-10",
        _ => "dlg-body-win-10"
    };

    private string GetDialogSizeStyle()
    {
        var parts = new List<string>();

        if (!string.IsNullOrEmpty(Options.Width))
            parts.Add($"width: {Options.Width}");

        if (!string.IsNullOrEmpty(Options.Height))
            parts.Add($"height: {Options.Height}");

        return string.Join("; ", parts);
    }
}