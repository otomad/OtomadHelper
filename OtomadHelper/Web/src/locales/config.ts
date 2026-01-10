import { IN_CONTEXT_LANGUAGE_CODE } from "helpers/jipt-activator_constants";
import i18n from "i18next";
import LanguageDetector from "i18next-browser-languagedetector";
import { initReactI18next } from "react-i18next";
import type { AvailableLanguageTags } from "./all";
import allLanguages from "./all";
import initFormatters from "./utils/formatters";
import { addWbrAfterSlashProcessor, fullwidthQuotesProcessor } from "./utils/processors";

i18n
	// Detect the language user used currently
	// Docs: https://github.com/i18next/i18next-browser-languageDetector
	.use(LanguageDetector)
	// Inject react-i18next instance
	.use(initReactI18next)
	// Add fullwidth quotation marks Unicode Standardized Variation Sequence (SVS)
	.use(fullwidthQuotesProcessor)
	// Add zero width space after the forward slashes, to allow line break after the slash.
	.use(addWbrAfterSlashProcessor)
	// Initial i18next
	// Docs: https://www.i18next.com/overview/configuration-options
	.init({
		debug: import.meta.env.DEV,
		ns: ["javascript", "shared"],
		defaultNS: "javascript",
		fallbackLng: "en",
		interpolation: {
			escapeValue: false,
		},
		postProcess: [
			fullwidthQuotesProcessor.name,
			addWbrAfterSlashProcessor.name,
		],
		resources: allLanguages,
		detection: {
			// htmlTag: document.documentElement,
		},
	});

initFormatters();

// <q> element will provide wrong quotes in zh-TW, however zh-Hant works fine. So we need the maximize form of language code.
const htmlLang = {
	get value() { const lng = document.documentElement.lang; return lng === IN_CONTEXT_LANGUAGE_CODE ? lng : new Intl.Locale(lng).maximize().baseName; },
	set value(lng) { document.documentElement.lang = lng === IN_CONTEXT_LANGUAGE_CODE ? lng : new Intl.Locale(lng).maximize().baseName; },
};
htmlLang.value = i18n.language;
document.dir = i18n.dir();

function useLanguageGetter() {
	const [language, setLanguage] = useState(i18n.language);
	i18n.on("languageChanged", language => setLanguage(language));
	return language as AvailableLanguageTags;
}

export function useLanguage() {
	const language = useLanguageGetter();

	function changeLanguage(lng: AvailableLanguageTags) {
		const maximizedLang = new Intl.Locale(lng).maximize().baseName;
		if (i18n.language === lng && htmlLang.value === maximizedLang) return;
		bridges.bridge.setCulture(i18n.t("metadata.culture", { lng }));
		const TRANSITION_DURATION = 500;
		const HALFTONE_SIZE = 4 * Math.SQRT1_2;
		startColorViewTransition(async () => {
			await i18n.changeLanguage(lng);
			const dir = i18n.dir();
			htmlLang.value = lng;
			document.dir = dir;
			devStore.rtl = dir === "rtl";
		}, [
			[{
				clipPath: ["inset(0 0 calc(100% + 1rlh))", "inset(0)"],
			}],
			[{
				clipPath: ["inset(0 0 0)", "inset(calc(100% + 1rlh) 0 0)"],
			}, {
				pseudoElement: "::view-transition-old(root)",
			}],
			[{
				filter: ["", "grayscale(1)"],
			}, {
				pseudoElement: "::view-transition-old(root)",
				easing: eases.easeOutSmooth,
			}],
			[{
				filter: ["grayscale(1) blur(var(--view-transition-blurriness))", "grayscale(0) blur(var(--view-transition-blurriness))"],
			}, {
				pseudoElement: "::view-transition-new(root)",
				easing: eases.easeInSmooth,
			}],
			[{
				scale: ["", "1.05"],
				transformOrigin: ["bottom", "bottom"],
			}, {
				pseudoElement: "::view-transition-old(root)",
				easing: eases.easeInMax,
			}],
			[{
				"--view-transition-blurriness": ["5px", ""],
				scale: ["1.05", ""],
				transformOrigin: ["top", "top"],
			}, {
				pseudoElement: "::view-transition-new(root)",
				easing: eases.easeOutMax,
			}],
			[{
				"--view-transition-halftone-gradient": ["0", `${HALFTONE_SIZE}px`],
			}, {
				pseudoElement: "::view-transition-new(root)",
				easing: "cubic-bezier(0, 0, 0.7, 1)",
			}],
			[{
				"--view-transition-halftone-gradient": [`${HALFTONE_SIZE}px`, "0"],
			}, {
				pseudoElement: "::view-transition-old(root)",
				easing: "cubic-bezier(0.3, 0, 1, 1)",
			}],
		], {
			duration: TRANSITION_DURATION,
			cursor: "wait",
			staticStyle: css`
				:root::view-transition-old(root),
				:root::view-transition-new(root) {
					mask-image:
						radial-gradient(white var(--view-transition-halftone-gradient), transparent var(--view-transition-halftone-gradient)),
						radial-gradient(white var(--view-transition-halftone-gradient), transparent var(--view-transition-halftone-gradient));
					mask-position: 0 0, ${HALFTONE_SIZE}px ${HALFTONE_SIZE}px;
					mask-size: ${HALFTONE_SIZE * 2}px ${HALFTONE_SIZE * 2}px;
				}
			`,
		});
	}

	return [language, changeLanguage] as StatePropertyNonNull<AvailableLanguageTags>;
}

export default i18n;
