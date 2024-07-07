import type { FormatFunction } from "i18next";
import formatOrdinal from "./ordinal";

const formatInterpolation: FormatFunction = function format(value, format, lng) {
	if (isI18nItem(value))
		value = value.toString();
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
};

export default formatInterpolation;
