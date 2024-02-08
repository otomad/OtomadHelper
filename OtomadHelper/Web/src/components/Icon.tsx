import "virtual:svg-icons-register";

const squared = styles.mixins.square("1em");
const StyledIcon = styled.i.attrs({
	role: "img",
})<{
	/** 是否保持图标本身的颜色？ */
	$filled?: boolean;
}>`
	${squared};
	display: inline-flex;

	:where(&) {
		font-size: 20px;
	}

	svg {
		${squared};

		${({ $filled }) => !$filled && css`
			fill: currentColor;
		`}
	}
`;

export default forwardRef(function Icon({ name, filled, ...htmlAttrs }: FCP<{
	/** 图标文件名称。 */
	name: string;
	/** 是否保持图标本身的颜色？ */
	filled?: boolean;
}, "span">, ref: ForwardedRef<HTMLSpanElement>) {
	if (!name) return <span hidden />;

	const symbolId = "#icon-" + name.replaceAll("/", "-");
	const ariaDescription = new VariableName(name).words;

	return (
		<StyledIcon $filled={filled} {...htmlAttrs} ref={ref} role="img" aria-description={ariaDescription}>
			<svg aria-hidden>
				<use href={symbolId} />
			</svg>
		</StyledIcon>
	);
});
