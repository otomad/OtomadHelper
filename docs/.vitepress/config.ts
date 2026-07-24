import { defineConfig, type DefaultTheme, type HeadConfig } from "vitepress";
import i18nMacroPlugin from "markdown-it-i18n";
import underlinePlugin from "markdown-it-underline-cjk-friendly";
import detailsHeadingPlugin from "markdown-it-container-details-heading";
import containerImportantPlugin from "./plugins/markdown-it/container-important";
import bracketedSpansPlugin from "markdown-it-bracketed-spans";
import kbdPlugin from "./plugins/markdown-it/kbd";
import fixCodeCopyI18n from "./plugins/markdown-it/fix-code-copy-i18n";
import { katex } from "@mdit/plugin-katex";
import { resolve } from "path";
import { join } from "path/posix";
import { pagefindPlugin, chineseSearchOptimize } from "vitepress-plugin-pagefind";
import { ImagePreviewPlugin } from "vitepress-plugin-image-preview";
import { back2topPlugin } from "vitepress-plugin-back2top";
import { llmstxtPlugin } from "vitepress-plugin-llmstxt";
import hostname from "./plugins/hostname";
import { createRssFeeds } from "./plugins/rss-feed";
import { getRssFeedLink } from "./plugins/rss-feed_get-link";
import llmsTransform from "./plugins/llms-transform";
import vueJsx from "@vitejs/plugin-vue-jsx";
import footnotePlugin from "./plugins/markdown-it/footnote";
import { useI18nThemeConfig } from "./use-i18n";
import smartypantsPlugin from "markdown-it-smartypants";
import { mermaidPlugin } from "./plugins/markdown-it/vitepress-mermaid";

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
			md.use(i18nMacroPlugin, {
				langAlias(locale, rawLang) {
					if (rawLang === "root") return "en";
					return locale?.language ?? rawLang;
				},
			});
			md.use(underlinePlugin);
			md.use(detailsHeadingPlugin);
			md.use(containerImportantPlugin);
			md.use(bracketedSpansPlugin);
			md.use(kbdPlugin);
			// VitePress 的默认数学公式渲染器 markdown-it-mathjax3 居然懒得添加 MathML 输出选项，所以换一个。
			// See: https://github.com/tani/markdown-it-mathjax3/issues/58
			md.use(katex, { output: "mathml" });
			md.use(fixCodeCopyI18n);
			md.use(footnotePlugin);
			md.use(smartypantsPlugin);
			md.use(mermaidPlugin);
			/* md.use(anchorPlugin, {
				permalink: anchorPlugin.permalink.linkInsideHeader({
					space: false,
					symbol: "",
				}),
				slugify: str =>
					str
						.toLowerCase()
						.replaceAll(/[\p{P}\p{S}]/gu, " ")
						.trim()
						.replaceAll(/\s+/g, "-"),
			}); */
		},
	},
	vue: {
		template: {
			compilerOptions: {
				isCustomElement: tag => {
					const deprecatedTags = ["nobr"];
					if (deprecatedTags.includes(tag)) return true;
				},
			},
		},
	},
	vite: {
		plugins: [
			vueJsx(),
			ImagePreviewPlugin({ hideOnClickModal: true }),
			back2topPlugin(),
			pagefindPlugin({
				customSearchQuery: chineseSearchOptimize,
				locales: {
					"zh-CN": useI18nThemeConfig("zh").pageFind,
				},
			}),
			llmstxtPlugin({
				hostname,
				llmsFullFile: false,
				transform: llmsTransform,
			}),
		],
		server: {
			port: 7000,
		},
		resolve: {
			alias: {
				"@vp": import.meta.dirname,
				"@assets": resolve(import.meta.dirname, "../assets"),
			},
		},
		build: {
			assetsInlineLimit: 200,
			// LightningCSS doesn't support "range syntax" in container style queries now.
			// `@container style(--outline-depth < 6)`
			// See: https://github.com/parcel-bundler/lightningcss/issues/1069
			// LightningCSS even messes property declaration order! It will break the CSS behavior!.
			// See: https://github.com/parcel-bundler/lightningcss/issues/1084
			cssMinify: "esbuild",
		},
	},
	lastUpdated: true,
	ignoreDeadLinks: true,
	sitemap: { hostname },
	async buildEnd(config) {
		await createRssFeeds(config);
	},
	async transformHead({ siteData, pageData, title: webPageTitleWithTitleTemplate }) {
		const lang = siteData.lang;
		const rssLink = getRssFeedLink(lang);
		const head: HeadConfig[] = [];
		head.push(["meta", { property: "og:title", content: pageData.title || webPageTitleWithTitleTemplate }]);
		head.push([
			"link",
			{
				rel: "alternate",
				type: "application/rss+xml",
				title: useI18nThemeConfig("zh").rssFeed.rssFeedTitle,
				href: rssLink,
			},
		]);
		return head;
	},
	title: "Otomad Helper",
	description: "Helps to create YTPMVs in Vegas Pro",
	head: [
		[
			"link",
			{
				rel: "icon",
				// Read the Docs 会强制替换掉 favicon.ico 为自己的图标，因此只好更名。
				href: withBase("favicon_1.ico"),
				type: "image/vnd.microsoft.icon",
				sizes: "16x16 24x24 32x32 48x48 64x64",
			},
		],
		["link", { rel: "icon", href: withBase("favicon.svg"), type: "image/svg+xml" }],
		[
			"link",
			{ rel: "apple-touch-icon", href: withBase("apple-touch-icon.png"), type: "image/png", sizes: "180x180" },
		],
		["meta", { property: "og:type", content: "article" }],
	],
	locales: {
		root: {
			label: "English",
			lang: "en-US",
			themeConfig: {
				["tableOfContentsLabel" as never]: "Table of Contents", // Used for llms.txt plugin.
				footer: {
					message: "Released under the GPL 3.0 License",
					copyright: "Copyright © 2021–present",
				},
				nav: [
					{ text: "Home", link: "/" },
					{ text: "New Docs (v8)", link: "/introduction", activeMatch: "^/[^/]+$" },
					{ text: "Old Docs (v4)", link: "/v4/introduction", activeMatch: "/v4/" },
				],
				sidebar: sidebar("en"),
			},
		},
		"zh-CN": {
			label: "简体中文",
			lang: "zh-CN",
			description: "在Vegas Pro中生成音MAD",
			themeConfig: {
				...useI18nThemeConfig("zh").vitepress,
				footer: {
					message: "基于 GPL 3.0 许可发布",
					copyright: "版权所有 © 2021~至今 兰音",
				},
				nav: [
					{ text: "主页", link: "/zh-CN/" },
					{ text: "新版文档 (v8)", link: "/zh-CN/introduction", activeMatch: "^/zh-CN/[^/]+$" },
					{ text: "旧版文档 (v4)", link: "/zh-CN/v4/introduction", activeMatch: "/zh-CN/v4/" },
				],
				sidebar: sidebar("zh"),
			},
		},
	},
	themeConfig: {
		outline: { level: "deep" },
		lastUpdated: { formatOptions: { forceLocale: true, year: "numeric", month: "2-digit", day: "2-digit" } },
		editLink: {
			pattern({ relativePath, filePath }) {
				const githubPath = "https://github.com/otomad/OtomadHelper/blob/docs/docs/";
				const showPlainCodeQuery = "?plain=1";
				if (!filePath.includes("[")) return githubPath + relativePath + showPlainCodeQuery;
				else return githubPath + relativePath.replace(/^.*?\//, "") + showPlainCodeQuery;
			},
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
						translations: useI18nThemeConfig("zh").localSearch,
					},
				},
			},
		},
	},
});

type SidebarLocales = "en" | "zh";
type SidebarItems = Record<string, DefaultTheme.SidebarItem[]>;
type Override<TSource, TOverrider> = Omit<TSource, keyof TOverrider> & TOverrider;
function sidebar(locale: SidebarLocales): SidebarItems {
	type SidebarTemplate = Record<
		string,
		((Record<SidebarLocales, string> | {}) &
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
				zh: "简介",
				items: [
					{ en: "What is Otomad Helper?", zh: "音MAD助手是什么？", link: "/introduction" },
					{ en: "Installation", zh: "安装", link: "/installation" },
					{ en: "Usage", zh: "用法", link: "/usage" },
				],
			},
			{
				en: "Pages",
				zh: "分页",
				items: [
					// { en: "Home", zh: "主页", link: "/home" },
					{ en: "Source", zh: "素材", link: "/source" },
					{ en: "Score", zh: "乐曲", link: "/score" },
					{ en: "Audio", zh: "音频", link: "/audio" },
					{ en: "Visual", zh: "画面", link: "/visual" },
					{ en: "Track", zh: "轨道", link: "/track" },
					{ en: "Sonar", zh: "声呐", link: "/sonar" },
					{ en: "Lyrics", zh: "歌词", link: "/lyrics" },
					{ en: "Shupeluner", zh: "原音系", link: "/shupeluner" },
					{ en: "YTP", zh: "YTP", link: "/ytp" },
					{ en: "Tools", zh: "工具", link: "/tools" },
					{ en: "Moshes", zh: "抹失", link: "/mosh" },
					{ en: "Management", zh: "管理", link: "/management" },
					{ en: "Wizard", zh: "精简", link: "/wizard" },
					{ en: "Settings", zh: "设置", link: "/settings" },
				],
			},
			{
				en: "Extra Info",
				zh: "额外信息",
				items: [
					{ en: "FAQ", zh: "常见问题解答", link: "/faq" },
					{ en: "Revise Docs", zh: "修订文档", link: "/revise" },
				],
			},
		],
		"/v4/": [
			{
				en: "Introduction",
				zh: "简介",
				items: [
					{ en: "What is Otomad Helper?", zh: "音MAD助手是什么？", link: "/introduction" },
					{ en: "Installation", zh: "安装", link: "/installation" },
					{ en: "Usage", zh: "用法", link: "/usage" },
				],
			},
			{
				en: "Tabs",
				zh: "页签",
				items: [
					{ en: "Source", zh: "素材", link: "/source" },
					{ en: "Score", zh: "乐曲", link: "/score" },
					{ en: "Audio", zh: "音频", link: "/audio" },
					{ en: "Visual", zh: "画面", link: "/visual" },
					{ en: "Staff", zh: "五线谱", link: "/staff" },
					{ en: "Sonar", zh: "声呐", link: "/sonar" },
					{ en: "YTP", zh: "YTP", link: "/ytp" },
					{ en: "Tools", zh: "工具", link: "/tools" },
					{ en: "Moshes", zh: "抹失", link: "/mosh" },
				],
			},
			{
				en: "Extra Info",
				zh: "额外信息",
				items: [
					{ en: "FAQ", zh: "常见问题解答", link: "/faq" },
					{ en: "References", zh: "参考", link: "/references" },
					{ en: "Revise Docs", zh: "修订文档", link: "/revise" },
				],
			},
		],
	};
	const lang = (() => {
		if (locale === "en") return "";
		else if (locale === "zh") return "zh-CN";
		else return locale;
	})();
	return Object.fromEntries(
		Object.entries(sidebar).map(([base, nav]) => {
			if (lang) base = "/" + lang + base;
			for (const section of nav) {
				if ((locale as "en") in section) section.text = section[locale];
				section.base = base;
				section.collapsed = false;
				for (const item of section.items) item.text = item[locale];
			}
			return [base, nav] as const;
		}),
	) as SidebarItems;
}
