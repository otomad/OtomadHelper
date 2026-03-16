const easeOutFocusRingBack = "cubic-bezier(0.8, 2.15, 0.67, 1)";
const focusRingTransitions = `box-shadow 0s, --focus-ring-length-outer ${easeOutFocusRingBack} 350ms, --focus-ring-length-inner ${eases.easeOutMax} 250ms`;

export default {
	focus: (inset = false, important = false) => css`
		--focus-ring-length-outer: 4px;
		--focus-ring-length-inner: 2px;
		box-shadow:
			0 0 0 var(--focus-ring-length-inner) ${c("stroke-color-focus-stroke-inner")} ${inset && "inset"},
			0 0 0 var(--focus-ring-length-outer) ${c("stroke-color-focus-stroke-outer")} ${inset && "inset"}
			${important && "!important"};
		transition: ${fallbackTransitions}, ${focusRingTransitions};
	`,
	focusRingTransitions,
	flyout: css`
		overflow: clip;
		background-color: ${c("background-fill-color-acrylic-background-default")};
		border-radius: 8px;
		outline: 1px solid ${c("stroke-color-surface-stroke-flyout")};
		box-shadow: 0 4px 8px ${c("shadows-flyout")};
		backdrop-filter: blur(10px);
	`,
	refreshedBackdropIfHasBackgroundImage: css`
		body:has(.background-image) & {
			background-color: transparent;
			border-color: ${c("stroke-color-surface-stroke-flyout-navigation-panel")};
			outline-color: ${c("stroke-color-surface-stroke-flyout-navigation-panel")};
		}
	`,
};
