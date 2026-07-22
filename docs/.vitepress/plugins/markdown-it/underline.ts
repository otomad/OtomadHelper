import type { PluginSimple } from "markdown-it";
import type { RenderRule } from "markdown-it/lib/renderer.mjs";

const markdownItUnderline: PluginSimple = md => {
	const renderEm: RenderRule = (tokens, index, options, _, self) => {
		const token = tokens[index];
		if (token.markup === "_") token.tag = "u";
		return self.renderToken(tokens, index, options);
	};

	md.renderer.rules.em_open = renderEm;
	md.renderer.rules.em_close = renderEm;
};

export default markdownItUnderline;
