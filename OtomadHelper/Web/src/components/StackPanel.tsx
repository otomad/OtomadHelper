const StackPanel = styled.div<{
	$direction?: "horizontal" | "vertical";
	$gap?: string | number;
}>`
	display: flex;
	align-items: center;
	gap: ${({ $gap = 8 }) => typeof $gap === "number" ? $gap + "px" : $gap};
	flex-direction: ${({ $direction = "horizontal" }) => $direction === "vertical" ? "column" : "row"};

	.icon {
		font-size: 16px;
	}
`;

export default StackPanel;
