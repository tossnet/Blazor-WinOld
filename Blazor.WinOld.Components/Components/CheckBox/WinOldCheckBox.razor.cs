using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldCheckBox : WinOldComponentBase
{
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; }

    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// DOS appearance only: letter of the label to highlight (access key).
    /// All appareances : use as accesskey attributes
    /// </summary>
    [Parameter]
    public string? HotKey { get; set; }

    /// </summary>
    [Parameter]
    public bool Checked { get; set; }

    /// </summary>
    [Parameter]
    public EventCallback<bool> CheckedChanged { get; set; }

    /// <summary>
    /// Gets or sets the value of the checkbox (alias for Checked, supports @bind-Value).
    /// </summary>
    [Parameter]
    public bool Value
    {
        get => Checked;
        set => Checked = value;
    }

    /// <summary>
    /// Callback for two-way binding with @bind-Value.
    /// </summary>
    [Parameter]
    public EventCallback<bool> ValueChanged { get; set; }

    /// </summary>
    private Guid ElementId { get; set; } = Guid.NewGuid();

    /// </summary>
    private async Task CheckboxChanged(ChangeEventArgs e)
    {
        if (e.Value == null)
        {
            return;
        }

        // get the checkbox state
        Checked = (bool)e.Value;

        if (CheckedChanged.HasDelegate)
        {
            await CheckedChanged.InvokeAsync(Checked);
        }

        if (ValueChanged.HasDelegate)
        {
            await ValueChanged.InvokeAsync(Checked);
        }
    }

    /// <summary>
    /// Splits the label around the first occurrence of the hot key.
    /// </summary>
    private (string Before, string Key, string After) GetLabelParts()
    {
        if (string.IsNullOrEmpty(HotKey))
        {
            return (Label, string.Empty, string.Empty);
        }

        var index = Label.IndexOf(HotKey[0]);
        if (index < 0)
        {
            index = Label.IndexOf(HotKey[0].ToString(), StringComparison.OrdinalIgnoreCase);
        }

        return index < 0
            ? (Label, string.Empty, string.Empty)
            : (Label[..index], Label.Substring(index, 1), Label[(index + 1)..]);
    }

    /// </summary>
    private string GetComponentClass()
    {
        var cls = Appearance switch
        {
            Appearance.DOS => "chk-dos",
            Appearance.Win7 => "chk-win-7",
            Appearance.WinXP => "chk-win-xp",
            Appearance.Win98 => "chk-win-98",
            Appearance.Win10 => "chk-win-10",
            _ => "chk-win-10"
        };
        if (IsTouch) cls += " win-touch";
        return cls;
    }
}
