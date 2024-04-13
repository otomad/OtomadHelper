function replaceLfToBr(longText: string) {
	return longText
		.replaceAll(/\r\n|\n\r|\r/g, "\n")
		.split("\n")
		.flatMap((line, i) => i ? [<br key={i} />, line] : line)
		.filter(line => typeof line === "string" ? line.trim() : true);
}

/**
 * 将传入字符串中的 `\n` 自动转为 `<br />`，以保留换行符。
 */
export default function Preserves({ children }: FCP) {
	return React.Children.map(children, child => {
		if (typeof child === "string" || isI18nItem(child)) return replaceLfToBr(child.toString());
		else return child;
	});
}

export function Br({ repeat = 1, ...htmlAttrs }: FCP<{
	/** 重复次数。 */
	repeat?: number;
}, "br">) {
	return forMap(repeat, i => <br key={i} {...htmlAttrs} />);
}
