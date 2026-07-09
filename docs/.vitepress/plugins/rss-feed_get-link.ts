export function getRssFeedLink(lang: string) {
	const langSubdirectory = lang === "en" || lang === "en-US" ? "" : `/${lang}`;
	return `${langSubdirectory}/feed.xml`;
}
