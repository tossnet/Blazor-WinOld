using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldSlider<TValue> : WinOldComponentBase
{
    /// </summary>
    [Parameter]
    public Appearance Appearance { get; set; }

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

    /// </summary>
    [Parameter]
    public EventCallback<TValue?> ValueChanged { get; set; }

    private async Task OnValueChanged(ChangeEventArgs e)
    {
        if (!ValueChanged.HasDelegate) return;

        var raw = e.Value?.ToString();
        TValue? converted = raw is null ? default : (TValue?)Convert.ChangeType(raw, typeof(TValue));
        Value = converted;
        await ValueChanged.InvokeAsync(Value);
    }

    /// </summary>
    private string GetComponentClass()
    {
        return Appearance switch
        {
            Appearance.Win98 => "sld-win-98",
            Appearance.WinXP => "sld-win-xp",
            Appearance.Win7 => "sld-win-7",
            Appearance.Win10 => "sld-win-10",
            _ => "sld-win-10"
        };
    }
}
