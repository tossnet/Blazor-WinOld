using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldTabs : WinOldComponentBase
{
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Visual style. When unset, falls back to the Appearance of the hosting <see cref="WinOldWindow"/>
    /// or <see cref="WinOldAppShell"/>, then to <see cref="Blazor.WinOld.Components.Appearance.Win10"/>.
    /// Cascaded to the content of the tabs (toolbar, menu, status bar...).
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance? Appearance { get; set; }

    /// </summary>
    [CascadingParameter(Name = "WindowAppearance")]
    public Appearance? WindowAppearance { get; set; }

    /// <summary>
    /// Appearance actually rendered, inherited by the tab panels and cascaded to their content.
    /// </summary>
    public Appearance EffectiveAppearance => Appearance ?? WindowAppearance ?? global::Blazor.WinOld.Components.Appearance.Win10;

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
        var cls = EffectiveAppearance switch
        {
            global::Blazor.WinOld.Components.Appearance.DOS => "tabs-win-dos",
            global::Blazor.WinOld.Components.Appearance.Win31 => "tabs-win-31",
            global::Blazor.WinOld.Components.Appearance.Win98 => "tabs-win-98",
            global::Blazor.WinOld.Components.Appearance.Win7 => "tabs-win-7",
            global::Blazor.WinOld.Components.Appearance.WinXP => "tabs-win-xp",
            global::Blazor.WinOld.Components.Appearance.Win10 => "tabs-win-10",
            _ => "tabs-win-10"
        };
        if (IsTouch) cls += " win-touch";
        return cls;
    }

    /// </summary>
    private string GetTabsListClass()
    {
        var cls = EffectiveAppearance switch
        {
            global::Blazor.WinOld.Components.Appearance.DOS => "tabs-list-dos",
            global::Blazor.WinOld.Components.Appearance.Win31 => "tabs-list-win31",
            _ => string.Empty
        };
        if (IsTouch) cls += " win-touch";
        return cls.Trim();
    }

    /// </summary>
    private string GetActiveTabClass(WinOldTabPanel tabPanel)
    {
        bool isActive = SelectedTabPanel == tabPanel;

        string activeClass = EffectiveAppearance switch
        {
            global::Blazor.WinOld.Components.Appearance.DOS => "active-tab-win-dos",
            global::Blazor.WinOld.Components.Appearance.Win31 => "active-tab-win-31",
            global::Blazor.WinOld.Components.Appearance.Win7 => "active-tab-win-7",
            global::Blazor.WinOld.Components.Appearance.WinXP => "active-tab-win-xp",
            global::Blazor.WinOld.Components.Appearance.Win98 => "active-tab-win-98",
            global::Blazor.WinOld.Components.Appearance.Win10 => "active-tab-win-10",
            _ => "active-tab-win-10"
        };

        return isActive ? activeClass : string.Empty;
    }
}
