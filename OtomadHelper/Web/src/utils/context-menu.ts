export type ContextMenuItemKind = "command" | "checkBox" | "radio" | "separator" | "submenu";

export type ContextMenuInput = ContextMenuItemInput[];

export type ContextMenuItemInput = {
	/** Icon. */
	// icon?: unknown; // Don't know how to add an icon. Tracking: https://github.com/MicrosoftEdge/WebView2Feedback/issues/4827
	/** Add check mark if the kind is a checkbox. */
	checked?: boolean;
	/** The menu item is enabled? */
	enabled?: boolean;
	/** Occurs when the menu item clicked. */
	onClick?(): void;
	/** Sub menu items if the kind is a submenu. */
	items?: ContextMenuItemInput[];
	/** Add a confirm delete message if this menu item command is to delete something. */
	confirmDeleteMessage?: string;
} & ({
	/** Menu item kind. */
	kind?: ContextMenuItemKind;
	/** Label. @required */
	label: string;
} | {
	/** The menu item is a separator. */
	kind: "separator";
	/** You can ignore the label of the menu item if it is a separator, which wouldn't be shown. @default "" */
	label?: string;
});

export interface ContextMenuOutput {
	/** A Universally Unique IDentifier that represents to the menu. */
	uuid: string;
	/** Child menu items. */
	items: ContextMenuItemOutput[];
}

export interface ContextMenuItemOutput {
	/** Menu item kind. */
	kind: ContextMenuItemKind;
	/** Label. */
	label: string;
	/** A Universally Unique IDentifier that represents to the menu item. */
	uuid: string;
	/** Add check mark if the kind is a checkbox. */
	checked?: boolean;
	/** The menu item is enabled? */
	enabled?: boolean;
	/** Occurs when the menu item clicked. */
	command?(): void;
	/** Sub menu items if the kind is a submenu. */
	items?: ContextMenuItemOutput[];
}

export function createContextMenu(menu: ContextMenuInput): MouseEventHandler<HTMLElement> {
	const getUuid = () => crypto.randomUUID();
	let currentTarget: HTMLElement | null = null;
	const menuOutput: ContextMenuOutput = { uuid: getUuid(), items: convertMenuInputToOutput(menu) ?? [] };
	if (menuOutput.items.length === 0)
		return e => {
			if (isProdMode())
				stopEvent(e);
			window.contextMenu = undefined;
		};

	function convertMenuInputToOutput(menu?: ContextMenuInput) {
		if (menu === undefined) return undefined;
		const items: ContextMenuItemOutput[] = [];
		for (const { kind, label, onClick, items: children, confirmDeleteMessage, ...item } of menu)
			items.push({
				kind: kind ?? "command",
				label: (label ?? "").toString(),
				uuid: getUuid(),
				command: async () => {
					if (!confirmDeleteMessage) onClick?.();
					else await confirmDelete(currentTarget, String(confirmDeleteMessage)) && onClick?.();
				},
				items: convertMenuInputToOutput(children),
				...item,
			});
		return items;
	}

	return e => {
		e.stopPropagation();
		currentTarget = e.currentTarget;
		if (window.isWebView)
			window.contextMenu = menuOutput;
		else {
			e.preventDefault();
			emit("dev:showContextMenu", e, menuOutput);
		}
	};
}

export async function confirmDelete(target: Element | EventTarget | Event | SyntheticEvent | null, message: string) {
	if (target && "currentTarget" in target) target = target.currentTarget;
	if (!(target instanceof Element) || !window.isWebView)
		return confirm(message);
	const rect = target.getBoundingClientRect();
	return await bridges.bridge.showConfirmDeleteFlyout(rect, message);
}
