import { FALLBACK_TRANSITIONS } from "utils/styles";
import { globalColors } from "./colors";
import counters from "./counters";
import fakeAnimations from "./fake-animations";
import fix from "./fix";
import focusTestStyle from "./focus-test";
import functions from "./functions";
import reset from "./reset";

const GlobalStyle = createGlobalStyle<{
	/** Has the page loaded completely? */
	$ready?: boolean;
}>`
	${globalColors()};
	${fakeAnimations};
	${functions};
	${counters};

	*,
	::before,
	::after {
		--cjk-font-family: "Microsoft YaHei UI";
		box-sizing: border-box;
		overscroll-behavior: none;
		scroll-behavior: smooth;
		// Kick out the \`system-ui\`.
		font-family: "Yozora Sans", Inter, "Segoe UI Variable", "Segoe UI", var(--cjk-font-family), "Microsoft YaHei UI", sans-serif;
		font-optical-sizing: auto;
		hyphens: auto;
		hyphenate-limit-chars: 10;
		-webkit-tap-highlight-color: transparent;
		user-select: none;
		transition: ${fallbackTransitions};
		forced-color-adjust: none;

		@layer base {
			/* color: var(--foreground-color); */
			text-wrap: pretty;
		}

		${({ $ready }) => !$ready && css`
			transition: none !important;
		`}
	}

	@layer base {
		[lang] {
			&:is(:lang(zh), :lang(ja), :lang(ko)) {
				text-align: justify;
				hanging-punctuation: none;
			}

			&:is(:lang(th), :lang(lo), :lang(km), :lang(my)) {
				text-align: justify;
			}

			&:not(:lang(zh), :lang(ja), :lang(ko)) {
				text-align: start;
				hanging-punctuation: first allow-end last;
			}
		}
	}

	:lang(zh-Hant) {
		&,
		&::before,
		&::after {
			--cjk-font-family: "Microsoft JhengHei UI";
		}
	}

	:lang(ja) {
		&,
		&::before,
		&::after {
			--cjk-font-family: "Yu Gothic UI", "Meiryo UI", "MS UI Gothic";
		}
	}

	:lang(ko) {
		&,
		&::before,
		&::after {
			--cjk-font-family: "Malgun Gothic";
		}
	}

	code,
	pre,
	kbd,
	samp,
	tt,
	xmp,
	.monospace {
		&,
		* {
			&,
			&::before,
			&::after {
				font-family: "Cascadia Code", "Cascadia Mono", "JetBrains Mono", "Fira Code", "Fira Mono", "Roboto Mono", monospace;
			}
		}
	}

	math {
		&,
		* {
			font-family: "Lete Sans Math", math;
		}
	}

	@layer base {
		:focus,
		:focus-visible {
			outline: none;
		}
	}

	:focus-visible {
		${styles.effects.focus()};
	}

	html {
		${styles.effects.text.body};
		line-height: normal;
		interpolate-size: allow-keywords;
	}

	html,
	body {
		overflow: clip;
	}

	body {
		position: relative;
		block-size: 100dvb;
		margin: 0;
		padding: 0;
		color: var(--foreground-color);
		font-feature-settings: "halt" on;
		font-kerning: normal;
		font-synthesis: none;
		font-variant-ligatures: common-ligatures historical-ligatures contextual;
		font-variant-numeric: proportional-nums;
		text-spacing-trim: trim-start;
		text-spacing-trim: trim-both;
		text-autospace: normal;
		-webkit-font-smoothing: antialiased;
		-moz-osx-font-smoothing: grayscale;
		text-rendering: geometricPrecision;
		tab-size: 4;
		background-color: var(--background-color);
		touch-action: manipulation;
		transition: ${fallbackTransitions}, width 0s, height 0s;
		accent-color: var(--accent-color);
	}

	#root {
		display: contents;
	}

	.enter:not(.enter-active),
	.appear:not(.appear-active),
	.enter-from,
	.appear-from {
		&,
		* {
			&,
			&::before,
			&::after {
				transition: none !important;
			}
		}
	}

	.calc-size {
		position: fixed;
		opacity: 0;
		visibility: hidden;
		transition: none;
	}

	svg,
	svg * {
		transition: ${fallbackTransitions}, color 0s;
	}

	// Color schemes
	:root {
		@layer base {
			--color-scheme: dark;
			--color-scheme-black: false;
			--color-scheme-contrast: false;
			--color-scheme-reduce-transparency: false;
			--color-scheme-reduce-motion: false;
		}

		@media (forced-colors: active) or (prefers-contrast: more) {
			--color-scheme-contrast: true;
		}

		@media (prefers-reduced-transparency: reduce) {
			--color-scheme-reduce-transparency: true;
		}

		@media (prefers-reduced-motion: reduce) {
			--color-scheme-reduce-motion: true;
		}
	}

	[data-scheme~="dark"] {
		--color-scheme: dark;
		color-scheme: dark;
	}

	[data-scheme~="light"] {
		--color-scheme: light;
		color-scheme: only light;
	}

	[data-scheme~="dark"][data-scheme~="black"] {
		--color-scheme-black: true;
	}

	[data-scheme~="contrast"] {
		--color-scheme-contrast: true;
		--colorization: transparent;
	}

	// Prevent DevTools Ctrl+Shift+C to select an element inside the svg.
	@layer base {
		svg * {
			pointer-events: none;
		}
	}

	::selection {
		color: ${c("fill-color-text-on-accent-selected-text")};
		background-color: ${c("accent-color")};
	}

	::target-text {
		color: ${c("fill-color-text-on-accent-selected-text")};
		background-color: ${c("fill-color-system-target")};
	}

	// Get rid of nested disabled with multiple translucent.
	/* [disabled] [disabled] {
		&,
		* {
			opacity: 1 !important;
		}
	} */

	#large-viewport-size {
		position: fixed;
		top: 0;
		left: 0;
		width: 100lvw;
		height: 100lvh;
		visibility: hidden;
	}

	// Additional calculated colors
	/* stylelint-disable-next-line no-duplicate-selectors */
	:root {
		--fill-color-system-accent-background: rgb(from var(--accent-color) r g b / 15%);
		--fallback-transitions: ${FALLBACK_TRANSITIONS};
		--fallback-transitions-for-contrast-scheme: content 0s; // Placeholder for a invalid property.
	}

	// Color mode transition
	:root:active-view-transition-type(instant) {
		@layer base {
			&::view-transition-old(root),
			&::view-transition-new(root) {
				mix-blend-mode: normal;
				animation-name: none;
			}
		}
	}

	// Force cursor
	html[style*="--cursor"] {
		&,
		* {
			&,
			::before,
			::after {
				cursor: var(--cursor) !important;
			}
		}
	}

	// Search anchor landmark
	[data-anchor] {
		view-transition-name: attr(data-anchor type(<custom-ident>));
	}

	// Focus testing
	html.focus-testing ${important()}:focus {
		${focusTestStyle};
	}

	// User requested to reduce dynamic effects
	@container style(--color-scheme-reduce-motion: true) { // \${important(2)}:not(.force-motion, .force-motion *)
		*,
		::before,
		::after {
			scroll-behavior: auto;
			transition-duration: 0s !important;
			transition-timing-function: step-start !important;
			transition-delay: 0s !important;
			animation-duration: 0s !important;
			animation-timing-function: step-start !important;
			animation-delay: 0s !important;
		}
	}

	// System requested high contrast theme.
	@container style(--color-scheme-contrast: true) { // :not(.focus-highlight-ring)
		*,
		::before,
		::after {
			backdrop-filter: none !important;
		}
	}

	:root[data-scheme~="contrast"] {
		--fallback-transitions-for-contrast-scheme: color 0s, background-color 0s, border-color 0s;
		--fallback-transitions: ${FALLBACK_TRANSITIONS}, var(--fallback-transitions-for-contrast-scheme);
	}

	@container style(--color-scheme-reduce-transparency: reduce) {
		*,
		::before,
		::after {
			backdrop-filter: none !important;
		}
	}

	${fix}
	${reset}
`;

export default GlobalStyle;
