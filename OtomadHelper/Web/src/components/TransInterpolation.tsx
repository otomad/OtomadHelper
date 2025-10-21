import type { I18nArgsFunction } from "locales/types";

const tagStart = String.fromCodePoint(0xe0000), tagCancel = String.fromCodePoint(0xe007f);

export default function TransInterpolation<TInterpolations>({ i18nKey, children: _children, ..._interpolations }: WithOtherProperties<{
	i18nKey: I18nArgsFunction | string;
	children?: never;
}, ReactNode, TInterpolations>) {
	const interpolations = _interpolations as Record<string, ReactNode>;
	const keys = Object.keys(interpolations);
	const internalInterpolations = keys.mapObject((key, index) => [key, encodeKeyToTag(index)] as const);
	const translatedString = (i18nKey as I18nArgsFunction)(internalInterpolations).toString();
	const lines = translatedString.split("\n");
	const split = lines
		.map((line, lineIndex) => line
			.split(new RegExp(`(${tagStart}.*?${tagCancel})`, "u"))
			.map((segment, segmentIndex) => {
				if (!segment.startsWith(tagStart)) return segment;
				const index = decodeKeyFromTag(segment);
				if (index == null) return "";
				const key = keys[index];
				let node = interpolations[key];
				if (React.isValidElement(node) && node.key == null)
					node = React.cloneElement(node, { key: [lineIndex, segmentIndex, key].join("-") });
				return node;
			}),
		)
		.interpose(i => <br key={`br-${i}`} />);
	return split;
}

function encodeKeyToTag(key: number) {
	return tagStart + Array.from(key.toString(), char => String.fromCodePoint(char.codePointAt(0)! + 0xe0000)).join("") + tagCancel;
}

function decodeKeyFromTag(tag: string) {
	const matched = tag.match(new RegExp(`${tagStart}(.*)${tagCancel}`, "u"))?.[1];
	if (matched == null) return null;
	return parseInt(Array.from(matched, char => String.fromCodePoint(char.codePointAt(0)! - 0xe0000)).join(""), 10);
}
