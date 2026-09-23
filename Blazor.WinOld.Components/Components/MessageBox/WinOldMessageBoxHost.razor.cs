using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldMessageBoxHost : WinOldComponentBase
{
    [Inject] private IDialogService? DialogService { get; set; } = default!;

    private ElementReference _windowRef;
    private WinOldTitleBar _titleBar = default!;
    private DraggableWindow _draggable = default!;
    private readonly string _titleId = $"msg-title-{Guid.NewGuid():N}";

    /// </summary>
    private MessageBoxOptions Options { get; set; } = new MessageBoxOptions();
    /// </summary>
    private TaskCompletionSource<bool?>? Tcs { get; set; }
    /// </summary>
    private bool IsVisible { get; set; }
    private DialogService? _service;
    private int _zIndex = 99999;

    protected override void OnInitialized()
    {
        if (DialogService is DialogService service)
        {
            _service = service;
            service.Register(ShowMessageBox);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsVisible)
            await _draggable.InitAsync(_windowRef, _titleBar.Element);
    }

    /// </summary>
    private Task<bool?> ShowMessageBox(MessageBoxOptions options)
    {
        Options = options;
        Tcs = new TaskCompletionSource<bool?>();
        IsVisible = true;
        _zIndex = _service?.NextZIndex() ?? _zIndex;
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
        _ = _draggable.ResetAsync(); // fire-and-forget
        StateHasChanged();
    }

    /// </summary>
    private string GetMessageBoxClass()
    {
        return Options.Appearance switch
        {
            Appearance.DOS => $"msg-dos msg-dos-{GetDosBoxColor().ToString().ToLowerInvariant()}",
            Appearance.Win31 => "msg-win-31",
            Appearance.Win98 => "msg-win-98",
            Appearance.WinXP => "msg-win-xp",
            Appearance.Win7 => "msg-win-7",
            Appearance.Win10 => "msg-win-10",
            _ => "msg-win-10"
        };
    }


    /// </summary>
    private string GetTitleBarClass()
    {
        return Options.Appearance switch
        {
            Appearance.DOS => "title-bar-dos",
            Appearance.Win31 => "title-bar-win-31",
            Appearance.Win98 => "title-bar-win-98",
            Appearance.WinXP => "title-bar-win-xp",
            Appearance.Win7 => "title-bar-win-7",
            Appearance.Win10 => "title-bar-win-10",
            _ => "title-bar-win-10"
        };
    }

    /// </summary>
    private string GetTitleBarTextClass()
    {
        return Options.Appearance switch
        {
            Appearance.DOS => "title-bar-text-dos",
            Appearance.Win31 => "title-bar-text-win-31",
            Appearance.Win98 => "title-bar-text-win-98",
            Appearance.WinXP => "title-bar-text-win-xp",
            Appearance.Win7 => "title-bar-text-win-7",
            Appearance.Win10 => "title-bar-text-win-10",
            _ => "title-bar-text-win-10"
        };
    }

    /// </summary>
    private string GetTitleBarControlsClass()
    {
        return Options.Appearance switch
        {
            Appearance.DOS => "title-bar-controls-dos",
            Appearance.Win31 => "title-bar-controls-win-31",
            Appearance.Win7 => "title-bar-controls-win-7",
            Appearance.WinXP => "title-bar-controls-win-xp",
            Appearance.Win98 => "title-bar-controls-win-98",
            Appearance.Win10 => "title-bar-controls-win-10",
            _ => "title-bar-controls-win-10"
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
            Appearance.Win10 => "10",
            _ => "10"
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

    /// <summary>DOS only: box background, explicit or derived from the icon (there is no icon bitmap in DOS).</summary>
    private DosColor GetDosBoxColor()
    {
        return Options.DosColor ?? Options.Icon switch
        {
            Icon.Information => DosColor.Green,
            Icon.Question => DosColor.Cyan,
            Icon.Alert => DosColor.Brown,
            Icon.Critical => DosColor.Red,
            _ => DosColor.Default
        };
    }

    /// <summary>DOS only: buttons contrast with the box (green on gray, gray on a colored box) unless set explicitly.</summary>
    private DosColor GetDosButtonColor()
    {
        return Options.DosButtonColor
            ?? (GetDosBoxColor() == DosColor.Default ? DosColor.Green : DosColor.Default);
    }

    private string GetMessageBodyClass()
    {
        return Options.Appearance switch
        {
            Appearance.DOS => "message-body-dos",
            Appearance.Win31 => "message-body-win-31",
            Appearance.Win7 => "message-body-win-7",
            Appearance.WinXP => "message-body-win-xp",
            Appearance.Win98 => "message-body-win-98",
            Appearance.Win10 => "message-body-win-10",
            _ => "message-body-win-10"
        };
    }
    /// </summary>
    private string GetMessageContentClass()
    {
        return Options.Appearance switch
        {
            Appearance.DOS => "msg-content-dos",
            Appearance.Win31 => "msg-content-win-31",
            Appearance.Win7 => "msg-content-win-7",
            Appearance.WinXP => "msg-content-win-xp",
            Appearance.Win98 => "msg-content-win-98",
            Appearance.Win10 => "msg-content-win-10",
            _ => "msg-content-win-10"
        };
    }
}
