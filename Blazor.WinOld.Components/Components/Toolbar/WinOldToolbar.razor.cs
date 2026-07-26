using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Blazor.WinOld.Components;

public partial class WinOldToolbar : WinOldComponentBase
{
    [Inject] private IJSRuntime JS { get; set; } = default!;

    /// <summary>
    /// Toolbar buttons (typically <see cref="WinOldToolbarButton"/> instances).
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Button.Appearance)]
    public Appearance Appearance { get; set; } = Appearance.Win10;

    /// <summary>
    /// <see cref="ToolbarOverflowMode.Wrap"/> (default) wraps to multiple lines and always shows labels.
    /// <see cref="ToolbarOverflowMode.Collapse"/> stays on a single line: as available width shrinks,
    /// trailing buttons drop their label (icon + tooltip only) one at a time, starting from the
    /// rightmost, based on the toolbar's actual measured width — fully automatic, no threshold to tune.
    /// </summary>
    [Parameter]
    public ToolbarOverflowMode Mode { get; set; } = ToolbarOverflowMode.Wrap;

    /// <summary>
    /// Default <c>Flat</c> value inherited by child <see cref="WinOldToolbarButton"/> instances
    /// that don't set their own — buttons show no background/border until hovered or pressed.
    /// </summary>
    [Parameter]
    public bool Flat { get; set; } = false;

    /// <summary>
    /// Default <c>IconOnly</c> value inherited by child <see cref="WinOldToolbarButton"/> instances
    /// that don't set their own — buttons never show a label (icon + tooltip only) and shrink to
    /// fit the icon, with no minimum width/height.
    /// </summary>
    [Parameter]
    public bool IconOnly { get; set; } = false;

    /// </summary>
    private string GetComponentClass()
    {
        var baseClass = Appearance switch
        {
            Appearance.DOS => "toolbar-dos",
            Appearance.Win98 => "toolbar-win-98",
            Appearance.WinXP => "toolbar-win-xp",
            Appearance.Win7 => "toolbar-win-7",
            Appearance.Win10 => "toolbar-win-10",
            _ => "toolbar-win-10"
        };

        var modeClass = Mode == ToolbarOverflowMode.Collapse ? "toolbar-collapse-win" : "toolbar-wrap-win";
        return $"{baseClass} {modeClass}";
    }

    private ElementReference _rootRef;
    private IJSObjectReference? _module;
    private IJSObjectReference? _collapseHandle;
    private bool _collapseActive;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (Mode == ToolbarOverflowMode.Collapse && !_collapseActive)
        {
            // Set before the await: guards against a second OnAfterRenderAsync firing
            // (and starting a second observer) before the import/init below completes.
            _collapseActive = true;
            _module ??= await JS.InvokeAsync<IJSObjectReference>("import", "./_content/BlazorWinOld/js/toolbar.js");
            _collapseHandle = await _module.InvokeAsync<IJSObjectReference>("initToolbarCollapse", _rootRef);
        }
        else if (Mode != ToolbarOverflowMode.Collapse && _collapseActive)
        {
            _collapseActive = false;
            await DisposeCollapseHandleAsync();
        }
    }

    private async Task DisposeCollapseHandleAsync()
    {
        if (_collapseHandle is not null)
        {
            await _collapseHandle.InvokeVoidAsync("dispose");
            await _collapseHandle.DisposeAsync();
            _collapseHandle = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeCollapseHandleAsync();
        if (_module is not null)
            await _module.DisposeAsync();
    }
}
