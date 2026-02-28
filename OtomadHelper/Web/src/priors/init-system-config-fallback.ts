globalThis.globals = globalThis;

if (typeof initialSystemConfig === "undefined")
	initialSystemConfig = {
		systemCursorConfig: {
			cursorSize: 32,
			cursorFill: "white",
		},
	};
