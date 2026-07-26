using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.WinOld.Components;

public partial class WinOldToolbarButton : WinOldComponentBase
{
    [CascadingParameter]
    public WinOldToolbar? RootToolbar { get; set; }

    /// <summary>
    /// Visible button text. Hidden (icon + tooltip only) once the ancestor <see cref="WinOldToolbar"/>
    /// collapses. Also used to auto-generate the native <c>title</c>/<c>aria-label</c> — pass an explicit
    /// <c>title</c>/<c>aria-label</c> attribute on this component to override that.
    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// CSS class(es) for an icon from any icon library (e.g. "bi bi-folder", "fa-solid fa-folder").
    /// Ignored when <see cref="IconTemplate"/> is set.
    /// </summary>
    [Parameter]
    public string? IconCssClass { get; set; }

    /// <summary>
    /// Custom icon content (SVG, img, Blazor component…).
    /// Takes priority over <see cref="IconCssClass"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? IconTemplate { get; set; }

    /// <summary>
    /// Command executed when the user clicks on the button.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Visual style. When unset, falls back to the ancestor <see cref="WinOldToolbar"/>'s
    /// <c>Appearance</c>, then to <see cref="Blazor.WinOld.Components.Appearance.Win10"/>.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance? Appearance { get; set; }

    /// </summary>
    [Parameter]
    public bool Default { get; set; }

    /// <summary>
    /// Shows no background/border until hovered or pressed (classic IE/Office toolbar look).
    /// When unset, falls back to the ancestor <see cref="WinOldToolbar"/>'s <c>Flat</c> value.
    /// </summary>
    [Parameter]
    public bool? Flat { get; set; }

    /// <summary>
    /// Never shows a label (icon + tooltip only, regardless of the ancestor's <c>CollapseWidth</c>)
    /// and drops the button's minimum width/height so it shrinks to fit the icon. When unset, falls
    /// back to the ancestor <see cref="WinOldToolbar"/>'s <c>IconOnly</c> value.
    /// </summary>
    [Parameter]
    public bool? IconOnly { get; set; }

    private Appearance EffectiveAppearance => Appearance ?? RootToolbar?.Appearance ?? global::Blazor.WinOld.Components.Appearance.Win10;

    private bool EffectiveFlat => Flat ?? RootToolbar?.Flat ?? false;

    private bool EffectiveIconOnly => IconOnly ?? RootToolbar?.IconOnly ?? false;

    private string GetButtonClass()
    {
        var cls = "toolbar-btn-win";
        if (EffectiveFlat) cls += " toolbar-btn-flat-win";
        if (EffectiveIconOnly) cls += " toolbar-btn-icon-only-win";
        return string.IsNullOrEmpty(Class) ? cls : $"{cls} {Class}";
    }
}
