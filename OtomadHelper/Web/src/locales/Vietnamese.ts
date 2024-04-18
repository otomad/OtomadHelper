import type { LocaleIdentifiers } from "./types";

export default {
	javascript: {
		metadata: {
			__translator__: "Cyahega",
			name: "Tiếng Việt",
			culture: "vi-VN",
		},
		colon: ": ",
		semicolon: "; ",
		titles: {
			home: "Trang chủ",
			source: "Nguồn",
			score: "Điểm",
			audio: "Âm thanh",
			visual: "Hình ảnh",
			sonar: "Sonar",
			lyrics: "Lời hát",
			shupelunker: "Shupelunker",
			shupelunker_full: "Kỹ thuật Shupelunker",
			ytp: "YTP",
			ytp_full: "YouTube Poop",
			mosh_full: "Datamosh",
			tools: "Công cụ",
			settings: "Cài đặt",
			prve: "Hiệu ứng nhịp điệu hình ảnh PV",
			staff: "Staff Visualizer",
			pixelScaling: "Tỷ lệ pixel",
			track: "Các track",
			mosh: "Mosh",
		},
		source: {
			trackEvent: "Track event",
			projectMedia: "Đa phương tiện của dự án",
			browseFile: "Duyệt tập tin",
			trim: "Cắt ngắn",
			startTime: {
				_: "Thời gian bắt đầu",
				projectStart: "Bắt đầu dự án",
				cursor: "Con trỏ",
			},
		},
		infoBar: {},
		selectionMode: {},
		subheaders: {},
		score: {
			bpm: {},
			constraint: {},
			pan: {},
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
				_: "Ngôn ngữ",
				en: "Tiếng Anh",
				"zh-CN": "Tiếng Trung - giản thể",
				ja: "Tiếng Nhật",
				vi: "Tiếng Việt",
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
				normal: "Datamosh là một kĩ thuật làm biến dạng video để tạo hiệu ứng glitch.",
				glitchy: "])4t4m0sh |_4ˋ m0^.t kj~ thu4^.t |_4ˋm |313^'n ])4.ng vj])30 +)3^? t4.0 hj3^.u u\"ng g|1tch.",
				additional: "Trong nghệ thuật video, một kỹ thuật được sử dụng là khai thác dữ liệu. Hai trong số các video được xen kẽ, vì vậy khung hình trung gian được nội suy từ hai nguồn riêng biệt. Và tận dụng sự khác biệt trong cách codec video độc lập xử lý thông tin chuyển động và màu sắc.",
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
		importToHere: "Nhập %1",
	},
} as const satisfies LocaleIdentifiers;
