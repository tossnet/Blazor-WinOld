using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldInputBoxHost : WinOldComponentBase
{
    [Inject] 
    private IDialogService? DialogService { get; set; } = default!;

    private ElementReference _windowRef;
    private ElementReference _titleBarRef;
    private DraggableWindow _draggable = default!;
    private readonly string _titleId = $"inp-title-{Guid.NewGuid():N}";

    /// <summary>
    /// Reference to the TextBox component
    /// </summary>
    private WinOldTextBox? TextBoxRef { get; set; }

    private WinOldNumberBox<int>? NumberBoxIntRef { get; set; }
    private WinOldNumberBox<decimal>? NumberBoxDecimalRef { get; set; }
    private WinOldNumberBox<double>? NumberBoxDoubleRef { get; set; }

    /// </summary>
    private InputBoxOptions Options { get; set; } = new InputBoxOptions();

    private TaskCompletionSource<object?>? Tcs { get; set; }
    /// </summary>
    private bool IsVisible { get; set; }

    private object? CurrentValue { get; set; }

    // Typed values for binding (non-nullable for proper binding)
    private int IntValue { get; set; }
    private decimal DecimalValue { get; set; }
    private double DoubleValue { get; set; }

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
    private async Task<object?> ShowInputBox(InputBoxOptions options)
    {
        Options = options;
        CurrentValue = options.Value;

        // Initialize typed values
        IntValue = GetIntValue() ?? 0;
        DecimalValue = GetDecimalValue() ?? 0m;
        DoubleValue = GetDoubleValue() ?? 0.0;

        Tcs = new TaskCompletionSource<object?>();
        IsVisible = true;
        StateHasChanged();

        // Wait for the component to render and then set focus
        await Task.Delay(100);

        // Focus sur le bon input
        try
        {
            switch (Options.InputType)
            {
                case InputBoxType.Text when TextBoxRef != null:
                    await TextBoxRef.FocusAsync();
                    break;
                case InputBoxType.Integer when NumberBoxIntRef != null:
                    await NumberBoxIntRef.FocusAsync();
                    break;
                case InputBoxType.Decimal when NumberBoxDecimalRef != null:
                    await NumberBoxDecimalRef.FocusAsync();
                    break;
                case InputBoxType.Double when NumberBoxDoubleRef != null:
                    await NumberBoxDoubleRef.FocusAsync();
                    break;
            }
        }
        catch { /* Ignore focus errors */ }

        return await Tcs.Task;
    }

    private void OnTextChanged(string value) => CurrentValue = value;

    // Helper methods to parse Value from object (can be int, string, etc.)
    private int? GetIntValue()
    {
        return Options.Value switch
        {
            int i => i,
            string s when int.TryParse(s, out var result) => result,
            _ => null
        };
    }

    private decimal? GetDecimalValue()
    {
        return Options.Value switch
        {
            decimal d => d,
            int i => i,
            string s when decimal.TryParse(s, out var result) => result,
            _ => null
        };
    }

    private double? GetDoubleValue()
    {
        return Options.Value switch
        {
            double d => d,
            int i => i,
            decimal dc => (double)dc,
            string s when double.TryParse(s, out var result) => result,
            _ => null
        };
    }


    /// </summary>
    private void HandleOk()
    {
        // Return the appropriate typed value
        var result = Options.InputType switch
        {
            InputBoxType.Integer => IntValue,
            InputBoxType.Decimal => DecimalValue,
            InputBoxType.Double => DoubleValue,
            InputBoxType.Text => CurrentValue,
            _ => CurrentValue
        };

        Tcs?.SetResult(result);
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
        CurrentValue = null;
        IntValue = 0;
        DecimalValue = 0m;
        DoubleValue = 0.0;
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
