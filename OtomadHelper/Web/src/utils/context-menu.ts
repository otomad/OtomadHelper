export type MenuItemKind = "command" | "checkBox" | "radio" | "separator" | "submenu";

export type MenuInput = MenuItemInput[];

export interface MenuItemInput {
	kind?: MenuItemKind;
	label: string;
	// icon?: unknown; // Don't know how to add an icon.
	checked?: boolean;
	enabled?: boolean;
	onClick?(): void;
	items?: MenuItemInput[];
}

export interface MenuOutput {
	uuid: string;
	items: MenuItemOutput[];
}

export interface MenuItemOutput {
	kind: MenuItemKind;
	label: string;
	uuid: string;
	checked?: boolean;
	enabled?: boolean;
	command?(): void;
	items?: MenuItemOutput[];
}

export function createContextMenu(menu: MenuInput): MouseEventHandler<HTMLElement> {
	const getUuid = () => crypto.randomUUID();
	const menuOutput: MenuOutput = { uuid: getUuid(), items: convertMenuInputToOutput(menu) ?? [] };
	if (menuOutput.items.length === 0)
		return e => {
			if (isProdMode())
				stopEvent(e);
			window.contextMenu = undefined;
		};

	function convertMenuInputToOutput(menu?: MenuInput) {
		if (menu === undefined) return undefined;
		const items: MenuItemOutput[] = [];
		for (const { kind, onClick, items: children, ...item } of menu)
			items.push({
				kind: kind ?? "command",
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
