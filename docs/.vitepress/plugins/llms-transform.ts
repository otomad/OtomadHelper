import type { LlmsConfig } from "vitepress-plugin-llmstxt";
import { parseI18nMacro } from "markdown-it-i18n/utils";
import type { DefaultTheme, LocaleConfig as _LocaleConfig } from "vitepress";
import { join } from "path/posix";
import { useI18nThemeConfig } from "../use-i18n.js";

type LocaleConfig = _LocaleConfig<DefaultTheme.Config> & Record<string, { themeConfig: {} }>;

let nonRootLanguages: string[] | undefined;

const llmsTransform: LlmsConfig["transform"] = async ({ page, vpConfig }) => {
	if (!nonRootLanguages) nonRootLanguages = Object.keys(vpConfig!.site.locales).filter(lang => lang !== "root");
	if (page.path.endsWith(".md")) {
		const lang =
			nonRootLanguages.find(lang => new RegExp(`^[/\\\\]${RegExp.escape(lang)}[/\\\\]`).test(page.path)) ?? "en";
		page.content = parseI18nMacro(page.content, lang);
	} else if (page.path.endsWith("/llms.txt")) {
		const { title: neutralTitle, description: neutralDescription } = vpConfig!.userConfig;
		const document: string[] = [];
		Object.values(vpConfig!.userConfig.locales as LocaleConfig).forEach(
			({ label, lang, title, description, themeConfig: { nav, sidebar } }, i) => {
				if (i > 0) document.push(`----`);
				document.push(`<main lang="${lang}">`);
				title ||= neutralTitle;
				description ||= neutralDescription;
				const { tableOfContentsLabel } = useI18nThemeConfig(lang!).llmsTxt;
				document.push(`> ${label}`);
				if (title) document.push(`# ${title}`);
				if (description) document.push(description);
				document.push(`## ${tableOfContentsLabel}`);
				const sidebars = Object.values(sidebar!).map(_sidebar => {
					const flattenLinks = new Set<string>();
					const sidebar = (_sidebar as DefaultTheme.SidebarItem[]).map(
						({ text: groupName, base = "/", items }) => ({
							groupName,
							links: items!.map(({ text, link: relativeLink }) => {
								const link = join(base, relativeLink!);
								flattenLinks.add(link);
								return { text, link };
							}),
						}),
					);
					return { groups: sidebar, flattenLinks };
				});
				const navigations = (nav as DefaultTheme.NavItemWithLink[]).map(({ text, link: _link }) => {
					const link = _link as string;
					for (const { groups, flattenLinks } of sidebars)
						if (flattenLinks.has(link)) return { nav: text, groups };
					return { groups: [{ links: [{ text, link }] }] as (typeof sidebars)[number]["groups"] };
				});
				for (const { nav, groups } of navigations) {
					if (nav) document.push(`### ${nav}`);
					for (const [i, { groupName, links }] of groups.entries()) {
						if (groupName) document.push(`#### ${groupName}`);
						else if (i > 0) document.push(`---`);
						document.push(
							links
								.map(({ text, link }) => {
									const mdLink = link + (link.endsWith("/") ? "index.md" : ".md");
									return `* [${text}](${mdLink})`;
								})
								.join("\n"),
						);
					}
				}
				document.push(`</main>`);
			},
		);
		page.content = document.join("\n\n");
	}
	return page;
};

export default llmsTransform;
