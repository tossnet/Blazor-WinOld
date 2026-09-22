using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Blazor.WinOld.Components;

public partial class WinOldNumberBox<TValue> : WinOldComponentBase
{
    private ElementReference InputElement { get; set; }

    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; }

    /// <summary>
    /// Color of the spin buttons when Appearance is DOS. Ignored for other appearances.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public DosColor DosColor { get; set; } = DosColor.Default;

    [Parameter]
    public string Label { get; set; } = string.Empty;

    [Parameter]
    public TValue? Value { get; set; }

    [Parameter]
    public EventCallback<TValue?> ValueChanged { get; set; }

    [Parameter]
    public string? InputStyle { get; set; }

    [Parameter]
    public string? Min { get; set; }

    [Parameter]
    public string? Max { get; set; }

    [Parameter]
    public string? Step { get; set; }

    /// <summary>
    /// Standard .NET numeric format string applied to the displayed value (e.g. "F2" or "0.00" to always show 2 decimals).
    /// /!\ Display only: the bound value is not rounded. The text is formatted with the invariant culture because
    /// <c>type="number"</c> only accepts a "." decimal separator and no group separators, so N/C/P formats are not supported.
    /// </summary>
    [Parameter]
    public string? Format { get; set; }

    private Guid ElementId { get; set; } = Guid.NewGuid();

    // Text rendered in the input. Left untouched while typing so Blazor doesn't rewrite the field under the cursor;
    // reformatted when the field is committed (change/blur/Enter), on spin buttons and on external Value changes.
    private string? DisplayText { get; set; }
    private TValue? _syncedValue;
    private string? _syncedFormat;
    private bool _displayInitialized;

    protected TValue? CurrentValue
    {
        get => Value;
        set
        {
            if (!EqualityComparer<TValue?>.Default.Equals(value, Value))
            {
                Value = value;
                _ = ValueChanged.InvokeAsync(value);
            }
        }
    }

    protected override void OnParametersSet()
    {
        if (!_displayInitialized
            || Format != _syncedFormat
            || !EqualityComparer<TValue?>.Default.Equals(Value, _syncedValue))
        {
            SyncDisplay();
            _syncedFormat = Format;
            _displayInitialized = true;
        }
    }

    private void SyncDisplay()
    {
        DisplayText = FormatValue(Value);
        _syncedValue = Value;
    }

    private string FormatValue(TValue? value)
    {
        if (value is null) return string.Empty;
        if (!string.IsNullOrEmpty(Format) && value is IFormattable formattable)
            return formattable.ToString(Format, CultureInfo.InvariantCulture);
        return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
    }

    private void ApplyInput(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
            CurrentValue = default;
        else if (BindConverter.TryConvertTo<TValue?>(text, CultureInfo.InvariantCulture, out var parsed))
            CurrentValue = parsed;
        else
            return;

        // Value now matches what the user typed: don't reformat it (and rewrite the field) until the field is committed.
        _syncedValue = Value;
    }

    private void OnInput(ChangeEventArgs e) => ApplyInput(e.Value?.ToString());

    private async Task OnChange(ChangeEventArgs e)
    {
        ApplyInput(e.Value?.ToString());

        var formatted = FormatValue(Value);
        if (formatted == DisplayText && e.Value?.ToString() != formatted)
        {
            // The DOM may hold a different typed text (ex: "12") while the last rendered text is already "12.00":
            // Blazor only diffs against the previous render, so blank the field for one render to force the rewrite.
            DisplayText = null;
            StateHasChanged();
            await Task.Yield();
        }

        DisplayText = formatted;
        _syncedValue = Value;
    }

    public async Task FocusAsync()
    {
        await InputElement.FocusAsync();
    }

    private void Increment() => StepBy(GetStep());

    private void Decrement() => StepBy(-GetStep());

    private void StepBy(double delta)
    {
        if (ParentDisabled == true || Disabled) return;

        double? min = ParseDouble(Min);
        double? max = ParseDouble(Max);
        double current = Value is null ? (min ?? 0d) : Convert.ToDouble(Value, CultureInfo.InvariantCulture);

        double next = current + delta;
        if (min.HasValue) next = Math.Max(next, min.Value);
        if (max.HasValue) next = Math.Min(next, max.Value);
        next = Math.Round(next, CountDecimals(Step), MidpointRounding.AwayFromZero);

        var underlyingType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
        CurrentValue = (TValue?)Convert.ChangeType(next, underlyingType, CultureInfo.InvariantCulture);
        SyncDisplay();
    }

    private double GetStep() => ParseDouble(Step) ?? 1d;

    private static double? ParseDouble(string? s) =>
        string.IsNullOrWhiteSpace(s) ? null : Convert.ToDouble(s, CultureInfo.InvariantCulture);

    private static int CountDecimals(string? step)
    {
        if (string.IsNullOrWhiteSpace(step)) return 0;
        int dot = step.IndexOf('.');
        return dot < 0 ? 0 : step.Length - dot - 1;
    }

    private string GetComponentClass()
    {
        var cls = Appearance switch
        {
            Appearance.DOS => "txt-win-dos",
            Appearance.Win98 => "txt-win-98",
            Appearance.WinXP => "txt-win-xp",
            Appearance.Win7 => "txt-win-7",
            Appearance.Win10 => "txt-win-10",
            _ => "txt-win-10"
        };
        if (IsTouch) cls += " win-touch";
        return cls;
    }

    private string GetComponentLabelClass()
    {
        if (string.IsNullOrEmpty(Label))
            return "label-txt-hide";

        var cls = Appearance switch
        {
            Appearance.DOS => "lbtxt-win-dos",
            Appearance.Win98 => "lbtxt-win-98",
            Appearance.WinXP => "lbtxt-win-xp",
            Appearance.Win7 => "lbtxt-win-7",
            Appearance.Win10 => "lbtxt-win-10",
            _ => "lbtxt-win-10"
        };
        if (IsTouch) cls += " win-touch";
        return cls;
    }

    private string GetSpinWrapClass()
    {
        var cls = Appearance switch
        {
            Appearance.DOS => "numbox-wrap-dos",
            Appearance.Win98 => "numbox-wrap-98",
            Appearance.WinXP => "numbox-wrap-xp",
            Appearance.Win7 => "numbox-wrap-7",
            Appearance.Win10 => "numbox-wrap-10",
            _ => "numbox-wrap-10"
        };
        if (IsTouch) cls += " win-touch";
        return cls;
    }

    private string GetSpinClass()
    {
        var cls = Appearance switch
        {
            Appearance.DOS => "numbox-spin-dos",
            Appearance.Win98 => "numbox-spin-98",
            Appearance.WinXP => "numbox-spin-xp",
            Appearance.Win7 => "numbox-spin-7",
            Appearance.Win10 => "numbox-spin-10",
            _ => "numbox-spin-10"
        };
        if (IsTouch) cls += " win-touch";
        return cls;
    }

    private string GetSpinBtnThemeClass()
    {
        var cls = Appearance switch
        {
            Appearance.DOS => "numbox-spin-btn-dos",
            Appearance.Win98 => "numbox-spin-btn-98",
            Appearance.WinXP => "numbox-spin-btn-xp",
            Appearance.Win7 => "numbox-spin-btn-7",
            Appearance.Win10 => "numbox-spin-btn-10",
            _ => "numbox-spin-btn-10"
        };
        if (Appearance == Appearance.DOS && DosColor != DosColor.Default)
            cls += $" numbox-spin-btn-dos-{DosColor.ToString().ToLowerInvariant()}";
        if (IsTouch) cls += " win-touch";
        return cls;
    }
}
