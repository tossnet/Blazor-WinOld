using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldOptionButtonGroup<T> :  WinOldComponentBase
{
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    public Guid GroupId = Guid.NewGuid();

    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; }

    /// </summary>
    [Parameter]
    public T? Value { get; set; }

    /// </summary>
    [Parameter]
    public EventCallback<T> ValueChanged { get; set; }

    public async Task SelectOption(T value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(Value);
    }
}
