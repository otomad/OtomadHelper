// TODO: remove when https://github.com/vuejs/vitepress/issues/4431 is fixed
// See: https://github.com/vuejs/vitepress/blob/d30f32246dffcf279e7bc2bc73a979821bd9f24c/docs/.vitepress/config.ts#L53-L82

import type MarkdownIt from "markdown-it";

export default function fixCodeCopyI18n(md: MarkdownIt) {
	const fence = md.renderer.rules.fence!;
	md.renderer.rules.fence = function (tokens, index, options, env, self) {
		const { localeIndex = "root" } = env;
		const codeCopyButtonTitle = (() => {
			switch (localeIndex) {
				// case "es":
				// 	return "Copiar código";
				// case "fa":
				// 	return "کپی کد";
				// case "ko":
				// 	return "코드 복사";
				// case "pt":
				// 	return "Copiar código";
				// case "ru":
				// 	return "Скопировать код";
				case "zh-CN":
					return "复制代码";
				// case "ja":
				// 	return "コードをコピー";
				default:
					return "Copy code";
			}
		})();
		return fence(tokens, index, options, env, self).replace(
			'<button title="Copy Code" class="copy"></button>',
			`<button title="${codeCopyButtonTitle}" class="copy"></button>`,
		);
	};
}
