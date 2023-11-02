import "virtual:svg-icons-register";

const squared = styles.mixins.square("1em");
const StyledIcon = styled.span.attrs({
	className: "icon",
	role: "img",
})<{
	/** 是否保持图标本身的颜色？ */
	$filled?: boolean;
}>`
	${squared};
	display: inline-flex;

	svg {
		${squared};

		${({ $filled }) => !$filled && css`
			fill: currentColor;
		`}
	}
`;

const Icon = forwardRef(({ name, filled, ...htmlAttrs }: FCP<{
	/** 图标文件名称。 */
	name: string;
	/** 是否保持图标本身的颜色？ */
	filled?: boolean;
}, HTMLSpanElement>, ref: ForwardedRef<HTMLSpanElement>) => {
	if (!name) return <span hidden />;

	const symbolId = "#icon-" + name.replaceAll("/", "-");

	return (
		<StyledIcon $filled={filled} {...htmlAttrs} ref={ref}>
			<svg aria-hidden>
				<use href={symbolId} />
			</svg>
		</StyledIcon>
	);
});

export default Icon;
