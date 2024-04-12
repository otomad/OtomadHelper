import i18n from "i18next";
import LanguageDetector from "i18next-browser-languagedetector";
import { spacing } from "pangu";
import { initReactI18next } from "react-i18next";

import zhCN from "./Chinese Simplified";
import en from "./English";
import ja from "./Japanese";
import vi from "./Vietnamese";

i18n
	// 检测用户当前使用的语言
	// 文档: https://github.com/i18next/i18next-browser-languageDetector
	.use(LanguageDetector)
	// 注入 react-i18next 实例
	.use(initReactI18next)
	// 盘古插件，在东亚文字与西方文字之间插入空格
	.use({
		type: "postProcessor",
		name: "pangu",
		process: (value: string) => spacing(value),
	})
	// 初始化 i18next
	// 配置参数的文档: https://www.i18next.com/overview/configuration-options
	.init({
		debug: import.meta.env.DEV,
		ns: ["javascript"],
		defaultNS: "javascript",
		fallbackLng: "en",
		interpolation: {
			escapeValue: false,
			format(value: string, format, _lng) {
				value = value.toString();
				if (value.match(/[a-z]/)) { // 如果字母全大写，则视为缩略词汇而不做大小写转换。
					if (format === "uppercase") return value.toUpperCase();
					if (format === "lowercase") return value.toLowerCase();
					if (format === "capitalize") return `${value[0].toUpperCase()}${value.slice(1).toLowerCase()}`;
				}
				return value;
			},
		},
		postProcess: ["pangu"],
		resources: {
			en,
			"zh-CN": zhCN,
			ja,
			vi,
		},
	});

document.documentElement.lang = i18n.language;
document.dir = i18n.dir();

export function useLanguage(): StateProperty<string> {
	const [language, setLanguage] = useState(i18n.language);

	function changeLanguage(lng: string) {
		setLanguage(lng);
		startColorViewTransition(async () => {
			await i18n.changeLanguage(lng);
			const dir = i18n.dir();
			document.documentElement.lang = lng;
			document.dir = dir;
		}, {
			clipPath: ["inset(0 0 100%)", "inset(0)"],
		}, {
			duration: 500,
		});
	}

	return [language, changeLanguage];
}

export default i18n;
