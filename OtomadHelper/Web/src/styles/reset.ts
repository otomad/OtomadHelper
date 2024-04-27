/*
 * Reset all CSS default junk configs here.
 */
export default css`
	@layer base {
		// Goodbye, the ugly native style.
		button,
		input {
			all: unset;
			display: inline-block;
		}

		figure {
			all: unset;
			display: block;
		}

		// So images and videos default to inline elements, causing the gap below the block to be caused by you, right?
		img,
		video,
		picture {
			display: block;
			vertical-align: bottom; // Retarded VSCode CSS propertyIgnoredDueToDisplay complains that the "vertical-align" property is ignored due to "display: block". I declare it in this global basic style now, but if a component needs to modify it to "display: inline-block" for some special needs, wouldn't this "vertical-align" property be effective?
		}

		// The mouse cursor for the label should use the inherited style.
		label {
			cursor: inherit;
		}

		// Disabled style overrides.
		[disabled],
		:disabled {
			cursor: not-allowed;
			pointer-events: none;
			user-select: none;
		}

		// Inert style overrides.
		[inert] {
			&,
			* {
				*,
				::before,
				::after {
					cursor: not-allowed;
					pointer-events: none !important;
					user-select: none;
				}
			}
		}

		// Hidden style overrides for the ID selector.
		[hidden] {
			display: none !important;
		}

		// Remove the unexpected margins in headings and paragraphs.
		h1,
		h2,
		h3,
		h4,
		h5,
		h6,
		p {
			margin: 0;
		}

		// Global hyperlink style.
		a {
			color: ${c("accent-color")};
			text-decoration: none;
			border-radius: 3px;
			cursor: pointer;

			&:hover {
				opacity: 0.8;
			}

			&:active {
				opacity: 0.5;
			}
		}

		// Allow shapes in SVG elements to exceed their boundaries.
		svg,
		svg * {
			overflow: visible;
		}

		// Prevent images from being dragged.
		img {
			-webkit-user-select: none;
			-moz-user-select: none;
			-ms-user-select: none;
			user-select: none;
			-webkit-user-drag: none;
			// stylelint-disable-next-line property-no-unknown
			user-drag: none;
		}
	}

	// System requested high contrast theme.
	@media (forced-colors: active) or (prefers-contrast: more) {
		*,
		::before,
		::after {
			backdrop-filter: none !important;
		}
	}
`;
