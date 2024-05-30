import i18n from "i18next";
import LanguageDetector from "i18next-browser-languagedetector";
import { spacing } from "pangu";
import { initReactI18next } from "react-i18next";
import allLanguages from "./all";

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
			format(value, format, lng) {
				switch (typeof value) {
					case "string":
						if (value.match(/[a-z]/)) // If the letters are all capital, treated them as abbreviations without case conversion.
							switch (format) {
								case "uppercase": return value.toUpperCase();
								case "lowercase": return value.toLowerCase();
								case "capitalize": return `${value[0].toUpperCase()}${value.slice(1).toLowerCase()}`;
								default: break;
							}
						break;
					case "number":
						switch (format) {
							case "ordinal": return formatOrdinal(value, lng!);
							default: break;
						}
						break;
					default:
						break;
				}
				return value;
			},
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

function formatOrdinal(number: number, localeTag: string) {
	let locale: Intl.Locale;
	try {
		locale = new Intl.Locale(localeTag);
	} catch { // Invalid, future, or artificial locale
		return String(number); // Where every number is "other" rule.
	}
	const abs = Math.abs(number);
	const pluralRules = new Intl.PluralRules(locale, { type: "ordinal" });
	const rule = pluralRules.select(number);
	let suffix!: string, result: string;
	const getSuffix = (suffixes: Partial<Record<Intl.LDMLPluralRule, string>>) => suffix = suffixes[rule]!;
	const { language } = locale;
	switch (language) {
		case "en":
			getSuffix({
				one: "st",
				two: "nd",
				few: "rd",
				other: "th",
			});
			result = abs + suffix;
			return number >= 0 ? result : number === -1 ? "last" : `${result} to last`;
		case "zh":
			result = `第${abs}`;
			return number >= 0 ? result : `倒数${result}`;
		case "ja":
			result = `${abs}番目`;
			return number >= 0 ? result : `最後から${result}`;
		case "vi":
			result = `thứ ${abs}`;
			return number >= 0 ? result : `${result} đến cuối cùng`;
		default:
			return String(number);
	}
}

export default i18n;
