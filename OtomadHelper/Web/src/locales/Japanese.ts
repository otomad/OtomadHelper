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
			track: "トラック",
			mosh: "モッシュ",
		},
		source: {
			startTime: {},
		},
		infoBar: {},
		subheaders: {},
		score: {
			bpm: {},
			constraint: {},
		},
		stream: {
			stretch: {},
			legato: {},
			transformMethod: {},
			tuning: {
				prelisten: {
					waveform: {},
				},
			},
		},
		track: {
			legato: {},
			clear: {},
		},
		ytp: {},
		mosh: {},
		tools: {},
		selectInfo: {},
		prve: {
			classes: {},
		},
		pixelScaling: {},
		settings: {
			about: {},
			language: {
				_: "言語",
				en: "英語",
				"zh-CN": "簡体字中国語",
				ja: "日本語",
				vi: "ベトナム語",
			},
			appearance: {
				colorScheme: {},
			},
			config: {},
			dev: {},
		},
		descriptions: {
			source: {},
			score: {},
			stream: {
				noLengthening: {},
				tuning: {},
				effects: {},
			},
			track: {},
			sonar: {},
			shupelunker: {},
			ytp: {},
			mosh: {
				normal: "データモッシュは素材に損傷を与えてグリッチ効果を作成する技術です。",
				glitchy: "繝ﾃﾞ繧ｰﾀ縺ﾓ薙ｯ橸｢ｼ縲ｭ･ﾚよ素木オﾚﾆ才員傷を与ぇτ勹″⺉⺍于交力果をイ乍成すゑ才支彳朮テτ″す。",
				additional: "ビデオ・アートでは、2つの映像がインターリーブされ、中間フレームが2つの別々のソースから補間される、「データモッシュ」という手法がある。これは、それぞれのビデオコーデックが動きや色情報をどのように処理するかの違いを利用する。",
			},
			tools: {
				converters: {},
			},
			staff: {},
			pixelScaling: {},
			settings: {},
		},
		empty: {
			disabled: {},
			ytpEnabled: {},
		},
	},
	csharp: {
		importToHere: "インポート %1",
	},
} as const satisfies LocaleIdentifiers;
