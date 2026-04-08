using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldMenu : WinOldComponentBase
{
    [CascadingParameter] public WinOldMenu? RootMenu { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// </summary>
    [Parameter]
    public Appearance Appearance { get; set; }

    /// <summary>Contrôlé par le MenuItem parent pour afficher/masquer </summary>
    [Parameter] public bool IsOpen { get; set; }

    public bool IsRoot => RootMenu is null;

    private string CssClass
    {
        get
        {
            var baseClass = RootMenu?.IsRoot == false ? "subsub-win" : "submenu-win";

            var suffix = ActualRoot.Appearance switch
            {
                Appearance.Win7 => "7",
                Appearance.WinXP => "xp",
                Appearance.Win98 => "98",
                Appearance.Win10 => "10",
                _ => "98"
            };

            return $"{baseClass} {baseClass}-{suffix}";
        }
    }

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

    public bool IsAnyOpen => OpenItemId is not null;

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

    public void CloseAll()
    {
        OpenItemId = null;
        OpenSubId = null;
        OnStateChanged?.Invoke();
    }

    public event Action? OnStateChanged;

    public void Dispose() => OnStateChanged = null;

    /// </summary>
    private string GetComponentClass()
    {
        return ActualRoot.Appearance switch
        {
            Appearance.Win7 => "menu-win-7",
            Appearance.WinXP => "menu-win-xp",
            Appearance.Win98 => "menu-win-98",
            Appearance.Win10 => "menu-win-10",
            _ => "menu-win-98"
        };
    }
}
