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
			preferredTrack: {},
			afterCompletion: {},
			randomOffsetForTracks: "Sử dụng độ lệnh ngẫu nhiên cho các track khác nhau",
		},
		on: "Bật",
		off: "Tắt",
		custom: "Tùy chỉnh",
		enabled: "Đã bật",
		enable: "Kích hoạt",
		learnMore: "Tìm hiểu thêm",
		condition: "Điều kiện",
		underConstruction: "Đang trong quá trình phát triển…",
		allFiles: "Tất cả các file",
		systemDefault: "Mặc định hệ thống",
		complete: "Hoàn tất",
		save: "Lưu",
		auto: "Tự động",
		back: "Trở lại",
		navigation: "Điều hướng",
		selectAll: "Chọn tất cả",
		invertSelection: "Đảo lựa chọn",
		infoBar: {
			warning: "Nhắc nhở",
		},
		selectionMode: {
			single: "Đơn lẻ",
			multiple: "Đa chọn",
		},
		subheaders: {
			moreOptions: "Tùy chọn khác",
			advanced: "Nâng cao",
			config: "Thiết Lập",
			parameters: "Thông số",
			effects: "Hiệu ứng",
		},
		units: {},
		score: {
			midi: "MIDI",
			midiFile: "File trình tự MIDI",
			st: "Văn bản trình tự",
			stFile: "Tất cả các file trình tự văn bản được hỗ trợ",
			ustFile: "File trình tự văn bản UTAU/OpenUTAU",
			vsqFile: "File trình tự Vocaloid",
			bpm: {},
			constraint: {},
			pan: {},
		},
		stream: {
			stretch: {},
			noLengthening: {},
			legato: {},
			transformMethod: {},
			playingTechniques: {
				glissando: {},
				appoggiatura: {},
				arpeggio: {},
			},
			tuning: {
				tuningMethod: {},
				stretchAttributes: {},
				alternativeForExceedsTheRange: {},
				prelisten: {
					waveform: {},
				},
			},
			mapping: {},
			preset: {},
			parameters: {},
		},
		track: {
			legato: {},
			clear: {},
		},
		sonar: {},
		lyrics: {
			sampleLyrics: "Gà lẩu cay",
			karaoke: {},
			pitchNotation: {},
		},
		shupelunker: {
			affix: {},
			unallocated: {},
		},
		ytp: {},
		mosh: {},
		tools: {},
		selectInfo: {},
		prve: {
			control: {},
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
				colorScheme: {
					auto: "Tự động",
				},
			},
			config: {},
			dev: {},
		},
		descriptions: {
			source: {
				preferredTrack: {
					belowAdjustmentTracks: {},
				},
			},
			score: {},
			stream: {
				playingTechniques: {
					glissando: {},
				},
				tuning: {
					tuningMethod: {},
					prelisten: {},
				},
				effects: {},
				mapping: {},
				preset: {},
			},
			track: {},
			sonar: {},
			lyrics: {
				karaoke: {},
				pitchNotation: {},
			},
			shupelunker: {
				unallocated: {},
			},
			ytp: {},
			mosh: {
				normal: "Datamosh là một kĩ thuật làm biến dạng video để tạo hiệu ứng glitch.",
				glitchy: "])4t4m0sh |_4ˋ m0^.t kj~ thu4^.t |_4ˋm |313^'n ])4.ng vj])30 +)3^? t4.0 hj3^.u u\"ng g|1tch.",
			},
			tools: {
				converters: {},
			},
			staff: {},
			prve: {
				control: {},
			},
			pixelScaling: {},
			settings: {},
		},
		empty: {
			disabled: {},
			ytpEnabled: {
				partial: {},
				fully: {},
			},
		},
	},
	csharp: {
		mainDock: {
			toolTip: {
				importToHere: "Nhập %1",
			},
		},
		coreWebView: {
			menuItem: {
				delete: "&Xoá",
			},
		},
		contentDialog: {
			button: {
				ok: "&OK",
				cancel: "&Huỷ",
				close: "&Đóng",
			},
			expander: {
				expandDetails: "Mở rộng chi tiết",
				collapseDetails: "Thu nhỏ chi tiết",
			},
			showError: {
				title: "Lỗi",
			},
		},
		wrongOpeningMethod: {
			script: {},
		},
	},
} as const satisfies LocaleIdentifiers;
