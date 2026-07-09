import type { LlmsConfig } from "vitepress-plugin-llmstxt";
import { parseI18nMacro } from "./markdown-it/i18n-macro";
import { createContentLoader } from "vitepress";

let nonRootLanguages: string[] | undefined;

const llmsTransform: LlmsConfig["transform"] = async ({ page, vpConfig }) => {
	if (!nonRootLanguages) nonRootLanguages = Object.keys(vpConfig!.site.locales).filter(lang => lang !== "root");
	if (page.path.endsWith(".md")) {
		const lang =
			nonRootLanguages.find(lang => new RegExp(`^[/\\\\]${RegExp.escape(lang)}[/\\\\]`).test(page.path)) ?? "en";
		page.content = parseI18nMacro(page.content, lang);
	}
	return page;
};

export default llmsTransform;
