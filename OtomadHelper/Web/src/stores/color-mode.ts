import type { ColorScheme } from "helpers/color-mode";

export const useColorModeStore = createStore<{
	scheme: ColorScheme;
	setScheme: (scheme: ColorScheme) => void;
}>()(persist(set => ({
	scheme: "auto",
	setScheme: scheme => set({ scheme }),
}), { name: "colorMode" }));
