using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldMessageBoxHost : WinOldComponentBase
{
    [Inject] private IDialogService? DialogService { get; set; } = default!;

    /// </summary>
    private DialogOptions Options { get; set; } = new DialogOptions();
    /// </summary>
    private TaskCompletionSource<bool?>? Tcs { get; set; }
    /// </summary>
    private bool IsVisible { get; set; }

    protected override void OnInitialized()
    {
        if (DialogService is DialogService service)
        {
            service.Register(ShowMessageBox);
        }
    }

    /// </summary>
    private Task<bool?> ShowMessageBox(DialogOptions options)
    {
        Options = options;
        Tcs = new TaskCompletionSource<bool?>();
        IsVisible = true;
        StateHasChanged();

        return Tcs.Task;
    }

    /// </summary>
    private void HandleOk()
    {
        Tcs?.SetResult(true);
        Close();
    }

    /// </summary>
    private void HandleCancel()
    {
        Tcs?.SetResult(false);
        Close();
    }

    /// </summary>
    private void Close()
    {
        IsVisible = false;
        StateHasChanged();
    }

    /// </summary>
    private string GetMessageBoxClass()
    {
        return Options.Appearance switch
        {
            Appearance.Win7 => "msg-win-7",
            Appearance.WinXP => "msg-win-xp",
            Appearance.Win98 => "msg-win-98",
            _ => "msg-win-98"
        };
    }


    /// </summary>
    private string GetTitleBarClass()
    {
        return Options.Appearance switch
        {
            Appearance.Win7 => "title-bar-win-7",
            Appearance.WinXP => "title-bar-win-xp",
            Appearance.Win98 => "title-bar-win-98",
            _ => "title-bar-win-98"
        };
    }

    /// </summary>
    private string GetTitleBarTextClass()
    {
        return Options.Appearance switch
        {
            Appearance.Win7 => "title-bar-text-win-7",
            Appearance.WinXP => "title-bar-text-win-xp",
            Appearance.Win98 => "title-bar-text-win-98",
            _ => "title-bar-text-win-98"
        };
    }

    /// </summary>
    private string GetTitleBarControlsClass()
    {
        return Options.Appearance switch
        {
            Appearance.Win7 => "title-bar-controls-win-7",
            Appearance.WinXP => "title-bar-controls-win-xp",
            Appearance.Win98 => "title-bar-controls-win-98",
            _ => "title-bar-controls-win-98"
        };
    }

    /// </summary>
    private string GetIConClass()
    {
        string appareance  = Options.Appearance switch
        {
            Appearance.Win7 => "7",
            Appearance.WinXP => "xp",
            Appearance.Win98 => "98",
            _ => "98"
        };

        return Options.Icon switch
        {
            Icon.Alert => $"icon-alert-win-{appareance}",
            Icon.Critical => $"icon-critical-win-{appareance}",
            Icon.Information => $"icon-info-win-{appareance}",
            Icon.Question => $"icon-question-win-{appareance}",
            _ => string.Empty
        };
    }

    private string GetMessageBodyClass()
    {
        return Options.Appearance switch
        {
            Appearance.Win7 => "message-body-win-7",
            Appearance.WinXP => "message-body-win-xp",
            Appearance.Win98 => "message-body-win-98",
            _ => "message-body-win-98"
        };
    }
    /// </summary>
    private string GetMessageContentClass()
    {
        return Options.Appearance switch
        {
            Appearance.Win7 => "msg-content-win-7",
            Appearance.WinXP => "msg-content-win-xp",
            Appearance.Win98 => "msg-content-win-98",
            _ => "msg-content-win-98"
        };
    }
}
