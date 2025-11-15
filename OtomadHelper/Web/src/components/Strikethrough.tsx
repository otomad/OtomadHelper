const StyledStrikethrough = styled.s`
	text-decoration: none !important;
	background-image: linear-gradient(
		to bottom,
		transparent calc(50% - 1px * var(--dpi) / 2),
		currentColor,
		transparent calc(50% + 1px * var(--dpi) / 2)
	);
	background-repeat: no-repeat;
	background-position: left;
	background-size: 100%;

	&:dir(rtl) {
		background-position: right;
	}

	&.remain {
		background-size: 0%;
	}

	&.translucent:not(.remain) {
		opacity: ${c("disabled-text-opacity")};
	}
`;

export default function Strikethrough({ strikeout = true, translucent = true, children, className, ...htmlAttrs }: FCP<{
	/** Add strikethrough to the text? @default true */
	strikeout?: boolean;
	/** Make the text semi-transparent if it is strikeout? @default true */
	translucent?: boolean;
}, "s">) {
	return (
		<StyledStrikethrough className={[className, { remain: !strikeout, translucent }]} {...htmlAttrs}>{children}</StyledStrikethrough>
	);
}
