using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldTitleBar : ComponentBase
{
    /// <summary>CSS class applied to the title bar container (computed by the host, per its own theme).</summary>
    [Parameter, EditorRequired]
    public string BarClass { get; set; } = string.Empty;

    /// <summary>CSS class applied to the title text.</summary>
    [Parameter, EditorRequired]
    public string TextClass { get; set; } = string.Empty;

    /// <summary>CSS class applied to the controls (buttons) container.</summary>
    [Parameter, EditorRequired]
    public string ControlsClass { get; set; } = string.Empty;

    /// <summary>Text displayed in the title bar.</summary>
    [Parameter]
    public string Title { get; set; } = string.Empty;

    /// <summary>Optional id on the title text, for aria-labelledby on the host's dialog container.</summary>
    [Parameter]
    public string? TitleId { get; set; }

    /// <summary>When true, a Close button is rendered.</summary>
    [Parameter]
    public bool ShowCloseButton { get; set; } = true;

    /// <summary>Callback invoked when the Close button is clicked.</summary>
    [Parameter]
    public EventCallback OnClose { get; set; }

    /// <summary>When true, a Maximize button is rendered and double-clicking the title bar toggles it too.</summary>
    [Parameter]
    public bool ShowMaximizeButton { get; set; }

    /// <summary>Callback invoked when the Maximize button is clicked, or the title bar is double-clicked.</summary>
    [Parameter]
    public EventCallback OnToggleMaximize { get; set; }

    /// <summary>The title bar's root element, for hosts that need it (e.g. to wire up DraggableWindow).</summary>
    public ElementReference Element { get; private set; }

    private Task HandleDoubleClickAsync()
        => ShowMaximizeButton ? OnToggleMaximize.InvokeAsync() : Task.CompletedTask;
}
