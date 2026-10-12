namespace Blazor.WinOld.Components;

/// <summary>
/// Position of a <see cref="WinOldWindow"/> after it was dragged, in pixels, relative to its positioned container.
/// </summary>
/// <param name="Left">Distance from the left edge of the container.</param>
/// <param name="Top">Distance from the top edge of the container.</param>
public record WindowMovedEventArgs(double Left, double Top);
