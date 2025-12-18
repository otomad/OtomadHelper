import { IN_CONTEXT_LANGUAGE_CODE } from "helpers/jipt-activator_constants";
import type { Formatter } from "i18next";
import i18n from "i18next";
import ordinal from "intl-ordinal";
type NewFormatFunction = Parameters<Formatter["add"]>[1];

const mapWordsIfNotAllUpper = (str: string, convert: (word: string) => string) => str.mapWords(word => word.areAllUpper() ? word : convert(word));
const isSimplifyChinese = (lng: string | undefined) => {
	let locale: Intl.Locale;
	try {
		return !!lng && (locale = new Intl.Locale(lng).maximize(), locale.language === "zh" && locale.script === "Hans");
	} catch {
		return false;
	}
};

const formatters: Record<string, NewFormatFunction> = {
	uppercase: (value: string) => value.toLocaleUpperCase(),
	lowercase: (value: string) => mapWordsIfNotAllUpper(value, word => word.toLocaleLowerCase()),
	capitalize: (value: string) => mapWordsIfNotAllUpper(value, word => word.toCapitalized()),
	titleCase: (value: string) => value.toTitleCase(),
	nowrapPerWord: (value: string) => value.nowrapPerWord(),
	nowrapPerChar: (value: string) => value.nowrapPerChar(),

	ordinal: (value: number, lng) => ordinal(lng!).format(value),

	and(value: string[], lng) {
		let result = new Intl.ListFormat(lng, { type: "conjunction", style: "long" }).format(value);
		if (isSimplifyChinese(lng)) result = result.replaceAll(/(?<=”\p{VS}*)、(?=“)/gu, "");
		return result;
	},
	quote(value: string[] | string) {
		const [left, right] = i18n.t("quotes").split("\n");
		if (typeof value === "string") return left + value + right;
		else return value.map(item => left + item + right) as never;
	},
};

export default function initFormatters() {
	for (const [format, func] of Object.entries(formatters))
		i18n.services.formatter!.add(format, (value, lng, options) => {
			if (isI18nItem(value)) value = value.toString();
			if (lng === IN_CONTEXT_LANGUAGE_CODE) return value;
			return func(value, lng, options);
		});
}
