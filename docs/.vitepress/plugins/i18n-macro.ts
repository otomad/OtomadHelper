import MarkdownIt from "markdown-it";

/**
 * VitePress / Markdown-it 宏预处理多语言插件 (支持所有复杂语法混写)
 */
export default function i18nMacroPlugin(md: MarkdownIt) {
	// 注册在 core 流程的最开始（block 之前），此时 state.src 还是纯字符串
	md.core.ruler.before("block", "i18n_macro_preprocessor", state => {
		let currentLang = state.env.localeIndex || "en";
		const locale = parseLocale(currentLang);
		// 中文的语言标签太复杂了，简化它们。
		if (locale?.language === "zh") {
			if (locale?.script === "Hans") currentLang = "zhs";
			else if (locale?.script === "Hant") currentLang = "zht";
		}
		let src = state.src;

		// ==========================================
		// 1. 宏处理：【块多语言】 (@@@)
		// ==========================================
		if (src.includes("@@@")) {
			// 匹配连续的多语言块，直到遇到孤立的 \n@@@
			const blockClusterRegex = /(?:@@@[\w-]+\n[\s\S]*?\n)+@@@/g;

			src = src.replace(blockClusterRegex, cluster => {
				const blockRegex = /@@@([\w-]+)\n([\s\S]*?)(?=\n@@@|\n@@@[\w-]+)/g;
				const languagesData = {} as any;
				let match;

				while ((match = blockRegex.exec(cluster)) !== null) {
					languagesData[match[1]] = match[2];
				}

				// Fallback 策略
				return languagesData[currentLang] !== undefined
					? languagesData[currentLang]
					: languagesData["en"] !== undefined
						? languagesData["en"]
						: Object.values(languagesData)[0] || "";
			});
		}

		// ==========================================
		// 2. 宏处理：【行多语言】 (@en / @zh-CN)
		// ==========================================
		if (src.includes("@")) {
			const lines = src.split("\n");
			const newLines = [];

			let i = 0;
			while (i < lines.length) {
				const line = lines[i];
				const match = line.trim().match(/^@([\w-]+)\s+(.*)$/);

				// 如果当前行是多语言宏指令
				if (match) {
					const currentCluster = {} as any;

					// 向下连续收集所有紧挨着的行多语言指令，形成一个“语言组”
					while (i < lines.length) {
						const innerMatch = lines[i].trim().match(/^@([\w-]+)\s+(.*)$/);
						if (!innerMatch) break; // 遇到了不是以 @ 开头的行，说明这组多语言结束了

						currentCluster[innerMatch[1]] = innerMatch[2];
						i++;
					}

					// 从这一组语言中提取符合当前路由的行 (带 Fallback 机制)
					let finalLineContent = "";
					if (currentCluster[currentLang] !== undefined) {
						finalLineContent = currentCluster[currentLang];
					} else if (currentCluster["en"] !== undefined) {
						finalLineContent = currentCluster["en"];
					} else {
						finalLineContent = (Object.values(currentCluster)[0] as string) || "";
					}

					newLines.push(finalLineContent);
					// 注意：此处不需要 i++，因为外层的 while 和内层结束条件已经正确递增了指针
				} else {
					// 普通 Markdown 行，原样保留
					newLines.push(line);
					i++;
				}
			}
			src = newLines.join("\n");
		}

		// 将替换后清爽的、标准的 Markdown 源码还给 state.src
		state.src = src;
	});
}

function parseLocale(tag: Intl.UnicodeBCP47LocaleIdentifier | Intl.Locale) {
	try {
		return new Intl.Locale(tag).maximize();
	} catch {
		return null;
	}
}
