using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldSelect<T> : WinOldComponentBase
{
    private readonly string _selectId = $"select-win-{Guid.NewGuid():N}";

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
    public T? Value { get; set; } = default(T);

    /// </summary>
    [Parameter]
    public EventCallback<T> ValueChanged { get; set; }

    /// </summary>
    internal HashSet<WinOldOption<T>> Options { get; } = new();

    /// </summary>
    internal void AddOption(WinOldOption<T> option)
    {
        if (!Options.Contains(option))
        {
            Options.Add(option);
        }
    }

    /// </summary>
    private async Task OnChange(ChangeEventArgs e)
    {
        if (e.Value is not null && Options.FirstOrDefault(opt => opt.Value?.ToString() == e.Value.ToString()) is WinOldOption<T> selectedOption)
        {
            Value = selectedOption.Value;
            await ValueChanged.InvokeAsync(Value);
        }
    }

    /// </summary>
    private string GetComponentClass()
    {
        var cls = Appearance switch
        {
            Appearance.DOS => "select-dos",
            Appearance.Win31 => "select-win-31",
            Appearance.Win7 => "select-win-7",
            Appearance.WinXP => "select-win-xp",
            Appearance.Win98 => "select-win-98",
            Appearance.Win10 => "select-win-10",
            _ => "select-win-10"
        };
        if (IsTouch) cls += " win-touch";
        return cls;
    }

    /// <summary>
    /// Win98 / WinXP draw the arrow button on the wrapper (not as a background of the select),
    /// so the button can opt out of dark mode while the select text follows it.
    /// </summary>
    private string? GetWrapClass()
    {
        var cls = Appearance switch
        {
            Appearance.Win98 => "select-wrap-98",
            Appearance.WinXP => "select-wrap-xp",
            _ => null
        };
        if (cls is not null && IsTouch) cls += " win-touch";
        return cls;
    }
}
