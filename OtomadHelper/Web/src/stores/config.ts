import type { legatos, stretches } from "views/visual";
import { immer } from "zustand/middleware/immer";

type Stretch = typeof stretches[number];
type Legato = typeof legatos[number];

interface IConfig {
	visual: {
		enabled: boolean;
		stretch: Stretch;
		loop: boolean;
		staticVisual: boolean;
		noLengthening: boolean;
		legato: Legato;
		multitrackForChords: boolean;
		glissando: boolean;
		mappingVelocity: boolean;
		transformOfx: boolean;
		enableStaffVisualizer: boolean;
	};
	createGroups: boolean;
	ytp: {
		enabled: boolean;
	};
}

export const useConfig = createStore<IConfig>()(
	immer(_set => ({
		visual: {
			enabled: true,
			stretch: "noStretching",
			loop: false,
			staticVisual: false,
			noLengthening: false,
			legato: "upToOneBeat",
			multitrackForChords: false,
			glissando: false,
			mappingVelocity: false,
			transformOfx: false,
			enableStaffVisualizer: false,
		},
		createGroups: true,
		ytp: {
			enabled: false,
		},
	})),
);

export const selectConfig = <T>(path: (state: IConfig) => T) => useStoreSelector(useConfig, path);
