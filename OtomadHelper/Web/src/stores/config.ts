import { immer } from "zustand/middleware/immer";

interface IConfig {
	visual: {
		enabled: boolean;
		setEnabled: (v: boolean) => void;
	};
}

export const useConfig = createStore<IConfig>()(
	immer(set => ({
		visual: {
			enabled: true,
			setEnabled: v => set(state => void (state.visual.enabled = v)),
		},
	})),
);
