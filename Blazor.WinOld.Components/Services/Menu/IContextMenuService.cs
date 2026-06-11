namespace Blazor.WinOld.Components;

public interface IContextMenuService
{
    void NotifyOpened(WinOldMenu menu);
    void Unregister(WinOldMenu menu);
}
