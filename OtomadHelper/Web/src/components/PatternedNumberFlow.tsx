import NumberFlow, { type NumberFlowProps } from "@number-flow/react";

export default function PatternedNumberFlow({ children, specialValue, ...htmlAttrs }: Omit<NumberFlowProps, "value"> & {
	children?: Readable | null | false;
	/** Specify to show special text when it is specific value. */
	specialValue?: Record<number, string>;
}) {
	if (children == null || children === false) return;
	const value = children.toString();

	const numberFlowProps = useMemo<NumberFlowProps>(() => {
		let prefix: string | undefined, value_number: number | undefined, suffix: string | undefined;

		if (typeof value === "number" || typeof value === "bigint")
			value_number = Number(value);

		if (typeof value === "string") {
			let value_string: string;
			({ 1: prefix = undefined, 2: value_string = "", 3: suffix = undefined } = value.toString().match(/^(.*?)(-?\d+(?:\.\d+)?)(.*)$/) ?? []);
			({ prefix, suffix } = fixPangu(prefix, suffix));
			value_number = +value_string;
		}

		if (specialValue && value_number !== undefined && value_number in specialValue)
			return { prefix: specialValue[value_number], value: undefined! };

		if (value_number != null)
			return { prefix, value: value_number, suffix };

		throw new TypeError("Unknown value pattern for `PatternedNumberFlow`");
	}, [value, specialValue]);

	// return numberFlowProps.map((props, i) => <NumberFlow key={i} {...htmlAttrs} {...props} />);
	return <NumberFlow isolate {...htmlAttrs} {...numberFlowProps} />;
}

const cjkCharacters = /(?![\uff00-\uffff])[\p{sc=Han}\p{sc=Hira}\p{sc=Kana}\p{sc=Hang}\p{sc=Bopo}]/u;
const PUNC_SPACE = "\u2008";
function fixPangu(prefix?: string, suffix?: string) {
	if (prefix && cjkCharacters.test(prefix.realCharAt(-1))) prefix += PUNC_SPACE;
	if (suffix && cjkCharacters.test(suffix.realCharAt(0))) suffix = PUNC_SPACE + suffix;
	return { prefix, suffix };
}
