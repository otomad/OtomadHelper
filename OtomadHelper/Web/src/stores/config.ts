import type { bpmUsings, constraintNoteLengths, encodings } from "views/score";
import type { startTimes } from "views/source";
import type { legatos, stretches } from "views/visual";
import { immer } from "zustand/middleware/immer";

type StartTime = typeof startTimes[number]["id"];
type BpmUsing = typeof bpmUsings[number]["id"];
type ConstraintNoteLength = typeof constraintNoteLengths[number];
type Encoding = typeof encodings[number];
type Stretch = typeof stretches[number];
type Legato = typeof legatos[number];

interface IConfig {
	source: {
		source: string;
		startTime: StartTime;
		belowTopAdjustmentTracks: boolean;
		removeSourceEventsAfterCompletion: boolean;
		selectAllEventsGenerated: boolean;
		randomOffsetForTracks: boolean;
	};
	score: {
		format: string;
		encoding: Encoding;
		bpmUsing: BpmUsing;
		timeSignature: string;
		constraintNoteLength: ConstraintNoteLength;
	};
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
		source: {
			source: "trackEvent",
			startTime: "projectStart",
			belowTopAdjustmentTracks: true,
			removeSourceEventsAfterCompletion: false,
			selectAllEventsGenerated: false,
			randomOffsetForTracks: false,
		},
		score: {
			format: "midi",
			encoding: "ANSI",
			bpmUsing: "dynamicMidi",
			timeSignature: "4/4",
			constraintNoteLength: "none",
		},
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
