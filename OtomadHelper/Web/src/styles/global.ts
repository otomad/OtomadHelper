import { globalColors } from "./colors";
import eases from "./eases";
import fakeAnimations from "./fake-animations";
import forceCursors from "./force-cursor";
import reset from "./reset";

const GlobalStyle = createGlobalStyle<{
	/** 页面已完成加载？ */
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
		user-select: none;
		transition: all ${eases.easeOutMax} 250ms, color ${eases.easeOutMax} 100ms, fill ${eases.easeOutMax} 100ms;
		-webkit-tap-highlight-color: transparent;

		/* :where(&) {
			color: var(--foreground-color);
		} */

		${({ $ready }) => !$ready && css`
			transition: all ${eases.easeOutMax} 250ms, color 0s, fill 0s, font-size 0s;
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

	:where(:lang(en), :lang(ru), :lang(vi), :lang(id)) {
		text-wrap: pretty;
		hyphens: auto;
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
		font-variant-east-asian: proportional-width;
		font-variant-ligatures: common-ligatures historical-ligatures contextual;
		font-variant-numeric: proportional-nums;
		text-rendering: optimizeLegibility;
		tab-size: 4;
		background-color: var(--background-color);
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
	.appear:not(.appear-active) {
		&,
		* {
			&,
			&::before,
			&::after {
				transition: none !important;
			}
		}
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
		z-index: 2147483646;
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
