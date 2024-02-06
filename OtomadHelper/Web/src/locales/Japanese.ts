import type { LocaleIdentifiers } from "./types";

const Japanese = {
	translation: {
		titles: {
			home: "ホーム",
			source: "素材",
			score: "譜面",
			audio: "音声",
			visual: "映像",
			track: "トラック",
			sonar: "ソナー",
			lyrics: "歌詞",
			shupelunker: "シュペランカー",
			shupelunker_full: "シュペランカー戦法",
			ytp: "YTP",
			ytp_full: "YouTube Poop",
			mosh: "モッシュ",
			mosh_full: "データモッシュ",
			tools: "ツール",
			settings: "設定",
			prve: "映像リズム視覚効果",
			staff: "五線譜視覚化",
		},
		settings: {
			language: {
				_: "言語",
				en: "英語",
				"zh-CN": "簡体字中国語",
				ja: "日本語",
			},
		},
	},
}/*  as const satisfies LocaleIdentifiers */;

export default Japanese;
