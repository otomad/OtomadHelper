export type ContextMenuItemKind = "command" | "checkBox" | "radio" | "separator" | "submenu";

export type ContextMenuInput = ContextMenuItemInput[];

export interface ContextMenuItemInput {
	kind?: ContextMenuItemKind;
	label: string;
	// icon?: unknown; // Don't know how to add an icon.
	checked?: boolean;
	enabled?: boolean;
	onClick?(): void;
	items?: ContextMenuItemInput[];
	confirmDeleteMessage?: string;
}

export interface ContextMenuOutput {
	uuid: string;
	items: ContextMenuItemOutput[];
}

export interface ContextMenuItemOutput {
	kind: ContextMenuItemKind;
	label: string;
	uuid: string;
	checked?: boolean;
	enabled?: boolean;
	command?(): void;
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
				label: label.toString(),
				uuid: getUuid(),
				command: async () => {
					if (!confirmDeleteMessage) onClick?.();
					else await confirmDelete(currentTarget, confirmDeleteMessage) && onClick?.();
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
			useEvent("dev:showContextMenu", e, menuOutput);
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
