using Microsoft.AspNetCore.Components;

namespace Blazor.WinOld.Components;

public partial class WinOldMenuItem : WinOldComponentBase
{
    [CascadingParameter] public WinOldMenu? RootMenu { get; set; }

    [Parameter] public string Label { get; set; } = string.Empty;
    [Parameter] public string? Shortcut { get; set; }
    [Parameter] public bool IsSeparator { get; set; }
    [Parameter] public EventCallback OnClick { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    private readonly string _id = Guid.NewGuid().ToString();

    protected override void OnInitialized()
    {
        if (RootMenu is not null)
            RootMenu.OnStateChanged += StateHasChanged;
    }

    private bool _isRootLevel => RootMenu?.IsRoot == true;

    // Niveau racine  → piloté par OpenItemId
    // Niveau submenu → piloté par OpenSubId (pour ne pas écraser le parent)
    private bool _isOpen => _isRootLevel
        ? RootMenu?.OpenItemId == _id
        : RootMenu?.OpenSubId == _id;

    private async Task OnLabelClick()
    {
        if (Disabled) return;

        if (ChildContent is not null)
        {
            if (_isOpen)
                RootMenu?.CloseAll();
            else
                RootMenu?.Open(_id, isSub: !_isRootLevel);
        }
        else
        {
            RootMenu?.CloseAll();
            if (OnClick.HasDelegate)
                await OnClick.InvokeAsync();
        }
    }

    private void OnMouseEnter()
    {
        if (RootMenu is null || Disabled) return;

        if (_isRootLevel)
        {
            if (RootMenu.IsContextMenu)
            {
                // Menu contextuel : ouvrir le sous-menu au survol sans condition
                if (ChildContent is not null)
                    RootMenu.Open(_id, isSub: false);
                else
                    RootMenu.CloseSubMenusOnly();
            }
            else
            {
                // Barre de menu classique : glissement horizontal uniquement si un autre est ouvert
                if (ChildContent is not null && RootMenu.IsAnyOpen)
                    RootMenu.Open(_id, isSub: false);
            }
        }
        else
        {
            if (ChildContent is not null)
                // Item avec sous-sous-menu : l'ouvrir
                RootMenu.Open(_id, isSub: true);
            else
                // Item feuille : juste fermer l'éventuel sous-sous-menu sans toucher au parent
                RootMenu.CloseSubOnly();
        }
    }

    public void Dispose()
    {
        if (RootMenu is not null)
            RootMenu.OnStateChanged -= StateHasChanged;
    }

    /// </summary>
    private string GetSeparatorClass()
    {
        return RootMenu?.ActualRoot.Appearance switch
        {
            Appearance.Win7  => "menu-separator-win-7",
            Appearance.WinXP => "menu-separator-win-xp",
            Appearance.Win98 => "menu-separator-win-98",
            Appearance.Win10 => "menu-separator-win-10",
            _ => "menu-separator-win-10"
        };
    }

    /// </summary>
    private string GetLabelClass()
    {
        return RootMenu?.ActualRoot.Appearance switch
        {
            Appearance.Win7  => "menu-label-win-7",
            Appearance.WinXP => "menu-label-win-xp",
            Appearance.Win98 => "menu-label-win-98",
            Appearance.Win10 => "menu-label-win-10",
            _ => "menu-label-win-10"
        };
    }

    /// </summary>
    private string GetArrowClass()
    {
        return RootMenu?.ActualRoot.Appearance switch
        {
            Appearance.Win7  => "menu-arrow-win-7",
            Appearance.WinXP => "menu-arrow-win-xp",
            Appearance.Win98 => "menu-arrow-win-98",
            Appearance.Win10 => "menu-arrow-win-10",
            _ => "menu-arrow-win-10"
        };
    }
}
