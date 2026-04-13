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

    /// <summary>When true, a close button is rendered in the title bar.</summary>
    [Parameter]
    public bool ShowCloseButton { get; set; } = false;

    /// <summary>Callback invoked when the close button is clicked.</summary>
    [Parameter]
    public EventCallback OnClose { get; set; }

    /// <summary>Content rendered inside the window body.</summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    // Returns the CSS class for the window container based on the selected theme.
    private string GetWindowClass() => Appearance switch
    {
        Appearance.Win7  => "win-window-7",
        Appearance.WinXP => "win-window-xp",
        Appearance.Win98 => "win-window-98",
        Appearance.Win10 => "win-window-10",
        _                => "win-window-10"
    };

    // Returns the CSS class for the title bar based on the selected theme.
    private string GetTitleBarClass() => Appearance switch
    {
        Appearance.Win7  => "win-title-bar-7",
        Appearance.WinXP => "win-title-bar-xp",
        Appearance.Win98 => "win-title-bar-98",
        Appearance.Win10 => "win-title-bar-10",
        _                => "win-title-bar-10"
    };

    // Returns the CSS class for the title bar text based on the selected theme.
    private string GetTitleTextClass() => Appearance switch
    {
        Appearance.Win7  => "win-title-text-7",
        Appearance.WinXP => "win-title-text-xp",
        Appearance.Win98 => "win-title-text-98",
        _                => "win-title-text-98"
    };

    // Returns the CSS class for the title bar controls container based on the selected theme.
    private string GetTitleControlsClass() => Appearance switch
    {
        Appearance.Win7  => "win-title-controls-7",
        Appearance.WinXP => "win-title-controls-xp",
        Appearance.Win98 => "win-title-controls-98",
        Appearance.Win10 => "win-title-controls-10",
        _                => "win-title-controls-10"
    };

    // Returns the CSS class for the window body based on the selected theme.
    private string GetBodyClass() => Appearance switch
    {
        Appearance.Win7  => "win-body-7",
        Appearance.WinXP => "win-body-xp",
        Appearance.Win98 => "win-body-98",
        Appearance.Win10 => "win-body-10",
        _                => "win-body-10"
    };
}
