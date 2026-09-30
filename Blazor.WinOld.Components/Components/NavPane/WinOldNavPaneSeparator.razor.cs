using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldNavPaneSeparator : WinOldComponentBase
{
    [CascadingParameter]
    public WinOldNavPane? Pane { get; set; }

    /// <summary>
    /// Optional section label (e.g. "Favorites"). Hidden in compact mode, where a plain line is drawn.
    /// </summary>
    [Parameter]
    public string? Text { get; set; }

    /// </summary>
    private string GetSeparatorClass()
    {
        var cls = "navpane-separator " + (Pane?.EffectiveAppearance ?? global::Blazor.WinOld.Components.Appearance.Win10) switch
        {
            global::Blazor.WinOld.Components.Appearance.DOS => "navpane-separator-dos",
            global::Blazor.WinOld.Components.Appearance.Win31 => "navpane-separator-31",
            global::Blazor.WinOld.Components.Appearance.Win98 => "navpane-separator-98",
            global::Blazor.WinOld.Components.Appearance.WinXP => "navpane-separator-xp",
            global::Blazor.WinOld.Components.Appearance.Win7 => "navpane-separator-7",
            _ => "navpane-separator-10"
        };
        if (IsTouch) cls += " win-touch";
        return string.IsNullOrEmpty(Class) ? cls : $"{cls} {Class}";
    }
}
