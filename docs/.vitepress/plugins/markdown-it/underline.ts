import type MarkdownIt from "markdown-it";

type RenderRule = NonNullable<InstanceType<typeof MarkdownIt>["renderer"]["rules"]["text"]>;

export default function markdownItUnderline(md: MarkdownIt) {
	const renderEm: RenderRule = (tokens, index, options, _, self) => {
		var token = tokens[index];
		if (token.markup === "_") token.tag = "u";
		return self.renderToken(tokens, index, options);
	};

	md.renderer.rules.em_open = renderEm;
	md.renderer.rules.em_close = renderEm;
}
