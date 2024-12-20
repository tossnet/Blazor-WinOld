using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldSelect<T> : WinOldComponentBase
{
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; }

    /// </summary>
    [Parameter]
    public T? Value { get; set; } = default(T);

    /// </summary>
    [Parameter]
    public EventCallback<T> ValueChanged { get; set; }

    internal HashSet<WinOldOption<T>> Options { get; } = new();

    internal void AddOption(WinOldOption<T> option)
    {
        if (!Options.Contains(option))
        {
            Options.Add(option);
        }
    }
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
        return Appearance switch
        {
            Appearance.Win7 => "select-win-7",
            Appearance.WinXP => "select-win-xp",
            Appearance.Win98 => "select-win-98",
            _ => "select-win-98"
        };
    }
}
