using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldInfoBar : WinOldComponentBase
{
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; } = Appearance.Win10;

    /// <summary>
    /// Icon and severity of the bar (background color on Win10, color on DOS, ARIA role).
    /// <see cref="Icon.None"/> shows no icon and uses the neutral style.
    /// </summary>
    [Parameter]
    public Icon Icon { get; set; } = Icon.Information;

    /// <summary>
    /// Bold text displayed before the message.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// Main text of the bar.
    /// </summary>
    [Parameter]
    public string? Message { get; set; }

    /// <summary>
    /// Free content displayed under the message (e.g. a progress bar).
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Action buttons displayed after the text.
    /// </summary>
    [Parameter]
    public RenderFragment? Actions { get; set; }

    /// <summary>
    /// Shows the close button.
    /// </summary>
    [Parameter]
    public bool Closable { get; set; } = true;

    /// <summary>
    /// Whether the bar is displayed. Set to false by the close button. Supports @bind-Visible.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// </summary>
    [Parameter]
    public EventCallback<bool> VisibleChanged { get; set; }

    /// <summary>
    /// Raised when the user clicks on the close button.
    /// </summary>
    [Parameter]
    public EventCallback OnClose { get; set; }

    /// <summary>
    /// Color of the bar when is DOS. When null, derived from <see cref="Icon"/>. Ignored for other appearances.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public DosColor? DosColor { get; set; }

    private async Task CloseAsync()
    {
        Visible = false;
        await VisibleChanged.InvokeAsync(false);
        await OnClose.InvokeAsync();
    }

    /// </summary>
    private string GetComponentClass()
    {
        var cls = Appearance switch
        {
            Appearance.DOS => $"infobar-win-dos infobar-dos-{GetDosColor().ToString().ToLowerInvariant()}",
            Appearance.Win31 => "infobar-win-31",
            Appearance.Win98 => "infobar-win-98",
            Appearance.WinXP => "infobar-win-xp",
            Appearance.Win7 => "infobar-win-7",
            Appearance.Win10 => "infobar-win-10",
            _ => "infobar-win-10"
        };

        cls += Icon switch
        {
            Icon.Alert => " infobar-alert",
            Icon.Critical => " infobar-critical",
            Icon.None => " infobar-neutral",
            _ => " infobar-info"
        };

        if (IsTouch) cls += " win-touch";
        return cls;
    }

    private string GetRole()
        => Icon is Icon.Alert or Icon.Critical ? "alert" : "status";

    /// <summary>Win31 icons are 32px pixel art and would blur at 16px.</summary>
    private int GetIconSize()
        => Appearance == Appearance.Win31 ? 32 : 16;

    /// <summary>Same mapping as the DOS MessageBox.</summary>
    private DosColor GetDosColor()
    {
        return DosColor ?? Icon switch
        {
            Icon.Information => Components.DosColor.Green,
            Icon.Question => Components.DosColor.Cyan,
            Icon.Alert => Components.DosColor.Brown,
            Icon.Critical => Components.DosColor.Red,
            _ => Components.DosColor.Default
        };
    }
}
