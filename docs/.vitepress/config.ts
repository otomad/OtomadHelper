import { defineConfig, type DefaultTheme } from "vitepress";
import i18nMacroPlugin from "./plugins/markdown-it/i18n-macro";
import underlinePlugin from "./plugins/markdown-it/underline";
import detailsHeadingPlugin from "./plugins/markdown-it/container-details-heading";
import containerImportantPlugin from "./plugins/markdown-it/container-important";
// @ts-ignore
import bracketedSpans from "markdown-it-bracketed-spans";
import { katex } from "@mdit/plugin-katex";
import { resolve } from "path";
import { join } from "path/posix";

const base = process.env.READTHEDOCS_CANONICAL_URL
	? new URL(process.env.READTHEDOCS_CANONICAL_URL).pathname.replace(/\/$/, "")
	: "";
const withBase = (path: string) => join(base || "/", path);

// https://vitepress.dev/reference/site-config
export default defineConfig({
	// Use Canonical URL, but only the path and with no trailing /
	// End result is like: `/en/latest`
	base,
	markdown: {
		breaks: true,
		cjkFriendlyEmphasis: true,
		config: md => {
			md.use(i18nMacroPlugin);
			md.use(underlinePlugin);
			md.use(detailsHeadingPlugin);
			md.use(containerImportantPlugin);
			md.use(bracketedSpans);
			// VitePress 的默认数学公式渲染器 markdown-it-mathjax3 居然懒得添加 MathML 输出选项，所以换一个。
			// See: https://github.com/tani/markdown-it-mathjax3/issues/58
			md.use(katex, { output: "mathml" });
		},
		attrs: {},
	},
	vite: {
		server: {
			port: 7000,
		},
		resolve: {
			alias: {
				"@vp": import.meta.dirname,
				"@assets": resolve(import.meta.dirname, "../assets"),
			},
		},
	},
	lastUpdated: true,
	ignoreDeadLinks: true,
	title: "Otomad Helper",
	head: [
		[
			"link",
			{
				rel: "icon",
				href: withBase("favicon.ico"),
				type: "image/vnd.microsoft.icon",
				sizes: "16x16 24x24 32x32 48x48 64x64",
			},
		],
		["link", { rel: "icon", href: withBase("favicon.svg"), type: "image/svg+xml" }],
		[
			"link",
			{ rel: "apple-touch-icon", href: withBase("apple-touch-icon.png"), type: "image/png", sizes: "180x180" },
		],
	],
	locales: {
		root: {
			label: "English",
			lang: "en",
			description: "Helps to create YTPMVs in Vegas Pro",
			themeConfig: {
				footer: {
					message: "Released under the GPL-3.0 License",
					copyright: "Copyright © 2021–present",
				},
				nav: [
					{ text: "Home", link: "/" },
					{ text: "New Documentations (v8)", link: "/introduction", activeMatch: "^/[^/]+$" },
					{ text: "Old Documentations (v4)", link: "/v4/introduction", activeMatch: "/v4/" },
				],
				sidebar: sidebar("en"),
			},
		},
		"zh-CN": {
			label: "简体中文",
			lang: "zh-CN",
			description: "在Vegas Pro中生成音MAD",
			themeConfig: {
				darkModeSwitchLabel: "主题",
				lightModeSwitchTitle: "切换到浅色模式",
				darkModeSwitchTitle: "切换到深色模式",
				skipToContentLabel: "跳转到内容",
				langMenuLabel: "多语言",
				editLink: { text: "编辑此页" },
				docFooter: { prev: "上一页", next: "下一页" },
				outline: { label: "页面导航" },
				lastUpdated: { text: "最后更新于" },
				sidebarMenuLabel: "菜单",
				returnToTopLabel: "回到顶部",
				footer: {
					message: "基于 GPL-3.0 许可发布",
					copyright: "版权所有 © 2021~至今 兰音",
				},
				notFound: {
					title: "页面未找到",
					quote: "但如果你不改变方向，并且继续寻找，你可能最终会到达你所前往的地方。",
					linkLabel: "前往首页",
					linkText: "带我回首页",
				},
				nav: [
					{ text: "主页", link: "/zh-CN/" },
					{ text: "新版文档 (v8)", link: "/zh-CN/introduction", activeMatch: "^/zh-CN/[^/]+$" },
					{ text: "旧版文档 (v4)", link: "/zh-CN/v4/introduction", activeMatch: "/zh-CN/v4/" },
				],
				sidebar: sidebar("zhs"),
			},
		},
	},
	themeConfig: {
		outline: { level: "deep" },
		editLink: {
			pattern: "https://github.com/otomad/OtomadHelper/tree/docs/docs/:path",
		},
		logo: {
			light: "/favicon.svg",
			dark: "/favicon_dark.svg",
		},
		// https://vitepress.dev/reference/default-theme-config
		socialLinks: [
			{ icon: "github", link: "https://github.com/otomad/OtomadHelper", ariaLabel: "GitHub" },
			{ icon: "youtube", link: "https://youtube.com/@cmosekil", ariaLabel: "YouTube" },
			{ icon: "bilibili", link: "https://space.bilibili.com/38207429", ariaLabel: "bilibili" },
		],
		search: {
			provider: "local",
			options: {
				locales: {
					"zh-CN": {
						translations: {
							button: {
								buttonText: "搜索",
								buttonAriaLabel: "搜索",
							},
							modal: {
								displayDetails: "显示详细列表",
								resetButtonTitle: "重置搜索",
								backButtonTitle: "关闭搜索",
								noResultsText: "没有结果",
								footer: {
									selectText: "选择",
									selectKeyAriaLabel: "输入",
									navigateText: "导航",
									navigateUpKeyAriaLabel: "上箭头",
									navigateDownKeyAriaLabel: "下箭头",
									closeText: "关闭",
									closeKeyAriaLabel: "Esc",
								},
							},
						},
					},
				},
			},
		},
	},
});

type SidebarLocales = "en" | "zhs";
type SidebarItems = Record<string, DefaultTheme.SidebarItem[]>;
type Override<TSource, TOverrider> = Omit<TSource, keyof TOverrider> & TOverrider;
function sidebar(locale: SidebarLocales): SidebarItems {
	type SidebarTemplate = Record<
		string,
		(Record<SidebarLocales, string> &
			Override<
				DefaultTheme.SidebarItem,
				{
					items: (Record<SidebarLocales, string> & DefaultTheme.SidebarItem)[];
				}
			>)[]
	>;
	const sidebar: SidebarTemplate = {
		"/": [
			{
				en: "Introduction",
				zhs: "简介",
				items: [
					{ en: "What is Otomad Helper?", zhs: "音MAD助手是什么？", link: "/introduction" },
					{ en: "Usage", zhs: "用法", link: "/usage" },
					{ en: "FAQ", zhs: "疑难解答", link: "/faq" },
				],
			},
			{
				en: "Pages",
				zhs: "分页",
				items: [
					// { en: "Home", zhs: "主页", link: "/home" },
					{ en: "Source", zhs: "素材", link: "/source" },
					{ en: "Score", zhs: "乐曲", link: "/score" },
					{ en: "Audio", zhs: "音频", link: "/audio" },
					{ en: "Visual", zhs: "画面", link: "/visual" },
					{ en: "Track", zhs: "轨道", link: "/track" },
					{ en: "Sonar", zhs: "声呐", link: "/sonar" },
					{ en: "Lyrics", zhs: "歌词", link: "/lyrics" },
					{ en: "Shupeluner", zhs: "原音系", link: "/shupeluner" },
					{ en: "YTP", zhs: "YTP", link: "/ytp" },
					{ en: "Tools", zhs: "工具", link: "/tools" },
					{ en: "Moshes", zhs: "抹失", link: "/mosh" },
					{ en: "Management", zhs: "管理", link: "/management" },
					{ en: "Wizard", zhs: "精简", link: "/wizard" },
					{ en: "Settings", zhs: "设置", link: "/settings" },
				],
			},
		],
		"/v4/": [
			{
				en: "Introduction",
				zhs: "简介",
				items: [
					{ en: "What is Otomad Helper?", zhs: "音MAD助手是什么？", link: "/introduction" },
					{ en: "Usage", zhs: "用法", link: "/usage" },
				],
			},
			{
				en: "Tabs",
				zhs: "分页",
				items: [
					{ en: "Source", zhs: "素材", link: "/source" },
					{ en: "Score", zhs: "乐曲", link: "/score" },
					{ en: "Audio", zhs: "音频", link: "/audio" },
					{ en: "Visual", zhs: "画面", link: "/visual" },
					{ en: "Staff", zhs: "五线谱", link: "/staff" },
					{ en: "Sonar", zhs: "声呐", link: "/sonar" },
					{ en: "YTP", zhs: "YTP", link: "/ytp" },
					{ en: "Tools", zhs: "工具", link: "/tools" },
					{ en: "Moshes", zhs: "抹失", link: "/mosh" },
				],
			},
		],
	};
	const lang = (() => {
		if (locale === "en") return "";
		else if (locale === "zhs") return "zh-CN";
		else return locale;
	})();
	return Object.fromEntries(
		Object.entries(sidebar).map(([base, nav]) => {
			if (lang) base = "/" + lang + base;
			for (const section of nav) {
				section.text = section[locale];
				section.base = base;
				for (const item of section.items) item.text = item[locale];
			}
			return [base, nav] as const;
		}),
	) as SidebarItems;
}
