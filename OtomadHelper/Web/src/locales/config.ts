import i18n from "i18next";
import LanguageDetector from "i18next-browser-languagedetector";
import { initReactI18next } from "react-i18next";
import en from "./English";
import zhCN from "./SChinese";

i18n
	// 检测用户当前使用的语言
	// 文档: https://github.com/i18next/i18next-browser-languageDetector
	.use(LanguageDetector)
	// 注入 react-i18next 实例
	.use(initReactI18next)
	// 初始化 i18next
	// 配置参数的文档: https://www.i18next.com/overview/configuration-options
	.init({
		debug: true,
		fallbackLng: "en",
		interpolation: {
			escapeValue: false,
			format(value: string, format, _lng) {
				if (format === "uppercase") return value.toUpperCase();
				if (format === "lowercase") return value.toLowerCase();
				if (format === "capitalize") return `${value[0].toUpperCase()}${value.slice(1)}`;
				return value;
			},
		},
		resources: {
			en,
			"zh-CN": zhCN,
		},
	});

export default i18n;
