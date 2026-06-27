import type MarkdownIt from "markdown-it";

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
			const blockClusterRegex = /(?:@@@[\w-]+\n[\s\S]*?\n)+@@@/g;

			src = src.replace(blockClusterRegex, cluster => {
				const blockRegex = /@@@([\w-]+)\n([\s\S]*?)(?=\n@@@|\n@@@[\w-]+)/g;
				const languagesData = {} as any;
				let match;

				while ((match = blockRegex.exec(cluster)) !== null) {
					languagesData[match[1]] = match[2];
				}

				return languagesData[currentLang] !== undefined
					? languagesData[currentLang]
					: languagesData["en"] !== undefined
						? languagesData["en"]
						: Object.values(languagesData)[0] || "";
			});
		}

		// ==========================================
		// 2. 宏处理：【行多语言】 - 状态机重构版
		// ==========================================
		if (src.includes("@")) {
			const lines = src.split("\n");
			const newLines = [];

			let currentCluster = null as any; // 当前正在收集的多语言组

			// 辅助函数：专门用来结算一组多语言，并把过滤后的文本塞进新行数组
			const flushCluster = (cluster: any) => {
				if (!cluster) return;
				if (cluster[currentLang] !== undefined) {
					newLines.push(cluster[currentLang]);
				} else if (cluster["en"] !== undefined) {
					newLines.push(cluster["en"]);
				} else {
					// 如果既没有当前语言，也没有英文 fallback，保底选择写在最前面的那个语言
					newLines.push(Object.values(cluster)[0] || "");
				}
			};

			for (let i = 0; i < lines.length; i++) {
				const line = lines[i];
				const match = line.trim().match(/^@([\w-]+) (.*)$/);

				if (match) {
					const lang = match[1];
					const text = match[2];

					// 初始化新组
					if (!currentCluster) {
						currentCluster = {};
					}
					// 关键修复点：如果当前语言在组里已经有了（例如已经有了 en，又遇到了下一个 en）
					// 说明开启了全新的一行（例如列表的第2项），必须立刻结算旧组，并为新行开启新组
					else if (currentCluster[lang] !== undefined) {
						flushCluster(currentCluster);
						currentCluster = {};
					}

					// 将当前语言的内容存入组中
					currentCluster[lang] = text;
				} else {
					// 遇到了普通 Markdown 行（非 @ 开头），先把之前可能积压的组结算掉
					if (currentCluster) {
						flushCluster(currentCluster);
						currentCluster = null;
					}
					newLines.push(line);
				}
			}

			// 循环结束后，如果末尾还有未结算的组，进行最后一次结算
			if (currentCluster) {
				flushCluster(currentCluster);
			}

			src = newLines.join("\n");
		}

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
