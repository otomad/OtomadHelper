import { changeColorScheme, type ColorScheme } from "helpers/color-mode";

export const useColorMode = createStore<{
	scheme: ColorScheme;
	setScheme: (scheme: ColorScheme) => void;
}>()(persist((set, _get) => ({
	scheme: "auto",
	setScheme: scheme => { set({ scheme }); changeColorScheme(scheme); },
}), { name: "colorMode" }));
