namespace OtomadHelper.Models;

internal class ContextMenuItemClickEventArgs(string menuUuid, string menuItemUuid) : BaseWebMessageEvent {
	public string MenuUuid => menuUuid;
	public string MenuItemUuid => menuItemUuid;
}
