using Microsoft.Web.WebView2.Core;

namespace OtomadHelper.Models;

internal struct ContextMenu() {
	public string uuid = "";
	public ContextMenuItem[] items = [];
}

internal struct ContextMenuItem() {
	public CoreWebView2ContextMenuItemKind kind = CoreWebView2ContextMenuItemKind.Command;
	public string label = "";
	public string uuid = "";
	public bool @checked = false;
	public bool enabled = true;
	public ContextMenuItem[]? items = null;
}
