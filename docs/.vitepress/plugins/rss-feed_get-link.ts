import type { DefaultTheme, PageData, TransformPageContext } from "vitepress";

export function getRssFeedLink(lang: string) {
	const langSubdirectory = lang === "en" || lang === "en-US" ? "" : `/${lang}`;
	return `${langSubdirectory}/feed.xml`;
}

export function getLangFromPageData(pageData: PageData, ctx: TransformPageContext<NoInfer<DefaultTheme.Config>>) {
	const langs = Object.entries(ctx.siteConfig.userConfig.locales ?? ({ root: {} } as never)).map(
		([subdirectory, locale]) => locale.lang ?? subdirectory,
	);
	for (const lang of langs) if (lang !== "root" && pageData.relativePath.startsWith(lang + "/")) return lang;
	return "en-US";
}
