import type { ISettingMeta, RedirectedTrans } from "./settings-metas";
const { t } = new PathObject<RedirectedTrans>();

export const settingsMetasInput = {
	source: {
		from: { icon: "video_clip_multiple" },
		trim: { icon: "aspect_ratio" },
		startTime: { icon: "start_point" },
		advanced: subheader(t.subheaders.advanced),
		afterCompletion: {
			icon: "post_processing",
			items: {
				removeSourceClips: { icon: "delete_track_event" },
				removeSourceClipsWithTracks: { icon: "delete_layer" },
				selectSourceClips: { icon: "select_all" },
				selectGeneratedClips: { icon: "select_all" },
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
		multisource: subheader(),
		secretBox: {
			icon: "question_square",
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
					icon: "question_square",
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
		from: { icon: "instrument" },
		trim: { icon: "aspect_ratio", title: t.source.trim },
		encoding: { icon: "globe" },
		tempo: { icon: "speed" },
		timeSignature: { icon: "health" },
		constrain: { icon: "constraint" },
		parser: { icon: "engine" },
		trackOrChannel: { icon: "midi" },
	},
	audio: {
		preferredTrack: { icon: "preferred_track", title: t.source.preferredTrack },
		createGroups: { icon: "group" },
		playbackRate: playbackRate(),
		normalize: { icon: "normalize" },
		loop: { icon: "loop" },
		prerender: {
			icon: "movie",
			items: {
				acidTag: { icon: "logo/acid" },
			},
		},
		stretch: { icon: "stretch" },
		truncate: { icon: "arrow_import_prohibited" },
		legato: { icon: "legato" },
		multitrackForChords: { icon: "chords" },
		stack: { icon: "database_stack" },
		timeUnremapping: { icon: "timer_off" },
		autoPan: { icon: "stereo" },
		tuning: {
			type: "subheader",
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
					aliases: [t.aliases.stream.articulations.glissando, t.shared.plugins.swirl, t.shared.plugins.wave],
					icon: "slide_note",
				},
			},
		},
		mapping: {
			type: "subheader",
			icon: "flow",
			items: {
				velocity: { icon: "signal" },
				pitch: { icon: "music_note" },
				duration: { icon: "timer", title: t.duration },
				pan: { icon: "stereo" },
				progress: { icon: "progress_bar" },
			},
		},
		parameters: subheader(t.subheaders.parameters),
	},
	visual: {
		preferredTrack: { icon: "preferred_track", title: t.source.preferredTrack },
		createGroups: { icon: "group" },
		playbackRate: playbackRate(),
		loop: { icon: "loop" },
		prerender: { icon: "movie" },
		stretch: { icon: "stretch" },
		truncate: { icon: "arrow_import_prohibited" },
		staticVisual: { icon: "image" },
		legato: { icon: "legato" },
		multitrackForChords: { icon: "chords" },
		stack: { icon: "database_stack" },
		timeUnremapping: { icon: "timer_off" },
		mimical: {
			title: t.stream.tuning.mimical,
			details: t.descriptions.stream.tuning.mimical,
			icon: "tuning_image",
			items: {
				resample: {
					title: t.stream.tuning.resample,
					details: t.descriptions.stream.tuning.mimical.resample,
					icon: "link_multiple",
				},
				oscillator: {
					title: t.stream.tuning.tuningMethod.oscillator_full,
					details: t.descriptions.stream.tuning.mimical.oscillator,
					aliases: [t.stream.tuning.tuningMethod.oscillator],
					icon: "waveforms/triangle",
				},
			},
		},
		transition: {
			icon: "transition",
			items: {
				alignment: { icon: "align_center_vertical" },
				duration: { icon: "timer", title: t.duration },
			},
		},
		transformMethod: { icon: "zoom_fit" },
		effects: subheader(t(2).titles.effect),
		articulations: {
			type: "subheader",
			items: {
				glissando: { icon: "slide_note" },
				appoggiatura: { icon: "appoggiatura" },
				arpeggio: {
					icon: "score",
					items: {
						negative: { icon: "invert_color", title: t.prve.effects.negative },
						applyCustomPreset: { icon: "preset", title: t.stream.articulations.applyCustomPreset },
					},
				},
			},
		},
		mapping: {
			type: "subheader",
			icon: "flow",
			items: {
				velocity: { icon: "signal" },
				pitch: { icon: "music_note" },
				duration: { icon: "timer", title: t.duration },
				pan: { icon: "stereo" },
				progress: { icon: "progress_bar" },
			},
		},
		parameters: subheader(t.subheaders.parameters),
	},
	visual_prve: {
		control: { icon: "prve_control_general" },
		classes: {
			type: "subheader",
			items: {
				flip: { icon: "flip_h" },
				rotation: { icon: "rotate" },
				scale: { icon: "resize_image" },
				mirror: { icon: "image_reflection" },
				invert: { icon: "invert_color" },
				hue: { icon: "hue" },
				chromatic: { icon: "grayscale" },
				time: { icon: "timer" },
				time2: { icon: "timer_2" },
				ec: { icon: "arrow_autofit_height_in" },
				swing: { icon: "arrow_rotate" },
				blur: { icon: "blur" },
				wipe: { icon: "double_tap_swipe" },
			},
		},
	},
	visual_staff: {},
	visual_pixelScaling: {
		scaleFactor: { icon: "zoom_in" },
		replaceSourceMedia: { icon: "replace" },
	},
	track: {
		layout: subheader({ icon: "layout_row_two_split_bottom" }),
		legato: {
			aliases: [t.aliases.stream.legato],
			icon: "legato",
			items: {
				forClips: { icon: "track_event" },
				includeGroup: { icon: "group" },
				backwards: { icon: "arrow_reply" },
				increaseSpacing: { icon: "increase_spacing", title: t.track.legato.increaseSpacingSetting },
			},
		},
		clear: {
			type: "subheader",
			icon: "eraser",
			items: {
				motion: { icon: "clear_motion" },
				effect: { icon: "clear_plugin" },
			},
		},
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
function subheader<const IMeta extends ISettingMeta>(meta: string | IMeta = {} as IMeta) {
	return {
		type: "subheader",
		...typeof meta === "string" || meta instanceof PathObject ? {
			title: meta as string,
		} : meta,
	} as const satisfies ISettingMeta;
}
