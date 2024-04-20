function replaceLfToBr(longText: string) {
	return longText
		.replaceAll(/\r\n|\n\r|\r/g, "\n")
		.split("\n")
		.flatMap((line, i) => i ? [<br key={i} />, line] : line)
		.filter(line => typeof line === "string" ? line.trim() : true);
}

/**
 * Automatically convert `\n` in the passed string to `<br />` to preserve line breaks.
 */
export default function Preserves({ children }: FCP) {
	return React.Children.map(children, child => {
		if (typeof child === "string" || isI18nItem(child)) return replaceLfToBr(child.toString());
		else return child;
	});
}

export function Br({ repeat = 1, ...htmlAttrs }: FCP<{
	/** Repeat times. */
	repeat?: number;
}, "br">) {
	return forMap(repeat, i => <br key={i} {...htmlAttrs} />);
}
