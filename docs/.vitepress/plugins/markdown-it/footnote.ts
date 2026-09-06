import type { PluginSimple } from "markdown-it";
import footnotePlugin from "markdown-it-footnote";
import { useI18nThemeConfig } from "../../use-i18n.js";

// See: https://github.com/markdown-it/markdown-it-footnote#customize
const footnotePluginWithCustomized: PluginSimple = md => {
	md.use(footnotePlugin);
	md.renderer.rules.footnote_block_open = (_1, _2, _3, env) => {
		const { localeIndex } = env;
		const { footnotesHeading } = useI18nThemeConfig(localeIndex).footnote;
		return `
			<h4 class="footnotes-heading">${footnotesHeading}</h4>
			<section class="footnotes">
			<ol class="footnotes-list">
		`;
	};
};

export default footnotePluginWithCustomized;
