import type { PluginSimple } from "markdown-it";

/**
 * VitePress / Markdown-it 宏预处理多语言插件 (支持所有复杂语法混写)
 */
const i18nMacroPlugin: PluginSimple = md => {
	// 注册在 core 流程的最开始（block 之前），此时 state.src 还是纯字符串
	md.core.ruler.before("block", "i18n_macro_preprocessor", state => {
		const currentLang = state.env.localeIndex;
		state.src = parseI18nMacro(state.src, currentLang);
	});
};

export default i18nMacroPlugin;

export function parseI18nMacro(src: string, currentLang: string) {
	// 简化语言标签。
	const locale = parseLocale(currentLang) ?? new Intl.Locale("en");
	currentLang = locale?.language;

	// ==========================================
	// 1. 宏处理：【块多语言】 (@@@)
	// ==========================================
	if (src.includes("@@@")) {
		// 匹配连续的多语言块，直到遇到独立的 \n@@@ 结束符
		// ⚠️ 关键修改：正则外侧加上了 (?:\n)?，用来顺手捕获多语言块后面紧跟的那一个换行符
		const blockClusterRegex = /(?:@@@[a-zA-Z0-9_-]+[\s\S]*?\n)+@@@(?:\n)?/g;

		src = src.replace(blockClusterRegex, cluster => {
			// 判定捕获到的块是否以换行符结尾（如果是，说明抹除内容时需要把这个换行一并干掉）
			const endsWithNewline = cluster.endsWith("\n");

			const languagesData = {} as any;
			// 1. 先用精准的行切分方式，把各个语言块提取出来
			// 把最后的 @@@ 结尾去掉，只留下 @@@en... @@@zh...
			// 去掉末尾可能存在的换行符以及最后的 @@@ 结束符，保持纯净切分
			const cleanCluster = cluster.replace(/\n?$/, "").replace(/\n\s*@@@$/, "");
			// 根据 @@@lang 进行切分
			const parts = cleanCluster.split(/(?=^@@@[a-zA-Z0-9_-]+)/m);

			for (const part of parts) {
				const match = part.match(/^@@@([a-zA-Z0-9_-]+)(?:\n|$)([\s\S]*)$/);
				if (match) {
					const lang = match[1];
					const text = match[2] || "";
					languagesData[lang] = !text.trim()
						? // 显式赋值为空字符串，代表该语言故意不展示任何内容
							""
						: // 移除首尾多余的单个换行，防止撑开间距
							text.replace(/^\n|\n$/g, "");
				}
			}

			// 2. 严谨的 Fallback 策略 (注意："" 也是有效值，不能用 !languagesData[currentLang] 判定)
			const finalBlockContent =
				languagesData[currentLang] !== undefined
					? // 如果当前语言存在（哪怕是空字符串 ""），也严格采用，不回退
						languagesData[currentLang]
					: languagesData["en"] !== undefined
						? // 当前语言完全没写（缺失），才回退到英文
							languagesData["en"]
						: // 保底策略
							Object.values(languagesData)[0] || "";
			return finalBlockContent === ""
				? // 如果内容完全为空，直接返回空串（因为正则已经把后面的换行捕获了，返回空串等于把换行也消灭了）
					""
				: // 如果内容不为空，需要把刚刚正则误吞的那个外侧换行符再补回来，保证后面内容的排版正常
					finalBlockContent + (endsWithNewline ? "\n" : "");
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

	return src;
}

function parseLocale(tag: Intl.UnicodeBCP47LocaleIdentifier | Intl.Locale) {
	try {
		return new Intl.Locale(tag).maximize();
	} catch {
		return null;
	}
}
