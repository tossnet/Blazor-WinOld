using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldTabs : WinOldComponentBase
{
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; }

    /// </summary>
    internal HashSet<WinOldTabPanel> TabPanels { get; } = new();

    /// </summary>
    public WinOldTabPanel? SelectedTabPanel { get; private set; }

    /// </summary>
    internal void RegisterTabPanel(WinOldTabPanel tabPanel)
    {
        if (!TabPanels.Contains(tabPanel))
        {
            TabPanels.Add(tabPanel);

            if (tabPanel.IsDefault)
            {
                foreach (var panel in TabPanels)
                {
                    if (panel != tabPanel)
                    {
                        panel.IsDefault = false;
                    }
                }

                SelectedTabPanel = tabPanel;
            }
            else if (SelectedTabPanel == null)
            {
                SelectedTabPanel = tabPanel;
            }

            StateHasChanged();
        }
    }

    /// </summary>
    public void SelectTab(WinOldTabPanel tabPanel)
    {
        SelectedTabPanel = tabPanel;
        StateHasChanged();
    }

    /// </summary>
    private string GetComponentClass()
    {
        return Appearance switch
        {
            Appearance.Win7 => "tabs-win-7",
            Appearance.WinXP => "tabs-win-xp",
            Appearance.Win98 => "tabs-win-98",
            _ => "tabs-win-98"
        };
    }

    /// </summary>
    private string GetActiveTabClass(WinOldTabPanel tabPanel)
    {
        bool isActive = SelectedTabPanel == tabPanel;

        string activeClass = Appearance switch
        {
            Appearance.Win7 => "active-tab-win-7",
            Appearance.WinXP => "active-tab-win-xp",
            Appearance.Win98 => "active-tab-win-98",
            _ => "active-tab-win-98"
        };

        return isActive ? activeClass : string.Empty;
    }
}
