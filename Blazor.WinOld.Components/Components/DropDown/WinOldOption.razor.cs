using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldOption<T> : WinOldComponentBase
{
    /// </summary>
    [CascadingParameter]
    public WinOldSelect<T?> Select { get; set; }

    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    public T Value { get; set; }

    private bool IsSelected => Select != null &&  EqualityComparer<T>.Default.Equals(Value, Select.Value);

    protected override void OnInitialized()
    {
        Select?.AddOption(this);
    }
}

