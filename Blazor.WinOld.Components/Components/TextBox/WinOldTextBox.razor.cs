using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.WinOld.Components;

public partial class WinOldTextBox : WinOldComponentBase, IAsyncDisposable
{
    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    /// <summary>
    /// Reference to the input element
    /// </summary>
    private ElementReference InputElement { get; set; }

    private IJSObjectReference? _module;

    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; }

    /// </summary>
    [Parameter]
    public string Label { get; set; } = string.Empty;

    /// </summary>
    [Parameter]
    public string? Value { get; set; } = string.Empty;

    /// <summary>
    /// Optional in-line styles applied to the input element specifically.
    /// </summary>
    [Parameter]
    public string? InputStyle { get; set; } = null;

    /// </summary>
    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    /// </summary>
    private Guid ElementId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Sets focus on the input element
    /// </summary>
    public async Task FocusAsync()
    {
        await InputElement.FocusAsync();
    }

    /// <summary>
    /// Sets focus on the input and selects its entire content, so typing immediately replaces it.
    /// </summary>
    public async Task SelectAllAsync()
    {
        _module ??= await JS.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorWinOld/js/textbox.js");
        await _module.InvokeVoidAsync("selectAll", InputElement);
    }

    public async ValueTask DisposeAsync()
    {
        if (_module is not null)
            await _module.DisposeAsync();
    }

    private async Task OnValueChanged(ChangeEventArgs e)
    {
        if (!ValueChanged.HasDelegate) return;

        Value = e.Value?.ToString() ?? string.Empty;
        await ValueChanged.InvokeAsync(Value);
    }

    /// </summary>
    private string GetComponentClass()
    {
        var cls = Appearance switch
        {
            Appearance.Win98 => "txt-win-98",
            Appearance.WinXP => "txt-win-xp",
            Appearance.Win7 => "txt-win-7",
            Appearance.Win10 => "txt-win-10",
            _ => "txt-win-10"
        };
        if (IsTouch) cls += " win-touch";
        return cls;
    }

    /// </summary>
    private string GetComponentLabelClass()
    {
        if (string.IsNullOrEmpty(Label))
            return "label-txt-hide";

        return Appearance switch
        {
            Appearance.Win98 => "lbtxt-win-98",
            Appearance.WinXP => "lbtxt-win-xp",
            Appearance.Win7 => "lbtxt-win-7",
            Appearance.Win10 => "lbtxt-win-10",
            _ => "lbtxt-win-10"
        };
    }
}
