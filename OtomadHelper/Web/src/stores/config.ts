import type { ImageFitTypes } from "components/BackgroundImage";
import type { Encodings } from "components/Preview/PreviewEncoding";
import type { QuickSelectIntervalPresets } from "components/QuickSelectInterval";
import type { LegatoDurations, LegatoModes } from "containers/Expander/ExpanderLegato";
import type { PrologueDurationUsings, PrologueEmphasisDurations, PrologueForms } from "containers/Expander/ExpanderStream/ExpanderStreamPrologue";
import type { NegativeTypes, VisualIdleEffects } from "containers/IdleEffectSettings";
import defaultPrveAmounts from "helpers/defaultPrveAmounts";
import { deepClone } from "valtio/utils";
import type { beepEngines, exactTuningMethods, normalizeTimes, tuningClassicModes, tuningElasticModes, tuningMethods } from "views/audio";
import type { musicalNotationSystems } from "views/lyrics";
import type { constrainNoteLengthTypes, multipleSelectTrackItems, tempoUsings, trackAndChannel } from "views/score";
import type { systemBackdrops } from "views/settings";
import type { textPlugins } from "views/settings/internal";
import type { Namings, SelectGeneratedClips, TrackGroupBy, barOrBeatUnitTypes, sequentialOrders, sourceFromEnums, startTimes } from "views/source";
import type { arrayTypes, directionTypes, fitTypes as gridFitTypes, parityTypes } from "views/track/grid";
import type { glissandoEffects, prerenders, stretches, transformMethods, truncates } from "views/visual";
import ConfigNS = Config;
namespace Config {
	export type StartTime = typeof startTimes[number]["id"];
	export type TempoUsing = typeof tempoUsings[number]["id"];
	export type ConstrainNoteLengthType = typeof constrainNoteLengthTypes[number]["id"];
	export type Encoding = typeof Encodings.keyType;
	export type Stretch = typeof stretches[number]["id"];
	export type LegatoDuration = typeof LegatoDurations.keyType;
	export type Truncate = typeof truncates[number]["id"];
	export type TransformMethod = typeof transformMethods[number];
	export type PitchNotation = typeof musicalNotationSystems[number];
	export type Timecode = string;
	export type MultipleSelectTrackItem = typeof multipleSelectTrackItems[number];
	export type SelectGeneratedClips = typeof SelectGeneratedClips.keyType;
	export type BeepEngine = typeof beepEngines[number];
	export type BarOrBeatUnit = typeof barOrBeatUnitTypes[number];
	export type SourceFrom = typeof sourceFromEnums[number];
	export type TrackOrChannel = typeof trackAndChannel[number];
	export type GridArrayType = typeof arrayTypes[number];
	export type GridDirectionOrderType = typeof directionTypes[number];
	export type GridFitType = typeof gridFitTypes[number];
	export type GridParityType = typeof parityTypes[number];
	export type LegatoMode = typeof LegatoModes.keyType;
	export type NormalizeTime = typeof normalizeTimes[number]["id"];
	export type SystemBackdrop = typeof systemBackdrops[number]["name"];
	export type PrveCustomStepSequences = Partial<Record<string, number[]>>;
	export type PrerenderAs = typeof prerenders[number]["id"];
	export type TuningMethod = typeof tuningMethods[number]["id"];
	export type ExactTuningMethod = typeof exactTuningMethods[number]["id"];
	export type SequentialOrder = typeof sequentialOrders[number]["id"];
	export type TuningElasticMode = typeof tuningElasticModes[number];
	export type TuningClassicMode = typeof tuningClassicModes[number];
	export type TextPlugin = typeof textPlugins[number]["id"];
	export type VisualGlissandoEffect = typeof glissandoEffects[number]["id"];
	export type ImageFitType = typeof ImageFitTypes.keyType;
	export type VisualIdleEffect = typeof VisualIdleEffects.keyType;
	export type VisualIdleEffectValue = ReturnType<typeof defaultVisualIdleEffectSettings>;
	export type AudioIdleEffectValue = Pick<VisualIdleEffectValue, "fade">;
	export type PrologueForm = typeof PrologueForms.keyType;
	export type PrologueDurationUsing = typeof PrologueDurationUsings.keyType;
	export type PrologueEmphasisDuration = typeof PrologueEmphasisDurations.keyType;
	export type OtomadTrackNameType = typeof Namings.otomadTrackNames[number]["id"];
	export type VocaloidTrackNameType = typeof Namings.vocaloidTrackNames[number]["id"];
	export type YtpTrackNameType = typeof Namings.ytpTrackNames[number]["id"];
	export type OtomadClipNameType = typeof Namings.otomadClipNames[number]["id"];
	export type VocaloidClipNameType = typeof Namings.vocaloidClipNames[number]["id"];
	export type YtpClipNameType = typeof Namings.ytpClipNames[number]["id"];
	export type ScoredTrackNameType = typeof Namings.scoredTrackNames[number]["id"];
	export type QuickSelectIntervalPreset = typeof QuickSelectIntervalPresets.keyType;
	export type NegativeType = typeof NegativeTypes.keyType;
	export type TrackGroupBy = typeof TrackGroupBy.keyType;

	const EMPTY_TIMECODE = "00:00:00.000" as Timecode;
	const defaultPrve = {
		isMultiple: false,
		effects: [{ fx: "normal", initial: [0] }],
		amounts: defaultPrveAmounts,
	};
	const defaultVisualIdleEffectSettings = (enabled?: VisualIdleEffect) => ({
		fade: { enabled: enabled === "fade", amount: 50 },
		monochrome: { enabled: enabled === "monochrome", amount: 100 },
		negative: { enabled: enabled === "negative", amount: "colorInvert" satisfies NegativeType },
	});
	const defaultQuickSelectIntervalBits = new BitArray([1, 0, 1, 0]).toBase64();

	export const configStore = createStore({
		source: {
			sourceFrom: "trackEvent" satisfies SourceFrom as SourceFrom,
			trimStart: EMPTY_TIMECODE,
			trimEnd: EMPTY_TIMECODE,
			startTime: "projectStart" satisfies StartTime as StartTime,
			customStartTime: EMPTY_TIMECODE,
			afterCompletion: {
				removeSourceClips: false,
				removeSourceClipsWithTracks: false,
				selectSourceClips: true,
				selectGeneratedClips: [] as true | SelectGeneratedClips[],
			},
			preferredTrack: 0,
			belowAdjustmentTracks: true,
			trackGroup: "byScoreTrack" satisfies TrackGroupBy as TrackGroupBy,
			collapseTrackGroup: true,
			reuseSameNameTrackGroup: true,
			unsetBorrowedTrackName: false,
			otomadTrackName: "track" satisfies OtomadTrackNameType as OtomadTrackNameType,
			vocaloidTrackName: "voicebank" satisfies VocaloidTrackNameType as VocaloidTrackNameType,
			ytpTrackName: "unnamed" satisfies YtpTrackNameType as YtpTrackNameType,
			otomadClipName: "unset" satisfies OtomadClipNameType as OtomadClipNameType,
			vocaloidClipName: "lyric" satisfies VocaloidClipNameType as VocaloidClipNameType,
			ytpClipName: "effect" satisfies YtpClipNameType as YtpClipNameType,
			groupByTaskSessionName: "score" satisfies ScoredTrackNameType as ScoredTrackNameType,
			groupByTaskSessionNameTreatSingleAsMultitrack: false,
			linearMap: false,
			linearMapDescending: false,
			matchCut: false,
			matchCutOrder: "sequential" satisfies SequentialOrder as SequentialOrder,
			matchCutLoop: true,
			matchCutLuckyDip: false,
			luckyDip: false,
			luckyDipLimitToSelected: false,
			luckyDipForTrack: false,
			luckyDipForMarker: false,
			luckyDipForBarOrBeat: false,
			luckyDipForBarOrBeatPeriod: [4, "bar"] as Unit<BarOrBeatUnit>,
			luckyDipForBarOrBeatPreparation: [0, "bar"] as Unit<BarOrBeatUnit>,
			consonant: false,
		},
		score: {
			format: "midi",
			trimEnabled: false,
			trimStart: EMPTY_TIMECODE,
			trimEnd: EMPTY_TIMECODE,
			periodicityEnabled: false,
			periodicityPreset: "odd" satisfies QuickSelectIntervalPreset as QuickSelectIntervalPreset,
			periodicityInterval: 4,
			periodicityBits: defaultQuickSelectIntervalBits,
			pitchRangeEnabled: false,
			pitchRange: ["C0", "F#10"] as [string, string],
			encoding: "ANSI" satisfies Encoding as Encoding,
			tempoUsing: "variableScore" satisfies TempoUsing as TempoUsing,
			customTempo: 120,
			timeSignature: "4/4",
			autoChangeProjectTempo: true,
			autoChangeProjectTimeSignature: true,
			constrainNoteLength: {
				type: "none" satisfies ConstrainNoteLengthType as ConstrainNoteLengthType,
				max: EMPTY_TIMECODE,
				fixed: EMPTY_TIMECODE,
				percentage: 100,
				fixedDecrement: EMPTY_TIMECODE,
				min: EMPTY_TIMECODE,
			},
			trackOrChannel: "track" satisfies TrackOrChannel as TrackOrChannel,
			selectedTrack: 0 as number | number[],
			multipleSelectTrackItems: {} as Record<number, Set<MultipleSelectTrackItem>>,
		},
		audio: {
			enabled: true,
			preferredTrack: 0,
			stretch: "noStretching" satisfies Stretch as Stretch,
			loop: false as TriState,
			normalize: "once" satisfies NormalizeTime as NormalizeTime,
			truncate: "lengthenable" satisfies Truncate as Truncate,
			legatoDuration: "portato" satisfies LegatoDuration,
			legatoAtLeast: false,
			legatoMode: "lengthen" satisfies LegatoMode,
			legatoStretchKeyframes: true as TriState,
			multitrackForChords: false,
			stack: false,
			timeUnremapping: false,
			autoPan: true,
			autoPanCurve: "linear" satisfies CurveType as CurveType,
			tuningMethod: "elastic" satisfies TuningMethod as TuningMethod,
			tuningMethodAcid: false,
			tuningMethodScaleless: false,
			stretchAttributeElastic: "efficient" satisfies TuningElasticMode as TuningElasticMode,
			stretchAttributeClassic: "a03" satisfies TuningClassicMode as TuningClassicMode,
			stretchAttributePitchShift: "a03" satisfies TuningClassicMode as TuningClassicMode,
			alternativeForExceedTheRange: "plugin",
			resample: false,
			preserveFormant: false,
			prerender: "instant" satisfies PrerenderAs as PrerenderAs,
			prerenderAcidTag: false,
			basePitch: "C5",
			cent: 0,
			basePitchBased: true,
			glissando: true,
			prelistenAttributes: {
				engine: "WebAudio" satisfies BeepEngine as BeepEngine,
				waveform: "sinusoid" satisfies OscillatorCommonType as OscillatorCommonType,
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
			stretch: "noStretching" satisfies Stretch as Stretch,
			loop: false as TriState,
			staticVisual: false,
			truncate: "lengthenable" satisfies Truncate as Truncate,
			truncateIdleEffect: defaultVisualIdleEffectSettings("monochrome"),
			truncateLoopRegion: 50,
			legatoDuration: "upToOneBeat" satisfies LegatoDuration,
			legatoAtLeast: false,
			legatoMode: "lengthen" satisfies LegatoMode,
			legatoStretchKeyframes: true as TriState,
			multitrackForChords: false,
			stack: false,
			timeUnremapping: false,
			mimicalResample: null as TriState,
			mimicalOscillator: true,
			transition: false,
			transitionAlignment: 0,
			transitionDuration: EMPTY_TIMECODE,
			transitionCrossfadeCurve: ["smooth", "smooth"] as CrossfadeCurveType,
			prerender: "instant" satisfies PrerenderAs as PrerenderAs,
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
			glissandoEffect: "swirl" satisfies VisualGlissandoEffect as VisualGlissandoEffect,
			glissandoAmount: 12,
			appoggiatura: false,
			arpeggio: false,
			arpeggioIdleEffect: defaultVisualIdleEffectSettings("negative"),
			currentPreset: "enter",
			presetPreviewIdeality: true,
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
		prologue: {
			form: "straightforward" satisfies PrologueForm,
			durationUsing: "untilTheStart" satisfies PrologueDurationUsing,
			customDuration: EMPTY_TIMECODE,
			visualIdleEffect: defaultVisualIdleEffectSettings("fade"),
			audioIdleEffect: { fade: { enabled: false, amount: 50 } } as AudioIdleEffectValue,
			once: true,
			emphasisTimes: 0,
			emphasisDuration: "sourceLength" satisfies PrologueEmphasisDuration,
		},
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
				array: "square" satisfies GridArrayType as GridArrayType,
				direction: "lr-tb" satisfies GridDirectionOrderType as GridDirectionOrderType,
				fit: "cover" satisfies GridFitType as GridFitType,
				dynamicDetection: false,
				mirrorEdgesHFlip: "unflipped" satisfies GridParityType,
				mirrorEdgesVFlip: "unflipped" satisfies GridParityType,
				descending: false,
				padding: 0,
				spans: [] as WebMessageEvents.GridSpanItem[],
				columnWidths: [] as WebMessageEvents.GridColumnWidthRowHeightItem[],
				rowHeights: [] as WebMessageEvents.GridColumnWidthRowHeightItem[],
				blanks: [] as WebMessageEvents.GridSpanItem[],
			},
			concentric: {
				enabled: false,
			},
			box3d: {
				enabled: false,
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
					direction: "lr-tb" satisfies GridDirectionOrderType as GridDirectionOrderType,
					parity: "even_checker" satisfies GridParityType,
					parity2: "even_rows" satisfies GridParityType,
				},
			},
			legato: {
				legatoDuration: "unlimited" satisfies LegatoDuration,
				legatoAtLeast: false,
				legatoMode: "stacking" satisfies LegatoMode,
				legatoStretchKeyframes: true as TriState,
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
				type: "scientific" satisfies PitchNotation as PitchNotation,
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
			isExpandedInExpandedMode: true,
			backgroundImage: -1,
			backgroundImageOpacity: 0.2,
			backgroundImageTint: 0,
			backgroundImageBlur: 0,
			systemBackdrop: "acrylic" satisfies SystemBackdrop as SystemBackdrop,
			accentColor: "wallpaper",
			backgroundColor: "windows",
			fontSize: 14,
			hideUseTips: false,
			autoSwitchSourceFrom: true,
			autoCollapsePrveClasses: true,
			previewWithSource: true,
			internal: {
				language: "zh-CN",
				autosaveInterval: [5, "minute"] as Unit<RoughTimeUnit>,
				defaultTextPlugin: "titlesAndText" satisfies TextPlugin as TextPlugin | (string & {}),
				defaultTuningMethod: "elastic" satisfies TuningMethod as TuningMethod,
				defaultElasticMode: "efficient" satisfies TuningElasticMode as TuningElasticMode,
				defaultClassicMode: "a03" satisfies TuningClassicMode as TuningClassicMode,
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

declare global {
	export import Config = ConfigNS;
}

// If declare these
useListenVegasCommand.on("useTrackEventAsSource", () => configStore.source.sourceFrom = "trackEvent");
useListenVegasCommand.on("useProjectMediaAsSource", () => configStore.source.sourceFrom = "projectMedia");
useListenVegasCommand.on("enableYtp", () => configStore.ytp.enabled = !configStore.ytp.enabled);
