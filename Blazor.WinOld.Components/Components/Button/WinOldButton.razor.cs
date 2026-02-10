using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.WinOld.Components;

public partial class WinOldButton : WinOldComponentBase
{
    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; }


    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool Default { get; set; } = false;

    /// <summary>
    /// Command executed when the user clicks on the button.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// </summary>
    protected async Task OnClickHandlerAsync(MouseEventArgs e)
    {
        if (!Disabled && OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(e);
        }
    }

    /// </summary>
    private string GetComponentClass()
    {
        var baseClass = Appearance switch
        {
            Appearance.Win7 => "btn-win-7",
            Appearance.WinXP => "btn-win-xp",
            Appearance.Win98 => "btn-win-98",
            _ => "btn-win-98"
        };

        return Default ? $"{baseClass} {baseClass}-default" : baseClass;
    }
}
