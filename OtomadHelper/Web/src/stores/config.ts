import type { bpmUsings, constraintNoteLengths, encodings } from "views/score";
import type { startTimes } from "views/source";
import type { legatos, noLengthenings, stretches, transformMethods } from "views/visual";

type StartTime = typeof startTimes[number]["id"];
type BpmUsing = typeof bpmUsings[number]["id"];
type ConstraintNoteLength = typeof constraintNoteLengths[number]["id"];
type Encoding = typeof encodings[number];
type Stretch = typeof stretches[number]["id"];
type Legato = typeof legatos[number]["id"];
type NoLengthening = typeof noLengthenings[number]["id"];
type TransformMethod = typeof transformMethods[number];

interface ConfigState {
	source: {
		source: string;
		startTime: StartTime;
		preferredTrack: {
			value: number;
			belowAdjustmentTracks: boolean;
		};
		afterCompletion: {
			removeSourceEvents: boolean;
			selectSourceEvents: boolean;
			selectGeneratedAudioEvents: boolean;
			selectGeneratedVideoEvents: boolean;
		};
		randomOffsetForTracks: boolean;
	};
	score: {
		format: string;
		encoding: Encoding;
		bpmUsing: BpmUsing;
		timeSignature: string;
		constraintNoteLength: ConstraintNoteLength;
		isMultipleSelectionMode: boolean;
	};
	visual: {
		enabled: boolean;
		preferredTrack: number;
		stretch: Stretch;
		loop: boolean;
		staticVisual: boolean;
		noLengthening: NoLengthening;
		legato: Legato;
		multitrackForChords: boolean;
		glissando: boolean;
		transformMethod: TransformMethod;
		enableStaffVisualizer: boolean;
		enablePixelScaling: boolean;
	};
	createGroups: boolean;
	ytp: {
		enabled: boolean;
	};
	settings: {
		uiScale: number;
		getUiScale1(): number;
		hideUseTips: boolean;
	};
	toJson(): string; // If named toJSON, it will conflict to the JSON built-in parameter, causing a recursion error.
}

export const useConfigStore = createStore<ConfigState>()(
	// @ts-ignore TypeScript brain convulsion.
	zustandImmer((_set, get) => ({
		source: {
			source: "trackEvent",
			startTime: "projectStart",
			preferredTrack: {
				value: 0,
				belowAdjustmentTracks: true,
			},
			afterCompletion: {
				removeSourceEvents: false,
				selectSourceEvents: true,
				selectGeneratedAudioEvents: true,
				selectGeneratedVideoEvents: true,
			},
			randomOffsetForTracks: false,
		},
		score: {
			format: "midi",
			encoding: "ANSI",
			bpmUsing: "variableMidi",
			timeSignature: "4/4",
			constraintNoteLength: "none",
			isMultipleSelectionMode: false,
		},
		visual: {
			enabled: true,
			preferredTrack: 0,
			stretch: "noStretching",
			loop: false,
			staticVisual: false,
			noLengthening: "lengthenable",
			legato: "upToOneBeat",
			multitrackForChords: false,
			glissando: false,
			transformMethod: "panCrop",
			enableStaffVisualizer: false,
			enablePixelScaling: false,
		},
		createGroups: true,
		ytp: {
			enabled: false,
		},
		settings: {
			uiScale: 100,
			getUiScale1: () => get().settings.uiScale / 100,
			hideUseTips: false,
		},
		toJson: () => JSON.stringify(get()),
	})),
);

export const selectConfig = <T>(path: (state: ConfigState) => T) => useStoreSelector(useConfigStore, path);
globals.config = useConfigStore;
