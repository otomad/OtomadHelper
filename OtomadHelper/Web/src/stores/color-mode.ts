import type { ColorScheme } from "helpers/color-mode";

interface IColorMode {
	scheme: ColorScheme;
	setScheme: (scheme: ColorScheme) => void;
}

export const useColorModeStore = createStore<IColorMode>()(
	persist(set => ({
		scheme: "auto",
		setScheme: scheme => set({ scheme }),
	}), { name: "colorMode" }),
);
