using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

/// <summary>
/// Application layout: menu, toolbar, side pane, content and status bar, filling the given height.
/// Only the side pane and the content scroll. Place it in your own MainLayout.
/// </summary>
public partial class WinOldAppShell : WinOldComponentBase
{
    /// <summary>
    /// Name of the section rendered at the end of the toolbar area: a page can add its own toolbar
    /// with <c>&lt;SectionContent SectionName="@WinOldAppShell.ToolbarSection"&gt;</c>.
    /// </summary>
    public const string ToolbarSection = "winold-appshell-toolbar";

    /// <summary>
    /// Name of the section rendered at the end of the status bar area: a page can add its own status bar
    /// with <c>&lt;SectionContent SectionName="@WinOldAppShell.StatusBarSection"&gt;</c>.
    /// </summary>
    public const string StatusBarSection = "winold-appshell-statusbar";

    /// <summary>
    /// Visual style, inherited by the Menu, Toolbar, NavPane and StatusBar placed inside that don't set their own.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; } = Appearance.Win10;

    /// <summary>
    /// Menu bar area (typically a <see cref="WinOldMenu"/>).
    /// </summary>
    [Parameter]
    public RenderFragment? Menu { get; set; }

    /// <summary>
    /// Toolbar area (typically a <see cref="WinOldToolbar"/>).
    /// </summary>
    [Parameter]
    public RenderFragment? Toolbar { get; set; }

    /// <summary>
    /// Side area on the left (typically a <see cref="WinOldNavPane"/>). Scrolls on its own.
    /// </summary>
    [Parameter]
    public RenderFragment? SidePane { get; set; }

    /// <summary>
    /// Status bar area (typically a <see cref="WinOldStatusBar"/>).
    /// </summary>
    [Parameter]
    public RenderFragment? StatusBar { get; set; }

    /// <summary>
    /// Main content (in a MainLayout: <c>@Body</c>). The only area that scrolls with the side pane.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Height of the shell. Default: the whole viewport (<c>100dvh</c>). Use <c>100%</c> or a fixed size inside a container.
    /// </summary>
    [Parameter]
    public string Height { get; set; } = "100dvh";

    /// <summary>
    /// Renders the toolbar area even without <see cref="Toolbar"/>, so that pages can fill it through <see cref="ToolbarSection"/>.
    /// </summary>
    [Parameter]
    public bool ShowToolbar { get; set; }

    /// <summary>
    /// Renders the status bar area even without <see cref="StatusBar"/>, so that pages can fill it through <see cref="StatusBarSection"/>.
    /// </summary>
    [Parameter]
    public bool ShowStatusBar { get; set; }

    /// <summary>
    /// Renders <see cref="WinOldMessageBoxHost"/>, <see cref="WinOldInputBoxHost"/> and <see cref="WinOldDialogHost"/>.
    /// Set to false when they are already declared elsewhere, otherwise dialogs would be shown twice.
    /// </summary>
    [Parameter]
    public bool IncludeHosts { get; set; } = true;

    /// </summary>
    private string GetAppearanceClass()
    {
        return Appearance switch
        {
            Appearance.DOS => "appshell-dos",
            Appearance.Win31 => "appshell-31",
            Appearance.Win98 => "appshell-98",
            Appearance.WinXP => "appshell-xp",
            Appearance.Win7 => "appshell-7",
            _ => "appshell-10"
        };
    }
}
