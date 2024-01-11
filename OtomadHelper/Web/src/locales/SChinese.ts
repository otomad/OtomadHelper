import type { LocaleIdentifiers } from "./types";

const SChinese = {
	translation: {
		home: "主页",
		source: "素材",
		midi: "MIDI",
		audio: "音频",
		visual: "视频",
		track: "轨道",
		ytp: "YTP",
		sonar: "声呐",
		shupelunker: "原音系",
		shupelunker_full: "原音系战法",
		tools: "工具",
		mosh: "抹失",
		mosh_full: "数据抹失",
		settings: "设置",
		trackEvent: "轨道剪辑",
		projectMedia: "项目媒体",
		browseFile: "浏览文件",
		trim: "修剪",
		startTime: "开始时间",
		moreOptions: "更多选项",
		advanced: "高级",
		on: "开",
		off: "关",
		belowTopAdjustmentTracks: "生成在顶部调整轨道下方",
		removeSourceEventsAfterCompletion: "完成后删除源轨道剪辑",
		selectAllEventsGenerated: "完成后选中生成的所有轨道剪辑",
		randomOffsetForTracks: "对不同的轨道使用随机的偏移时间",
		generateAtBegin: "项目开始处",
		generateAtCursor: "光标处",
		custom: "自定义",
		chart: "谱面",
		ust: "UST",
		refOtherTracks: "引用其它轨道",
		pureNotes: "纯音符",
		descriptions: {
			trim: "调整指定素材的开始或结束时间",
			startTime: "指定何时从项目开始生成",
		},
	},
} as const satisfies LocaleIdentifiers;

export default SChinese;
