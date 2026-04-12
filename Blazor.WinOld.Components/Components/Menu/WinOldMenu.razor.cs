using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.WinOld.Components;

public partial class WinOldMenu : WinOldComponentBase
{
    [CascadingParameter] 
    public WinOldMenu? RootMenu { get; set; }
    [Parameter] 
    public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    public Appearance Appearance { get; set; }

    /// <summary>Contrôlé par le MenuItem parent pour afficher/masquer </summary>
    [Parameter] 
    public bool IsOpen { get; set; }

    /// <summary>Rend ce menu utilisable comme menu contextuel (clic droit).</summary>
    [Parameter] 
    public bool IsContextMenu { get; set; }

    public bool IsRoot => RootMenu is null;

    // Context Menu
    private double _contextX;
    private double _contextY;
    private bool _isContextMenuVisible;

    private string CssClass
    {
        get
        {
            var baseClass = (RootMenu?.IsRoot == false || ActualRoot.IsContextMenu) ? "subsub-win" : "submenu-win";

            var suffix = ActualRoot.Appearance switch
            {
                Appearance.Win7  => "7",
                Appearance.WinXP => "xp",
                Appearance.Win98 => "98",
                Appearance.Win10 => "10",
                _ => "10"
            };

            return $"{baseClass} {baseClass}-{suffix}";
        }
    }

    private string ContextMenuAppearanceClass => Appearance switch
    {
        Appearance.Win7  => "submenu-win-7",
        Appearance.WinXP => "submenu-win-xp",
        Appearance.Win98 => "submenu-win-98",
        Appearance.Win10 => "submenu-win-10",
        _ => "submenu-win-10"
    };

    // ── État : deux niveaux distincts ─────────────────────────────────────
    // _openItemId  : id du MenuItem racine ouvert (ex: "Fichier")
    // _openSubId   : id du MenuItem sous-menu ouvert (ex: "Disposition")
    private string? _openItemId;
    private string? _openSubId;

    public WinOldMenu ActualRoot => IsRoot ? this : RootMenu!.ActualRoot;

    protected override void OnInitialized()
    {
        if (RootMenu is not null)
            RootMenu.OnStateChanged += PropagateStateChanged;
    }

    private void PropagateStateChanged()
    {
        StateHasChanged();
        OnStateChanged?.Invoke();
    }

    public string? OpenItemId
    {
        get => IsRoot ? _openItemId : RootMenu!.OpenItemId;
        set { if (IsRoot) _openItemId = value; else RootMenu!.OpenItemId = value; }
    }

    public string? OpenSubId
    {
        get => IsRoot ? _openSubId : RootMenu!.OpenSubId;
        set { if (IsRoot) _openSubId = value; else RootMenu!.OpenSubId = value; }
    }

    public bool IsAnyOpen => OpenItemId is not null || (IsRoot && IsContextMenu && _isContextMenuVisible);

    /// <summary>Ouvre un item (racine ou sous-menu avec enfants).</summary>
    public void Open(string id, bool isSub = false)
    {
        if (isSub)
        {
            OpenSubId = id;
        }
        else
        {
            OpenItemId = id;
            OpenSubId = null; // ferme tout sous-sous-menu ouvert
        }
        StateHasChanged();
        OnStateChanged?.Invoke();
    }

    /// <summary>Ferme uniquement le sous-sous-menu ouvert, sans toucher à l'item parent.</summary>
    public void CloseSubOnly()
    {
        if (OpenSubId is null) return;
        OpenSubId = null;
        StateHasChanged();
        OnStateChanged?.Invoke();
    }

    /// <summary>Ferme les sous-menus ouverts sans fermer le menu contextuel lui-même.</summary>
    public void CloseSubMenusOnly()
    {
        if (OpenItemId is null && OpenSubId is null) return;
        OpenItemId = null;
        OpenSubId = null;
        StateHasChanged();
        OnStateChanged?.Invoke();
    }

    public void CloseAll()
    {
        OpenItemId = null;
        OpenSubId = null;
        if (ActualRoot.IsContextMenu)
            ActualRoot._isContextMenuVisible = false;
        StateHasChanged();
        OnStateChanged?.Invoke();
    }

    public event Action? OnStateChanged;

    /// </summary>
    private string GetComponentClass()
    {
        return ActualRoot.Appearance switch
        {
            Appearance.Win7  => "menu-win-7",
            Appearance.WinXP => "menu-win-xp",
            Appearance.Win98 => "menu-win-98",
            Appearance.Win10 => "menu-win-10",
            _ => "menu-win-10"
        };
    }
    // ── Long Press (touch) ────────────────────────────────────────────────
    private CancellationTokenSource? _longPressCts;
    private const int LongPressDelayMs = 500;

    /// <summary>Démarre la détection d'appui long pour les appareils tactiles.</summary>
    public async Task StartLongPress(TouchEventArgs e)
    {
        CancelLongPress();
        if (e.Touches.Length == 0) return;

        var x = e.Touches[0].ClientX;
        var y = e.Touches[0].ClientY;

        _longPressCts = new CancellationTokenSource();
        var token = _longPressCts.Token;

        try
        {
            await Task.Delay(LongPressDelayMs, token);
            await InvokeAsync(() => ShowContextMenuAt(x, y));
        }
        catch (TaskCanceledException) { }
    }

    /// <summary>Annule le long press en cours (touchend / touchmove).</summary>
    public void CancelLongPress()
    {
        _longPressCts?.Cancel();
        _longPressCts?.Dispose();
        _longPressCts = null;
    }

    private void ShowContextMenuAt(double x, double y)
    {
        _contextX = x;
        _contextY = y;
        _isContextMenuVisible = true;
        OpenItemId = null;
        OpenSubId = null;
        StateHasChanged();
    }

    /// <summary>Affiche le menu contextuel à la position du pointeur souris.</summary>
    public void ShowContextMenu(MouseEventArgs e) => ShowContextMenuAt(e.ClientX, e.ClientY);
    public void Dispose()
    {
        OnStateChanged = null;
        CancelLongPress();
    }
}
