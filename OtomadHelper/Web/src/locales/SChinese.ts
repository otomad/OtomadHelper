import type { LocaleIdentifiers } from "./types";

const SChinese = {
	translation: {
		titles: {
			home: "主页",
			source: "素材",
			score: "乐谱",
			audio: "音频",
			visual: "视觉",
			track: "轨道",
			sonar: "声呐",
			shupelunker: "原音系",
			shupelunker_full: "原音系战法",
			ytp: "YTP",
			mosh: "抹失",
			mosh_full: "数据抹失",
			tools: "工具",
			settings: "设置",
			track_other: undefined,
		},
		source: {
			trackEvent: "轨道剪辑",
			projectMedia: "项目媒体",
			browseFile: "浏览文件",
			trim: "修剪",
			startTime: {
				_: "开始时间",
				projectStart: "项目开始处",
				cursor: "光标处",
			},
			belowTopAdjustmentTracks: "生成在顶部调整轨道下方",
			removeSourceEventsAfterCompletion: "完成后删除源轨道剪辑",
			selectAllEventsGenerated: "完成后选中生成的所有轨道剪辑",
			randomOffsetForTracks: "对不同的轨道使用随机的偏移时间",
		},
		on: "开",
		off: "关",
		custom: "自定义",
		subheader: {
			moreOptions: "更多选项",
			advanced: "高级",
			config: "配置",
		},
		score: {
			midi: "MIDI",
			ust: "UST",
			refOtherTracks: "引用其它轨道",
			pureNotes: "纯音符",
			bpm: "BPM",
			timeSignature: "节拍",
			constraint: "限制音符长度",
			encoding: "编码",
			dynamicMidiTempo: "动态 MIDI 速度",
			midiTempo: "MIDI 速度",
			projectTempo: "项目速度",
			unconstrainted: "不限制",
			constraintMaxLength: "最大长度",
			constraintFixedLength: "固定长度",
		},
		descriptions: {
			source: {
				trim: "调整指定素材的开始或结束时间",
				startTime: "指定何时从项目开始生成",
			},
			score: {
				trim: "选取乐曲的生成时间范围",
				bpm: "指定一分钟多少拍",
				constraint: "控制乐谱中的音符输出长度",
				encoding: "指定文件的文本编码",
			},
		},
	},
} as const satisfies LocaleIdentifiers;

export default SChinese;
