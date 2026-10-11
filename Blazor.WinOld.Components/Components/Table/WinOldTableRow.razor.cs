using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldTableRow : WinOldComponentBase
{
    /// </summary>
    [CascadingParameter]
    public WinOldTable? Parent { get; set; }

    /// <summary>
    /// The cells (<c>td</c>) of the row.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Value sent to <see cref="WinOldTable.SelectedValue"/> when the row is clicked.
    /// Without a value, the row is not selectable.
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// </summary>
    private bool IsSelected => Parent?.IsSelected(Value) == true;

    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        Parent?.AddRow(this);
    }

    /// </summary>
    private Task SelectAsync()
        => Parent?.SelectAsync(Value) ?? Task.CompletedTask;

    /// </summary>
    internal void NotifySelectionChanged()
    {
        StateHasChanged();
    }

    /// </summary>
    public void Dispose()
    {
        Parent?.RemoveRow(this);
    }
}
