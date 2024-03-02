const CommandBar = styled.div`
	// box-shadow: 0 8px 16px ${c("shadows-flyout")};
	display: flex;
	padding: 4px;
	background-color: ${c("background-fill-color-acrylic-background-default")};
	border: 1px solid ${c("stroke-color-surface-stroke-flyout")};
	border-radius: 6px;

	> * {
		${styles.effects.text.body};
		height: 100%;
	}
`;

export default CommandBar;
