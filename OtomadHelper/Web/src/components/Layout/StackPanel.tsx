const StackPanel = styled.div<{
	$direction?: "horizontal" | "vertical";
	$gap?: string | number;
	$align?: CSSProperties["justifyContent"];
	$endAlignWhenWrap?: boolean;
}>`
	display: flex;
	flex-flow: ${({ $direction = "horizontal" }) => $direction === "vertical" ? "column" : "row"} wrap;
	gap: ${({ $gap = 8 }) => typeof $gap === "number" ? $gap + "px" : $gap};
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
`;

export default StackPanel;
