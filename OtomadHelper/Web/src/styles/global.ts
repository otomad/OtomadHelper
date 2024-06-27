import { globalColors } from "./colors";
import eases from "./eases";
import fakeAnimations from "./fake-animations";
import forceCursors from "./force-cursor";
import reset from "./reset";

const GlobalStyle = createGlobalStyle<{
	/** The page has completed loading? */
	$ready?: boolean;
}>`
	${globalColors()};
	${fakeAnimations};

	*,
	::before,
	::after {
		box-sizing: border-box;
		scroll-behavior: smooth;
		font-family: "Segoe UI Variable", "Segoe UI", "Microsoft YaHei UI", sans-serif;
		hyphens: auto;
		user-select: none;
		transition: ${fallbackTransitions};
		-webkit-tap-highlight-color: transparent;

		/* :where(&) {
			color: var(--foreground-color);
		} */

		${({ $ready }) => !$ready && css`
			transition: all ${eases.easeOutMax} 250ms, color 0s, fill 0s, font-size 0s, tab-size 0s;
		`}
	}

	:lang(zh-Hant) {
		&,
		&::before,
		&::after {
			font-family: "Segoe UI Variable", "Segoe UI", "Microsoft JhengHei", "Microsoft YaHei UI", sans-serif;
		}
	}

	:lang(ja) {
		&,
		&::before,
		&::after {
			font-family: "Segoe UI Variable", "Segoe UI", "Yu Gothic UI", "Meiryo UI", "MS UI Gothic", "Microsoft YaHei UI", sans-serif;
		}
	}

	:lang(ko) {
		&,
		&::before,
		&::after {
			font-family: "Segoe UI Variable", "Segoe UI", "Malgun Gothic", "Microsoft YaHei UI", sans-serif;
		}
	}

	code,
	pre,
	kbd,
	samp,
	tt,
	xmp {
		&,
		* {
			&,
			&::before,
			&::after {
				font-family: "Cascadia Code", "Cascadia Mono", "JetBrains Mono", "Segoe UI Mono", Consolas, "SF Mono", monospace;
			}
		}
	}

	:where(:lang(en), :lang(ru), :lang(vi), :lang(id)) {
		text-wrap: pretty;
	}

	:focus,
	:focus-visible {
		outline: none;
	}

	:focus-visible {
		${styles.effects.focus()};
	}

	html {
		${styles.effects.text.body};
		line-height: normal;
	}

	body {
		height: 100dvh;
		margin: 0;
		padding: 0;
		overflow: clip;
		color: var(--foreground-color);
		font-kerning: normal;
		font-synthesis: none;
		font-variant-ligatures: common-ligatures historical-ligatures contextual;
		font-variant-numeric: proportional-nums;
		text-rendering: optimizeLegibility;
		tab-size: 4;
		background-color: var(--background-color);
		transition: ${fallbackTransitions}, width 0s, height 0s;
		color-scheme: dark;
		-webkit-font-smoothing: antialiased;
		-moz-osx-font-smoothing: grayscale;

		&:where(:lang(zh), :lang(ja), :lang(ko)) {
			text-align: justify;
		}

		${ifColorScheme.light} & {
			color-scheme: light;
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

	// stylelint-disable-next-line selector-id-pattern
	/* #__svg__icons__dom__ {
		content-visibility: hidden;
	} */

	.calc-size {
		position: fixed;
		opacity: 0;
		visibility: hidden;
		transition: none;
	}

	// Color mode transition
	::view-transition-old(root),
	::view-transition-new(root) {
		mix-blend-mode: normal;
		animation: none;
	}

	::view-transition-old(root),
	${ifColorScheme.dark}.changing-color-scheme::view-transition-new(root) {
		z-index: 1;
	}

	::view-transition-new(root),
	${ifColorScheme.dark}.changing-color-scheme::view-transition-old(root) {
		z-index: calc(infinity);
	}

	${() => {
		return forceCursors.map(cursor => css`
			body[data-cursor="${cursor}"] {
				&,
				* {
					cursor: ${cursor} !important;
				}
			}
		`);
	}}

	${reset}
`;

export default GlobalStyle;
