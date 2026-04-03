using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldInputBoxHost : WinOldComponentBase
{
    [Inject] 
    private IDialogService? DialogService { get; set; } = default!;

    private ElementReference _windowRef;
    private ElementReference _titleBarRef;
    private DraggableWindow _draggable = default!;

    /// <summary>
    /// Reference to the TextBox component
    /// </summary>
    private WinOldTextBox? TextBoxRef { get; set; }

    /// </summary>
    private InputBoxOptions Options { get; set; } = new InputBoxOptions();

    private TaskCompletionSource<string?>? Tcs { get; set; }
    /// </summary>
    private bool IsVisible { get; set; }

    protected override void OnInitialized()
    {
        if (DialogService is DialogService service)
        {
            service.RegisterInputBox(ShowInputBox);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (IsVisible)
            await _draggable.InitAsync(_windowRef, _titleBarRef);
    }

    /// </summary>
    private async Task<string?> ShowInputBox(InputBoxOptions options)
    {
        Options = options;
        Tcs = new TaskCompletionSource<string?>();
        IsVisible = true;
        StateHasChanged();

        // Wait for the component to render and then set focus
        await Task.Delay(100);
        if (TextBoxRef != null)
        {
            await TextBoxRef.FocusAsync();
        }

        return await Tcs.Task;
    }

    /// </summary>
    private void HandleOk()
    {
        Tcs?.SetResult(TextBoxRef?.Value);
        Close();
    }

    /// </summary>
    private void HandleCancel()
    {
        Tcs?.SetResult(null);
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
    private string GetInputBoxClass()
    {
        return Options.Appearance switch
        {
            Appearance.Win7 => "inp-win-7",
            Appearance.WinXP => "inp-win-xp",
            Appearance.Win98 => "inp-win-98",
            Appearance.Win10 => "inp-win-10",
            _ => "inp-win-10"
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
            Appearance.Win10 => "title-bar-win-10",
            _ => "title-bar-win-10"
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
            Appearance.Win10 => "title-bar-text-win-10",
            _ => "title-bar-text-win-10"
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
            Appearance.Win10 => "title-bar-controls-win-10",
            _ => "title-bar-controls-win-10"
        };
    }

    private string GetInputBoxBodyClass()
    {
        return Options.Appearance switch
        {
            Appearance.Win7 => "input-body-win-7",
            Appearance.WinXP => "input-body-win-xp",
            Appearance.Win98 => "input-body-win-98",
            Appearance.Win10 => "input-body-win-10",
            _ => "input-body-win-10"
        };
    }
    /// </summary>
    private string GetInputBoxContentClass()
    {
        return Options.Appearance switch
        {
            Appearance.Win7 => "inp-content-win-7",
            Appearance.WinXP => "inp-content-win-xp",
            Appearance.Win98 => "inp-content-win-98",
            Appearance.Win10 => "inp-content-win-10",
            _ => "inp-content-win-10"
        };
    }
}
