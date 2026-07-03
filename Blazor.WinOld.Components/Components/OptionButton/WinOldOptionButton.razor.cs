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

    /// </summary>
    private string GetComponentClass()
    {
        var cls = Group.Appearance switch
        {
            Appearance.Win7 => "opt-win-7",
            Appearance.WinXP => "opt-win-xp",
            Appearance.Win98 => "opt-win-98",
            Appearance.Win10 => "opt-win-10",
            _ => "opt-win-10"
        };
        if (IsTouch || Group?.IsTouch == true) cls += " win-touch";
        return cls;
    }
}

