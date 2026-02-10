using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.WinOld.Components;

public partial class WinOldTextBox : WinOldComponentBase
{
    /// <summary>
    /// Reference to the input element
    /// </summary>
    private ElementReference InputElement { get; set; }

    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; }

    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    /// </summary>
    [Parameter]
    public string? Value { get; set; } = string.Empty;

    /// </summary>
    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    /// </summary>
    private Guid ElementId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Sets focus on the input element
    /// </summary>
    public async Task FocusAsync()
    {
        await InputElement.FocusAsync();
    }

    private async Task OnValueChanged(ChangeEventArgs e)
    {
        if (!ValueChanged.HasDelegate) return;

        Value = e.Value?.ToString() ?? string.Empty;
        await ValueChanged.InvokeAsync(Value);
    }

    /// </summary>
    private string GetComponentClass()
    {
        return Appearance switch
        {
            Appearance.Win7 => "txt-win-7",
            Appearance.WinXP => "txt-win-xp",
            Appearance.Win98 => "txt-win-98",
            _ => "txt-win-98"
        };
    }

    /// </summary>
    private string GetComponentLabelClass()
    {
        if (string.IsNullOrEmpty(Label))
            return "label-txt-hide";

        return Appearance switch
        {
            Appearance.Win7 => "lbtxt-win-7",
            Appearance.WinXP => "lbtxt-win-xp",
            Appearance.Win98 => "lbtxt-win-98",
            _ => "lbtxt-win-98"
        };
    }
}
