using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial  class WinOldListBoxItem : WinOldComponentBase
{
    /// </summary>
    [CascadingParameter]
    public WinOldListBox Parent { get; set; }

    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    public string Text { get; set; } = string.Empty;

    [Parameter]
    public string? Value { get; set; }

    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Parent?.AddItem(this);
    }

    /// </summary>
    public void SelectItem(WinOldListBoxItem item)
    {
        Parent?.SetSelectedItem(item);
    }

    /// </summary>
    public void NotifySelectionChanged()
    {
        StateHasChanged();
    }

    /// </summary>
    private string GetComponentClass()
    {
        return Parent.Appearance switch
        {
            Appearance.Win7 => "list-item-win-7",
            Appearance.WinXP => "list-item-win-xp",
            Appearance.Win98 => "list-item-win-98",
            _ => "list-item-win-98"
        };
    }

    /// </summary>
    private string GetActiveItemClass(WinOldListBoxItem item)
    {
        bool isActive = Parent?.SelectedItem == item;

        string activeClass = Parent.Appearance switch
        {
            Appearance.Win7 => "active-list-item-win-7",
            Appearance.WinXP => "active-list-item-win-xp",
            Appearance.Win98 => "active-list-item-win-98",
            _ => "active-list-item-win-98"
        };

        return isActive ? activeClass : string.Empty;
    }
}
