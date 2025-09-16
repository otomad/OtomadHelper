import defaultPrveAmounts from "helpers/defaultPrveAmounts";
import { deepClone } from "valtio/utils";
import type { beepEngines, exactTuningMethods, normalizeTimes, tuningClassicModes, tuningElasticModes, tuningMethods } from "views/audio";
import type { musicalNotationSystems } from "views/lyrics";
import type { constrainNoteLengthTypes, encodings, multipleSelectTrackItems, tempoUsings, trackAndChannel } from "views/score";
import type { systemBackdrops } from "views/settings";
import type { textPlugins } from "views/settings/internal";
import type { barOrBeatUnitTypes, selectGeneratedClipsType, sequentialOrders, sourceFromEnums, startTimes, trackNames } from "views/source";
import type { trackLegatoModes } from "views/track";
import type { arrayTypes, directionTypes, fitTypes, parityTypes } from "views/track/grid";
import type { legatos, preRenders, stretches, transformMethods, truncates } from "views/visual";

namespace Config {
	export type StartTime = typeof startTimes[number]["id"];
	export type TempoUsing = typeof tempoUsings[number]["id"];
	export type ConstrainNoteLengthType = typeof constrainNoteLengthTypes[number]["id"];
	export type Encoding = typeof encodings[number];
	export type Stretch = typeof stretches[number]["id"];
	export type Legato = typeof legatos[number]["id"];
	export type Truncate = typeof truncates[number]["id"];
	export type TransformMethod = typeof transformMethods[number];
	export type PitchNotation = typeof musicalNotationSystems[number];
	export type Timecode = string;
	export type MultipleSelectTrackItem = typeof multipleSelectTrackItems[number];
	export type SelectGeneratedClips = typeof selectGeneratedClipsType[number]["id"];
	export type BeepEngine = typeof beepEngines[number];
	export type TrackNameType = typeof trackNames[number]["id"];
	export type BarOrBeatUnit = typeof barOrBeatUnitTypes[number];
	export type SourceFrom = typeof sourceFromEnums[number];
	export type TrackOrChannel = typeof trackAndChannel[number];
	export type GridArrayType = typeof arrayTypes[number];
	export type GridDirectionOrderType = typeof directionTypes[number];
	export type GridFitType = typeof fitTypes[number];
	export type GridParityType = typeof parityTypes[number];
	export type TrackLegatoMode = typeof trackLegatoModes[number];
	export type NormalizeTime = typeof normalizeTimes[number]["id"];
	export type SystemBackdrop = typeof systemBackdrops[number]["name"];
	export type PrveCustomStepSequences = Partial<Record<string, number[]>>;
	export type PreRenderAs = typeof preRenders[number]["id"];
	export type TuningMethod = typeof tuningMethods[number]["id"];
	export type ExactTuningMethod = typeof exactTuningMethods[number]["id"];
	export type SequentialOrder = typeof sequentialOrders[number]["id"];
	export type TuningElasticMode = typeof tuningElasticModes[number];
	export type TuningClassicMode = typeof tuningClassicModes[number];
	export type TextPlugin = typeof textPlugins[number];

	const EMPTY_TIMECODE = "00:00:00.000" as Timecode;
	const defaultPrve = {
		isMultiple: false,
		effects: [{ fx: "normal", initial: [0] }],
		amounts: defaultPrveAmounts,
	};

	export const configStore = createStore({
		source: {
			sourceFrom: "trackEvent" as SourceFrom,
			trimStart: EMPTY_TIMECODE,
			trimEnd: EMPTY_TIMECODE,
			startTime: "projectStart" as StartTime,
			customStartTime: EMPTY_TIMECODE,
			afterCompletion: {
				removeSourceClips: false,
				removeSourceClipsWithTracks: false,
				selectSourceClips: true,
				selectGeneratedClips: [] as true | SelectGeneratedClips[],
			},
			preferredTrack: 0,
			belowAdjustmentTracks: true,
			trackGroup: true,
			collapseTrackGroup: true,
			trackName: "track" as TrackNameType,
			secretBox: false,
			secretBoxLimitToSelected: false,
			secretBoxForTrack: false,
			secretBoxForMarker: false,
			secretBoxForBarOrBeat: false,
			secretBoxForBarOrBeatPeriod: [4, "bar"] as Unit<BarOrBeatUnit>,
			secretBoxForBarOrBeatPreparation: [0, "bar"] as Unit<BarOrBeatUnit>,
			consonant: false,
			matchCut: false,
			matchCutOrder: "sequential" as SequentialOrder,
			matchCutLoop: true,
			matchCutSecretBox: false,
			linearMap: false,
			linearMapDescending: false,
		},
		score: {
			format: "midi",
			trimStart: EMPTY_TIMECODE,
			trimEnd: EMPTY_TIMECODE,
			encoding: "ANSI" as Encoding,
			tempoUsing: "variableScore" as TempoUsing,
			customTempo: 120,
			timeSignature: "4/4",
			constrainNoteLength: {
				type: "none" as ConstrainNoteLengthType,
				max: EMPTY_TIMECODE,
				fixed: EMPTY_TIMECODE,
				percentage: 100,
				fixedDecrement: EMPTY_TIMECODE,
				min: EMPTY_TIMECODE,
			},
			trackOrChannel: "track" as TrackOrChannel,
			selectedTrack: 0 as number | number[],
			multipleSelectTrackItems: {} as Record<number, Set<MultipleSelectTrackItem>>,
		},
		audio: {
			enabled: true,
			preferredTrack: 0,
			stretch: "noStretching" as Stretch,
			loop: "false" as TrueFalseAuto,
			normalize: "once" as NormalizeTime,
			truncate: "lengthenable" as Truncate,
			legato: "portato" as Legato,
			multitrackForChords: false,
			stack: false,
			timeUnremapping: false,
			autoPan: true,
			autoPanCurve: "linear" as CurveType,
			tuningMethod: "elastic" as TuningMethod,
			tuningMethodAcid: false,
			tuningMethodScaleless: false,
			stretchAttributeElastic: "efficient" as TuningElasticMode,
			stretchAttributeClassic: "a03" as TuningClassicMode,
			stretchAttributePitchShift: "a03" as TuningClassicMode,
			alternativeForExceedTheRange: "plugin",
			resample: false,
			preserveFormant: false,
			preRender: "instant" as PreRenderAs,
			preRenderAcidTag: false,
			basePitch: "C5",
			cent: 0,
			basePitchBased: true,
			glissando: true,
			prelistenAttributes: {
				engine: "WebAudio" as BeepEngine,
				waveform: "sinusoid" as OscillatorCommonType,
				duration: 500,
				volume: 1,
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
			loop: "false" as TrueFalseAuto,
			staticVisual: false,
			truncate: "lengthenable" as Truncate,
			legato: "upToOneBeat" as Legato,
			multitrackForChords: false,
			stack: false,
			timeUnremapping: false,
			imitativeResample: "auto" as TrueFalseAuto,
			imitativeOscillator: "auto" as TrueFalseAuto,
			transition: false,
			transitionAlignment: 0,
			transitionDuration: EMPTY_TIMECODE,
			preRender: "instant" as PreRenderAs,
			transformMethod: ["panCrop", "pictureInPicture", "transformOfx"] as TransformMethod[],
			prve: {
				general: {
					control: true,
					...deepClone(defaultPrve),
					effects: [{ fx: "hFlip", initial: [1, 2] }],
				},
				samePitch: {
					control: false,
					...deepClone(defaultPrve),
				},
				differentSyllables: {
					control: false,
					...deepClone(defaultPrve),
				},
			},
			prveCustomStepSequences: {} as PrveCustomStepSequences,
			staff: {
				enabled: false,
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
		playbackRate: {
			sync: true,
			audioRate: 1,
			visualRate: 1,
			audioBased: true,
			visualBased: true,
		},
		track: {
			grid: {
				enabled: true,
				columns: 5,
				array: "square" as GridArrayType,
				direction: "lr-tb" as GridDirectionOrderType,
				fit: "cover" as GridFitType,
				mirrorEdgesHFlip: "unflipped" as GridParityType,
				mirrorEdgesVFlip: "unflipped" as GridParityType,
				descending: false,
				padding: 0,
				spans: [] as WebMessageEvents.GridSpanItem[],
				columnWidths: [] as WebMessageEvents.GridColumnWidthRowHeightItem[],
				rowHeights: [] as WebMessageEvents.GridColumnWidthRowHeightItem[],
				blanks: [] as WebMessageEvents.GridSpanItem[],
			},
			box3d: {
				enabled: true,
				deleteTracks: false,
				useLongerSide: false,
			},
			gradient: {
				enabled: true,
				effect: "rainbow",
				descending: false,
				viewOverlay: false,
				viewSquare: false,
				viewMirrorEdges: false,
				viewSize: 325,
				gridIntegration: {
					enabled: true,
					columns: 5,
					autoColumns: true,
					direction: "lr-tb" as GridDirectionOrderType,
					parity: "even_checker" as GridParityType,
					parity2: "even_rows" as GridParityType,
				},
			},
			legato: {
				mode: "stacking" as TrackLegatoMode,
				increaseSpacing: EMPTY_TIMECODE,
				forClips: false,
				includeGroup: false,
				backwards: false,
			},
		},
		sonar: {
			enabled: false,
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
				fillUp: true,
				fillDown: true,
				default: true,
			},
			exclusiveTrack: true,
			offset: 0,
		},
		ytp: {
			enabled: false,
			constraint: [10, 5000, "millisecond"] as RangeUnit<RoughTimeUnit>,
			clips: 30,
		},
		settings: {
			backgroundImage: -1,
			backgroundImageOpacity: 0.2,
			backgroundImageTint: 0,
			backgroundImageBlur: 0,
			systemBackdrop: "acrylic" as SystemBackdrop,
			accentColor: "wallpaper",
			backgroundColor: "windows",
			fontSize: 14,
			hideUseTips: false,
			autoSwitchSourceFrom: true,
			autoCollapsePrveClasses: true,
			internal: {
				language: "zh-CN",
				autosaveInterval: [5, "minute"] as Unit<RoughTimeUnit>,
				defaultTextPlugin: "titlesAndText" as TextPlugin | (string & {}),
				defaultTuningMethod: "elastic" as TuningMethod,
				defaultElasticMode: "efficient" as TuningElasticMode,
				defaultClassicMode: "a03" as TuningClassicMode,
				preserveClipboardOnClose: false,
				eventGroupSelection: false,
				openglInterop: false,
			},
		},
		// If named toJSON, it will conflict to the JSON built-in parameter, causing a recursion error.
		// toJson() { return JSON.stringify(this); }, // DELETE: Do not put methods in config store which cannot be serialized.
	});
}

export const configStore = Config.configStore;
export const useSelectConfig = <T extends object>(path: (state: typeof configStore) => T) => useStoreState(path(configStore));
export const useSelectConfigArray = <T extends object>(path: (state: typeof configStore) => T[]) => useStoreStateArray(path(configStore));
if (import.meta.env.DEV) globals.config = configStore;

import ConfigNS = Config;
declare global {
	export import Config = ConfigNS;
}

// If declare these
useListenVegasCommand.on("useTrackEventAsSource", () => configStore.source.sourceFrom = "trackEvent");
useListenVegasCommand.on("useProjectMediaAsSource", () => configStore.source.sourceFrom = "projectMedia");
useListenVegasCommand.on("enableYtp", () => configStore.ytp.enabled = !configStore.ytp.enabled);
