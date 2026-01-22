export type ApplicationEvents = {
	// Define events that need to be used globally here.
	[host: `host:${string}` & {}]: [AnyObject];
	"host:dragOver": [WebMessageEvents.DragOver];
	"host:consoleLog": [WebMessageEvents.ConsoleLog];
	"host:contextMenuItemClickEventArgs": [WebMessageEvents.ContextMenuItemClickEventArgs];
	"host:systemConfig": [WebMessageEvents.SystemConfig];
	"host:vegasCommandEvent": [WebMessageEvents.VegasCommandEvent];
	"dev:showContextMenu": [e: MouseEvent, menu: typeof window["contextMenu"] & {}];
	"app:toast": [message: string, status?: Status];
	"app:hideOtherFlyouts": [exceptId?: string];
	"app:startColorPaletteViewTransition": [changeFunc: () => MaybePromise<void>];
	"app:evaluateContrastPalette": [];
};

expectType<Record<string, unknown[]>>(undefined! as ApplicationEvents);
