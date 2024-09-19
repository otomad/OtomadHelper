import type { pitchNotations } from "views/lyrics";
import type { bpmUsings, constraintNoteLengthTypes, encodings, multipleSelectTrackItems } from "views/score";
import type { selectGeneratedClipsType, startTimes } from "views/source";
import type { legatos, stretches, transformMethods, unlengthens } from "views/visual";

type StartTime = typeof startTimes[number]["id"];
type BpmUsing = typeof bpmUsings[number]["id"];
type ConstraintNoteLengthType = typeof constraintNoteLengthTypes[number]["id"];
type Encoding = typeof encodings[number];
type Stretch = typeof stretches[number]["id"];
type Legato = typeof legatos[number]["id"];
type Unlengthen = typeof unlengthens[number]["id"];
type TransformMethod = typeof transformMethods[number];
type PitchNotation = typeof pitchNotations[number];
type Timecode = string;
type MultipleSelectTrackItem = typeof multipleSelectTrackItems[number];
type SelectGeneratedClips = typeof selectGeneratedClipsType[number]["id"];

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
			selectGeneratedClips: [] as true | SelectGeneratedClips[],
		},
		blindBoxForTrack: false,
		blindBoxForMarker: false,
	},
	score: {
		format: "midi",
		trimStart: EMPTY_TIMECODE,
		trimEnd: EMPTY_TIMECODE,
		encoding: "ANSI" as Encoding,
		bpmUsing: "variableMidi" as BpmUsing,
		customBpm: 120,
		timeSignature: "4/4",
		constraintNoteLengthType: "none" as ConstraintNoteLengthType,
		constraintNoteLengthValue: EMPTY_TIMECODE,
		selectedTrack: 0 as number | number[],
		multipleSelectTrackItems: {} as Record<number, Set<MultipleSelectTrackItem>>,
	},
	audio: {
		enabled: true,
		preferredTrack: 0,
		stretch: "noStretching" as Stretch,
		loop: false,
		normalize: true,
		unlengthen: "lengthenable" as Unlengthen,
		legato: "portato" as Legato,
		multitrackForChords: false,
		autoPan: true,
		autoPanCurve: "linear" as CurveType,
		timeUnremapping: false,
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
		currentPreset: "fadeOut",
		activeParameterScheme: [
			{
				name: "淡出",
				enabled: false,
				parameters: ["淡入淡出"],
			},
		],
	},
	visual: {
		enabled: true,
		preferredTrack: 0,
		stretch: "noStretching" as Stretch,
		loop: false,
		staticVisual: false,
		unlengthen: "lengthenable" as Unlengthen,
		legato: "upToOneBeat" as Legato,
		multitrackForChords: false,
		timeUnremapping: false,
		resampleImitatively: "auto" as TrueFalseAuto,
		transformMethod: ["panCrop", "pictureInPicture", "transformOfx"] as TransformMethod[],
		prve: {
			general: {
				control: true,
				isMultiple: false,
				effects: [{ fx: "normal", initial: 0 }],
			},
			samePitch: {
				control: false,
				isMultiple: false,
				effects: [{ fx: "normal", initial: 0 }],
			},
			differentSyllables: {
				control: false,
				isMultiple: false,
				effects: [{ fx: "normal", initial: 0 }],
			},
		},
		pixelScaling: {
			enabled: false,
			scaleFactor: 100,
			autoScaleFactor: true,
			replaceSource: true,
		},
		glissando: false,
		glissandoEffect: "swirl" as "swirl" | "pingpong",
		glissandoAmount: 12,
		appoggiatura: false,
		arpeggio: false,
		arpeggioNegative: true,
		enableStaffVisualizer: false,
		currentPreset: "enter",
		activeParameterScheme: [
			{
				id: "ZW50ZXI=",
				name: "进入",
				enabled: true,
				parameters: ["缩放", "水平位移", "垂直位移"],
			},
			{
				id: "ZmFkZQ==",
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
		shadowColor: "#000000",
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
	lyrics: {
		enabled: false,
		presetTemplate: "Text",
		karaoke: {
			enabled: false,
			futureFill: "#005fb7",
			pastFill: "#0f7b0f",
		},
		pitchNotation: {
			enabled: false,
			type: "scientific" as PitchNotation,
		},
	},
	shupelunker: {
		enabled: false,
		affix: "prefix",
		unallocated: {
			octaves: true,
			lowerNeighbors: true,
			higherNeighbors: true,
			default: true,
		},
		exclusiveTrack: true,
		offset: 0,
	},
	ytp: {
		enabled: false,
		constraintStart: 10,
		constraintEnd: 5000,
		clips: 30,
	},
	settings: {
		backgroundImage: -1,
		backgroundImageOpacity: 0.2,
		backgroundImageTint: 0,
		backgroundImageBlur: 0,
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
