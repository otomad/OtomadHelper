import { useData, type DefaultTheme } from "vitepress";
import { computed } from "vue";
import type { SearchConfig } from "vitepress-plugin-pagefind";

export function useI18n() {
	const { lang } = useData();

	return function t(object: Record<string, string>) {
		return computed(() => {
			let key = lang.value;
			if (key === "zh-CN") key = "zh";
			return key in object ? object[key] : object.en;
		});
	};
}

export function useI18nThemeConfig(lang: "en" | "zh" | (string & {})) {
	if (lang === "zh-CN") lang = "zh";
	else if (lang === "root" || lang === "en-US") lang = "en";
	const t = (dictionary: Partial<Record<typeof lang, string>>) => dictionary[lang] ?? dictionary["en"]!;
	return {
		vitepress: {
			darkModeSwitchLabel: t({ zh: "主题" }),
			lightModeSwitchTitle: t({ zh: "切换到浅色模式" }),
			darkModeSwitchTitle: t({ zh: "切换到深色模式" }),
			skipToContentLabel: t({ zh: "跳转到内容" }),
			langMenuLabel: t({ zh: "多语言" }),
			editLink: { text: t({ zh: "编辑此页" }) },
			docFooter: { prev: t({ zh: "上一页" }), next: t({ zh: "下一页" }) },
			outline: { label: t({ zh: "页面导航" }) },
			lastUpdated: { text: t({ zh: "最后更新于" }) },
			sidebarMenuLabel: t({ zh: "菜单" }),
			returnToTopLabel: t({ zh: "回到顶部" }),
			notFound: {
				title: t({ zh: "页面未找到" }),
				quote: t({ zh: "但如果你不改变方向，并且继续寻找，你可能最终会到达你所前往的地方。" }),
				linkLabel: t({ zh: "前往首页" }),
				linkText: t({ zh: "带我回首页" }),
			},
		} as DefaultTheme.Config,
		localSearch: {
			button: {
				buttonText: t({ zh: "搜索" }),
				buttonAriaLabel: t({ zh: "搜索" }),
			},
			modal: {
				displayDetails: t({ zh: "显示详细列表" }),
				resetButtonTitle: t({ zh: "重置搜索" }),
				backButtonTitle: t({ zh: "关闭搜索" }),
				noResultsText: t({ zh: "没有结果" }),
				footer: {
					selectText: t({ zh: "选择" }),
					selectKeyAriaLabel: t({ zh: "输入" }),
					navigateText: t({ zh: "导航" }),
					navigateUpKeyAriaLabel: t({ zh: "上箭头" }),
					navigateDownKeyAriaLabel: t({ zh: "下箭头" }),
					closeText: t({ zh: "关闭" }),
					closeKeyAriaLabel: t({ zh: "Esc" }),
				},
			},
		} as NonNullable<DefaultTheme.LocalSearchOptions["translations"]>,
		pageFind: {
			btnPlaceholder: t({ zh: "搜索" }),
			placeholder: t({ zh: "搜索文档" }),
			emptyText: t({ zh: "空空如也" }),
			heading: t({ zh: "共{{searchResult}}条结果" }),
			toSelect: t({ zh: "选择" }),
			toNavigate: t({ zh: "导航" }),
			toClose: t({ zh: "关闭" }),
			searchBy: t({ zh: "搜索提供" }),
			displayDetailedList: t({ zh: "显示详细列表" }),
			resetSearch: t({ zh: "重置搜索" }),
			closeSearch: t({ zh: "关闭搜索" }),
			loadingText: t({ zh: "搜索中⋯⋯" }),
		} as SearchConfig,
		rssFeed: {
			rssFeedTitle: t({ en: "RSS Feed", zh: "RSS 订阅" }),
		},
		llmsTxt: {
			tableOfContentsLabel: t({ en: "Table of Contents", zh: "目录" }),
		},
		fence: {
			codeCopyButtonTitle: t({
				en: "Copy code",
				zh: "复制代码",
				ko: "코드 복사",
				ja: "コードをコピー",
				es: "Copiar código",
				fa: "کپی کد",
				pt: "Copiar código",
				ru: "Скопировать код",
			}),
		},
		footnote: {
			footnotesHeading: t({ en: "Footnotes", zh: "脚注" }),
		},
		anchor: {
			permalinkTo: t({ en: "Permalink to “{}”", zh: "“\ufe01{}”\ufe01的永久链接" }),
		},
	};
}
