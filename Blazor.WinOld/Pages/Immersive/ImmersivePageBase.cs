using Blazor.WinOld.Components;
using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Pages.Immersive;

/// <summary>
/// Screens of the full-screen demo: they follow the Appearance chosen in the toolbar of the shell.
/// </summary>
public abstract class ImmersivePageBase : ComponentBase
{
    [CascadingParameter(Name = "WindowAppearance")]
    public Appearance? ShellAppearance { get; set; }

    /// <summary>
    /// Appearance to pass to every component of the screen.
    /// </summary>
    protected Appearance A => ShellAppearance ?? Appearance.Win98;
}
