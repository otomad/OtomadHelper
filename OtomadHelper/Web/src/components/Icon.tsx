import "virtual:svg-icons-register";

const squared = styles.mixins.square("1em");
const StyledIcon = styled.i.attrs({
	role: "img",
})<{
	/** Keep the color of the icon itself? */
	$filled?: boolean;
}>`
	${squared};
	display: inline-flex;

	:where(&) {
		${styles.effects.text.icon};
	}

	svg {
		${squared};

		${({ $filled }) => !$filled && css`
			fill: currentColor;
		`}
	}
`;

export function getIconSymbolId(name: string) {
	return "#icon-" + name.replaceAll("/", "-");
}

export default forwardRef(function Icon({ name, filled, ...htmlAttrs }: FCP<{
	/** Icon file name. */
	name: DeclaredIcons | "" | boolean;
	/** Keep the color of the icon itself? */
	filled?: boolean;
}, "span">, ref: ForwardedRef<"span">) {
	if (!name || typeof name === "boolean") return <span hidden />;

	const symbolId = getIconSymbolId(name);
	const ariaDescription = name.replaceAll("_", " ").replaceAll("/", ": ");

	return (
		<StyledIcon $filled={filled} {...htmlAttrs} ref={ref} role="img" aria-description={ariaDescription}>
			<svg aria-hidden>
				<use href={symbolId} />
			</svg>
		</StyledIcon>
	);
});
