using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldOptionButton<T> : WinOldComponentBase
{
    /// </summary>
    [CascadingParameter]
    public WinOldOptionButtonGroup<T?> Group { get; set; }

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
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    public T Value { get; set; }

    /// </summary>
    private Guid ElementId { get; set; } = Guid.NewGuid();

    /// </summary>
    private bool IsChecked => Group != null && EqualityComparer<T>.Default.Equals(Group.Value, Value);

    /// </summary>
    private void OnChange(ChangeEventArgs e)
    {
        Group?.SelectOption(Value);
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
        var cls = Group.Appearance switch
        {
            Appearance.DOS => "opt-dos",
            Appearance.Win31 => "opt-win-31",
            Appearance.Win98 => "opt-win-98",
            Appearance.WinXP => "opt-win-xp",
            Appearance.Win7 => "opt-win-7",
            Appearance.Win10 => "opt-win-10",
            _ => "opt-win-10"
        };
        if (IsTouch || Group?.IsTouch == true) cls += " win-touch";
        return cls;
    }
}

