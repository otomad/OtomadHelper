import type { ContextMenuItemOutput } from "utils/context-menu";

export type ApplicationEvents = {
	// Define events that need to be used globally here.
	[host: `host:${string}` & {}]: [AnyObject];
	"host:dragOver": [WebMessageEvents.DragOver];
	"host:consoleLog": [WebMessageEvents.ConsoleLog];
	"host:contextMenuItemClickEventArgs": [WebMessageEvents.ContextMenuItemClickEventArgs];
	"host:accentPalette": [WebMessageEvents.AccentPalette];
	"dev:showContextMenu": [e: MouseEvent, menu: typeof window["contextMenu"] & {}];
	"app:toast": [string];
};

expectType<Record<string, unknown[]>>(undefined! as ApplicationEvents);

useListen("host:consoleLog", ({ severity, message }) => {
	console[severity]?.(message);
});

useListen("host:contextMenuItemClickEventArgs", ({ menuUuid, menuItemUuid }) => {
	const { contextMenu } = window;
	if (contextMenu == null || contextMenu.uuid !== menuUuid) return;
	const item = findItem(contextMenu.items);
	item?.command?.();

	function findItem(items: ContextMenuItemOutput[]): ContextMenuItemOutput | null {
		for (const item of items) {
			let result: ContextMenuItemOutput | undefined;
			if (item.uuid === menuItemUuid) result = item;
			else if (item.items?.length) {
				const child = findItem(item.items);
				if (child) result = child;
			}
			if (result) return result;
		}
		return null;
	}
});
