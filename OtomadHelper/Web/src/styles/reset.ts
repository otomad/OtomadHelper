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

		ul,
		ol,
		li {
			all: unset;
		}

		table {
			border-spacing: 0;
		}

		th,
		td {
			padding: 0;
		}

		// So images and videos default to inline elements, causing the gap below the block to be caused by you, right?
		img,
		video,
		picture {
			display: block;
			vertical-align: bottom; // Retarded VSCode CSS propertyIgnoredDueToDisplay complains that the "vertical-align" property is ignored due to "display: block". I declare it in this global basic style now, but if a component needs to modify it to "display: inline-block" for some special needs, wouldn't this "vertical-align" property be effective?
			image-rendering: -webkit-optimize-contrast;
			image-rendering: crisp-edges;

			.pixelated & {
				image-rendering: pixelated;
			}
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
			interactivity: inert;
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
			text-decoration: none;
			cursor: pointer;

			&:not(.button) {
				color: ${c("accent-color")};
				border-radius: 3px;

				&:hover {
					opacity: 0.8;
				}

				&:active {
					opacity: 0.5;
				}
			}
		}

		// Allow shapes in SVG elements to exceed their boundaries.
		svg,
		svg * {
			overflow: visible;
		}

		// Prevent images from being dragged.
		img {
			// For alt text.
			font-style: italic;
			text-wrap: balance;

			-webkit-user-select: none;
			-moz-user-select: none;
			-ms-user-select: none;
			user-select: none;
			-webkit-user-drag: none;
			// stylelint-disable-next-line property-no-unknown
			user-drag: none;
		}

		// Progress bar style.
		progress {
			width: 100%;
			border: none;
			appearance: none;

			&::-webkit-progress-bar,
			& {
				background-color: transparent;
				transition: ${fallbackTransitions};
			}

			${progressFinishedPart`
				${styles.mixins.oval()};
				background-color: ${c("accent-color")};
				transition: ${fallbackTransitions};
			`}
		}

		// Do not change font in math.
		math {
			${styles.mixins.inherit("font-weight", "line-height", "word-spacing")};
		}

		// Do not semi-transparent Combobox when disabled
		select:disabled {
			opacity: 1;
		}
	}
`;
