import type { LocaleIdentifiers } from "./types";

const Vietnamese = {
	translation: {
		metadata: {
			__translator__: "Cyahega",
			name: "Tiếng Việt",
			culture: "vi-VN",
		},
		titles: {
			home: "Trang chủ",
			source: "Nguồn",
			score: "Bản nhạc",
			audio: "Âm thanh",
			visual: "Thị giác",
			track: "Theo dõi",
			sonar: "Sóng âm",
			lyrics: "Thơ trữ tình",
			shupelunker: "Shupelunker",
			shupelunker_full: "Chiến thuật Shupelunker",
			ytp: "YTP",
			ytp_full: "YouTube Poop",
			mosh: "Mosh",
			mosh_full: "Datamosh",
			tools: "Công cụ",
			settings: "Thiết lập",
			prve: "Hiệu ứng nhịp điệu PV Visual",
			staff: "Nhân viên Visualizer",
		},
		settings: {
			language: {
				_: "Ngôn ngữ",
				en: "Tiếng Anh",
				"zh-CN": "Tiếng Trung - giản thể",
				ja: "Tiếng Nhật",
				vi: "Tiếng Việt",
			},
		},
		descriptions: {
			mosh: {
				normal: "Datamosh là một kĩ thuật làm biến dạng video để tạo hiệu ứng glitch.",
				// cspell:disable-next-line
				glitchy: addWjBetweenEachChar("])4t4m0sh |_4` m0^.t kj~ thu4^.t |_4`m |313^'n ])4.ng vj])30 +)3^? t4.0 hj3^.u u\"ng g|1tch."),
				additional: "Trong nghệ thuật video, một kỹ thuật được sử dụng là khai thác dữ liệu. Hai trong số các video được xen kẽ, vì vậy khung hình trung gian được nội suy từ hai nguồn riêng biệt. Và tận dụng sự khác biệt trong cách codec video độc lập xử lý thông tin chuyển động và màu sắc.",
			},
		},
	},
}/*  as const satisfies LocaleIdentifiers */;

export default Vietnamese;
