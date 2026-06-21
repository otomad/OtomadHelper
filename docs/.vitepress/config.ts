import { defineConfig } from "vitepress";

// https://vitepress.dev/reference/site-config
export default defineConfig({
	// Use Canonical URL, but only the path and with no trailing /
	// End result is like: `/en/latest`
	base: process.env.READTHEDOCS_CANONICAL_URL
		? new URL(process.env.READTHEDOCS_CANONICAL_URL).pathname.replace(/\/$/, "")
		: "",
	title: "Otomad Helper",
	description: "Helps to create YTPMVs in Vegas Pro",
	themeConfig: {
		editLink: {
			pattern: "https://github.com/otomad/OtomadHelper/tree/docs/docs/:path",
		},
		logo: {
			light: "/favicon.svg",
			dark: "/favicon_dark.svg",
		},
		// https://vitepress.dev/reference/default-theme-config
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
						{ text: "What’s Otomad Helper", link: "/introduction" },
						{ text: "Usage", link: "/usage" },
						{ text: "FAQ", link: "/faq" },
					],
				},
			],
			"/v4/": [
				{
					text: "Introduction",
					items: [{ text: "What’s Otomad Helper", link: "/v4/introduction" }],
				},
			],
		},
		socialLinks: [
			{ icon: "github", link: "https://github.com/otomad/OtomadHelper" },
			{ icon: "youtube", link: "https://youtube.com/@cmosekil" },
			{ icon: "bilibili", link: "https://space.bilibili.com/38207429" },
		],
	},
});
