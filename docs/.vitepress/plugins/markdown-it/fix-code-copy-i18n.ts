// TODO: remove when https://github.com/vuejs/vitepress/issues/4431 is fixed
// See: https://github.com/vuejs/vitepress/blob/d30f32246dffcf279e7bc2bc73a979821bd9f24c/docs/.vitepress/config.ts#L53-L82

import type MarkdownIt from "markdown-it";
import { useI18nThemeConfig } from "../../use-i18n";

export default function fixCodeCopyI18n(md: MarkdownIt) {
	const fence = md.renderer.rules.fence!;
	md.renderer.rules.fence = function (tokens, index, options, env, self) {
		const { localeIndex = "root" } = env;
		const { codeCopyButtonTitle } = useI18nThemeConfig(localeIndex).fence;
		return fence(tokens, index, options, env, self).replace(
			'<button title="Copy Code" class="copy"></button>',
			`<button title="${codeCopyButtonTitle}" class="copy"></button>`,
		);
	};
}
