import type { LocaleIdentifiers } from "./types";

const SChinese = {
	javascript: {
		metadata: {
			__translator__: "",
			name: "简体中文",
			culture: "zh-CN",
		},
		colon: "：",
		semicolon: "；",
		titles: {
			home: "主页",
			source: "素材",
			score: "乐曲",
			audio: "音频",
			visual: "画面",
			track: "轨道",
			sonar: "声呐",
			lyrics: "歌词",
			shupelunker: "原音系",
			shupelunker_full: "原音系战法",
			ytp: "YTP",
			ytp_full: "YouTube Poop",
			mosh: "抹失",
			mosh_full: "数据抹失",
			tools: "工具",
			settings: "设置",
			prve: "画面节奏视觉效果",
			staff: "五线谱可视化",
			pixelScaling: "像素硬边缘放大",
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
			randomOffsetForTracks: "对不同的音轨使用随机的偏移时间",
		},
		on: "开",
		off: "关",
		custom: "自定义",
		enabled: "启用",
		enable: "启用",
		learnMore: "了解更多",
		condition: "条件",
		underConstruction: "功能研发中⋯⋯",
		allFiles: "所有文件",
		systemDefault: "系统默认",
		complete: "完成",
		dragToImport: "拖放以导入{{item, lowercase}}",
		save: "保存",
		auto: "自动",
		subheaders: {
			moreOptions: "更多选项",
			advanced: "高级",
			config: "配置",
			parameters: "参数",
			effects: "效果",
		},
		score: {
			midi: "MIDI",
			midiFile: "MIDI 序列文件",
			st: "序列文本",
			ustFile: "UTAU 序列文本文件",
			vsqFile: "Vocaloid 序列文件",
			refOtherTracks: "引用其它轨道",
			tts: "文字转语音",
			pureNotes: "纯音符",
			encoding: "编码",
			bpm: {
				_: "BPM",
				dynamicMidi: "动态MIDI速度",
				midi: "MIDI速度",
				project: "项目速度",
			},
			timeSignature: "节拍",
			constraint: {
				_: "限制音符长度",
				none: "不限制",
				max: "最大长度",
				fixed: "固定长度",
			},
		},
		stream: {
			preview: "预览",
			stretch: {
				_: "拉伸",
				noStretching: "不拉伸",
				flexingAndExtending: "可屈伸",
				extendingOnly: "仅伸展",
				flexingOnly: "仅屈折",
			},
			loop: "循环",
			normalize: "规范化音频",
			staticVisual: "静态画面",
			noLengthening: "禁止延长",
			legato: {
				_: "填补间隙",
				portato: "不填补",
				upToOneBeat: "最长一拍",
				upToOneBar: "最长一小节",
				unlimited: "无限填补",
			},
			multitrackForChords: "复音多轨",
			createGroups: "创建分组",
			glissando: "弯音效果",
			autoPan: "自动声像",
			mapping: "映射",
			transformMethod: {
				_: "变换方法",
				panCrop: "平移/裁切",
				transformOfx: "变换效果插件",
			},
			tuning: {
				_: "调音",
				tuningMethod: "调音方法",
				stretchAttributes: "拉伸属性",
				alternativeForExceedsTheRange: "超出音域替代方法",
				resample: "重采样音频",
				preserveFormant: "保持共振峰",
				basePitch: "原始音高",
				prelisten: {
					_: "预听",
					engine: "引擎",
					waveform: {
						_: "波形",
						sinusoid: "正弦波",
						triangle: "三角波",
						square: "方波",
						sawtooth: "锯齿波",
					},
					duration: "持续时间",
					adjustAudioToBasePitch: "调整音频到主音高",
				},
			},
			newTrack: "新增轨道",
		},
		track: {
			layout: "布局",
			useInGeneration: "在生成中应用",
			grid: "网格布局",
			box3d: "3D方盒布局",
			gradient: "渐变轨道",
			applyToSelectedTracks: "应用于选中轨道",
			legato: {
				_: "填补间隙",
			},
			clear: {
				_: "清除",
				motion: "清除轨道运动",
				effect: "清除轨道效果",
			},
		},
		ytp: {
			constraint: "限制长度",
			clips: "剪辑数目",
			effects: "YTP效果",
		},
		mosh: {
			datamosh: "数据抹失",
			datamix: "数据抹拭",
			layer: "多层叠化",
			render: "预渲染化",
			scramble: "随机扰乱",
			automator: "自动生效",
			stutter: "结巴效果",
			shake: "镜头摇晃",
		},
		tools: {
			selector: "选择轨道剪辑",
			replacer: "替换轨道剪辑",
			normalizer: "快速规范音量",
			subtitles: "批量生成字幕",
			visualizer: "应用视觉效果",
			fader: "自定渐入增益",
			scoreExporter: "导出乐谱文件",
			flow: "自定运动曲线",
			converters: "转换",
		},
		selectInfo: {
			source: "已选中{{count}}项媒体素材",
			track: "已选中{{count}}条轨道",
			videoTrack: "已选中{{count}}条视频轨道",
			audioTrack: "已选中{{count}}条音频轨道",
			trackEvent: "已选中{{count}}段轨道剪辑",
			videoEvent: "已选中{{count}}段视频轨道剪辑",
			audioEvent: "已选中{{count}}段音频轨道剪辑",
			trackEventOnlyOne: "必须恰好选中1段轨道剪辑，不得多选或少选",
			videoEventOnlyOne: "必须恰好选中1段视频轨道剪辑，不得多选或少选",
			audioEventOnlyOne: "必须恰好选中1段音频轨道剪辑，不得多选或少选",
			trackGenerated: "将会生成{{count}}条轨道",
			videoTrackGenerated: "将会生成{{count}}条视频轨道",
			audioTrackGenerated: "将会生成{{count}}条音频轨道",
			trackGeneratedGeq: "将会生成{{count}}条及以上的轨道",
			trackGeneratedGeq_zero: "将会生成{{count}}条轨道",
			videoTrackGeneratedGeq: "将会生成{{count}}条及以上的视频轨道",
			videoTrackGeneratedGeq_zero: "将会生成{{count}}条视频轨道",
			audioTrackGeneratedGeq: "将会生成{{count}}条及以上的音频轨道",
			audioTrackGeneratedGeq_zero: "将会生成{{count}}条音频轨道",

		},
		prve: {
			classes: {
				flip: "翻转类",
				rotation: "旋转类",
				scale: "缩放类",
				mirror: "镜像类",
				invert: "反转类",
				hue: "色相类",
				chromatic: "单色类",
				time: "时间类",
				time2: "时间类 2",
				ec: "扩缩类",
				swing: "摇摆类",
				blur: "模糊类",
				wipe: "擦除类",
			},
		},
		pixelScaling: {
			scaleFactor: "缩放因子",
		},
		settings: {
			about: {
				checkForUpdates: "检查更新",
				repositoryLink: "仓库地址",
				documentation: "说明文档",
				version: "版本",
				author: "作者",
				__author__: "兰音",
				originalAuthor: "原作者",
				__originalAuthor__: "Chaosinism",
				translator: "翻译",
			},
			language: {
				_: "语言",
				en: "英语",
				"zh-CN": "简体中文",
				ja: "日语",
				vi: "越南语",
			},
			appearance: {
				_: "外观",
				colorScheme: {
					_: "配色方案",
					light: "浅色",
					dark: "深色",
					auto: "自动",
				},
				uiScale: "界面缩放",
			},
			config: {
				hideUseTips: "隐藏使用贴士",
			},
			dev: {
				_: "开发",
				devMode: "开发者模式",
			},
		},
		descriptions: {
			condition: "指定当满足何条件时才会应用下述配置",
			source: {
				trim: "调整指定素材的开始或结束时间",
				startTime: "指定何时从项目开始生成",
			},
			score: {
				trim: "截取乐曲生成的时间范围",
				bpm: "指定每分钟多少拍",
				constraint: "控制乐曲中的音符输出长度",
				encoding: "指定读取文件的文本编码",
			},
			stream: {
				stretch: "开启后，将拉伸剪辑而不是改变剪辑的持续时间",
				loop: "当剪辑延长到源媒体的末尾后，将会重头开始播放",
				normalize: "如果音频太安静则很有用",
				staticVisual: "在剪辑开始处冻结帧",
				noLengthening: {
					visual: "如果音符比剪辑更长，则在剪辑末尾处冻结帧",
					audio: "如果音符比剪辑更长，则不要延长剪辑",
				},
				legato: "填补音符与音符之间的间隙",
				multitrackForChords: "为和弦生成多条音轨",
				createGroups: "将一个音符所表示的视频与音频剪辑创建分组",
				glissando: "在弯音或滑音时产生漩涡效果",
				autoPan: "自动化控制音频的声像包络",
				mapping: "将音符的参数映射到指定项目",
				transformMethod: "指定在哪个目标属性上应用变换关键帧",
				tuning: {
					stretchAttributes: "有关选中调音方法的更多配置",
					resample: "锁定伸缩与音调，调整伸缩以改变音调",
					preserveFormant: "调音时保持音色不变",
					basePitch: "指定音频剪辑的原始音高是多少",
				},
				effects: {
					prve: "让你的画面更带有节奏感",
					staff: "以自定义图案为音符，画出与钢琴五线谱类似风格的视觉效果",
					pixelScaling: "使用邻近硬边缘放大插值算法缩放图像",
				},
			},
			track: {
				useInGeneration: "在生成的轨道中应用，而不是在选中的轨道中应用",
				gradient: "使视频轨道在布局中具有渐变样式的颜色效果",
				legato: "填补轨道剪辑中的间隙",
			},
			sonar: {
				_: "声呐是利用区域裁切的形状为鼓组创建节拍风格的视觉效果。",
			},
			shupelunker: {
				_: "原音系战法（シュペランカー战法）是一种不调音的音MAD战法。此战法通过使用与旋律音高相同的原素材片段（通常为人声）来演奏旋律，即将素材片段本身做到音高与乐曲的旋律保持一致。\n如果素材片段的音高与旋律不匹配，则生成“鞑靼战法”。此战法同样不改变音高，另外可任意选取素材被切分的位置（通常是在台词的位置），音频伸缩与倒放经常使用，并加入十六分~六十四分休止符音符。在制作时为了唱歌感因此会把素材和乐曲的节奏对齐。",
			},
			ytp: {
				_: "YouTube Poop(YTP)是使用其类型中已知的各种效果来创作荒谬的视频。YTP支持多素材。\nYTP是一种新达达主义的艺术形式，是一种通过模仿和嘲弄低级的视频技术和审美观念以实现对视频文化本身的评价的荒诞派艺术。它由大量视频剪辑而成的视频混剪组成，目的是迷惑、震惊或娱乐观众。这些素材可以全部混合在一起形成一个无厘头的交叉故事，也可以只是重复播放人物古怪的手势的片段。",
				constraint: "控制要生成的剪辑的长度",
				clips: "要生成剪辑的数目",
				effects: "指定YTP的效果",
			},
			mosh: {
				normal: "数据抹失是一种通过磨损素材以产生故障效果的技术。",
				glitchy: "锘挎薮琚沬妷缇㊀種嗵過礳陨嫊豺姒浐泩诂瘴効淉菂攲朮。",
				additional: "在影像艺术中，有一种技术是数据抹失。它是利用两个不同的视频关键帧交错，以致于两帧之间的帧是由两个不同的视频源内插出来的。它利用了不同的视频编码器在对运动及色彩信息处理上的差异。",
				datamosh: "对视频进行数据抹失，最好是在具有大量运动的画面的时间段上应用",
				datamix: "将剪辑的运动应用到另一段剪辑的画面",
				layer: "通过多次复制视频剪辑来进行多层叠化",
				render: "预渲染可能包含非常复杂的视频剪辑的部分时间段，并将其替换为单段视频剪辑",
				scramble: "将剪辑切割成大量子剪辑，并对这些子剪辑进行打乱",
				automator: "自动为添加到视频剪辑中的每一种效果在每帧添加随机关键帧",
				stutter: "通过以随机间隔的正放和倒放来口吃剪辑",
				shake: "使用平移/裁切来摇晃或摆动剪辑",
			},
			tools: {
				_: "以下辅助功能使创作音MAD对使用或调参数时不那么痛苦。它们不需要对先前的任何参数进行任何调整。",
				selector: "查找并选中符合指定的条件的所有剪辑",
				replacer: "将多段剪辑替换为指定的新剪辑",
				normalizer: "将选中的音频剪辑全部规范化音量",
				subtitles: "预先设定好“字幕和文字”的预设，然后在此添加多行文本",
				visualizer: "将选中的视频剪辑应用画面节奏视觉效果",
				fader: "将选中的剪辑根据指定规则来调整增益（音量/不透明度）",
				scoreExporter: "将选中轨道中的剪辑导出为乐谱序列文件",
				flow: "使用贝塞尔曲线创建令人惊叹的动画",
				converters: {
					tuningMethod: "将选中的音频剪辑统一转换到指定的调音算法",
					timeSignature: "将选中音乐的节拍在四四拍、四三拍、八六拍等之间进行转换",
				},
			},
			staff: {
				_: "五线谱可视化是以自定义图案为音符，根据乐曲旋律来画出与钢琴五线谱类似风格的视觉效果。\n该视觉效果风格模仿自YouTube视频作者@grantwoolard，他的特色是使用音乐家的头像来画出经典音乐的五线谱。",
			},
			settings: {
				about: "音MAD助手，Vegas Pro的音MAD扩展程序，旨在使Vegas接受乐谱如MIDI序列文件作为输入，自动生成音MAD的轨道。",
			},
		},
		empty: {
			disabled: {
				title: "已关闭{{name, lowercase}}",
				details: "启用以生成{{name, lowercase}}",
			},
			ytpEnabled: {
				title: "已启用YTP，其它相关参数均不可用",
				details: "禁用YTP功能以使用并调整其它参数",
				disableYtp: "禁用YTP",
				gotoYtp: "转到YTP",
			},
		},
	},
	csharp: {
		ImportToHere: "导入 %1",
	},
} as const satisfies LocaleIdentifiers;

export default SChinese;
