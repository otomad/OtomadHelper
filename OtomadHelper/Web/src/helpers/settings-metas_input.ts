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
				belowAdjustmentTracks: {
					icon: "layer_sparkle_add_below",
					details: undefined,
				},
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
				order: {
					title: t.order,
					icon: "arrow_sort_horizontal",
				},
				loop: {
					title: t.stream.loop,
					icon: "arrow_repeat_all",
				},
				secretBox: {
					title: t.source.secretBox,
					icon: "dice",
				},
			},
		},
		linearMap: {
			icon: "launchpad",
			items: {
				descending: {
					title: t.descending,
					icon: "descending",
				},
			},
		},
	},
	score: {
		from: {},
		trim: {
			title: t.source.trim,
			icon: "aspect_ratio",
		},
		encoding: { icon: "globe" },
		tempo: { icon: "speed" },
		timeSignature: { icon: "health" },
		constrain: { icon: "constraint" },
		parser: { icon: "engine" },
		trackOrChannel: { icon: "midi" },
	},
} as const satisfies Record<string, Record<string, ISettingMeta>>;
