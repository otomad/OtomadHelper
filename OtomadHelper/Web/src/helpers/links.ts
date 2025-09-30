import crowdinBadgeApiLink from "./links_crowdin-badge-api";

const Links = {
	otomadHelper: {
		documentation: fallbackWithLocale({
			en: "https://otomadhelper.readthedocs.io/",
			"zh-CN": "https://otomadhelper.readthedocs.io/zh-cn",
		}),
		repository: "https://github.com/otomad/OtomadHelper",
		changelog: "https://github.com/otomad/OtomadHelper/releases",
		issues: "https://github.com/otomad/OtomadHelper/issues",
	},
	helpV4: {
		chinese: {
			documentation_chaosinism_v0_1: "https://www.bilibili.com/read/cv392013",
			troubleshooting_chaosinism_v0_1: "https://www.bilibili.com/read/cv495309",
			tutorialVideo_chaosinism_v0_1: "https://www.bilibili.com/video/av22226321",
			documentation_staffVisualizer_chaosinism_v0_1: "https://www.bilibili.com/read/cv392013",
			releaseNotes_v4_9_25_0: "http://www.bilibili.com/read/cv13335178",
			releaseNotes_v4_10_17_0: "https://www.bilibili.com/read/cv13614419",
			tutorialVideo_v4_26_14_0: "https://www.bilibili.com/video/av613241077",
		},
		english: {
			documentation_evauation: "https://docs.google.com/document/d/1PEkh0_WFDLUAYGD-YzIDNXUQiAKqogEvpuRQhfqz9ng",
			tutorialVideo_ytpPlus: "https://www.youtube.com/watch?v=_zqUvTr-Y1I", // 2019/07/21
			tutorialVideo_datamosh_delthas_v1_4_0: "https://www.youtube.com/watch?v=6D2lW6H0bb8", // 2020/09/24
			tutorialVideo_greenBean_v4_16_4_0: "https://www.youtube.com/watch?v=fVWfUAf063o", // 2022/05/09
			tutorialVideo_cassidy_v4_16_4_0: "https://www.youtube.com/watch?v=8vSpzgL_86A", // 2022/05/11
			exploreVisualEffects_v4_23_11_0: "https://www.youtube.com/watch?v=cY2Qa3Owetw", // 2022/11/19
			tutorialVideo_v4_26_14_0: "https://www.youtube.com/watch?v=amDtqY_HsGM", // 2023/04/27
		},
		vietnamese: {
			tutorialVideo_cyahega_v4_26_14_0: "https://www.youtube.com/watch?v=vLqYIaw0hMc", // 2023/03/11
		},
	},
	api: {
		versionTagGitHubApi: "https://api.github.com/repos/otomad/OtomadHelper/releases/latest", // 60 times per hour maximum.
		versionTagGitHubRaw: "https://raw.githubusercontent.com/otomad/OtomadHelper/webview2/version.txt",
	},
	crowdin: {
		contributeTranslation: fallbackWithLocale({
			en: "https://crowdin.com/project/otomadhelper",
			"zh-CN": "https://zh.crowdin.com/project/otomadhelper",
			ja: "https://ja.crowdin.com/project/otomadhelper",
		}),
		badgeApi: crowdinBadgeApiLink,
	},
	gpl3: "https://www.gnu.org/licenses/gpl-3.0.html",
	react: "https://github.com/facebook/react",
} as const;

export default Links;
