import type { ContextMenuItemOutput } from "utils/context-menu";

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
