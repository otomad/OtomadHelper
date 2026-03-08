const StackPanel = styled.div<{
	/** The flow direction. @default "horizontal" */
	$direction?: "horizontal" | "vertical";
	/** The gap between items. Unit: px. @default 8 */
	$gap?: Numberish | [rowGap: Numberish, columnGap: Numberish];
	/** The alignment of items. @default "normal" */
	$align?: CSSProperties["justifyContent"];
	/** If some items are wrapping to the second line, they will align to right (last). @default false */
	$endAlignWhenWrap?: boolean;
	/** Disable line wrap. @default false */
	$nowrap?: boolean;
	/** Sticky it to the top (block start). @default false */
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
		inset-block-start: 0;
		z-index: 1;
	`)}
`;

export default StackPanel;
