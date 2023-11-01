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

export default function Icon({ name, filled, ...htmlAttrs }: FCP<{
	/** 图标文件名称。 */
	name: string;
	/** 是否保持图标本身的颜色？ */
	filled?: boolean;
}, HTMLSpanElement>) {
	const symbolId = "#icon-" + name.replaceAll("/", "-");

	return (
		<StyledIcon $filled={filled} {...htmlAttrs}>
			<svg aria-hidden>
				<use href={symbolId} />
			</svg>
		</StyledIcon>
	);
}
