import type { ColorScheme } from "helpers/color-mode";

interface IColorMode {
	scheme: ColorScheme;
	setScheme: (scheme: ColorScheme) => void;
	backgroundColor: string;
}

export const useColorModeStore = createStore<IColorMode>()(
	subscribeWithSelector(
		persist(set => ({
			scheme: "auto",
			setScheme: scheme => set({ scheme }),
			backgroundColor: "#202020",
		}), { name: "colorMode" }),
	),
);
