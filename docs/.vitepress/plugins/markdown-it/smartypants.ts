import type MarkdownIt from "markdown-it";

const FULL_WIDTH_RE = /[\u3000-\u303F\uFF00-\uFFEF\u4E00-\u9FFF\u2000-\u206F]/;

export default function smartypantsPlugin(md: MarkdownIt) {
	// 1. 必须禁用内置的替换规则，防止直引号提前被切碎成引号 Token
	md.core.ruler.disable("replacements");

	// 2. 缓存原有的文本渲染函数
	const defaultRender =
		md.renderer.rules.text ||
		function (tokens, idx, options, env, self) {
			return self.renderToken(tokens, idx, options);
		};

	// 3. 代理文本渲染：此时每个 token.content 只有纯文本，绝无 HTML 标签或 markdown 标记干扰
	md.renderer.rules.text = function (tokens, idx, options, env, self) {
		const token = tokens[idx];
		let text = token.content;

		// 在这里执行您的核心替换逻辑（此时 *"foo"* 中的星号已被剥离，文本只有 "foo"）
		text = text
			.replaceAll(/(?<!\s|^)"/g, "”")
			.replaceAll('"', "“")
			.replaceAll(/(?<!\s|^)'/g, "’")
			.replaceAll("'", "‘");

		// 将替换后的文本写回，并交由默认渲染器输出（它会自动处理 HTML 转义，如 &quot; 变回普通字符）
		token.content = text;
		return defaultRender(tokens, idx, options, env, self);
	};
}
