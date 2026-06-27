import type MarkdownIt from "markdown-it";
import container from "markdown-it-container";

export type Token = ReturnType<InstanceType<typeof MarkdownIt>["parseInline"]>[number];

export default function containerDetailsHeadingPlugin(md: MarkdownIt) {
	// 1. 渲染 HTML 标签
	md.use(container, "details", {
		render(tokens: Token[], index: number) {
			const token = tokens[index];

			if (token.nesting === 1) {
				// 提取 ::: details 后面的原始文本
				const rawInfo = token.info.trim().slice(7).trim();
				const parsed = parseTitleInfo(rawInfo);

				if (parsed) {
					// 【情况 A】带井号：自定义标题行为
					const id = slugify(parsed.text);
					const { level } = parsed;
					// 注入自定义 class 和 动态 id，并为内部的 summary 添加对应的 h 标签样式或结构（可选）
					return `<details class="details custom-block">\n<summary><h${level} id="${id}">${parsed.text}</h${level}></summary>\n`;
				} else {
					// 【情况 B】不带井号：保持 VitePress 原生默认行为
					const summaryText = rawInfo || "Details";
					return `<details class="details custom-block">\n<summary>${summaryText}</summary>\n`;
				}
			} else {
				return "</details>\n";
			}
		},
	});
}

// 辅助函数：将中英文文本转换为合法的 URL hash / id
function slugify(str: string) {
	return encodeURIComponent(String(str).trim().toLowerCase().replace(/\s+/g, "-"));
}

// 解析带有井号的 info 字符串，例如 "### 我是三级标题" -> { level: 3, text: "我是三级标题" }
function parseTitleInfo(infoStr: string) {
	const match = infoStr.match(/^(#{1,6})\s+(.+)$/);
	if (match) {
		return {
			level: match[1].length, // 井号的数量代表 H1 - H6
			text: match[2].trim(),
		};
	}
	return null; // 如果没有井号，返回 null 保持原始行为
}
