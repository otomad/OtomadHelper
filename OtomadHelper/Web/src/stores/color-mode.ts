import type { ColorScheme } from "helpers/color-mode";

export const colorModeStore = createPersistStore("colorMode", {
	scheme: "auto" as ColorScheme,
	backgroundColor: "#202020",
});
