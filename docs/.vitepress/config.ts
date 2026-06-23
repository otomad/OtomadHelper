import { defineConfig } from "vitepress";
import i18nMacroPlugin from "./plugins/i18n-macro";
import fs from "fs";

// https://vitepress.dev/reference/site-config
export default defineConfig({
	// Use Canonical URL, but only the path and with no trailing /
	// End result is like: `/en/latest`
	base: process.env.READTHEDOCS_CANONICAL_URL
		? new URL(process.env.READTHEDOCS_CANONICAL_URL).pathname.replace(/\/$/, "")
		: "",
	markdown: {
		breaks: true,
		cjkFriendlyEmphasis: true,
		config: md => {
			md.use(i18nMacroPlugin);
		},
	},
	vite: {
		server: {
			port: 7000,
		},
		resolve: {
			alias: {
				"@vp": import.meta.dirname,
				"@assets": import.meta.resolve("../assets"),
			},
		},
	},
	lastUpdated: true,
	title: "Otomad Helper",
	description: "Helps to create YTPMVs in Vegas Pro",
	head: [
		[
			"link",
			{
				rel: "icon",
				href: "/favicon.ico",
				type: "image/vnd.microsoft.icon",
				sizes: "16x16 24x24 32x32 48x48 64x64",
			},
		],
		["link", { rel: "icon", href: "/favicon.svg", type: "image/svg+xml" }],
		["link", { rel: "apple-touch-icon", href: "/apple-touch-icon.png", type: "image/png", sizes: "180x180" }],
	],
	locales: {
		root: {
			label: "English",
			lang: "en",
			themeConfig: {
				nav: [
					{ text: "Home", link: "/" },
					{ text: "New Documentations (v8)", link: "/introduction" },
					{ text: "Old Documentations (v4)", link: "/v4/introduction" },
				],
				sidebar: {
					"/": [
						{
							text: "Introduction",
							items: [
								{ text: "What’s Otomad Helper?", link: "/introduction" },
								{ text: "Usage", link: "/usage" },
								{ text: "FAQ", link: "/faq" },
							],
						},
					],
					"/v4/": [
						{
							text: "Introduction",
							items: [{ text: "What’s Otomad Helper?", link: "/v4/introduction" }],
						},
					],
				},
			},
		},
		"zh-CN": {
			label: "简体中文",
			lang: "zh-CN",
			themeConfig: {
				darkModeSwitchLabel: "主题",
				lightModeSwitchTitle: "切换到浅色模式",
				darkModeSwitchTitle: "切换到深色模式",
				editLink: { text: "编辑此页" },
				docFooter: { prev: "上一页", next: "下一页" },
				outline: { label: "页面导航" },
				lastUpdated: { text: "最后更新于" },
				sidebarMenuLabel: "菜单",
				returnToTopLabel: "回到顶部",
				nav: [
					{ text: "主页", link: "/zh-CN/" },
					{ text: "新版文档 (v8)", link: "/zh-CN/introduction" },
					{ text: "旧版文档 (v4)", link: "/zh-CN/v4/introduction" },
				],
				sidebar: {
					"/zh-CN/": [
						{
							text: "简介",
							items: [
								{ text: "音MAD助手是什么？", link: "/zh-CN/introduction" },
								{ text: "用法", link: "/zh-CN/usage" },
								{ text: "疑难解答", link: "/zh-CN/faq" },
							],
						},
					],
					"/zh-CN/v4/": [
						{
							text: "简介",
							items: [{ text: "音MAD助手是什么？", link: "/zh-CN/v4/introduction" }],
						},
					],
				},
			},
		},
	},
	themeConfig: {
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
