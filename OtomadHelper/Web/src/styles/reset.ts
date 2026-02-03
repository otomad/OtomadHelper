/*
 * Reset all CSS default junk configs here.
 */
export default css`
	@layer base {
		// Goodbye, the ugly native style.
		input,
		select,
		textarea,
		button {
			font: inherit;
		}

		button,
		input {
			all: unset;
			display: inline-block;
		}

		input,
		textarea,
		[contenteditable]:not([contenteditable="false"]) {
			field-sizing: content;

			@supports (caret-animation: manual) {
				caret-animation: if(
					media(prefers-reduced-motion: no-preference): manual;
						else: auto;
				);

				&:focus {
					animation: ${keyframes`
						to {
							caret-color: transparent;
						}
					`} 500ms infinite ${eases.easeInOutMax} alternate;
				}
			}
		}

		figure {
			all: unset;
			display: block;
			inline-size: fit-content;
			margin-inline: auto;
		}

		figcaption {
			font-size: inherit;
		}

		ul,
		ol,
		li {
			all: unset;
			display: block;
		}

		li {
			display: list-item;
			list-style: none;
		}

		table {
			caption-side: bottom;
			border-spacing: 0;
			border-collapse: collapse;
		}

		[role="region"][aria-labelledby][tabindex] {
			overflow: auto;
		}

		th,
		td {
			padding: 0;
		}

		// So images and videos default to inline elements, causing the gap below the block to be caused by you, right?
		img,
		video,
		picture,
		iframe {
			display: block;
			vertical-align: bottom; // Retarded VSCode CSS propertyIgnoredDueToDisplay complains that the "vertical-align" property is ignored due to "display: block". I declare it in this global basic style now, but if a component needs to modify it to "display: inline-block" for some special needs, wouldn't this "vertical-align" property be effective?
			image-rendering: -webkit-optimize-contrast;
			image-rendering: crisp-edges;
			border-style: none;

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
		:disabled,
		[aria-disabled="true" i] {
			cursor: not-allowed;
			pointer-events: none;
			user-select: none;
			// DO NOT add \`interactivity: inert\` which will let screen reader to skip it.
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
					interactivity: inert;
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

		// Balance heading wrap typography.
		h1,
		h2,
		h3,
		h4,
		h5,
		h6 {
			&,
			* {
				text-wrap: balance;
			}
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
		svg *,
		// Chromium 144 and FireFox 137 (maybe) use the new SVG2 cascading behavior, so beside the <svg>, we should also add the <symbol>.
		// See: https://issues.chromium.org/issues/476755407
		symbol,
		symbol * {
			overflow: visible !important;
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

			${progressFinishedPart(css`
				${styles.mixins.oval()};
				background-color: ${c("accent-color")};
				transition: ${fallbackTransitions};
			`)}
		}

		// Do not change font in math.
		math {
			${styles.mixins.inherit(
				"font-weight",
				"line-height",
				"word-spacing",
			)};
		}

		// Do not semi-transparent Combobox when disabled.
		select:disabled {
			opacity: 1;
		}

		// Hide browser default input clear all button.
		input::-webkit-search-cancel-button {
			display: none;
		}

		// No headbutting
		:target {
			scroll-margin: 3rlh;
		}

		// I don't know why Chromium user agent stylesheet use \`text-autospace: no-autospace !important;\` for the <select>.
		select,
		select * {
			text-autospace: normal !important;
		}
	}
`;
