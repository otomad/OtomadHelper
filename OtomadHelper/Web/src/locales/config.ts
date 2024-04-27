import i18n from "i18next";
import LanguageDetector from "i18next-browser-languagedetector";
import { spacing } from "pangu";
import { initReactI18next } from "react-i18next";

import zhCN from "./Chinese Simplified";
import en from "./English";
import ja from "./Japanese";
import vi from "./Vietnamese";

i18n
	// Detect the language user used currently
	// Docs: https://github.com/i18next/i18next-browser-languageDetector
	.use(LanguageDetector)
	// Inject react-i18next instance
	.use(initReactI18next)
	// Pangu plugin, insert spaces between East-asian word and Western word
	.use({
		type: "postProcessor",
		name: "pangu",
		process: (value: string) => spacing(value),
	})
	// Initial i18next
	// Docs: https://www.i18next.com/overview/configuration-options
	.init({
		debug: import.meta.env.DEV,
		ns: ["javascript"],
		defaultNS: "javascript",
		fallbackLng: "en",
		interpolation: {
			escapeValue: false,
			format(value: string, format, _lng) {
				value = value.toString();
				if (value.match(/[a-z]/)) { // If the letters are all capital, they are treated as abbreviations without case conversion.
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
