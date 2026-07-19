import type { PluginSimple } from "markdown-it";
import type Token from "markdown-it/lib/token.mjs";
import container from "markdown-it-container";

const containerDetailsHeadingPlugin: PluginSimple = md => {
	// 1. 渲染 HTML 标签
	md.use(container, "details", {
		render(tokens: Token[], index: number) {
			const token = tokens[index];

			if (token.nesting === 1) {
				// 提取 ::: details 后面的原始文本
				const rawInfo = token.info.trim().slice(7).trim();
				const parsed = parseTitleInfo(rawInfo);

				// 初始化 attrs 数组（确保不会覆盖 markdown-it-attrs 已经解析好的属性）
				token.attrs ||= [];

				// 寻找现有的 class 并追加，或者新建 class
				const detailsClasses = `details custom-block`;
				token.attrJoin("class", detailsClasses);

				// 如果已经手动显式指定了 id 名，就不用自动从标题中转换了。
				// 将 id 从 details 的属性中剥离出来，移植到内部 h 标签上。
				const idIndex = token.attrIndex("id");
				let id = token.attrGet("id");

				// 将已处理过的属性数组转换为 HTML 字符串（保留了 attrs 插件写入的所有属性）
				const renderAttrs = () => md.renderer.renderAttrs(token);

				if (parsed) {
					// 【情况 A】带井号：自定义标题行为
					id ||= slugify(parsed.rawText);
					const { level } = parsed;
					if (idIndex !== -1) token.attrs.splice(idIndex, 1);
					const detailsAttrsStr = renderAttrs();
					// 关键点：使用 md.renderInline() 渲染 HTML 行内标签，让删除线、加粗完美生效
					const renderedSummary = md.renderInline(parsed.rawText);
					// 注入自定义 class 和 动态 id，并为内部的 summary 添加对应的 h 标签样式或结构（可选）
					return `<details ${detailsAttrsStr}>\n<summary><h${level} id="${id}">${renderedSummary}</h${level}></summary>\n`;
				} else {
					// 【情况 B】不带井号：保持 VitePress 原生默认行为
					const detailsAttrsStr = renderAttrs();
					let result = `<details ${detailsAttrsStr}>\n`;
					if (rawInfo) {
						const renderedSummary = md.renderInline(rawInfo);
						result += `<summary>${renderedSummary}</summary>\n`;
					}
					return result;
				}
			} else {
				return "</details>\n";
			}
		},
	});
};

export default containerDetailsHeadingPlugin;

// 辅助函数：将中英文文本转换为合法的 URL hash / id
function slugify(str: string) {
	return String(str)
		.toLowerCase()
		.replaceAll(/[\p{P}\p{S}]/gu, " ")
		.trim()
		.replaceAll(/\s+/g, "-");
}

// 解析带有井号的 info 字符串，例如 "### 我是三级标题" -> { level: 3, text: "我是三级标题" }
function parseTitleInfo(infoStr: string) {
	const match = infoStr.match(/^(#{1,6})\s+(.+)$/);
	if (match) {
		return {
			level: match[1].length, // 井号的数量代表 H1 - H6
			rawText: match[2].trim(),
		};
	}
	return null; // 如果没有井号，返回 null 保持原始行为
}
