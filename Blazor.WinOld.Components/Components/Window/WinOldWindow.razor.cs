using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldWindow : WinOldComponentBase
{
    /// <summary>Visual theme applied to the window chrome.</summary>
    [Parameter]
    public Appearance Appearance { get; set; } = Appearance.WinXP;

    /// <summary>Text displayed in the title bar.</summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>When true, a close button is rendered in the title bar</summary>
    [Parameter]
    public bool ShowCloseButton { get; set; } = false;

    /// <summary>Callback invoked when the close button is clicked.</summary>
    [Parameter]
    public EventCallback OnClose { get; set; }

    /// <summary>When true, a maximize button is rendered in the title bar.</summary>
    [Parameter]
    public bool MaxButton { get; set; } = false;

    private bool _isMaximized;

    private void ToggleMaximize() => _isMaximized = !_isMaximized;

    // Returns the modifier CSS class applied to the window container when maximized.
    private string GetMaximizedClass() => _isMaximized ? "win-window-maximized" : string.Empty;

    /// <summary>Content rendered inside the window body.</summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Status bar rendered under the window body, flush against the bottom edge of the window
    /// (typically a <see cref="WinOldStatusBar"/>, which inherits the window's Appearance).
    /// </summary>
    [Parameter]
    public RenderFragment? StatusBar { get; set; }

    // Returns the CSS class for the status bar slot based on the selected theme.
    private string GetStatusBarSlotClass() => Appearance switch
    {
        Appearance.DOS   => "win-statusbar-dos",
        Appearance.Win31 => "win-statusbar-31",
        Appearance.Win98 => "win-statusbar-98",
        Appearance.WinXP => "win-statusbar-xp",
        Appearance.Win7  => "win-statusbar-7",
        Appearance.Win10 => "win-statusbar-10",
        _                => "win-statusbar-10"
    };

    // Returns the CSS class for the window container based on the selected theme.
    private string GetWindowClass() => Appearance switch
    {
        Appearance.DOS   => "win-window-dos",
        Appearance.Win31 => "win-window-31",
        Appearance.Win98 => "win-window-98",
        Appearance.WinXP => "win-window-xp",
        Appearance.Win7  => "win-window-7",
        Appearance.Win10 => "win-window-10",
        _                => "win-window-10"
    };

    // Returns the CSS class for the title bar based on the selected theme.
    private string GetTitleBarClass() => Appearance switch
    {
        Appearance.DOS   => "title-bar-dos",
        Appearance.Win31 => "title-bar-win-31",
        Appearance.Win98 => "win-title-bar-98",
        Appearance.WinXP => "win-title-bar-xp",
        Appearance.Win7  => "win-title-bar-7",
        Appearance.Win10 => "win-title-bar-10",
        _                => "win-title-bar-10"
    };

    // Returns the CSS class for the title bar text based on the selected theme.
    private string GetTitleTextClass() => Appearance switch
    {
        Appearance.DOS   => "title-bar-text-dos",
        Appearance.Win31 => "title-bar-text-win-31",
        Appearance.Win98 => "win-title-text-98",
        Appearance.WinXP => "win-title-text-xp",
        Appearance.Win7  => "win-title-text-7",
        _                => "win-title-text-98"
    };

    // Returns the CSS class for the title bar controls container based on the selected theme.
    private string GetTitleControlsClass() => Appearance switch
    {
        Appearance.DOS   => "title-bar-controls-dos",
        Appearance.Win31 => "title-bar-controls-win-31",
        Appearance.Win98 => "win-title-controls-98",
        Appearance.WinXP => "win-title-controls-xp",
        Appearance.Win7  => "win-title-controls-7",
        Appearance.Win10 => "win-title-controls-10",
        _                => "win-title-controls-10"
    };

    // Returns the CSS class for the window body based on the selected theme.
    private string GetBodyClass() => Appearance switch
    {
        Appearance.DOS   => "win-body-dos",
        Appearance.Win31 => "win-body-31",
        Appearance.Win98 => "win-body-98",
        Appearance.WinXP => "win-body-xp",
        Appearance.Win7  => "win-body-7",
        Appearance.Win10 => "win-body-10",
        _                => "win-body-10"
    };
}
