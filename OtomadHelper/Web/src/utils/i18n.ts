import { EnumItemClass } from "enum-plus/enum-item";
import { IN_CONTEXT_LANGUAGE_CODE } from "helpers/jipt-activator";
import type { TOptions as _TOptions } from "i18next";
import type { AvailableLanguageTags } from "locales/all";
import i18n from "locales/config";
import type { LocaleWithDefaultValue } from "locales/types";
const I18N_ITEM_SYMBOL = Symbol.for("react-i18next.i18n_item");
const I18N_ITEM_GET_KEY_SYMBOL = Symbol.for("react-i18next.i18n_item.get_key");
const toPrimitives = [Symbol.toPrimitive, "toString", "toJSON", "valueOf"];
interface AdditionalOptions {
	format: string | string[];
}
type TOptions = _TOptions & Partial<AdditionalOptions>;

export function isI18nItem(newChild: Any): newChild is Record<string | symbol, string> {
	return !!newChild?.[I18N_ITEM_SYMBOL];
}

const wellknownSymbols = Reflect.ownKeys(Symbol).map(key => Symbol[key as never]).filter(value => typeof value === "symbol");
const DEFAULT_NAMESPACE = "javascript";

const getProxy = (target: object, fallbackMode: boolean = false, tInHook?: typeof i18n.t) => {
	const $t = tInHook ?? i18n.t;
	const getParentsPrefix = (...prefixes: string[]) => prefixes.length > 0 ? prefixes.join(".") : "";
	const getDeclarationInfo = (...keys: string[]) => {
		const hasNamespace = !!i18n.options.ns?.includes(keys[0]);
		const namespace = hasNamespace ? keys[0] : DEFAULT_NAMESPACE;
		if (hasNamespace) keys = keys.toShifted();
		let key = getParentsPrefix(...keys);
		const raw = i18n.getResource("en", namespace, key) as string | object;
		const isCategory = typeof raw === "object";
		if (isCategory) key += key ? "._" : "_";
		if (hasNamespace) key = namespace + ":" + key;
		return {
			isCategory,
			includesInterpolation: typeof raw === "string" && raw.includes("{{"),
			missing: raw === undefined,
			missingDefault: isCategory && !("_" in raw),
			isStringMethod: keys.last() in String.prototype || wellknownSymbols.includes(keys.last()),
			key,
			raw,
			namespace,
		};
	};
	const has = (keys: string[], currentName: string) => !getDeclarationInfo(...keys, currentName).missing;
	const sharedProxyHandler = (keys: string[] = []): ProxyHandler<Any> => ({
		has(target, currentName) {
			if (typeof currentName === "symbol")
				return currentName in target;
			return has(keys, currentName);
		},
		ownKeys() {
			const { raw } = getDeclarationInfo(...keys);
			return isObject(raw) ? Reflect.ownKeys(raw) : [];
		},
		getOwnPropertyDescriptor(_target, currentName) {
			if (typeof currentName === "string" && has(keys, currentName))
				return { enumerable: true, configurable: true };
		},
	});
	return new Proxy(target, {
		get(target, rootName) {
			if (typeof rootName === "symbol")
				if (rootName === I18N_ITEM_SYMBOL) return true;
				else return;
			if (typeof target === "function") target = {};
			const getMissingKey = (key: string) => {
				if (fallbackMode) return undefined;
				const displayValue = `<${key}>`;
				debugger;
				console.error("Missing translation key: " + key);
				return displayValue;
			};
			const translate = (keys: string[], options?: TOptions) => {
				const { missingDefault, key } = getDeclarationInfo(...keys);
				if (missingDefault) return getMissingKey(key);
				const formatters = [
					...target?.format ? wrapIfNotArray(target.format) : [],
					...options?.format ? wrapIfNotArray(options.format) : [],
				];
				let result = $t(key, { ...target, ...options }) as string;
				if (formatters.length > 0) {
					const sep: string = options?.interpolation?.formatSeparator ?? target?.interpolation?.formatSeparator ?? ",";
					result = i18n.format(result, formatters.join(sep), options?.lng);
				}
				return result;
				// NOTE: If directly return a string, when user switches language, some local variables which store the i18n items will not update the language.
			};
			const getWithArgsFunction = (...prefixes: string[]) => {
				const func = (options: TOptions) => translate(prefixes, options instanceof EnumItemClass ? undefined : options);
				func[I18N_ITEM_SYMBOL] = true;
				return func;
			};
			const getWithArgsProxy = (...parents: string[]) => {
				const keys = [rootName, ...parents];
				const info = getDeclarationInfo(...keys);
				if (info.missing) {
					const lastKey = keys.last();
					if (lastKey === "name")
						return translate(keys.toPopped());
					if (info.isStringMethod)
						return translate(keys.toPopped())?.toString()?.[lastKey as "toUpperCase"];
					return getMissingKey(info.key);
					// eslint-disable-next-line @stylistic/brace-style
				}
				// else if (!info.includesInterpolation && !info.isCategory)
				// 	return translate(keys);
				else return new Proxy(getWithArgsFunction(...keys), {
					get(target, currentName): unknown {
						if (toPrimitives.includes(currentName))
							return () => translate(keys);
						if (currentName === "displayName")
							return translate(keys);
						if (currentName === Symbol.toStringTag)
							return "String";
						if (typeof currentName === "string")
							return getWithArgsProxy(...parents, currentName);
						if (typeof currentName === "symbol")
							if (currentName === I18N_ITEM_SYMBOL) return target[currentName];
							else if (currentName === I18N_ITEM_GET_KEY_SYMBOL) return info.key;
							else return translate(keys)![currentName as SymbolConstructor["iterator"]];
					},
					...sharedProxyHandler(keys),
				});
			};
			return getWithArgsProxy();
		},
		...sharedProxyHandler(),
	}) as LocaleDictionary;
};
type LocaleDictionary = LocaleWithDefaultValue;
const targetFunction = (options?: number | bigint | TOptions) => {
	if (options === undefined) options = {};
	else if (typeof options === "number" || typeof options === "bigint") options = { count: Number(options) };
	return getProxy(options);
};
/** Get localize string objects. */
export const t = getProxy(targetFunction) as Trans;
export const tf = getProxy(targetFunction, true) as Trans;
export const useT = () => { const { t } = useTranslation(); return getProxy(targetFunction, false, t) as Trans; };
export const tAlias = t;
export /* @internal */ type Trans = LocaleDictionary & typeof targetFunction;

declare global {
	// @ts-ignore
	export type { LocaleIdentifiers } from "locales/types";
}

/**
 * Check if the current page is written from right to left (such as in Arabic) rather than from left to right (such as in English).
 * @param container - Specify the container. Defaults to `<html>`.
 * @returns Is the horizontal writing direction of the current page written from right to left?
 */
export function isRtl(container?: Element | null): boolean {
	container ??= document.documentElement;
	if (container === document.documentElement) return document.dir === "rtl"; // `getComputedStyle` has bad performance.
	return getComputedStyle(container).direction === "rtl";
}

/**
 * Swaps the "ArrowLeft" and "ArrowRight" key codes if the current layout is right-to-left (RTL).
 *
 * @template T - A string type representing the key code.
 * @param code - The key code to potentially swap.
 * @returns The original key code if the layout is not RTL, or the swapped key code if the layout is RTL.
 */
export function swapArrowLeftRightIfRtl<T extends string>(code: T) {
	return !isRtl() ? code : (code === "ArrowLeft" ? "ArrowRight" : code === "ArrowRight" ? "ArrowLeft" : code) as T;
}

/**
 * Returns a string with a language-specific representation of the list.
 * @param list - An iterable object, such as an Array.
 * @param lang - Specific the language or automatically obtain.
 * @param type - The format of output message. Defaults to "conjunction".
 * @param style - The length of the internationalized message. Defaults to "narrow".
 * @returns A language-specific formatted string representing the elements of the list.
 */
export function listFormat(list: (string | false | undefined | null)[], lang?: Intl.UnicodeBCP47LocaleIdentifier, type: Intl.ListFormatType = "conjunction", style: Intl.ListFormatStyle = "narrow") {
	lang ||= i18n.language;
	const formatter = new Intl.ListFormat(lang, { type, style });
	return formatter.format(list.toCompacted());
}

interface UseLanguageTagsOptions {
	/** Omit in-context language? @default true */
	omitInContextLanguage?: boolean;
}

/**
 * A hook to get all language tags.
 * @returns All language tags.
 */
export function useLanguageTags({ omitInContextLanguage = true }: UseLanguageTagsOptions = {}) {
	const { i18n } = useTranslation();
	const languages = Object.keys(i18n.options.resources ?? {});
	if (omitInContextLanguage)
		languages.removeItem(IN_CONTEXT_LANGUAGE_CODE);
	return languages as AvailableLanguageTags[];
}

/**
 * A util to get all language tags.
 * @returns All language tags.
 */
export function getAllLanguageTags({ omitInContextLanguage = true }: UseLanguageTagsOptions = {}) {
	const languages = Object.keys(i18n.options.resources ?? {});
	if (omitInContextLanguage)
		languages.removeItem(IN_CONTEXT_LANGUAGE_CODE);
	return languages as AvailableLanguageTags[];
}

/**
 * Get reactive current language.
 * @returns Reactive current language.
 */
export function useCurrentLanguage() {
	const { i18n } = useTranslation();
	const [language, setLanguage] = useState(i18n.language);

	useMountEffect(() => {
		const onLanguageChanged = (lang: string) => setLanguage(lang);
		i18n.on("languageChanged", onLanguageChanged);
		return () => i18n.off("languageChanged", onLanguageChanged);
	});

	return language as AvailableLanguageTags;
}

/**
 * Validates and normalizes a given locale identifier.
 *
 * This function checks if the input is a valid BCP 47 locale identifier or `Intl.Locale` object.
 * It first attempts to construct an `Intl.Locale` instance from the input string, returning `null`
 * if the construction fails (i.e., due to incorrect format).
 *
 * Then, it compares the maximized and minimized forms of the locale. If both forms are equal,
 * it indicates that the locale does not correspond to a real-world locale, and the function returns `null`.
 * Otherwise, the locale is considered valid and existing.
 *
 * @param locale - The locale identifier as a string or an `Intl.Locale` object.
 * @returns The valid `Intl.Locale` object if the locale is valid and exists, otherwise `null`.
 */
export function getValidLocale(locale: Intl.UnicodeBCP47LocaleIdentifier | Intl.Locale) {
	if (typeof locale === "string")
		try {
			// If error "Incorrect locale information provided" is raised when constructing the `Intl.Locale` object,
			// the provided locale is considered invalid.
			// For example, `locale` is "a" or "abcd", because the language identifier can only be two to three letters.
			// But at this point it means that locale does not match the format of locale identifier.
			// However, even if a locale identifier that does not exist at all will be passed here.
			// For example, `locale` is "cc-cccc-cc", which is valid, but there is no such locale in the real world.
			locale = new Intl.Locale(locale);
		} catch {
			return null;
		}
	if (locale.maximize().toString() === locale.minimize().toString())
		// By maximizing and minimizing the locale, if its maximized value is equal to its minimized value,
		// the locale is considered not to exist in the real world at all.
		// At this point, no matter it is "oo", "cc-cccc-cc", etc., invalid locale can be found.
		return null;
	return locale;
}

/**
 * Determines whether the provided locale identifier is valid and exists in the real world.
 *
 * This function checks if the input is a valid BCP 47 locale identifier or `Intl.Locale` object.
 * It first attempts to construct an `Intl.Locale` instance from the input string, returning `false`
 * if the construction fails (i.e., due to incorrect format).
 *
 * Then, it compares the maximized and minimized forms of the locale. If both forms are equal,
 * it indicates that the locale does not correspond to a real-world locale, and the function returns `false`.
 * Otherwise, the locale is considered valid and existing.
 *
 * @param locale - The locale identifier as a string or `Intl.Locale` object.
 * @returns Is the locale valid and exists?
 */
export function isValidLocale(locale: Intl.UnicodeBCP47LocaleIdentifier | Intl.Locale) {
	return getValidLocale(locale) != null;
}

/**
 * Check if the provided language is English? Whether it is American English, British English, or something else.
 * @param lang - Language tag string or Intl.Locale object.
 * @returns The provided language is English?
 */
export function isEnglish(lang: Intl.UnicodeBCP47LocaleIdentifier | Intl.Locale) {
	return getValidLocale(lang)?.language === "en";
}

/**
 * Get the name of the target language in the current display language.
 * @param targetLocale - Target language.
 * @param displayLocale - Current display language. ~~It will be automatically gotten when it is not provided.~~
 * @returns The name of the target language.
 * @example
 * ```typescript
 * console.log(getLocaleName("en", "zh")); // "英语"
 * console.log(getLocaleName("zh", "en")); // "Chinese"
 * ```
 */
export function getLocaleName(targetLocale: string | Intl.Locale, displayLocale: string | Intl.Locale) {
	if (targetLocale instanceof Intl.Locale) targetLocale = targetLocale.toString();
	if (displayLocale instanceof Intl.Locale) displayLocale = displayLocale.toString();
	targetLocale = targetLocale === "zh-CN" ? "zh-Hans" : targetLocale === "zh-TW" ? "zh-Hant" : targetLocale;
	const fallbackLocales = [displayLocale];
	// if (displayLocale === "yue") fallbackLocales.push("zh-Hant-HK");
	return new Intl.DisplayNames(fallbackLocales, { type: "language" }).of(targetLocale)!;
}

/**
 * Uses the same resolve functionality as the `t` function and returns true if a key and context exists.
 * @param i18nItem - An i18n item.
 * @param context - Provide the context if required.
 * @param enableFallbackLang - When set it to `false`, if the specific key exists but haven't translated in current language,
 * the function will also return `false`.
 * @returns The key and the context exist.
 * @example
 * ```javascript
 * i18nExists(t.my.key, "context"); // -> true if exists, false if not.
 * ```
 */
export function i18nExists(i18nItem: string, context?: string, enableFallbackLang = true) {
	let path = getI18nKey(i18nItem);
	path = path.replaceEnd("()");
	if (context) path += `_${context}`;
	const fallbackLng = enableFallbackLang ? undefined : false;
	const notCategoryExists = i18n.exists(path, { fallbackLng, returnObjects: false });
	if (notCategoryExists) return true;
	if (notCategoryExists === i18n.exists(path, { fallbackLng, returnObjects: true })) return false;
	return i18n.exists(path + "._", { fallbackLng, returnObjects: false });
}

/**
 * Get i18n key from an i18n item.
 * @param i18nItem - An i18n item.
 * @returns The i18n key.
 * @example
 * ```javascript
 * getI18nKey(t.my.key); // "my.key"
 * ```
 */
export function getI18nKey(i18nItem: string) {
	return isI18nItem(i18nItem) ? i18nItem[I18N_ITEM_GET_KEY_SYMBOL] : i18nItem;
}
