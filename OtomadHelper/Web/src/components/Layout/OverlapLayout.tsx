const OverlapLayout = styled.div<{
	$horizontalAlign?: CSSProperty.JustifyItems;
	$verticalAlign?: CSSProperty.AlignItems;
}>`
	display: inline-grid;
	place-items: ${styledProp("$verticalAlign", "normal")} ${styledProp("$horizontalAlign", "normal")};

	> * {
		grid-area: 1 / 2;
	}
`;

export default OverlapLayout;
