export default css`
	/**
	 * Get the sibling index of parent element, but starts with 0 instead of 1.
	 * While \`sibling-index()\` function starts with 1.
	 */
	@function --sibling-index-0() returns <integer> {
		result: calc((sibling-index() - 1));
	}

	/**
	 * Same as native \`light-dark()\` function, but accept non-color values.
	 */
	@function --light-dark(--light, --dark) {
		result: if(
			style(--color-scheme: light): var(--light);
			else: var(--dark);
		);
	}

	/**
	 * "t" stand for "transparent", do not manually use it, this function is called by JavaScript function \`c()\`.
	 * @param --color - Static color.
	 * @param --alpha - The alpha value that multiply by the alpha channel of the static color, defaults to 1.
	 * @returns A new color with the transparent value.
	 */
	@function --t(--color <color>, --alpha type(<number> | <percentage>): 1) returns <color> {
		result: rgb(from var(--color) r g b / calc(alpha * var(--alpha)));
	}
`;
