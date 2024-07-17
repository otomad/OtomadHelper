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
type Timecode = string;

const EMPTY_TIMECODE = "00:00:00.000" as Timecode;

export const configStore = createStore({
	source: {
		source: "trackEvent",
		trimStart: EMPTY_TIMECODE,
		trimEnd: EMPTY_TIMECODE,
		startTime: "projectStart" as StartTime,
		customStartTime: EMPTY_TIMECODE,
		preferredTrack: 0,
		belowAdjustmentTracks: true,
		afterCompletion: {
			removeSourceClips: false,
			selectSourceClips: true,
			selectGeneratedAudioClips: true,
			selectGeneratedVideoClips: true,
		},
		randomOffsetForTracks: false,
	},
	score: {
		format: "midi",
		trimStart: EMPTY_TIMECODE,
		trimEnd: EMPTY_TIMECODE,
		encoding: "ANSI" as Encoding,
		bpmUsing: "variableMidi" as BpmUsing,
		customBpm: 120,
		timeSignature: "4/4",
		constraintNoteLength: "none" as ConstraintNoteLength,
		isMultiple: false,
	},
	audio: {
		enabled: true,
		preferredTrack: 0,
		stretch: "noStretching" as Stretch,
		loop: false,
		normalize: true,
		noLengthening: "lengthenable" as NoLengthening,
		legato: "portato" as Legato,
		multitrackForChords: false,
		autoPan: true,
		autoPanCurve: "linear" as CurveType,
		tuningMethod: "elastic",
		stretchAttribute: "efficient",
		alternativeForExceedsTheRange: "plugin",
		resample: false,
		preserveFormant: false,
		basePitch: "C5",
		prelistenAttributes: {
			engine: "NAudio",
			waveform: "sinusoid",
			duration: 500,
			adjustAudioToBasePitch: false,
		},
	},
	visual: {
		enabled: true,
		preferredTrack: 0,
		stretch: "noStretching" as Stretch,
		loop: false,
		staticVisual: false,
		noLengthening: "lengthenable" as NoLengthening,
		legato: "upToOneBeat" as Legato,
		multitrackForChords: false,
		glissando: false,
		glissandoAmount: 12,
		transformMethod: "panCrop" as TransformMethod,
		enableStaffVisualizer: false,
		prve: {
			general: {
				control: true,
				isMultiple: false,
				effects: ["normal"],
			},
			samePitch: {
				control: false,
				isMultiple: false,
				effects: ["normal"],
			},
			differentSyllables: {
				control: false,
				isMultiple: false,
				effects: ["normal"],
			},
		},
		pixelScaling: {
			enabled: false,
			scaleFactor: 100,
			autoScaleFactor: true,
			replaceSource: true,
		},
		currentPreset: "enter",
		activeParameterScheme: [
			{
				name: "进入",
				enabled: true,
				parameters: ["缩放", "水平位移", "垂直位移"],
			},
			{
				name: "淡出",
				enabled: false,
				parameters: ["淡入淡出"],
			},
		],
	},
	createGroups: true,
	sonar: {
		enabled: true,
		separateDrums: false,
		differenceCompositeMode: false,
		shadow: false,
		shadowColor: "#ffffff",
		graphs: [
			{
				enabled: true,
				drumSound: "Kick",
				color: "#ffffff",
				shape: "square",
			},
			{
				enabled: true,
				drumSound: "Snare",
				color: "#ffffff",
				shape: "diamond",
			},
		],
	},
	ytp: {
		enabled: false,
		constraintStart: 10,
		constraintEnd: 5000,
		clips: 30,
	},
	settings: {
		uiScale: 100,
		get uiScale1() { return this.uiScale / 100; },
		hideUseTips: false,
	},
	// If named toJSON, it will conflict to the JSON built-in parameter, causing a recursion error.
	toJson() { return JSON.stringify(this); },
});

export const selectConfig = <T extends object>(path: (state: typeof configStore) => T) => useStoreState(path(configStore));
export const selectConfigArray = <T extends object>(path: (state: typeof configStore) => T[]) => useStoreStateArray(path(configStore));
globals.config = configStore;
