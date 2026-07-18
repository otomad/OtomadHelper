import type MarkdownIt from "markdown-it";
import container from "markdown-it-container";
import type Token from "markdown-it/lib/token.mjs";

export default function containerImportantPlugin(md: MarkdownIt) {
	md.use(container, "important", {
		render(tokens: Token[], index: number) {
			const token = tokens[index];
			const title = token.info.trim().slice("important".length).trim();
			if (token.nesting === 1) {
				return `<div class="important custom-block"><p class="custom-block-title custom-block-title-default">${title ? title : "IMPORTANT"}</p>\n`;
			} else {
				return "</div>\n";
			}
		},
	});
}
