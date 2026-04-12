using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldTabPanel : WinOldComponentBase
{
    internal string PanelId { get; } = Guid.NewGuid().ToString("N");

    /// </summary>
    [CascadingParameter]
    public WinOldTabs ParentTabs { get; set; }

    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    /// </summary>
    [Parameter]
    public bool IsDefault { get; set; } = false;

    /// </summary>
    protected override void OnInitialized()
    {
        ParentTabs?.RegisterTabPanel(this);
    }

    /// </summary>
    private string GetComponentClass()
    {
        return ParentTabs.Appearance switch
        {
            Appearance.Win7 => "panel-win-7",
            Appearance.WinXP => "panel-win-xp",
            Appearance.Win98 => "panel-win-98",
            Appearance.Win10 => "panel-win-10",
            _ => "panel-win-10"
        };
    }
}
