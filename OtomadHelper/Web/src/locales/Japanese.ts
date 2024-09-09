import type { LocaleIdentifiers } from "./types";

export default {
	javascript: {
		metadata: {
			__translator__: "",
			name: "日本語",
			culture: "ja-JP",
		},
		colon: "：",
		semicolon: "；",
		titles: {
			home: "ホーム",
			source: "素材",
			score: "譜面",
			audio: "音声",
			visual: "映像",
			sonar: "ソナー",
			lyrics: "歌詞",
			shupelunker: "シュペランカー",
			shupelunker_full: "シュペランカー戦法",
			ytp: "YTP",
			ytp_full: "YouTube Poop",
			mosh_full: "データモッシュ",
			tools: "ツール",
			settings: "設定",
			prve: "映像リズム視覚効果",
			staff: "五線譜視覚化",
			pixelScaling: "ピクセルスケーリング",
			parameters: "パラメータ",
			track: "トラック",
			mosh: "モッシュ",
			effect: "効果",
		},
		source: {
			trackEvent: "予定を追跡",
			projectMedia: "メディアをプロジェクト",
			browseFile: "ファイルを参照",
			trim: "切り落とし",
			startTime: {
				_: "開始日時",
				projectStart: "プロジェクトの開始",
				cursor: "カーソル",
			},
			preferredTrack: {
				_: "優先トラック",
				index: "トラックの優先インデックス",
				top: "最上部",
				ordinal: "{{count, ordinal}}トラックの下",
				belowAdjustmentTracks: "1つ以上の調整トラックがこのトラックの下にある場合は、次のトラックを選択してください。",
				newTrack: "新しいトラック",
			},
			afterCompletion: {
				_: "完了後",
				removeSourceClips: "ソーストラッククリップを削除",
				selectSourceClips: "ソーストラッククリップを選択",
				selectGeneratedAudioClips: "生成されたすべてのオーディオクリップを選択",
				selectGeneratedVideoClips: "生成されたすべてのビデオクリップを選択",
			},
			blindBox: {
				_: "素材盲の箱",
				track: "各トラックまたはチャンネルに対して",
				marker: "マーカーごとに1回ずつ切り替える",
			},
		},
		on: "オン",
		off: "オフ",
		custom: "カスタム",
		enabled: "有効",
		enable: "有効にする",
		learnMore: "もっと詳しく",
		condition: "条件",
		underConstruction: "工事中⋯⋯",
		allFiles: "すべてのファイル",
		systemDefault: "システムのデフォルト",
		complete: "完了",
		dragToImport: "ドラッグ＆ドロップで {{item, lowercase}} としてインポート",
		save: "保存",
		auto: "自動",
		back: "戻る",
		navigation: "ナビ",
		selectAll: "すべて選択",
		invertSelection: "選択を反転",
		variableBeginWith: "{{first, lowercase}} からの変数",
		reset: "リセット",
		new: "新規作成",
		etc: "{{examples}}など",
		offset: "オフセット",
		curve: "補間曲線",
		unselected: "選択を解除",
		topPriority: "最初に {{item, capitalize}}",
		infoBar: {
			warning: "警告",
		},
		selectionMode: {
			single: "Single",
			multiple: "Multiple",
		},
		subheaders: {
			moreOptions: "その他のオプション",
			advanced: "高度な設定",
			config: "設定",
			parameters: "パラメータ",
			seeAlso: "関連項目",
		},
		units: {
			milliseconds: "ms",
			percents: "%",
			pixels: "px",
			beatsPerMinute: "BPM",
			semitones: "st",
			degrees: "°",
		},
		fileFormats: {
			allFiles: "すべてのファイル",
			txt: "文書ドキュメント",
			midi: "MIDI シーケンスファイル",
			singthesis: "サポートされているすべてのシングルテーゼ(テキスト合成/音声合成ソフトウェアプロジェクト) ファイル",
			ust: "UTAU/OpenUTAU シーケンステキスト ファイル",
			vsq: "Vocaloid シーケンスファイル",
		},
		score: {
			midi: "MIDI",
			singthesis: "Singthesis",
			refOtherTracks: "他の曲を参照",
			tts: "テキストから発話する",
			pureNotes: "純粋なノート",
			encoding: "エンコード",
			bpm: {
				_: "BPM",
				variableMidi: "MIDIテンポを変更する",
				constantMidi: "定数MIDIテンポ",
				project: "プロジェクトのテンポ",
			},
			timeSignature: "拍子記号",
			constraint: {
				_: "音符の長さを拘束する",
				none: "拘束なし",
				max: "最大長さ",
				fixed: "固定長さ",
			},
			noteCount: "ノート数",
			beginNote: "ノートの開始",
			pan: {
				_: "パン",
				left: "左",
				right: "右",
				center: "中央揃え",
			},
			instrument: "機器",
			drumKit: "ドラムキット",
			musicalTrack: "トラック",
		},
		stream: {
			preview: "プレビュー",
			stretch: {
				_: "ストレッチ",
				noStretching: "ストレッチなし",
				flexingAndExtending: "屈曲と拡張",
				extendingOnly: "拡張のみ",
				flexingOnly: "曲げのみ",
			},
			loop: "ループ",
			normalize: "正規化",
			staticVisual: "静的ビジュアル",
			noLengthening: {
				_: "延長しない",
				lengthenable: "Lengthenable",
				freezeEndFrames: "終了フレームを固定",
				trimEndFrames: "終了フレームをトリムする",
				splitThenFreeze: "分割して凍結する",
				freezeToGray: "グレーに固定",
				freezeToPreset: "プリセットに固定",
			},
			legato: {
				_: "Legato",
				portato: "Portato",
				upToOneBeat: "1ビートまで",
				upToOneBar: "1小節まで",
				unlimited: "無制限です",
			},
			multitrackForChords: "コードのマルチラック",
			createGroups: "グループを作成",
			autoPan: "オート パン",
			noTimeRemapping: "再マッピングに時間がありません",
			transformMethod: {
				_: "変換方法",
				panCrop: "パン/トリミング",
				pictureInPicture: "Picture in Picture",
				transformOfx: "TransformOFX",
			},
			playingTechniques: {
				_: "プレーの技術",
				applyCustomPreset: "カスタムプリセットを適用する",
				glissando: {
					_: "Glissando",
					swirl: "渦巻き",
					pingpong: "ピンポン",
					swirlAmount: "渦巻きの量",
				},
				appoggiatura: {
					_: "Appoggiatura",
				},
				arpeggio: {
					_: "Arpeggio",
				},
			},
			tuning: {
				_: "チューニング",
				tuningMethod: {
					_: "チューニング方法",
					noTuning: "チューニングなし",
					pitchShift: "ピッチシフト",
					elastic: "Élastique",
					classic: "クラシック",
					scaleless: "スケールレス",
				},
				stretchAttributes: {
					_: "ストレッチ属性",
				},
				alternativeForExceedsTheRange: {
					_: "範囲を超えた場合",
					multiple: "複数のオーディオエフェクトプラグインを使用",
					plugin: "ピッチシフトオーディオエフェクトプラグインに切り替える",
					octave: "高オクターブまたは低オクターブです",
					dock: "端にドックする",
					silent: "サイレントモード",
				},
				resample: "リサンプ",
				preserveFormant: "フォルマントを保存する",
				basePitch: "ベース ピッチ",
				prelisten: {
					_: "プレ再生",
					basePitch: "プレリッスンのピッチ:",
					audio: "Prelisten audio",
					engine: "エンジン",
					waveform: {
						_: "波形",
						sinusoid: "Sinusoid",
						triangle: "Triangle",
						square: "矩形波",
						sawtooth: "ノコギリ波",
					},
					duration: "期間",
					adjustAudioToBasePitch: "音声をベースピッチに調整",
				},
			},
			mapping: {
				_: "マッピング",
				velocity: "Velocity",
				pitch: "Pitch",
				duration: "期間",
				pan: "パン",
				progress: "進捗状況",
			},
			preset: {
				add: "カスタムプリセットに追加",
			},
			parameters: {
				copyFromAnotherParameterScheme: "別のパラメータスキームからここにコピー",
				copyAttributesFromSelectedClip: "選択したクリップから属性をコピー",
			},
		},
		track: {
			layout: "レイアウト",
			grid: "グリッドレイアウト",
			box3d: "3D ボックスレイアウト",
			gradient: "グラデーショントラック",
			applyToSelectedTracks: "選択したトラックに適用",
			resetAllLayouts: "すべてリセット",
			legato: {
				_: "Legato",
			},
			clear: {
				_: "クリア",
				motion: "トラックの動きを消去",
				effect: "トラックエフェクトをクリア",
			},
		},
		sonar: {
			separateDrums: "ドラムを別々にする",
			differenceCompositeMode: "差分コンポジットモード",
			shadow: "影",
			graphs: "グラフ",
		},
		lyrics: {
			useStaticText: "静的テキストから直接字幕を挿入する",
			sampleLyrics: "鶏のスパイシー鍋",
			presetTemplate: "プリセットテンプレート",
			enableMode: "{{mode, lowercase}} モードを有効にする",
			karaoke: {
				_: "Karaoke",
				futureFill: "テキストの塗りつぶし色を再生する",
				pastFill: "再生中のテキストの塗りつぶし色",
			},
			pitchNotation: {
				_: "ピッチ表記法",
				type: "ピッチ表記タイプ",
				scientific: "科学的ピッチ表記法",
				helmholtz: "ヘルムホルツピッチ表示",
				solfeggio: "Solfeggio Syllable",
				numbered: "番号付けされた楽譜の表示",
				gongche: "Gongche表記法",
			},
		},
		shupelunker: {
			affix: {
				_: "ピッチ接着位置を一致させる",
				prefix: "プレフィックス",
				suffix: "Suffix",
			},
			unallocated: {
				_: "未割り当て",
				octaves: "Octaves",
				lowerNeighbors: "下の隣人",
				higherNeighbors: "より高い隣人へ",
				default: "既定の完全な範囲",
			},
			keyMappingZones: "キーマッピングゾーン",
		},
		ytp: {
			constraint: "拘束の長さ",
			clips: "クリップ",
			effects: "YTPエフェクト",
		},
		mosh: {
			datamosh: "データモッシュ",
			datamix: "データミックス",
			layer: "レイヤー",
			render: "レンダリング",
			scramble: "スクランブル",
			automator: "Automator",
			stutter: "Stutter",
			shake: "シェイクする",
			specifyClipsFolder: "データモッシュ クリップフォルダーを指定します",
			install: "データモッシュ拡張パックをダウンロード",
		},
		tools: {
			flow: "フロー",
			selectorAndReplacer: "選択と置換器",
			normalizer: "ノーマライザー",
			subtitles: "字幕",
			effector: "エフェクト",
			fader: "Fader",
			exportScore: "点数をエクスポート",
			converters: "コンバータ",
			clawer: "Clawer",
		},
		selectInfo: {
			trackEventOnlyOne: "あなたはトラックイベントを1つだけ選択する必要があります。それ以上、それ以下ではありません。",
			videoEventOnlyOne: "あなたは1つと1つのビデオトラックのイベントを選択する必要があります。それ以上、そしてそれ以上ではありません。",
			audioEventOnlyOne: "オーディオトラックのイベントを1つとして選択する必要があります。それ以上ではありません。",
			source: "{{count}}のメディアソースが選択されました",
			track: "{{count}}トラックが選択されました",
			videoTrack: "{{count}}ビデオトラックが選択されました",
			audioTrack: "{{count}}オーディオトラックが選択されました",
			trackEvent: "{{count}}トラックイベントが選択されました",
			videoEvent: "{{count}}ビデオトラックイベントが選択されました",
			audioEvent: "{{count}}オーディオトラックイベントが選択されました",
		},
		prve: {
			control: {
				general: "全般",
				general_full: "一般的なコントロール",
				samePitch: "同じピッチです",
				samePitch_full: "同じピッチコントロール",
				differentSyllables: "異なる音節",
				differentSyllables_full: "異なる音節制御",
			},
			classes: {
				_: "クラス",
				flip: "フリップクラス",
				rotation: "回転クラス",
				scale: "スケールクラス",
				mirror: "ミラークラス",
				invert: "クラスを反転",
				hue: "色相クラス",
				chromatic: "モノクロクラス",
				time: "タイムクラス",
				time2: "時間クラス2",
				ec: "拡張と圧縮クラス",
				swing: "スウィングクラス",
				blur: "ぼかしクラス",
				wipe: "クラスの消去",
			},
			effects: {
				normal: "標準",
				hFlip: "水平方向の反転",
				vFlip: "垂直反転",
				ccwFlip: "反時計回りに反転",
				cwFlip: "Clockwise Flip",
				ccwRotate: "反時計回りの回転",
				cwRotate: "時計回りの回転",
				turned: "ターンしました",
				zoomOutIn: "ズームイン",
				hMirror: "水平ミラー",
				vMirror: "垂直ミラー",
				ccwMirror: "反時計回りミラー",
				cwMirror: "時計回りのミラー",
				negative: "負の値",
				luminInvert: "照度を反転",
				hueInvert: "Hue Invert",
				chromatic: "クロマティックとモノクローム",
				pingpong: "Ping-Pong Effect",
				whirl: "愛の魔法、スピン、円の中で回転する",
				sharpRewind: "シャープ巻き戻し",
				wobblePeriod: "ぐらつきの期間",
				vExpansion: "垂直拡張",
				vExpansionBounce: "バウンス付き垂直拡張",
				vCompression: "垂直圧縮",
				vCompressionBounce: "バウンス付き垂直圧縮",
				vBounce: "垂直バウンス",
				slantDown: "Slant Down",
				slantUp: "傾斜上",
				puyo: "Puyo Puyo",
				pendulum: "振り子を再生",
				gaussianBlur: "ガウスぼかし（ぼかし）",
				radialBlur: "放射状ぼかし（ぼかし）",
				wipeRight: "右端で消去",
				splitVOut: "垂直方向に分割",
				stepChangeHue: "{{count}} ステップの色づけについて",
			},
		},
		pixelScaling: {
			scaleFactor: "拡大率",
			replaceSourceMedia: "ソースメディアを置き換え",
		},
		settings: {
			about: {
				checkForUpdates: "アップデートを確認",
				repositoryLink: "リポジトリリンク",
				documentation: "ドキュメント",
				translation: "翻訳に貢献",
				feedback: "フィードバック",
				version: "バージョン",
				author: "作成者",
				__author__: "蘭澈 祈",
				originalAuthor: "オリジナルの著者",
				__originalAuthor__: "Chaosinism",
				translator: "翻訳",
			},
			language: {
				_: "言語",
				en: "英語",
				"zh-CN": "簡体字中国語",
				ja: "日本語",
				vi: "ベトナム語",
			},
			appearance: {
				_: "外観",
				colorScheme: {
					_: "配色設定",
					light: "ライト",
					dark: "ダーク",
					auto: "自動",
				},
				uiScale: "UIスケール",
			},
			config: {
				hideUsageTips: "使用ヒントを非表示",
			},
			dev: {
				_: "開発",
				devMode: "開発者モード",
				rtl: "レイアウト方向右から左へ",
			},
		},
		descriptions: {
			condition: "この設定をいつ適用するかを指定します",
			curve: "キーフレームタイプの補間曲線を指定します。",
			source: {
				trim: "指定したソースのインポイントまたはアウトポイントタイムを調整します。",
				startTime: "プロジェクトから生成を開始するタイミングを指定します",
				preferredTrack: {
					_: "生成する既存のトラックを指定できます(マルチトラックを除く)",
					belowAdjustmentTracks: {
						versionRequest: "注意: この機能はVEGAS Pro 19以上が必要です。現在のバージョンは {{version}}です。",
					},
				},
				blindBox: {
					_: "ソースのポイントでランダムに使用します。\nこれは、ランダムに選択されたソースクリップが異なるベースピッチを持つ可能性があります。 娯楽用の面白い動画の作成にのみ役立ちますし、高度な動画の作成にはほとんど使用されません。",
					track: "トラックまたはチャンネルはスコア構成によって異なります",
					marker: "スコアにマーカーがある場合、ソースのインポイントは一度変更されます。 複数のマーカーに同じ名前が空でない場合、ソースのポイントで同じ名前が使用されます。",
				},
			},
			score: {
				trim: "スコアの生成時間範囲をインターセプトします",
				bpm: "分あたりの拍数を指定します",
				constraint: "スコアからのノートの出力長さを制御します",
				encoding: "ファイルの読み込み時に使用するテキストエンコーディングを指定します",
				ytpEnabled: "YTP機能が有効になっているため、スコアに依存しないので、ここでのすべての設定は効果がありません。",
			},
			stream: {
				stretch: "クリップの長さを変更する代わりに、クリップを伸ばします。",
				loop: "クリップがソースメディアの最後まで長くなると再生が開始されます",
				normalize: "オーディオを標準化し、静かなオーディオに便利です",
				staticVisual: "クリップの先頭にあるフレームをフリーズします。",
				noLengthening: "一部のノートが長すぎる場合、トリミング時間を超えてパートを誤って再生しないようにクリップのアウトポイントでフリーズしようとします。",
				legato: "ノート間のギャップを埋めます",
				noLengtheningAndLegatoConflictInAudio: "オーディオの Legato との長時間の競合がありません。同時に有効にすることはできません！",
				multitrackForChords: "コード用に複数のトラックを作成",
				createGroups: "ビデオクリップとオーディオクリップのグループを1つのノートで表します。",
				autoPan: "エンベロープオートメーションを使用してオーディオをパンする",
				noTimeRemapping: "ノートが発生した場合、クリップはポイント時間をリセットしません。 再生を続けますソースにエフェクトを適用する場合に便利です",
				transformMethod: "変換キーフレームを適用するためのターゲットプロパティの優先順位を指定します",
				playingTechniques: {
					glissando: {
						_: "ピッチベンド、スライド、またはglissandiを再生するときにエフェクトを生成します。",
						swirlAmount: "旋回ツイスト振幅の大きさを指定します",
					},
					appoggiatura: {
						_: "appoggiaturasを再生するときに効果を生成します。\n連続して1〜2個の音符がある場合、それらはappoggiaturasとみなされます。",
					},
					arpeggio: {
						_: "アルペジオをプレイするときに効果を生成します。デフォルトはマイナスです。\n連続して音符が3つ以上あるいは短い場合は、アルペジオとみなされます。",
						negative: "通常、アルペジオを表すために負の値を使用します",
					},
				},
				tuning: {
					tuningMethod: {
						noTuning: "ピッチエフェクトなし",
						pitchShift: "ピッチオーディオエフェクトプラグインを使用します。ピッチオーディオエフェクトは低速で、再生レートを変更する際には効果がありません。 しかし、範囲外で動作することができ、VEGAS Pro 15でスクリプトAPIにアクセスできる唯一の方法です。",
						elastic: "Elastic Pitch Change メソッドを使用します。+/- キーを直接押すデフォルトのメソッドです。範囲外では動作しません。",
						classic: "範囲外では動作しないクラシックピッチ変更メソッドを使用します。VEGAS Pro 8で利用可能な唯一の方法です。",
						scaleless: "ストレッチとピッチをロックし、ストレッチを変更してノートピッチに関係なく対応するピッチを取得します。",
					},
					stretchAttributes: "選択したチューニング方法の詳細設定",
					alternativeForExceedsTheRange: "別の方法で範囲外のメモを処理",
					resample: "ストレッチとピッチをロックし、ストレッチを調整してピッチを変更します",
					preserveFormant: "チューニング中は音声音の特性を維持します",
					basePitch: "オーディオクリップのベースピッチを指定します",
					prelisten: {
						_: "クリップとベースピッチを比較し、簡単に調整できます",
						adjustAudioToBasePitch: "クリップのオーディオを逆の方法ではなくベースピッチに合わせて、古いスタイルのリミックスに便利です。",
					},
				},
				effects: {
					prve: "ビジュアルのリズムを上げます。",
					staff: "カスタムパターンをメモとして使用し、ピアノ譜面と同様のビジュアルを描きます。",
					pixelScaling: "最も近い近傍のハードエッジ増幅と補間アルゴリズムを使用してスケーリングします",
				},
				mapping: {
					_: "指定されたアイテムにノートのプロパティをマップ",
				},
				preset: {
					_: "便宜のためにあらかじめ定義されたパラメータスキームを使用または保存します",
				},
			},
			track: {
				gradient: "レイアウトにグラデーションカラー効果を与えます。",
				legato: "トラッククリップ間のギャップを埋めます",
				resetLayout: "このレイアウトの状態をリセットして無効にする",
			},
			sonar: {
				_: "ソナーは、クッキーカッターの形を利用してビートスタイルのモーショングラフィックスを作成する視覚効果です。この機能を使用すると、スコア内のドラムキット（チャンネル10）のさまざまなパーカッションインストゥルメントに異なるシェイプやエフェクトをディスパッチできます。\n音MADの一般的なジャンルは、さまざまな形状を加えてモーショングラフィックス(MG)を作成することです。拡散した円やビートに基づいて視覚化することができますモーショングラフィックスは、グラフィックデザインとアニメーションデザインの間の製品です時間の流れに基づいた視覚的表現とビデオアートの一種ですモーショングラフィックスという用語は、時間の流れによって変形するグラフィックスを指します。モーグラフのみを含む単一の音MADビデオは、しばしば「グラフ」または「形」と題されます。",
				enabled: "スコアのアクティブトラックにドラムキットが含まれている場合、ソナー効果を有効にする",
				separateDrums: "各ドラムをそれぞれのトラックに置きます",
				differenceCompositeMode: "差分ブレンドで作成された曲を作成します。影絵の様式を表示します",
				shadow: "グラフに影を追加します",
			},
			lyrics: {
				_: "シーケンステキストなど、スコアに歌詞が含まれている場合、歌詞の字幕は自動的に生成されます。\n楽譜に歌詞がない場合でもピッチ表記を生成することができます。",
				presetTemplate: "歌詞のテンプレートとして使用するタイトルとテキストメディアジェネレーターを選択してください",
				karaoke: {
					_: "カラオケスタイルの字幕を使用します。色指標は現在のフレーズの進行状況を反映します",
					futureFill: "歌われていない歌詞の文字色を指定します。",
					pastFill: "歌われている歌詞の文字色を指定します。",
				},
				pitchNotation: {
					_: "現在のノートのピッチをテキストとして視覚化します",
					type: "世界には楽譜を表現する方法がいくつかあります。好きな楽譜を選んでください",
				},
			},
			shupelunker: {
				_: "シュペランカー戦法は非チューニング音MAD戦術です。メロディと同じピッチでソース（通常はボーカル）のクリップを使用してメロディを演奏するために使用されます。つまり音源クリップ自体がメロディに合うようにピッチされています\nクリップのピッチがメロディと一致しない場合、「韃靼戦法」が生成されます。調整されていないため、クリップがカットされている場所（通常はダイアログ）を選択できます。オーディオのストレッチと巻き戻しが頻繁に使用されるだけでなく、16〜64分の1の休憩ノートを追加します。制作中、クリップは歌う感覚のリズムに合わせられます。",
				unallocated: {
					_: "ソースがすべてのキーをカバーしていない場合、空き領域を埋める方法を指定します",
					octaves: "最も近い高オクターブまたは低いオクターブのクリップを使用します。これは最も優先度が高く、歌われたsolfeggiosのソースに便利です。",
					lowerNeighbors: "低い隣人を使用しますが、最も低い鍵は高い隣人から満たされます。これは高い隣人よりも優先度が高いです",
					higherNeighbors: "高い隣人を使用しますが、最も高いキーは下の隣人から満たされます",
					default: "すべての残りの空きをカバーするために、任意のアフィックスのない最初のクリップを使用します。これは最も優先度が低いです",
				},
			},
			ytp: {
				_: "YouTubeのプープ(YTP)は、YTPジャンルで知られている様々な効果を使用して無意味なビデオを作成するために使用されます。\nYTPは、リミックス文化自体にコメントするために、リミックス文化の最も低い技術的、美的基準を模倣し、モック不条理なリミックスで構成されるネオダダのアートフォームです。 これは、視聴者を混乱させるために、さまざまなビデオクリップから編集されたビデオリミックスで構成されています。 ソースはすべて無意味なクロスオーバー物語にまとめたり、奇妙に身振りをするキャラクターの単純に繰り返された映像にマッシュアップすることができます。",
				constraint: "生成するクリップの長さを制御します",
				clips: "生成するクリップの数を設定します",
				effects: "YTP の効果を指定します。",
			},
			mosh: {
				normal: "データモッシュは素材に損傷を与えてグリッチ効果を作成する技術です。",
				glitchy: "繝ﾃﾞ繧ｰﾀ縺ﾓ薙ｯ橸｢ｼ縲ｭ･ﾚよ素木オﾚﾆ才員傷を与ぇτ勹″⺉⺍于交力果をイ乍成すゑ才支彳朮テτ″す。",
				additional: "ビデオアートには、datamoshing と呼ばれるテクニックがあります。中間フレームが 2 つの別々のソースから補間されるように、2 つのビデオがインターリーブされています。 そして、別々のビデオコーデックがどのように動きと色情報を処理するかの違いを利用します。",
				datamosh: "ビデオをDatamoshes, 好ましくは、タイムライン上で動くビジュアルの多いです",
				datamix: "あるクリップの動きを別のクリップのビジュアルに適用します。",
				layer: "ビデオクリップを繰り返しコピーすることで多層化します",
				render: "非常に複雑なビデオ編集を含むタイムラインの一部をプリレンダリングし、1つのビデオクリップに置き換えます。",
				scramble: "クリップを多くのクリップフラグメントに分割し、それらをシャッフルします",
				automator: "ビデオクリップに追加したエフェクトごとに、ランダムなキーフレームを各フレームに自動的に追加します",
				stutter: "クリップを転送し、ランダムに反転させることでクリップを切り離します",
				shake: "パン/切り取りを使用してクリップを振ったり揺らしたりできます",
				notInstalled: "Datamosh Extension Pack はインストールされておらず、ダウンロードされるまで、完全な機能は利用できません。",
			},
			tools: {
				_: "これらのQoL機能により、YTPMVsを作成したり、セットアップしたりするのに少し苦痛を与えることができます。 以前のパラメータに調整する必要はありません。",
				flow: "ベジーの曲線を使用して素晴らしいアニメーションを作成します",
				selectorAndReplacer: "指定された条件に一致するすべてのクリップを検索して選択し、指定された新しいクリップに置き換えることができます。",
				normalizer: "選択したすべてのオーディオクリップの音量を標準化します。",
				subtitles: "タイトルとテキストのプリセットをプリセットし、ここに複数行のテキストを追加します。",
				effector: "選択したクリップにエフェクトを適用します",
				fader: "指定したルールを使用して、選択したクリップのゲイン（音量/不透明度）を調整します。",
				exportScore: "選択したトラック内のクリップをスコアシーケンスファイルにエクスポートします",
				converters: {
					tuningMethod: "選択したオーディオクリップのチューニングアルゴリズムを変更します",
					transformMethod: "選択したビデオクリップに適用される変換されたキーフレームのターゲットプロパティを変更します",
					clawer: "音楽のビートや拍子を変更して、面白い新しいリズムを作ります。",
				},
			},
			staff: {
				_: "Staff Visualizer は、スコアのメロディーに基づいてピアノの譜面シートと同様のビジュアルを描くために、カスタムパターンをノートとして使用するように設計されています。\nYouTuber@grantwoolardの動画を真似たビジュアルエフェクトスタイル。 音楽家のアバターを使ってクラシック音楽のスタッフシートを描きました",
			},
			prve: {
				control: {
					general: "YTPMVまたはSentence Mixingモードでは、連続したイベントやピッチの連続したイベント、同じ音節の通常の条件をコントロールします。\n他の別個のコントロールがオフの場合、ケースにも含まれます。",
					samePitch: "YTPMVまたはSentence Mixingモードでは、同じピッチの連続イベントのコントロールを分離します。",
					differentSyllables: "文混合モードでは、異なる音節の連続したイベントのコントロールを分離します。",
				},
				forceStretch: "現在使用しているPVリズムビジュアルエフェクトにはタイムクラスエフェクトが含まれています。 これはストレッチを強制的に\"$t\"に設定します(ストリーム)。 \"tretch.flexingAndExtending\") これらのエフェクトで、設定によって制御されていない場合",
			},
			pixelScaling: {
				_: "ピクセルスケーリング機能は、VEGAS Pan/Crop による滑らかなグラデーションアルゴリズムを使用して、元のピクセル画像の再スケーリングによるピクセルの歪みの問題を回避するために、ソースのピクセル画像を拡大することができます。 使用するには、FFmpegをシステム環境変数に追加するか、Datamosh Extension Packを直接インストールすることができます。\nピクセルスケーリング機能は、現在のプロジェクトサイズに合わせて、最も近い近傍補間アルゴリズムを使用してソースファイルを拡大するためにFFmpegを使用します。 そしてVEGASのソースメディアファイルを新しく生成されたメディアファイルに置き換えます。 新しく生成されたファイルは、拡張子「_Scaled」をその名前に追加することで識別されます。 この機能は、従来の方法でVEGASにインポートされたイメージシーケンスファイルを含む、任意の画像/ビデオファイル形式をサポートしています。",
			},
			settings: {
				about: "音MADヘルパー、VEGAS Pro用の音MADエクステンションVEGASがMIDIシーケンスファイルのようなスコアを入力として受け入れ、音MADトラックを自動的に生成できるように設計されています。",
			},
		},
		empty: {
			disabled: {
				title: "{{name, capitalize}}は無効です",
				details: "{{name, lowercase}}の生成を有効にする",
			},
			ytpEnabled: {
				partial: {
					title: "YTPを有効にしました。他の関連パラメータは利用できません。",
					details: "他のパラメータを使用したり調整するには、YTP機能を無効にしてください",
				},
				fully: {
					title: "YTPが有効になっており、 {{feature, lowercase}} 機能を使用できません",
					details: "{{feature, lowercase}} 機能を使用するにはYTP機能を無効にしてください",
				},
				disableYtp: "YTPを無効にする",
				gotoYtp: "YTPに移動",
			},
		},
		preset: "プリセット",
	},
	csharp: {
		mainDock: {
			toolTip: {
				importToHere: "インポート %1",
			},
		},
		coreWebView: {
			menuItem: {
				delete: "削除(&D)",
			},
		},
		contentDialog: {
			button: {
				ok: "&OK",
				cancel: "キャンセル",
				close: "閉じる",
				learnMore: "詳細(&L)",
			},
			expander: {
				expandDetails: "詳細を展開",
				collapseDetails: "詳細を隠す",
			},
			showError: {
				title: "エラー",
			},
		},
		wrongOpeningMethod: {
			script: {
				title: "拡張機能を外しました！",
				content: "Omad Helper の新バージョンは拡張機能です。旧バージョンとは異なり、スクリプトではありません。\n\nスクリプトメニューディレクトリの代わりにVEGASのApplication Extensionsディレクトリに拡張を移動してください。\n\n場所:",
			},
		},
	},
} as const satisfies LocaleIdentifiers;
