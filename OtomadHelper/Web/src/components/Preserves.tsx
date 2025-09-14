type RTFs = (string | React.JSX.Element)[];

function replaceLfToBr(longText: string | string[], spacing?: string | boolean) {
	longText = wrapIfNotArray(longText);
	return longText.flatMap(text => text
		.replaceAll(/\r\n|\n\r|\r/g, "\n")
		.split("\n")
		.interpose(i => <Br key={`br-${i}`} spacing={spacing} />)
		.filter(line => typeof line === "string" ? line.trim() : true));
}

function replaceAsteriskToEmAndStrong(longText: string | RTFs) {
	longText = wrapIfNotArray(longText);
	return longText.flatMap((text, i) => typeof text !== "string" || !text.includes("*") ? text : markdownToJsx(text, `line-${i}`));
}

/**
 * Automatically convert `\n` in the passed string to `<br />` to preserve line breaks.
 * @returns An array of each lines of the source string separated by `<br />`.
 */
export default function Preserves({ spacing, children }: FCP<{
	/**
	 * Paragraph spacing, or height of `<br>`. CSS `<length>` type. Defaults to `0`.\
	 * If you pass `true`, it will be `0.5em`.
	 */
	spacing?: string | boolean;
	/** Allows auto convert text enclosed by asterisk to italic (`*italic*`)? Defaults to true. */
	// autoItalic?: boolean;
}>): ReactNode {
	return React.Children.map(children, child => {
		if (typeof child === "string" || isI18nItem(child)) {
			let result = replaceLfToBr(child.toString(), spacing);
			result = replaceAsteriskToEmAndStrong(result);
			return result;
		} else if (spacing && React.isValidElement<FCP<{}, "br">>(child) && child.type === "br") {
			const { key, props: { children: _0, ...props } } = child;
			return <Br key={key} spacing={spacing} {...props} />;
		} else return child;
	});
}

const SpacingBr = styled("x-br")<{ // WARN: <br> tag not work for override `display: block` since Chromium 138 (guess). So change it to <x-br>.
	/** Paragraph spacing, or height of `<br>`. */
	$spacing?: string;
}>`
	content: "";
	display: block;
	margin-block-start: ${({ $spacing = "0.5em" }) => $spacing};
`;

export function Br({ repeat = 1, spacing, ...htmlAttrs }: FCP<{
	/** Repeat times. Defaults to 1. */
	repeat?: number;
	/**
	 * Paragraph spacing, or height of `<br>`. CSS `<length>` type. Defaults to `0`.\
	 * If you pass `true`, it will be `0.5em`.
	 */
	spacing?: string | boolean;
	children?: never;
}, "br">) {
	const BrTag = spacing ? SpacingBr : "br";
	return forMap(repeat, i =>
		<BrTag key={`br-${i}`} $spacing={typeof spacing === "string" ? spacing : undefined} {...htmlAttrs} />);
}

/**
 * Converts markdown text (bold and italic only) to React JSX.\
 * Supports **bold**, *italic*, and ***bold italic***.
 * @param markdown - The markdown text to convert.
 * @param keyPrefix - If use it in a loop, please provide a key to all child JSX elements, which can prevent same key among all children.
 * @returns React fragment with formatted elements.
 */
function markdownToJsx(markdown: string, keyPrefix: string = "") {
	keyPrefix &&= keyPrefix + "-";
	let boldOn = false, italicOn = false;
	const boldOpen = Symbol("<strong>"), italicOpen = Symbol("<em>"), tagClose = Symbol("</?>");
	const tokens: (string | symbol)[] = [];
	let cursor = 0;
	const findNextAsterisk = (moveCursor = true): [text: string, tag?: string] => {
		let subsequentText = markdown.slice(cursor);
		const matched = subsequentText.match(/\*+/);
		if (!matched) return [subsequentText, undefined];
		const tag = matched[0];
		if (tag.length > 3) throw new SyntaxError(`Invalid asterisk token: ${tag}`);
		subsequentText = subsequentText.slice(0, matched.index);
		if (moveCursor) cursor += matched.index! + tag.length;
		return [subsequentText, tag];
	};
	while (cursor < markdown.length) {
		const [text, tag] = findNextAsterisk();
		if (text) tokens.push(text);
		if (!tag) break;
		if (tag === "*") {
			italicOn = !italicOn;
			tokens.push(italicOn ? italicOpen : tagClose);
		} else if (tag === "**") {
			boldOn = !boldOn;
			tokens.push(boldOn ? boldOpen : tagClose);
		} else if (tag === "***") {
			boldOn = !boldOn;
			italicOn = !italicOn;
			if (!boldOn && !italicOn)
				tokens.push(tagClose, tagClose);
			else if (!boldOn && italicOn)
				tokens.push(tagClose, italicOpen);
			else if (!italicOn && boldOn)
				tokens.push(tagClose, boldOpen);
			else {
				const nextTag = findNextAsterisk(false)[1];
				tokens.push(...nextTag === "**" ? [italicOpen, boldOpen] : [boldOpen, italicOpen]);
			}
		}
	}

	cursor = 0;
	function parse() {
		const layer: (string | ReactElement)[] = [];
		while (true) {
			const token = tokens[cursor++];
			if (typeof token === "string") { layer.push(token); continue; }
			const TagName = token === boldOpen ? "strong" : italicOpen ? "em" : undefined;
			if (token === undefined || token === tagClose || !TagName) break;
			layer.push(<TagName key={`${keyPrefix}asterisk-${cursor}`}>{parse()}</TagName>);
		}
		return layer;
	}
	const nodes = parse();
	return nodes;
}
