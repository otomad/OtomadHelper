export default {
	focus: (valueOnly: boolean = false) => {
		const value = css`0 0 0 3px ${c("foreground-color")}`;
		return valueOnly ? value : css`
			box-shadow: ${value};
		`;
	},
};
