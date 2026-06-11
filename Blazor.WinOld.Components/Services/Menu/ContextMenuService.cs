namespace Blazor.WinOld.Components;

public class ContextMenuService : IContextMenuService
{
    private WinOldMenu? _activeMenu;

    public void NotifyOpened(WinOldMenu menu)
    {
        if (_activeMenu is not null && _activeMenu != menu)
            _activeMenu.CloseAll();
        _activeMenu = menu;
    }

    public void Unregister(WinOldMenu menu)
    {
        if (_activeMenu == menu)
            _activeMenu = null;
    }
}
