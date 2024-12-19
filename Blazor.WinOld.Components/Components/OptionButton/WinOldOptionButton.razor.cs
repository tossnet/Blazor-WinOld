using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldOptionButton<T> : WinOldComponentBase
{
    /// </summary>
    [CascadingParameter]
    public WinOldOptionButtonGroup<T?> Group { get; set; }

    /// </summary>
    [CascadingParameter(Name = "Appearance")]
    public virtual Appearance Appearance { get; set; }

    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

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
        return Appearance switch
        {
            Appearance.Win7 => "opt-win-7",
            Appearance.WinXP => "opt-win-xp",
            Appearance.Win98 => "opt-win-98",
            _ => "opt-win-98"
        };
    }
}

