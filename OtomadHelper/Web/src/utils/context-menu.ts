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
		for (const { kind, label, onClick, items: children, ...item } of menu)
			items.push({
				kind: kind ?? "command",
				label: label.toString(),
				uuid: getUuid(),
				command: onClick,
				items: convertMenuInputToOutput(children),
				...item,
			});
		return items;
	}

	return e => {
		e.stopPropagation();
		if (window.isWebView)
			window.contextMenu = menuOutput;
		else {
			e.preventDefault();
			useEvent("dev:showContextMenu", [e, menuOutput]);
		}
	};
}
