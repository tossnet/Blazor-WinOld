using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldProgressBar : WinOldComponentBase
{
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; } = Appearance.Win10;

    /// <summary>
    /// Continuous (default) draws a solid bar; Blocks draws separated segments.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public ProgressBarStyle BarStyle { get; set; } = ProgressBarStyle.Continuous;

    /// <summary>
    /// Current progress, between 0 and <see cref="Max"/>.
    /// </summary>
    [Parameter]
    public double Value { get; set; }

    /// <summary>
    /// Maximum value (default 100).
    /// </summary>
    [Parameter]
    public double Max { get; set; } = 100;

    /// <summary>
    /// When true, the progress is unknown: <see cref="Value"/> is ignored and an animation is shown.
    /// </summary>
    [Parameter]
    public bool Indeterminate { get; set; }

    // A <progress> without a value attribute is indeterminate
    private string? ValueAttribute => Indeterminate ? null : Value.ToString(CultureInfo.InvariantCulture);

    private string MaxAttribute => Max.ToString(CultureInfo.InvariantCulture);

    /// </summary>
    private string GetComponentClass()
    {
        var cls = Appearance switch
        {
            Appearance.DOS => "prgbar-dos",
            Appearance.Win31 => "prgbar-win-31",
            Appearance.Win98 => "prgbar-win-98",
            Appearance.WinXP => "prgbar-win-xp",
            Appearance.Win7 => "prgbar-win-7",
            Appearance.Win10 => "prgbar-win-10",
            _ => "prgbar-win-10"
        };
        if (BarStyle == ProgressBarStyle.Blocks)
            cls += " prgbar-blocks";
        return cls;
    }
}