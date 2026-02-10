using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldInputBoxHost : WinOldComponentBase
{
    [Inject] 
    private IDialogService? DialogService { get; set; } = default!;

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
            _ => "inp-win-98"
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

    private string GetInputBoxBodyClass()
    {
        return Options.Appearance switch
        {
            Appearance.Win7 => "input-body-win-7",
            Appearance.WinXP => "input-body-win-xp",
            Appearance.Win98 => "input-body-win-98",
            _ => "input-body-win-98"
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
            _ => "inp-content-win-98"
        };
    }
}
