import type { LocaleIdentifiers } from "./types";

const SChinese = {
	translation: {
		source: "素材",
		midi: "MIDI",
		audio: "音频",
		visual: "视频",
		track: "轨道",
		ytp: "YTP",
		sonar: "声呐",
		shupelunker: "原音系",
		tools: "工具",
		mosh: "抹失",
		settings: "设置",
	},
} as const satisfies LocaleIdentifiers;

export default SChinese;
