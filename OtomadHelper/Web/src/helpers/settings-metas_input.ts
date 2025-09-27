import type { Trans } from "utils/i18n";
import type { ISettingMeta } from "./settings-metas";
const t = new PathObject() as Trans;

export const settingsMetasInput = {
	source: {
		from: {},
		trim: { icon: "aspect_ratio" },
		startTime: { icon: "start_point" },
		advanced: { title: t.subheaders.advanced },
		afterCompletion: {
			icon: "post_processing",
			items: {
				removeSourceClips: { icon: "delete_track_event" },
				removeSourceClipsWithTracks: { icon: "delete_layer" },
				selectSourceClips: { icon: "select_all" },
				selectGeneratedClips: { icon: undefined! },
			},
		},
		preferredTrack: {
			icon: "preferred_track",
			items: {
				index: {
					icon: "layer_number",
					details: t.descriptions.source.preferredTrack.fillingInstructions,
				},
				belowAdjustmentTracks: { icon: "layer_sparkle_add_below", details: undefined },
			},
		},
		trackGroup: {
			icon: "group",
			items: {
				collapse: { icon: "chevron_down_up" },
			},
		},
		trackName: { icon: "rename" },
		multisource: {},
		secretBox: {
			icon: "dice",
			items: {
				limitToSelected: { icon: "video_clip_multiple_checkmark" },
				track: { icon: "layer" },
				marker: { icon: "flag" },
				barOrBeat: {
					icon: "music_bar",
					items: {
						period: { icon: "timer" },
						preparation: { icon: "hourglass" },
					},
				},
			},
		},
		consonant: { icon: "consonant" },
		matchCut: {
			icon: "flag_auto_beat",
			items: {
				order: { icon: "arrow_sort_horizontal", title: t.order },
				loop: { icon: "arrow_repeat_all", title: t.stream.loop },
				secretBox: {
					title: t.source.secretBox,
					aliases: [t.aliases.source.secretBox],
					icon: "dice",
				},
			},
		},
		linearMap: {
			icon: "launchpad",
			items: {
				descending: { icon: "descending", title: t.descending },
			},
		},
	},
	score: {
		from: {},
		trim: { icon: "aspect_ratio", title: t.source.trim },
		encoding: { icon: "globe" },
		tempo: { icon: "speed" },
		timeSignature: { icon: "health" },
		constrain: { icon: "constraint" },
		parser: { icon: "engine" },
		trackOrChannel: { icon: "midi" },
	},
	audio: {
		preferredTrack: {
			title: t.source.preferredTrack,
			details: t.descriptions.source.preferredTrack,
			icon: "preferred_track",
		},
		createGroups: { icon: "group" },
		playbackRate: playbackRate(),
		normalize: { icon: "normalize" },
		loop: { icon: "loop" },
		preRender: preRender(),
		stretch: { icon: "stretch" },
		truncate: { icon: "arrow_import_prohibited" },
		legato: { icon: "legato" },
		multitrackForChords: { icon: "chords" },
		stack: { icon: "database_stack" },
		timeUnremapping: { icon: "timer_off" },
		autoPan: { icon: "stereo" },
		tuning: {
			items: {
				tuningMethod: {
					icon: "tuning",
					items: {
						acid: { icon: "logo/acid" },
						scaleless: { icon: "scaleless" },
					},
				},
				stretchAttributes: { icon: "tuning_wrench", details: undefined },
				alternativeForExceedTheRange: { icon: "tuning_warning" },
				resample: { icon: "link_multiple" },
				preserveFormant: { icon: "speech" },
				basePitch: {
					icon: "music_note",
					items: {
						cent: { icon: "fine_tune" },
						based: { icon: "relative" },
						auto: { icon: "tuning_sparkle" },
					},
				},
				prelisten: {
					icon: "headphone",
					items: {
						engine: { icon: "table_column_top_bottom" },
						waveform: { icon: "sound_wave" },
						duration: { icon: "timer", title: t.duration },
						volumeForBasePitch: { icon: "volume" },
						adjustAudioToBasePitch: { icon: "remix_add" },
					},
				},
				glissando: {
					title: t.stream.articulations.glissando,
					details: t.descriptions.stream.articulations.glissando,
					icon: "slide_note",
				},
			},
		},
		mapping: {
			items: {
				velocity: { icon: "signal" },
				pitch: { icon: "music_note" },
				duration: { icon: "timer", title: t.duration },
				pan: { icon: "stereo" },
				progress: { icon: "progress_bar" },
			},
		},
		parameters: { title: t.subheaders.parameters },
	},
} as const satisfies Record<string, Record<string, ISettingMeta>>;

function playbackRate() {
	return {
		icon: "play_circle_hint_half",
		items: {
			based: { icon: "relative" },
			sync: {
				icon: "sync",
				details: undefined,
			},
		},
	} as const satisfies ISettingMeta;
}
function preRender() {
	return {
		icon: "movie",
		items: {
			acidTag: { icon: "logo/acid" },
		},
	} as const satisfies ISettingMeta;
}
