export default function formatOrdinal(number: number, localeTag: string) {
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
