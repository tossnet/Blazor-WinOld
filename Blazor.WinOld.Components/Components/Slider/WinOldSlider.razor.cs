using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Blazor.WinOld.Components;

public partial class WinOldSlider<TValue> : WinOldComponentBase
{
    /// </summary>
    [Parameter]
    public Appearance Appearance { get; set; } = Appearance.Win10;

    /// </summary>
    [Parameter]
    public string LowLabel { get; set; } = string.Empty;

    /// </summary>
    [Parameter]
    public string HighLabel { get; set; } = string.Empty;

    /// </summary>
    [Parameter]
    public TValue? Min { get; set; }

    /// </summary>
    [Parameter]
    public TValue? Max { get; set; }

    /// </summary>
    [Parameter]
    public TValue? Step { get; set; }

    /// </summary>
    [Parameter]
    public TValue? Value { get; set; }

    /// <summary>
    /// Optional in-line styles applied to the input element specifically.
    /// </summary>
    [Parameter]
    public string? RangeStyle { get; set; } = null;

    /// <summary>
    /// When true, displays tick marks along the slider track. 
    /// Requires <see cref="Step"/> to be set.
    /// </summary>
    [Parameter]

    public bool ShowTicks { get; set; } = false;

    /// </summary>
    [Parameter]
    public EventCallback<TValue?> ValueChanged { get; set; }

    private readonly string _listId = $"sld-win-ticks-{Guid.NewGuid():N}";
    private readonly string _inputId = $"sld-win-{Guid.NewGuid():N}";

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

    private void OnInput(ChangeEventArgs e)
    {
        var raw = e.Value?.ToString();
        CurrentValue = raw is null
            ? default
            : (TValue?)Convert.ChangeType(raw, typeof(TValue), CultureInfo.InvariantCulture);
    }

    private void Nudge(int direction)
    {
        double min = Min is null ? 0 : Convert.ToDouble(Min, CultureInfo.InvariantCulture);
        double max = Max is null ? 100 : Convert.ToDouble(Max, CultureInfo.InvariantCulture);
        double step = Step is null ? 1 : Convert.ToDouble(Step, CultureInfo.InvariantCulture);
        if (step <= 0) step = 1;
        double current = Value is null ? min : Convert.ToDouble(Value, CultureInfo.InvariantCulture);

        // Snap to the Step grid (anchored on Min) so floating-point drift or an off-grid value never leaves the grid
        double next = min + Math.Round((current + direction * step - min) / step, MidpointRounding.AwayFromZero) * step;
        next = Math.Round(Math.Clamp(next, min, max), 10);
        CurrentValue = (TValue?)Convert.ChangeType(next, typeof(TValue), CultureInfo.InvariantCulture);
    }

    private string? FormatInvariant(TValue? value)
    {
        if (value is null) return null;
        return Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture);
        //return Convert.ToDouble(value).ToString(CultureInfo.InvariantCulture);
    }

    private IEnumerable<string> GetTicks()
    {
        if (Min is null || Max is null || Step is null) yield break;

        double min = Convert.ToDouble(Min);
        double max = Convert.ToDouble(Max);
        double step = Convert.ToDouble(Step);

        if (step <= 0 || min >= max) yield break;

        for (double v = min; v <= max; v += step)
            yield return v.ToString(CultureInfo.InvariantCulture);
    }

    private int GetTickCount()
    {
        if (Min is null || Max is null || Step is null) return 10;

        double min = Convert.ToDouble(Min);
        double max = Convert.ToDouble(Max);
        double step = Convert.ToDouble(Step);

        if (step <= 0 || min >= max) return 10;

        return (int)Math.Round((max - min) / step);
    }

    /// </summary>
    private string GetComponentClass()
    {
        return Appearance switch
        {
            Appearance.DOS => "sld-win-dos",
            Appearance.Win31 => "sld-win-31",
            Appearance.Win98 => "sld-win-98",
            Appearance.WinXP => "sld-win-xp",
            Appearance.Win7 => "sld-win-7",
            Appearance.Win10 => "sld-win-10",
            _ => "sld-win-10"
        };
    }

    /// </summary>
    private string GetComponentLabelClass()
    {
        return Appearance switch
        {
            Appearance.DOS => "lbtxt-win-dos",
            Appearance.Win31 => "lbtxt-win-31",
            Appearance.Win98 => "lbtxt-win-98",
            Appearance.WinXP => "lbtxt-win-xp",
            Appearance.Win7 => "lbtxt-win-7",
            Appearance.Win10 => "lbtxt-win-10",
            _ => "lbtxt-win-10"
        };
    }
}
