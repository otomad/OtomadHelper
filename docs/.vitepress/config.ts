import { defineConfig } from "vitepress";

// https://vitepress.dev/reference/site-config
export default defineConfig({
	title: "Otomad Helper",
	description: "Help to create YTPMV in Vegas Pro",
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
			{ text: "Documentations", link: "/introduction" },
		],
		sidebar: [
			{
				text: "Introduction",
				items: [
					{ text: "What’s Otomad Helper", link: "/introduction" },
					{ text: "Usage", link: "/usage" },
				],
			},
		],
		socialLinks: [{ icon: "github", link: "https://github.com/otomad/OtomadHelper" }],
	},
});
