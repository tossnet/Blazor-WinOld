using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

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

    /// <summary>
    /// When false, the window is drawn inactive. DOS: no Close/Maximize buttons, single gray line and gray title.
    /// Win7 and Win10: inactive title bar colors.
    /// </summary>
    [Parameter]
    public bool Active { get; set; } = true;

    /// <summary>DOS only: background color of the window (gray when not set).</summary>
    [Parameter]
    public DosColor? DosColor { get; set; }

    /// <summary>Normal or maximized. Supports two-way binding (<c>@bind-WindowState</c>).</summary>
    [Parameter]
    public WindowState WindowState { get; set; } = WindowState.Normal;

    /// <summary>Callback invoked when the user toggles the maximized state.</summary>
    [Parameter]
    public EventCallback<WindowState> WindowStateChanged { get; set; }

    /// <summary>
    /// When true, the window can be moved by dragging its title bar, inside its positioned container
    /// (the window becomes <c>position: absolute</c> if it is not already). A maximized window can't be dragged.
    /// </summary>
    [Parameter]
    public bool Draggable { get; set; }

    /// <summary>
    /// Callback invoked when a drag ends, with the new position. Store it and render it back in <see cref="WinOldComponentBase.Style"/>
    /// (<c>left</c> / <c>top</c>), otherwise the next change of Style puts the window back where it was.
    /// </summary>
    [Parameter]
    public EventCallback<WindowMovedEventArgs> OnMoved { get; set; }

    private ElementReference _windowRef;
    private WinOldTitleBar _titleBar = default!;
    private DraggableWindow _draggable = default!;
    private DotNetObjectReference<WinOldWindow>? _dotNetRef;
    private bool _dragInitialized;

    private bool _isMaximized;
    private WindowState? _lastWindowState;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (Draggable && !_dragInitialized)
        {
            _dragInitialized = true;
            _dotNetRef ??= DotNetObjectReference.Create(this);
            await _draggable.InitAsync(_windowRef, _titleBar.Element, withinParent: true, _dotNetRef);
        }
        else if (!Draggable && _dragInitialized)
        {
            _dragInitialized = false;
            await _draggable.ResetAsync();
        }
    }

    /// <summary>Called by draggable.js when the user releases the title bar.</summary>
    [JSInvokable]
    public Task OnDragEnd(double left, double top) => OnMoved.InvokeAsync(new WindowMovedEventArgs(left, top));

    public ValueTask DisposeAsync()
    {
        // The DraggableWindow child disposes its own JS instance.
        _dotNetRef?.Dispose();
        return ValueTask.CompletedTask;
    }

    protected override void OnParametersSet()
    {
        // Only follow the parameter when it changes, so a parent that doesn't bind it keeps the user's toggle.
        if (_lastWindowState != WindowState)
        {
            _lastWindowState = WindowState;
            _isMaximized = WindowState == WindowState.Maximized;
        }
    }

    private async Task ToggleMaximize()
    {
        _isMaximized = !_isMaximized;
        await WindowStateChanged.InvokeAsync(_isMaximized ? WindowState.Maximized : WindowState.Normal);
    }

    // Returns the modifier CSS class applied to the window container when inactive.
    private string GetInactiveClass() => Active ? string.Empty : "win-window-inactive";

    // DOS only: returns the color modifier class of the window container.
    private string GetDosColorClass() => Appearance == Appearance.DOS && DosColor is { } color && color != Components.DosColor.Default
        ? $"win-window-dos-{color.ToString().ToLowerInvariant()}"
        : string.Empty;

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
        Appearance.Win10 => "win-title-text-10",
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
