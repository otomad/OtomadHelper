declare global {
	namespace WebMessageEvents {
		export type DragOver = {
			extension: string;
			contentType: string;
			isDirectory: boolean;
			isDragging: true;
		} | {
			isDragging: false | null;
		};

		export interface ConsoleLog {
			severity: "log" | "error" | "warn";
			message: string;
		}

		export interface ContextMenuItemClickEventArgs {
			menuUuid: string;
			menuItemUuid: string;
		}

		export interface SystemConfig {
			accentPalette?: {
				colorization?: string | null;
				lightAccentColor?: string | null;
				darkAccentColor?: string | null;
			};
			systemCursorConfig: {
				cursorSize: number;
				cursorFill: string;
			};
			panelBackgroundColor?: string | null;
		}

		export interface VegasCommandEvent {
			event: VegasCommandType;
		}

		export interface GridSpanItem {
			sameLine: number;
			crossLine: number;
			sameLineSpan?: number;
			crossLineSpan?: number;
		}

		export interface GridColumnWidthRowHeightItem {
			index: number;
			value: number;
			type: GridUnitType;
		}

		type GridUnitType = "auto" | "pixel" | "star";
	}

	type VegasCommandType = "useTrackEventAsSource" | "useProjectMediaAsSource" | "enableYtp" | "disableYtp" | "startGenerating" | "reset";

	var initialSystemConfig: WebMessageEvents.SystemConfig;
}

export default WebMessageEvents;
