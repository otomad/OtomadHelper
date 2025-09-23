import { FALLBACK_TRANSITIONS } from "utils/styles";
import { globalColors } from "./colors";
import counters from "./counters";
import eases from "./eases";
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
		user-select: none;
		transition: ${fallbackTransitions};
		forced-color-adjust: none;
		-webkit-tap-highlight-color: transparent;

		:where(&) {
			/* color: var(--foreground-color); */
			text-wrap: pretty;
		}

		${({ $ready }) => !$ready && css`
			transition: all ${eases.easeOutMax} 250ms, color 0s, fill 0s, font-size 0s, tab-size 0s;
		`}
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
		height: 100dvh;
		margin: 0;
		padding: 0;
		color: var(--foreground-color);
		font-feature-settings: "halt" on;
		font-kerning: normal;
		font-synthesis: none;
		font-variant-ligatures: common-ligatures historical-ligatures contextual;
		font-variant-numeric: proportional-nums;
		text-rendering: optimizeLegibility;
		tab-size: 4;
		background-color: var(--background-color);
		touch-action: manipulation;
		transition: ${fallbackTransitions}, width 0s, height 0s;
		color-scheme: dark;
		accent-color: var(--accent-color);
		-webkit-font-smoothing: antialiased;
		-moz-osx-font-smoothing: grayscale;

		&:where(:lang(zh), :lang(ja), :lang(ko)) {
			text-align: justify;
		}

		${ifColorScheme.light} & {
			color-scheme: only light;
		}

		${({ $ready }) => !$ready && css`
			transition: background-color 0s;
		`}
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

	#large-viewport-size {
		position: fixed;
		top: 0;
		left: 0;
		width: 100lvw;
		height: 100lvh;
		visibility: hidden;
	}

	// Additional calculated colors
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

	// Focus testing
	html.focus-testing ${important()}:focus {
		${focusTestStyle};
	}

	// User requested to reduce dynamic effects
	${ifColorScheme.reduceMotion} {
		${important(2)}:not(.${ifColorScheme.forceMotion}, .${ifColorScheme.forceMotion} *) {
			&,
			&::before,
			&::after {
				scroll-behavior: auto;
				transition-duration: 0s !important;
				transition-delay: 0s !important;
				animation-duration: 0s !important;
				animation-delay: 0s !important;
			}
		}
	}

	// System requested high contrast theme.
	${ifColorScheme.contrast} {
		*,
		::before,
		::after {
			backdrop-filter: none !important;
		}

		:root& {
			--fallback-transitions-for-contrast-scheme: color 0s, background-color 0s, border-color 0s;
			--fallback-transitions: ${FALLBACK_TRANSITIONS}, var(--fallback-transitions-for-contrast-scheme);
		}
	}

	${ifColorScheme.reduceTransparency} {
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
