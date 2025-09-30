const StackPanel = styled.div<{
	$direction?: "horizontal" | "vertical";
	$gap?: Numberish | [rowGap: Numberish, columnGap: Numberish];
	$align?: CSSProperties["justifyContent"];
	$endAlignWhenWrap?: boolean;
	$nowrap?: boolean;
	$sticky?: boolean;
}>`
	display: flex;
	flex-direction: ${({ $direction = "horizontal" }) => $direction === "vertical" ? "column" : "row"};
	// stylelint-disable-next-line declaration-block-no-redundant-longhand-properties
	flex-wrap: ${ifProp("$nowrap", "nowrap", "wrap")};
	gap: ${({ $gap = 8 }) => !Array.isArray($gap) ? styles.toValue($gap) : $gap.map(gap => styles.toValue(gap)).join(" ")};
	justify-content: ${styledProp("$align", "normal")};
	align-items: center;

	.icon {
		font-size: 16px;
	}

	${ifProp("$endAlignWhenWrap", css`
		> :not(:first-child) {
			margin-inline-start: auto;
		}
	`)}

	${ifProp("$sticky", css`
		position: sticky;
		top: 0;
		z-index: 1;
	`)}
`;

export default StackPanel;
