useListen.on("host:consoleLog", ({ severity, message }) => {
	console[severity]?.(message);
});

useListen.on("host:contextMenuItemClickEventArgs", ({ menuUuid, menuItemUuid }) => {
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

useListen.on("host:vegasCommandEvent", ({ event }) => {
	if (event === "enableYtp" && !configStore.ytp.enabled) event = "disableYtp";
	emit("app:toast", i18n.t(`csharp:keybindings.commands.${event}`));
});
