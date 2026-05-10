/*
 * This global style is used to unify other styles introduced by third-party plugins.
 *
 * In this global style partial file, the selector class and id pattern will be ignored,
 * this is because other plugins have inconsistent naming conventions.
 */

export default css`
	/* stylelint-disable selector-class-pattern */

	// #region Crowdin JIPT
	#crowdin-jipt-mask {
		background-color: ${c("background-color")} !important;

		&,
		* {
			transition: none;
		}
	}

	#jipt-close-btn {
		color: ${c("foreground-color")} !important;
	}

	.crowdin_phrase {
		pointer-events: auto;
	}

	.jipt-loader {
		transition: scale 1s ${eases.easeOutElastic} !important;

		@starting-style {
			scale: 0;
		}
	}
	// #endregion

	main.page.exit [data-focus-ring] {
		display: none;
	}

	// Hide NaN in Number Flow.
	number-flow-react::part(symbol nan) {
		display: none;
	}

	number-flow-react {
		font-variant-numeric: tabular-nums;
	}
`;
