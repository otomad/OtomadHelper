using Microsoft.Web.WebView2.Core;

namespace OtomadHelper.Models;

internal struct ContextMenu {
	public string uuid;
	public ContextMenuItem[] items;
}

internal struct ContextMenuItem {
	public CoreWebView2ContextMenuItemKind kind;
	public string label;
	public string uuid;
	public bool @checked;
	public bool enabled;
	public ContextMenuItem[]? items;
}
