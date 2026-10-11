using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldTable : WinOldComponentBase
{
    /// <summary>
    /// Accessible name of the table (<c>aria-label</c>).
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// The <c>thead</c> / <c>tbody</c> of the table. Use <see cref="WinOldTableRow"/> for selectable rows.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; } = Appearance.Win10;

    /// <summary>
    /// Shows (true) or hides (false) the cell grid. Null keeps the default of the style:
    /// grid for every style except DOS, which has its own column separators.
    /// </summary>
    [Parameter]
    public bool? Gridlines { get; set; }

    /// <summary>
    /// Adds an Excel-like first column with the row numbers.
    /// </summary>
    [Parameter]
    public bool RowNumbers { get; set; }

    /// <summary>
    /// Value of the selected <see cref="WinOldTableRow"/>.
    /// </summary>
    [Parameter]
    public string? SelectedValue { get; set; }

    /// </summary>
    [Parameter]
    public EventCallback<string?> SelectedValueChanged { get; set; }

    /// </summary>
    internal HashSet<WinOldTableRow> Rows { get; } = new();

    private string? _renderedSelectedValue;

    /// </summary>
    internal void AddRow(WinOldTableRow row) => Rows.Add(row);

    /// </summary>
    internal void RemoveRow(WinOldTableRow row) => Rows.Remove(row);

    /// </summary>
    internal bool IsSelected(string? value) => value is not null && value == SelectedValue;

    /// </summary>
    protected override void OnParametersSet()
    {
        // Keep the highlighted row in sync when SelectedValue is changed from outside
        if (_renderedSelectedValue != SelectedValue)
        {
            _renderedSelectedValue = SelectedValue;
            NotifyRows();
        }
    }

    /// </summary>
    internal async Task SelectAsync(string? value)
    {
        if (value is null || value == SelectedValue)
        {
            return;
        }

        SelectedValue = value;
        _renderedSelectedValue = value;
        NotifyRows();

        if (SelectedValueChanged.HasDelegate)
        {
            await SelectedValueChanged.InvokeAsync(value);
        }
    }

    /// </summary>
    private void NotifyRows()
    {
        foreach (var row in Rows)
        {
            row.NotifySelectionChanged();
        }
    }

    /// </summary>
    private string GetTableClass()
    {
        var gridClass = Gridlines switch
        {
            true => " table-win-gridlines",
            false => " table-win-nogridlines",
            _ => string.Empty
        };

        return $"table-win table-win-{GetStyleSuffix()}{gridClass}{(RowNumbers ? " table-win-rownumbers" : string.Empty)}";
    }

    /// </summary>
    private string GetStyleSuffix()
    {
        return Appearance switch
        {
            Appearance.DOS => "dos",
            Appearance.Win31 => "31",
            Appearance.Win7 => "7",
            Appearance.WinXP => "xp",
            Appearance.Win98 => "98",
            _ => "10"
        };
    }
}
