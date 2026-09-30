using Microsoft.JSInterop;

namespace Blazor.WinOld.Components;

public static class ScrollbarJSRuntimeExtensions
{
    /// <summary>
    /// Applies the scrollbar style of an appearance to the whole page (the &lt;html&gt; element),
    /// or to the elements matching <paramref name="selector"/>. Pass <c>null</c> as appearance to restore the native scrollbar.
    /// </summary>
    public static async ValueTask SetScrollbarAppearanceAsync(this IJSRuntime js, Appearance? appearance, string? selector = null)
    {
        await using var module = await js.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorWinOld/js/scrollbar.js");
        await module.InvokeVoidAsync("setScrollbarClass", selector, appearance?.ToScrollbarClass());
    }
}
