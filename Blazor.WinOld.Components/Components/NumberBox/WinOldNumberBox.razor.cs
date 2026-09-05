using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Blazor.WinOld.Components;

public partial class WinOldNumberBox<TValue> : WinOldComponentBase
{
    private ElementReference InputElement { get; set; }

    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; }

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

    private Guid ElementId { get; set; } = Guid.NewGuid();

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
            Appearance.Win7 => "txt-win-7",
            Appearance.WinXP => "txt-win-xp",
            Appearance.Win98 => "txt-win-98",
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
            Appearance.Win7 => "lbtxt-win-7",
            Appearance.WinXP => "lbtxt-win-xp",
            Appearance.Win98 => "lbtxt-win-98",
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
            Appearance.Win7 => "numbox-wrap-7",
            Appearance.WinXP => "numbox-wrap-xp",
            Appearance.Win98 => "numbox-wrap-98",
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
            Appearance.Win7 => "numbox-spin-7",
            Appearance.WinXP => "numbox-spin-xp",
            Appearance.Win98 => "numbox-spin-98",
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
            Appearance.Win7 => "numbox-spin-btn-7",
            Appearance.WinXP => "numbox-spin-btn-xp",
            Appearance.Win98 => "numbox-spin-btn-98",
            Appearance.Win10 => "numbox-spin-btn-10",
            _ => "numbox-spin-btn-10"
        };
        if (IsTouch) cls += " win-touch";
        return cls;
    }
}
