import type { bpmUsings, constraintNoteLengths, encodings } from "views/score";
import type { startTimes } from "views/source";
import type { legatos, stretches, transformMethods } from "views/visual";

type StartTime = typeof startTimes[number]["id"];
type BpmUsing = typeof bpmUsings[number]["id"];
type ConstraintNoteLength = typeof constraintNoteLengths[number]["id"];
type Encoding = typeof encodings[number];
type Stretch = typeof stretches[number]["id"];
type Legato = typeof legatos[number]["id"];
type TransformMethod = typeof transformMethods[number];

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
		transformMethod: TransformMethod;
		enableStaffVisualizer: boolean;
		enablePixelScaling: boolean;
	};
	createGroups: boolean;
	track: {
		layout: boolean;
	};
	ytp: {
		enabled: boolean;
	};
	settings: {
		uiScale: number;
		getUiScale1(): number;
		hideUseTips: boolean;
		devMode: boolean;
	};
	toJson(): string; // 如果叫 toJSON 则会和 JSON 内置参数重名导致递归错误。
}

export const useConfigStore = createStore<IConfig>()(
	zustandImmer((_set, get) => ({
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
			transformMethod: "panCrop",
			enableStaffVisualizer: false,
			enablePixelScaling: false,
		},
		createGroups: true,
		track: {
			layout: false,
		},
		ytp: {
			enabled: false,
		},
		settings: {
			uiScale: 100,
			getUiScale1: () => get().settings.uiScale / 100,
			hideUseTips: false,
			devMode: import.meta.env.DEV,
		},
		toJson: () => JSON.stringify(get()),
	})),
);

export const selectConfig = <T>(path: (state: IConfig) => T) => useStoreSelector(useConfigStore, path);
globals.config = useConfigStore;
