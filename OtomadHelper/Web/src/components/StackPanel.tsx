const StackPanel = styled.div<{
	$direction?: "horizontal" | "vertical";
	$gap?: string | number;
}>`
	display: flex;
	flex-direction: ${({ $direction = "horizontal" }) => $direction === "vertical" ? "column" : "row"};
	gap: ${({ $gap = 8 }) => typeof $gap === "number" ? $gap + "px" : $gap};
	align-items: center;

	.icon {
		font-size: 16px;
	}
`;

export default StackPanel;
