import type { LocaleIdentifiers } from "./types";

export default {
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
			sonar: "声呐",
			lyrics: "歌词",
			shupelunker: "原音系",
			shupelunker_full: "原音系战法",
			ytp: "YTP",
			ytp_full: "YouTube Poop",
			mosh_full: "数据抹失",
			tools: "工具",
			settings: "设置",
			prve: "画面节奏视觉效果",
			staff: "五线谱可视化",
			pixelScaling: "像素硬边缘放大",
			parameters: "参数",
			track: "轨道",
			mosh: "抹失",
		},
		source: {
			trackEvent: "轨道剪辑",
			projectMedia: "项目媒体",
			browseFile: "浏览文件",
			trim: "修剪",
			startTime: {
				_: "起始时间",
				projectStart: "项目开始处",
				cursor: "光标处",
			},
			preferredTrack: {
				_: "首选轨道",
				index: "首选轨道序号",
				top: "顶部",
				ordinal: "在{{count, ordinal}}条轨道下方",
				belowAdjustmentTracks: "如果该轨道下方有一条或多条调整轨道，则选择下一条不是调整轨道的轨道",
				newTrack: "新增轨道",
			},
			afterCompletion: {
				_: "完成后",
				removeSourceClips: "删除源轨道剪辑",
				selectSourceClips: "选中源轨道剪辑",
				selectGeneratedAudioClips: "选中生成的所有音频剪辑",
				selectGeneratedVideoClips: "选中生成的所有视频剪辑",
			},
			randomInPoint: {
				_: "随机入点",
				tracks: "为不同的音轨",
			},
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
		back: "返回",
		navigation: "导航",
		selectAll: "全选",
		invertSelection: "反选",
		variableBeginWith: "{{first, capitalize}}起始的可变值",
		reset: "重置",
		new: "新建",
		etc: "{{examples}} 等",
		offset: "偏移",
		curve: "插值曲线",
		unselected: "未选择",
		topPriority: "{{item, capitalize}} 最优先",
		infoBar: {
			warning: "警告",
		},
		selectionMode: {
			single: "单选",
			multiple: "多选",
		},
		subheaders: {
			moreOptions: "更多选项",
			advanced: "高级",
			config: "配置",
			parameters: "参数",
			effects: "效果",
			seeAlso: "另请参阅",
		},
		units: {
			milliseconds: "毫秒",
			percents: "%",
			pixels: "像素",
			beatsPerMinute: "拍每分",
			semitones: "半音",
			degrees: "°",
		},
		fileFormats: {
			allFiles: "所有文件",
			txt: "文本文档",
			midi: "MIDI序列文件",
			singthesis: "所有受支持的歌声合成软件工程文件",
			ust: "UTAU/OpenUTAU序列文本文件",
			vsq: "Vocaloid序列文件",
		},
		score: {
			midi: "MIDI",
			singthesis: "歌声合成",
			refOtherTracks: "引用其它轨道",
			tts: "文字转语音",
			pureNotes: "纯音符",
			encoding: "编码",
			bpm: {
				_: "BPM",
				variableMidi: "可变MIDI速度",
				constantMidi: "恒定MIDI速度",
				project: "项目速度",
			},
			timeSignature: "节拍",
			constraint: {
				_: "限制音符长度",
				none: "不限制",
				max: "最大长度",
				fixed: "固定长度",
			},
			noteCount: "音符数",
			beginNote: "起音",
			pan: {
				_: "声像",
				left: "左",
				right: "右",
				center: "中",
			},
			instrument: "乐器",
			drumKit: "鼓组",
			musicalTrack: "音轨",
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
			noLengthening: {
				_: "禁止延长",
				lengthenable: "可延长",
				freezeEndFrames: "冻结尾帧",
				trimEndFrames: "截断尾帧",
				splitThenFreeze: "分割后冻结",
				freezeToGray: "冻结至黑白",
				freezeToPreset: "冻结至预设",
			},
			legato: {
				_: "填补间隙",
				portato: "不填补",
				upToOneBeat: "最长一拍",
				upToOneBar: "最长一小节",
				unlimited: "无限填补",
			},
			multitrackForChords: "复音多轨",
			createGroups: "创建分组",
			autoPan: "自动声像",
			noTimeRemapping: "时间不重映射",
			transformMethod: {
				_: "变换方法",
				panCrop: "平移/裁切",
				pictureInPicture: "画中画",
				transformOfx: "变换效果插件",
			},
			playingTechniques: {
				_: "演奏技法",
				applyCustomPreset: "应用自定义预设",
				glissando: {
					_: "滑音",
					swirlAmount: "漩涡大小",
				},
				appoggiatura: {
					_: "倚音",
				},
				arpeggio: {
					_: "琶音",
				},
			},
			tuning: {
				_: "调音",
				tuningMethod: {
					_: "调音方法",
					noTuning: "不调音",
					pitchShift: "移调",
					elastic: "弹性",
					classic: "古典",
					scaleless: "无音阶",
				},
				stretchAttributes: {
					_: "拉伸属性",
				},
				alternativeForExceedsTheRange: {
					_: "超出音域替代方法",
					multiple: "使用多次音效插件",
					plugin: "切换到移调音效插件",
					octave: "高/低八度",
					dock: "停靠在边缘",
					silent: "不发声",
				},
				resample: "重采样音频",
				preserveFormant: "保持共振峰",
				basePitch: "原始音高",
				prelisten: {
					_: "预听",
					basePitch: "预听标准音高",
					audio: "预听音频",
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
			mapping: {
				_: "映射",
				velocity: "力度",
				pitch: "音高",
				duration: "持续时间",
				pan: "声像",
				progress: "进度",
			},
			preset: {
				add: "添加到自定义预设",
			},
			parameters: {
				copyFromAnotherParameterScheme: "从另一套参数方案复制至此",
				copyAttributesFromSelectedClip: "复制所选剪辑的属性",
			},
		},
		track: {
			layout: "布局",
			grid: "网格布局",
			box3d: "3D方盒布局",
			gradient: "渐变轨道",
			applyToSelectedTracks: "应用于所选轨道",
			resetAllLayouts: "全部重置",
			legato: {
				_: "填补间隙",
			},
			clear: {
				_: "清除",
				motion: "清除轨道运动",
				effect: "清除轨道效果",
			},
		},
		sonar: {
			separateDrums: "分离不同鼓声",
			differenceCompositeMode: "差值合成模式",
			shadow: "阴影",
			graphs: "图形 ",
		},
		lyrics: {
			useStaticText: "从静态文本直接插入字幕",
			sampleLyrics: "香辣火鍋雞",
			presetTemplate: "预设模板",
			enableMode: "启用{{mode, lowercase}}模式",
			karaoke: {
				_: "卡拉OK",
				futureFill: "未播放文本填充颜色",
				pastFill: "已播放文本填充颜色",
			},
			pitchNotation: {
				_: "音高记号",
				type: "音高记号形式",
				scientific: "国际谱（科学音高记法）",
				helmholtz: "女唱谱（亥姆霍兹音高记法）",
				solfeggio: "唱名",
				numbered: "简谱",
				gongche: "工尺谱",
			},
		},
		shupelunker: {
			affix: {
				_: "匹配音高词缀位置",
				prefix: "前缀",
				suffix: "后缀",
			},
			unallocated: {
				_: "未分配",
				octaves: "八度",
				lowerNeighbors: "邻近低音",
				higherNeighbors: "邻近高音",
				default: "缺省兜底",
			},
			keyMappingZones: "音调映射区域",
		},
		ytp: {
			constraint: "限制长度",
			clips: "剪辑数目",
			effects: "YTP效果",
		},
		mosh: {
			datamosh: "数据抹失",
			datamix: "数据抿拭",
			layer: "多层叠化",
			render: "预渲染化",
			scramble: "随机扰乱",
			automator: "自动乱调",
			stutter: "结巴演说",
			shake: "镜头摇晃",
			specifyClipsFolder: "指定数据抹失片段文件夹",
			install: "下载数据抹失扩展包",
		},
		tools: {
			flow: "运动曲线",
			selectorAndReplacer: "选择替换",
			normalizer: "规范音量",
			subtitles: "批量字幕",
			effector: "应用效果",
			fader: "渐入增益",
			exportScore: "导出乐谱",
			converters: "转换",
			clawer: "耙爪器",
		},
		selectInfo: {
			trackEventOnlyOne: "必须恰好选中1段轨道剪辑，不得多选或少选",
			videoEventOnlyOne: "必须恰好选中1段视频轨道剪辑，不得多选或少选",
			audioEventOnlyOne: "必须恰好选中1段音频轨道剪辑，不得多选或少选",
			source: "已选中{{count}}项媒体素材",
			track: "已选中{{count}}条轨道",
			videoTrack: "已选中{{count}}条视频轨道",
			audioTrack: "已选中{{count}}条音频轨道",
			trackEvent: "已选中{{count}}段轨道剪辑",
			videoEvent: "已选中{{count}}段视频轨道剪辑",
			audioEvent: "已选中{{count}}段音频轨道剪辑",
		},
		prve: {
			control: {
				general: "常规",
				general_full: "常规控制",
				samePitch: "同音高",
				samePitch_full: "连续相同音高控制",
				differentSyllables: "异音节",
				differentSyllables_full: "连续不同音节控制",
			},
			classes: {
				_: "效果类",
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
			effects: {
				normal: "正常",
				hFlip: "水平翻转",
				vFlip: "垂直翻转",
				ccwFlip: "逆时针翻转",
				cwFlip: "顺时针翻转",
				ccwRotate: "逆时针旋转",
				cwRotate: "顺时针旋转",
				turned: "颠倒",
				zoomOutIn: "缩小后放大",
				hMirror: "水平镜像",
				vMirror: "垂直镜像",
				ccwMirror: "逆时针镜像",
				cwMirror: "顺时针镜像",
				negative: "颜色反转",
				luminInvert: "亮度反转",
				hueInvert: "色相反转",
				chromatic: "彩灰",
				pingpong: "乒乓效应（来回）",
				whirl: "爱的魔力转圈圈",
				sharpRewind: "急剧倒带",
				wobblePeriod: "摆动周期",
				vExpansion: "垂直扩张",
				vExpansionBounce: "垂直扩张并回弹",
				vCompression: "垂直压缩",
				vCompressionBounce: "垂直压缩并回弹",
				vBounce: "垂直弹起",
				slantDown: "斜下扩缩",
				slantUp: "斜上扩缩",
				puyo: "魔法气泡",
				pendulum: "钟摆",
				gaussianBlur: "高斯模糊",
				radialBlur: "径向模糊",
				wipeRight: "向右擦除",
				splitVOut: "垂直分割",
				stepChangeHue: "{{count}}步色差",
			},
		},
		pixelScaling: {
			scaleFactor: "缩放因子",
			replaceSourceMedia: "替换原始媒体",
		},
		settings: {
			about: {
				checkForUpdates: "检查更新",
				repositoryLink: "仓库地址",
				documentation: "说明文档",
				translation: "贡献翻译",
				feedback: "反馈建议",
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
				hideUsageTips: "隐藏使用小贴士",
			},
			dev: {
				_: "开发",
				devMode: "开发者模式",
				rtl: "从右到左的布局方向",
			},
		},
		descriptions: {
			condition: "指定当满足何条件时才会应用该配置",
			curve: "指定关键帧类型中的插值曲线",
			source: {
				trim: "调整指定素材的入点或出点时间",
				startTime: "指定何时从项目开始生成",
				preferredTrack: {
					_: "允许你指定一条现有轨道来生成（多轨除外）",
					fillingInstructions: "若为0，则生成在所有轨道的上方；\n若为正，则生成在第几条轨道的下方；\n若为负，则生成在倒数第几条轨道的下方。\n如果在音频或画面配置中指定了任何首选轨道，它将会覆盖此选项。",
					belowAdjustmentTracks: {
						versionRequest: "注意：此功能要求至少为VEGAS Pro 19。当前版本为 {{version}}。",
					},
				},
				randomInPoint: "这可能会导致随机选择的素材片段具有不同的原始音高，因此只能用来做成搞笑视频供娱乐，几乎不能做成高品质视频",
			},
			score: {
				trim: "截取乐曲生成的时间范围",
				bpm: "指定每分钟多少拍",
				constraint: "控制乐曲中的音符输出长度",
				encoding: "指定在读取文件时要使用的文本编码",
				ytpEnabled: "YTP功能已启用，但它不依赖于乐曲，因此该处的所有设置均不起任何作用。",
			},
			stream: {
				stretch: "拉伸剪辑而不是改变剪辑的持续时间",
				loop: "当剪辑延长到源媒体的末尾后，将会重头开始播放",
				normalize: "适用于音频太安静的素材",
				staticVisual: "在剪辑起始处冻结帧",
				noLengthening: "尝试在剪辑的出点处冻结，以避免在某些音符过长时，意外播放到超出修剪时间的部分",
				legato: "填补音符与音符之间的间隙",
				noLengtheningAndLegatoConflictInAudio: "禁止延长和填补间隙在音频里互相冲突，不能同时开启！",
				multitrackForChords: "为和弦创建多条轨道",
				createGroups: "将一个音符所表示的视频与音频剪辑创建分组",
				autoPan: "自动化控制音频的声像包络",
				noTimeRemapping: "音符开时将不会重置剪辑的入点时间，而是继续播放，适用于如仅对素材应用效果",
				transformMethod: "指定要应用变换关键帧的目标属性的优先级顺序",
				playingTechniques: {
					glissando: {
						_: "在演奏弯音或滑音时产生效果。默认为漩涡。",
						swirlAmount: "指定漩涡扭动幅度的大小",
					},
					appoggiatura: {
						_: "在演奏倚音时产生效果。\n当有连续一到两个十六分音符或更短的音符时，它们将被视为倚音。",
					},
					arpeggio: {
						_: "在演奏琶音时产生效果。默认为颜色反转。\n当有连续三个及以上十六分音符或更短的音符时，它们将被视为琶音。",
						negative: "典型地使用颜色反转来表现琶音",
					},
				},
				tuning: {
					tuningMethod: {
						noTuning: "无变调效果",
						pitchShift: "使用移调音频效果插件，它很慢，且在改变回放速率时失效，但可以超出音域，并且是唯一一个可以在VEGAS Pro ≤ 15中访问脚本API的方法",
						elastic: "使用弹性音调更改方法，也就是加减键调音的默认方法，它无法超出音域",
						classic: "使用古典音调更改方法，它无法超出范围，是VEGAS Pro ≤ 8所使用的唯一方法",
						scaleless: "锁定伸缩与音调，完全通过改变拉伸值来获取相应的音调，而不考虑音符的音高，仅供娱乐",
					},
					stretchAttributes: "有关所选调音方法的更多配置",
					alternativeForExceedsTheRange: "使用另一种平替方法来处理超出音域之外的音符",
					resample: "锁定伸缩与音调，调整伸缩以改变音调",
					preserveFormant: "调音时保持音色不变",
					basePitch: "指定音频剪辑的原始音高",
					prelisten: {
						_: "将剪辑与原始音高进行比较，以便调整",
						adjustAudioToBasePitch: "将剪辑的音频匹配到原始音高，而不是反过来，适用于旧式混音",
					},
				},
				effects: {
					prve: "让你的画面更带有节奏感",
					staff: "以自定义图案为音符，画出与钢琴五线谱类似风格的视觉效果",
					pixelScaling: "使用邻近硬边缘放大插值算法缩放图像",
				},
				mapping: {
					_: "将音符的参数映射到指定项目",
				},
				preset: {
					_: "使用或保存预定义的参数方案以方便使用",
				},
			},
			track: {
				gradient: "使视频轨道在布局中具有渐变样式的颜色效果",
				legato: "填补轨道剪辑中的间隙",
				resetLayout: "重置此布局的状态来禁用它",
			},
			sonar: {
				_: "声呐是利用区域裁切的形状创建节拍风格动态图形的视觉效果。本功能可利用乐曲中的鼓组乐器（通道10）为其各种打击乐器分配不同的形状或效果。\n一种常见的音MAD流派是根据节奏在画面中加入例如扩散的圆圈等各种图形来制作动态图形（MG动画）。MG动画是介于平面设计与动画设计之间的一种产物，是基于时间流动而设计的视觉表现形式，是影像艺术的一种。其中“动态图形”指的是会随时间流动而改变形态的图形。仅包含MG动画的单个音MAD视频通常以《图形》《形状》等视频标题命名。",
				enabled: "如果乐曲的待生成音轨中包含鼓组，则会启用声呐效果",
				separateDrums: "将每个鼓声放置在各自的轨道上",
				differenceCompositeMode: "创建带有差值混合模式的轨道，这将会呈现一种皮影（影绘）风格",
				shadow: "为图形添加阴影",
			},
			lyrics: {
				_: "当乐曲中包含歌词如序列文本时，可以同时自动生成歌词的字幕。\n如果乐曲中不包含歌词，仍然可以生成音高记号供使用。",
				presetTemplate: "选择“字幕和文字”媒体发生器的预设作为歌词的模板",
				karaoke: {
					_: "使用卡拉OK风格字幕，颜色指示器会反映当前乐句的演唱进度",
					futureFill: "指定未播放歌词的文字填充颜色",
					pastFill: "指定已播放歌词的文字填充颜色",
				},
				pitchNotation: {
					_: "以文本形式可视化当前音符的音调",
					type: "世界上有各种各样的表达音乐符号的方式，选择你喜欢的方式",
				},
			},
			shupelunker: {
				_: "原音系战法（シュペランカー战法）是一种不调音的音MAD战法。此战法通过使用与旋律音高相同的原素材片段（通常为人声）来演奏旋律，即将素材片段本身做到音高与乐曲的旋律保持一致。\n如果素材片段的音高与旋律不匹配，则生成“鞑靼战法”。此战法同样不改变音高，另外可任意选取素材被切分的位置（通常是在台词的位置），音频伸缩与倒放经常使用，并加入十六分~六十四分休止符。在制作时为了唱歌感因此会把素材和乐曲的节奏对齐。",
				affix: "需要通过对剪辑命名来检测素材的原始音高，请指定音高信息位于剪辑名称的前缀还是后缀上",
				unallocated: {
					_: "如果素材未能涵盖所有音调，请指定如何填补这些空缺",
					octaves: "使用最邻近高或低八度的剪辑，这具有最高的优先级，适用于吟唱唱名或视唱练耳的素材",
					lowerNeighbors: "使用邻近的低音，除了最低音部分由邻近的高音填充，这比邻近高音的优先级更高",
					higherNeighbors: "使用邻近的高音，除了最高音部分由邻近的低音填充",
					default: "使用第一个不包含任何词缀的剪辑来覆盖剩余所有空缺的区域，这具有最低的优先级",
				},
				offset: "整体偏移音调所对应的素材",
			},
			ytp: {
				_: "YouTube Poop(YTP)是使用其类型中已知的各种效果来创作荒谬的视频。YTP支持多素材。\nYTP是一种新达达主义的艺术形式，是一种通过模仿和嘲弄低级的视频技术和审美观念以实现对视频文化本身的评价的荒诞派艺术。它由大量视频剪辑而成的视频混剪组成，目的是迷惑、震惊或娱乐观众。这些素材可以全部混合在一起形成一个无厘头的交叉故事，也可以只是重复播放人物古怪的手势的片段。",
				constraint: "控制要生成的剪辑的长度",
				clips: "设定要生成的剪辑的数目",
				effects: "指定YTP的效果",
			},
			mosh: {
				normal: "数据抹失是一种通过磨损素材以产生故障效果的技术。",
				glitchy: "锘挎薮琚沬妷缇㊀種嗵過礳陨嫊豺姒浐泩诂瘴効淉菂攲朮。",
				additional: "在影像艺术中，有一种技术是数据抹失。它是利用两个不同的视频关键帧交错，以致于两帧之间的帧是由两个不同的视频源内插出来的。它利用了不同的视频编码器在对运动及色彩信息处理上的差异。",
				datamosh: "对视频进行数据抹失，最好是在具有大量运动的画面的时间段上应用",
				datamix: "将剪辑的运动应用到另一段剪辑的画面",
				layer: "通过反复复制视频剪辑来实现多层叠化",
				render: "预渲染可能包含非常复杂的视频剪辑的部分时间段，并将其替换为单段视频剪辑",
				scramble: "将剪辑分割成大量剪辑碎屑，并对它们进行打乱",
				automator: "自动为添加到视频剪辑中的每一种效果在每帧添加随机关键帧",
				stutter: "通过以随机的时间间隔正放和倒放来使剪辑断断续续",
				shake: "使用平移/裁切来摇晃或摆动剪辑",
				notInstalled: "未安装数据抹失扩展包，下载后方可使用全部功能。",
			},
			tools: {
				_: "以下辅助功能使得创作音MAD的工作或设置稍微轻松一些。它们不需要对先前的任何参数进行任何调整。",
				flow: "使用贝塞尔曲线创建令人惊叹的动画",
				selectorAndReplacer: "查找并选中符合指定条件的所有剪辑，然后可以替换为指定的新剪辑",
				normalizer: "将所选音频剪辑全部规范化音量",
				subtitles: "预先设定好“字幕和文字”的预设，然后在此处添加多行文本",
				effector: "为所选剪辑应用效果",
				fader: "将所选剪辑根据指定规则来调整增益（音量/不透明度）",
				exportScore: "将所选轨道中的剪辑导出为乐谱序列文件",
				converters: {
					tuningMethod: "为所选音频剪辑更换调音算法",
					transformMethod: "为所选视频剪辑更换应用变换关键帧的目标属性",
					clawer: "改变音乐节拍以创建有趣的新节奏",
				},
			},
			staff: {
				_: "五线谱可视化是以自定义图案为音符，根据乐曲旋律来画出与钢琴五线谱类似风格的视觉效果。\n该视觉效果风格模仿自YouTube视频作者@grantwoolard，他的特色是使用音乐家的头像来画出经典音乐的五线谱。",
			},
			prve: {
				control: {
					general: "一般条件下控制间断事件或不同音高和相同音节的连续事件（音MAD和人力模式）。\n如果其它分别控制已关闭，则它们的用例也将被包含在这里。",
					samePitch: "对相同音高的连续事件分别控制（音MAD和人力模式）",
					differentSyllables: "对不同音节的连续事件分别控制（人力模式）",
				},
				forceStretch: "正在使用的画面节奏视觉效果中包含时间类的效果，这会导致在这些效果下拉伸将被强制设定为“$t(stream.stretch.flexingAndExtending)”，而不受你的设置控制",
			},
			pixelScaling: {
				_: "像素硬边缘放大功能，可以将原素材的像素画图像放大并替换为新图像，以避免VEGAS自带的平移/裁切对原始像素画使用平滑渐变算法重新缩放而造成的像素失真问题。如需使用该功能，你必须提前将FFmpeg添加至系统环境变量内，或者也可以直接安装数据抹失扩展包。\n像素硬边缘放大功能会利用FFmpeg对素材原文件使用最近邻插值算法进行放大处理，以适配当前工程大小，再将VEGAS内的原始媒体文件替换成新生成的媒体文件。新生成的文件会在名称中添加“_Scaled”后缀作为标识。此功能理论上支持任意图像/视频文件格式，同时也支持通过常规方式导入进VEGAS的图像序列文件。",
			},
			settings: {
				about: "音MAD助手是VEGAS Pro的音MAD扩展程序，旨在使VEGAS能够接受如MIDI序列文件等乐谱作为输入并自动生成音MAD的轨道。",
			},
		},
		empty: {
			disabled: {
				title: "已关闭{{name, lowercase}}",
				details: "启用以生成{{name, lowercase}}",
			},
			ytpEnabled: {
				partial: {
					title: "已启用YTP，其它相关参数均不可用",
					details: "禁用YTP功能以使用并调整其它参数",
				},
				fully: {
					title: "已启用YTP，无法使用{{feature, lowercase}}功能",
					details: "禁用YTP功能以使用{{feature, lowercase}}功能",
				},
				disableYtp: "禁用YTP",
				gotoYtp: "转到YTP",
			},
		},
		preset: "预设",
	},
	csharp: {
		mainDock: {
			toolTip: {
				importToHere: "导入 %1",
			},
		},
		coreWebView: {
			menuItem: {
				delete: "删除(&D)",
			},
		},
		contentDialog: {
			button: {
				ok: "确定",
				cancel: "取消",
				close: "关闭",
				learnMore: "了解更多",
			},
			expander: {
				expandDetails: "展开详细信息",
				collapseDetails: "收起详细信息",
			},
			showError: {
				title: "错误",
			},
		},
		wrongOpeningMethod: {
			script: {
				title: "嘿，你把该扩展程序放错了位置！",
				content: "新版的Otomad Helper是一个扩展程序。与旧版不同，它不是一个脚本。\n\n请将该扩展移动到VEGAS的Application Extensions目录，而不是Script Menu目录。\n\n位置：",
			},
		},
	},
} as const satisfies LocaleIdentifiers;
