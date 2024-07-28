import "virtual:svg-icons-register";

const squared = styles.mixins.square("1em");
const StyledIcon = styled.i.attrs({
	role: "img",
})<{
	/** Keep the color of the icon itself? */
	$filled?: boolean;
}>`
	${squared};
	${styles.mixins.flexCenter()};
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

function Icon(props: FCP<{
	/** Icon file name. */
	name: DeclaredIcons | "" | boolean;
	/** Keep the color of the icon itself? */
	filled?: boolean;
}, "i">, ref: ForwardedRef<"i">): JSX.Element;
function Icon(props: FCP<{
	/** Hold the place, but nothing shown? */
	shadow: boolean;
}, "i">, ref: ForwardedRef<"i">): JSX.Element;
function Icon({ name, filled, shadow, className, ...htmlAttrs }: FCP<{
	/** Icon file name. */
	name?: DeclaredIcons | "" | boolean;
	/** Keep the color of the icon itself? */
	filled?: boolean;
	/** Hold the place, but nothing shown? */
	shadow?: boolean;
}, "i">, ref: ForwardedRef<"i">) {
	if (shadow) return <StyledIcon className={["shadow", className]} {...htmlAttrs} ref={ref} />;

	if (!name || typeof name === "boolean") return <i hidden className={className} {...htmlAttrs} ref={ref} />;

	const symbolId = getIconSymbolId(name);
	const ariaDescription = name.replaceAll("_", " ").replaceAll("/", ": ");

	return (
		<StyledIcon
			$filled={filled}
			className={className}
			{...htmlAttrs}
			ref={ref}
			role="img"
			aria-description={ariaDescription}
		>
			<svg aria-hidden>
				<use href={symbolId} />
			</svg>
		</StyledIcon>
	);
}

export default forwardRef(Icon) as unknown as typeof Icon; // The `forwardRef` doesn't support overload function.
