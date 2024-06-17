import i18n from "i18next";
import LanguageDetector from "i18next-browser-languagedetector";
import { initReactI18next } from "react-i18next";
import allLanguages from "./all";
import formatInterpolation from "./utils/interpolation";
import panguProcessor from "./utils/pangu";

i18n
	// Detect the language user used currently
	// Docs: https://github.com/i18next/i18next-browser-languageDetector
	.use(LanguageDetector)
	// Inject react-i18next instance
	.use(initReactI18next)
	// Pangu plugin, insert spaces between East-asian word and Western word
	.use(panguProcessor)
	// Initial i18next
	// Docs: https://www.i18next.com/overview/configuration-options
	.init({
		debug: import.meta.env.DEV,
		ns: ["javascript"],
		defaultNS: "javascript",
		fallbackLng: "en",
		interpolation: {
			escapeValue: false,
			format: formatInterpolation,
		},
		postProcess: ["pangu"],
		resources: allLanguages,
	});

document.documentElement.lang = i18n.language;
document.dir = i18n.dir();

export function useLanguage(): StateProperty<string> {
	const [language, setLanguage] = useState(i18n.language);

	function changeLanguage(lng: string) {
		setLanguage(lng);
		bridges.bridge.setCulture(i18n.t("metadata.culture", { lng }));
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
