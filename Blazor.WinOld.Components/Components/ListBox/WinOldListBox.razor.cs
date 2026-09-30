using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldListBox : WinOldComponentBase
{
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; } = Appearance.Win10;

    /// </summary>
    [Parameter]
    public string? SelectedValue { get; set; }

    /// </summary>
    [Parameter]
    public EventCallback<string?> SelectedValueChanged { get; set; }

    /// </summary>
    internal HashSet<WinOldListBoxItem> ListBoxItems { get; } = new();

    /// </summary>
    internal WinOldListBoxItem? SelectedItem { get; private set; }

    /// </summary>
    internal void AddItem(WinOldListBoxItem item)
    {
        if (!ListBoxItems.Contains(item))
        {
            ListBoxItems.Add(item);
        }

        if (SelectedItem is null && item.Value is not null && item.Value == SelectedValue)
        {
            SelectedItem = item;
        }
    }

    /// </summary>
    protected override void OnParametersSet()
    {
        // Keep the highlighted item in sync when SelectedValue is changed from outside
        if (SelectedItem?.Value != SelectedValue)
        {
            SelectedItem = ListBoxItems.FirstOrDefault(i => i.Value == SelectedValue);
            foreach (var listBoxItem in ListBoxItems)
            {
                listBoxItem.NotifySelectionChanged();
            }
        }
    }

    /// </summary>
    internal async Task SetSelectedItemAsync(WinOldListBoxItem item)
    {
        if (SelectedItem != item)
        {
            SelectedItem = item;
            SelectedValue = item.Value;
            foreach (var listBoxItem in ListBoxItems)
            {
                listBoxItem.NotifySelectionChanged();
            }

            if (SelectedValueChanged.HasDelegate)
            {
                await SelectedValueChanged.InvokeAsync(item.Value);
            }
        }
    }

    /// </summary>
    private string GetComponentClass()
    {
        return Appearance switch
        {
            Appearance.DOS => "list-win-dos",
            Appearance.Win31 => "list-win-31",
            Appearance.Win7 => "list-win-7",
            Appearance.WinXP => "list-win-xp",
            Appearance.Win98 => "list-win-98",
            _ => "list-win-10"
        };
    }
}
