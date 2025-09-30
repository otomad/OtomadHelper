import "virtual:svg-icons-register";

const squared = styles.mixins.square("1em", false, true);
const StyledIcon = styled.i<{
	/** Keep the color of the icon itself? */
	$filled?: boolean;
}>`
	${squared};
	${styles.mixins.flexCenter()};
	display: inline-flex;
	vertical-align: middle;

	:where(&) {
		${styles.effects.text.icon};
	}

	svg {
		${squared};
		pointer-events: none;

		${({ $filled }) => !$filled && css`
			fill: currentColor;
		`}
	}
`;

export const getIconSymbolId = (name: string) => `#icon-${name.replaceAll("/", "-")}` as const;
export const getIconAriaLabel = (name: string) => `Icon - ${name.replace(/^off_slash_alt\//, "").replaceAll("_", " ").replaceAll("/", ": ")}` as const;

export default function Icon(props: FCP<{
	/** Icon file name. */
	name: DeclaredIcons | "" | boolean;
	/** Keep the color of the icon itself? */
	filled?: boolean;
	children?: never;
}, "i">): React.JSX.Element;
export default function Icon(props: FCP<{
	/** Hold the place, but nothing shown? */
	shadow: boolean;
	children?: never;
}, "i">): React.JSX.Element;
export default function Icon({ name, filled, shadow, className, ref, ...htmlAttrs }: FCP<{
	/**
	 * Icon file name.
	 * - If it is a boolean, it will be disguised as a (fake) icon element, but the content will be empty.
	 * - If it is an empty string or undefined, it will return nothing.
	 */
	name?: DeclaredIcons | "" | boolean;
	/** Keep the color of the icon itself? */
	filled?: boolean;
	/** Hold the place, but nothing shown? */
	shadow?: boolean;
}, "i">) {
	if (shadow) return <StyledIcon className={["shadow", className]} {...htmlAttrs} ref={ref} />;

	if (typeof name === "boolean") return <i hidden className={className} {...htmlAttrs} ref={ref} />;
	if (!name) return;

	const symbolId = getIconSymbolId(name);
	const ariaLabel = getIconAriaLabel(name);

	if (filled === undefined && name.startsWith("colored/")) filled = true;

	return (
		<StyledIcon
			$filled={filled}
			className={className}
			{...htmlAttrs}
			ref={ref}
			role="img"
			aria-description={ariaLabel}
			aria-hidden
		>
			<svg aria-hidden>
				<use href={symbolId} />
			</svg>
		</StyledIcon>
	);
}

const xmlSerializer = new XMLSerializer();

/**
 * Get raw SVG of the icon.
 * @remarks Only available after the React app loaded.
 * @param name - Icon file name.
 * @returns Get raw SVG content from sprite icons.
 */
function getRawSvg(name: DeclaredIcons) {
	// Old method, it will accidentally build a large number of svg module chunks.
	// const iconsImport = import.meta.glob<string>("/src/assets/icons/**/*.svg", { import: "default", query: "?raw" });
	// return iconsImport[`/src/assets/icons/${name}.svg`]?.();

	const symbolId = getIconSymbolId(name);
	const symbol = document.querySelector<SVGSymbolElement>(symbolId);
	if (!symbol) return null;
	const SVG_NAMESPACE = "http://www.w3.org/2000/svg";
	const svg = document.createElementNS(SVG_NAMESPACE, "svg");
	for (const attribute of symbol.attributes) {
		if (attribute.name === "id") continue;
		if (attribute.name === "viewBox") {
			const viewBox = symbol.viewBox.baseVal;
			svg.setAttribute("width", String(viewBox.width));
			svg.setAttribute("height", String(viewBox.height));
		}
		svg.setAttributeNode(attribute.cloneNode(true) as Attr);
	}
	for (const child of symbol.childNodes)
		svg.appendChild(child.cloneNode(true));
	return xmlSerializer.serializeToString(svg);
}

Icon.getRawSvg = getRawSvg;
