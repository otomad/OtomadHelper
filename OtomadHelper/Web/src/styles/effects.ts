export default {
	focus: (valueOnly: boolean = false) => {
		const value = css`0 0 0 2px ${c("stroke-color-focus-stroke-inner")}, 0 0 0 4px ${c("stroke-color-focus-stroke-outer")}`;
		return valueOnly ? value : css`
			box-shadow: ${value};
		`;
	},
};
