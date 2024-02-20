const CommandBar = styled.div`
	border: 1px solid ${c("stroke-color-surface-stroke-flyout")};
	background-color: ${c("background-fill-color-acrylic-background-default")};
	border-radius: 6px;
	// box-shadow: 0 8px 16px ${c("shadows-flyout")};
	display: flex;
	padding: 4px;

	> * {
		height: 100%;
	}
`;

export default CommandBar;
