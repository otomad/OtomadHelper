export default css`
	/**
	 * Get the sibling index of parent element, but starts with 0 instead of 1.
	 * While \`sibling-index()\` function starts with 1.
	 */
	@function --sibling-index-0() returns <integer> {
		result: calc((sibling-index() - 1));
	}
	
	/**
	 * Use pure CSS to calculate the high contrast text color (black or white) by the oklab model from
	 * the specified background color.
	 * @param --color - Background color.
	 * @param --alpha - The alpha value of the result color, defaults to 1.
	 * @returns A contrastive text color.
	 * @remarks When \`contrast-color()\` available, this function will be deprecated.
	 */
	@function --contrast-color(--color <color>, --alpha type(<number> | <percentage>): 1) returns <color> {
		result: oklch(from var(--color) calc(1 - round(to-zero, L / 0.65)) 0 0 / var(--alpha));
		// Cannot use \`infinity\`.
	}
`;
