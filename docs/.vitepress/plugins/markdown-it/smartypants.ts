import type { PluginSimple } from "markdown-it";
import { enableSvsQuotes } from "fullwidth-quotes";

const smartypantsPlugin: PluginSimple = md => {
	// 1. 必须禁用内置的替换规则，防止直引号提前被切碎成引号 Token
	md.core.ruler.disable("replacements");

	// 2. 缓存原有的文本渲染函数
	const defaultRender =
		md.renderer.rules.text || ((tokens, index, options, _env, self) => self.renderToken(tokens, index, options));

	// 3. 代理文本渲染：此时每个 token.content 只有纯文本，绝无 HTML 标签或 markdown 标记干扰
	md.renderer.rules.text = function (tokens, index, options, env, self) {
		const token = tokens[index];
		let text = token.content;

		// 在这里执行您的核心替换逻辑（此时 *"foo"* 中的星号已被剥离，文本只有 "foo"）
		text = parseText(text);

		// 将替换后的文本写回，并交由默认渲染器输出（它会自动处理 HTML 转义，如 &quot; 变回普通字符）
		token.content = text;
		return defaultRender(tokens, index, options, env, self);
	};
};

export default smartypantsPlugin;

const chars = {
	leftDoubleQuotesAmbiguous: "“",
	rightDoubleQuotesAmbiguous: "”",
	leftSingleQuotesAmbiguous: "‘",
	rightSingleQuotesAmbiguous: "’",
	leftDoubleQuotesHalfwidth: "“\ufe00",
	rightDoubleQuotesHalfwidth: "”\ufe00",
	leftSingleQuotesHalfwidth: "‘\ufe00",
	rightSingleQuotesHalfwidth: "’\ufe00",
	leftDoubleQuotesFullwidth: "“\ufe01",
	rightDoubleQuotesFullwidth: "”\ufe01",
	leftSingleQuotesFullwidth: "‘\ufe01",
	rightSingleQuotesFullwidth: "’\ufe01",
	leftDoubleQuotesMongolian: "“\ufe02",
	rightDoubleQuotesMongolian: "”\ufe02",
	leftSingleQuotesMongolian: "‘\ufe02",
	rightSingleQuotesMongolian: "’\ufe02",
	leftAngleQuotes: "«",
	rightAngleQuotes: "»",
	bottomEllipsis: "…",
	middleEllipsis: "⋯",
	enDash: "–",
	emDash: "—",
	emDashX2: "⸺",
	emDashX3: "⸻",
} as const;

const regexes = {
	// 特殊情况：如果最前面的字符是引号，后面紧跟标点符号且标点符号不在单词分隔符处，则需要强制关闭引号：
	// Special case if the very first character is a quote followed by punctuation at a non-word-break. Close the quotes by brute force:
	singleQuoteStart: /^'(?=[\p{P}\p{S}]\B)/gu,
	doubleQuoteStart: /^"(?=[\p{P}\p{S}]\B)/gu,

	// 特殊情况：双引号。例：
	// <p>He said, "'Quoted' words in a larger quote."</p>
	doubleQuoteSets: /"'(?=\w)/g,
	singleQuoteSets: /'"(?=\w)/g,
	doubleQuoteSets2: /(?<=\p{Term})'"/gu,
	singleQuoteSets2: /(?<=\p{Term})"'/gu,

	// 特殊情况：省年号 (the '80s)：
	decadeAbbr: /(?<!\w)'(?=\d+s)/g,

	// 获得最多的左双引号：
	openingDoubleQuotes: /([\s\-–—])"(?=\w)/g,

	// 右双引号：
	closingDoubleQuotes: /"(?=\s)/g, // r'"(?=\s)',
	closingDoubleQuotes2: /(?<=[^ \t\r\n\p{Ps}\p{Pi}\-\x02\x03])"/gu,

	// 获得最多的左单引号：
	openingSingleQuotes: /([\s\-–—])'(?=\w)/g,

	// 右单引号：
	closingSingleQuotes: /(?<=[^ \t\r\n\p{Ps}\p{Pi}\-\x02\x03])'(?!\s|s\b|\d)/gu,
	closingSingleQuotes2: /'(\s|s\b)/g,

	// 所有其余引号都应该左引号
	remainingSingleQuotes: "'",
	remainingDoubleQuotes: '"',
} as const;

function parseText(text: string) {
	// 3个连字符 → M宽长划（英文破折号）
	if (text.includes("---")) text = text.replaceAll(/(?<!-)---(?!-)/g, chars.emDash);
	// 2个连字符 → N宽长划（表示数值范围）
	if (text.includes("--")) text = text.replaceAll(/(?<!-)--(?!-)/g, chars.enDash);
	// 3个M宽长划 → 3倍M宽长划
	if (text.includes(chars.emDash.repeat(3))) text = text.replaceAll(/(?<!—)———(?!—)/g, chars.emDashX3);
	// 2个M宽长划（中文破折号） → 2倍M宽长划
	if (text.includes(chars.emDash.repeat(2))) text = text.replaceAll(/(?<!—)——(?!—)/g, chars.emDashX2);

	// 12个句点（不规范形式） → 4个居中省略号（中文长文省略号）
	if (text.includes(".".repeat(12))) text = text.replaceAll(/(?<!\.)\.{12}(?!\.)/g, chars.middleEllipsis.repeat(4));
	// 6个句点（不规范形式） → 2个居中省略号（中文省略号）
	if (text.includes(".".repeat(6))) text = text.replaceAll(/(?<!\.)\.{6}(?!\.)/g, chars.middleEllipsis.repeat(2));
	// 4个句点（英文省略号+句号） → 居底省略号+句号
	if (text.includes(".".repeat(4))) text = text.replaceAll(/(?<!\.)\.{4}(?!\.)/g, chars.bottomEllipsis + ".");
	// 3个句点（英文省略号） → 居底省略号
	if (text.includes(".".repeat(3))) text = text.replaceAll(/(?<!\.)\.{3}(?!\.)/g, chars.bottomEllipsis);
	// 4个居底省略号 → 4个居中省略号（中文长文省略号）
	if (text.includes(chars.bottomEllipsis.repeat(4)))
		text = text.replaceAll(/(?<!…)…{4}(?!…)/g, chars.middleEllipsis.repeat(4));
	// 2个居底省略号 → 2个居中省略号（中文省略号）
	if (text.includes(chars.bottomEllipsis.repeat(2)))
		text = text.replaceAll(/(?<!…)…{2}(?!…)/g, chars.middleEllipsis.repeat(2));
	// 12个中文句号（不规范形式） → 4个居中省略号（中文长文省略号）
	if (text.includes("。".repeat(12))) text = text.replaceAll(/(?<!。)。{12}(?!。)/g, chars.middleEllipsis.repeat(4));
	// 6个中文句号（不规范形式） → 2个居中省略号（中文省略号）
	if (text.includes("。".repeat(6))) text = text.replaceAll(/(?<!。)。{6}(?!。)/g, chars.middleEllipsis.repeat(2));
	// 3个中文句号（不规范形式） → 2个居中省略号（中文省略号）
	if (text.includes("。".repeat(3))) text = text.replaceAll(/(?<!。)。{3}(?!。)/g, chars.middleEllipsis.repeat(2));

	// 2个左大于号 → 左尖引号（法式引号）
	if (text.includes("<<")) text = text.replaceAll("<<", chars.leftAngleQuotes);
	// 2个右大于号 → 右尖引号（法式引号）
	if (text.includes(">>")) text = text.replaceAll(">>", chars.rightAngleQuotes);

	// 直引号 → 弯引号（识别）
	if (text.includes('"') || text.includes("'")) {
		// Convert from Python version:
		// https://github.com/Python-Markdown/markdown/blob/8453df010daf9547efcc20a05bb12c0e2fd2016a/markdown/extensions/smarty.py
		text = text
			.replaceAll(regexes.singleQuoteStart, chars.rightSingleQuotesAmbiguous)
			.replaceAll(regexes.doubleQuoteStart, chars.rightDoubleQuotesAmbiguous)
			.replaceAll(regexes.doubleQuoteSets, chars.leftDoubleQuotesAmbiguous + chars.leftSingleQuotesAmbiguous)
			.replaceAll(regexes.singleQuoteSets, chars.leftSingleQuotesAmbiguous + chars.leftDoubleQuotesAmbiguous)
			.replaceAll(regexes.doubleQuoteSets2, chars.rightSingleQuotesAmbiguous + chars.rightDoubleQuotesAmbiguous)
			.replaceAll(regexes.singleQuoteSets2, chars.rightDoubleQuotesAmbiguous + chars.rightSingleQuotesAmbiguous)
			.replaceAll(regexes.decadeAbbr, chars.rightSingleQuotesAmbiguous)
			.replaceAll(regexes.openingSingleQuotes, "$1" + chars.leftSingleQuotesAmbiguous)
			.replaceAll(regexes.closingSingleQuotes, chars.rightSingleQuotesAmbiguous)
			.replaceAll(regexes.closingSingleQuotes2, chars.rightSingleQuotesAmbiguous + "$1")
			.replaceAll(regexes.remainingSingleQuotes, chars.leftSingleQuotesAmbiguous)
			.replaceAll(regexes.openingDoubleQuotes, "$1" + chars.leftDoubleQuotesAmbiguous)
			.replaceAll(regexes.closingDoubleQuotes, chars.rightDoubleQuotesAmbiguous)
			.replaceAll(regexes.closingDoubleQuotes2, chars.rightDoubleQuotesAmbiguous)
			.replaceAll(regexes.remainingDoubleQuotes, chars.rightDoubleQuotesAmbiguous);
	}

	// 半角弯引号 → 全角弯引号（识别）
	if (text.includes("“") || text.includes("”") || text.includes("‘") || text.includes("’")) {
		text = enableSvsQuotes(text);
	}

	return text;
}
