import { globalColors } from "./colors";
import eases from "./eases";
import fakeAnimations from "./fake-animations";
import forceCursors from "./force-cursor";
import reset from "./reset";

const GlobalStyle = createGlobalStyle<{
	/** 页面已完成加载？ */
	$ready?: boolean;
}>`
	@layer base, theme, layout, props, utilities, components, special;

	${globalColors()};
	${fakeAnimations};

	*,
	::before,
	::after {
		transition: all ${eases.easeOutMax} 250ms, color ${eases.easeOutMax} 100ms, fill ${eases.easeOutMax} 100ms;
		font-family: "Segoe UI Variable", "Segoe UI", "Microsoft YaHei UI", sans-serif;
		user-select: none;
		box-sizing: border-box;
		-webkit-tap-highlight-color: transparent;
		scroll-behavior: smooth;

		${({ $ready }) => !$ready && css`transition: none !important;`}
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
		background-color: var(--background-color);
		color: var(--foreground-color);
		margin: 0;
		padding: 0;
		height: 100dvh;
		overflow: hidden;
		color-scheme: dark;

		${ifColorScheme.light} & {
			color-scheme: light;
		}
	}

	#root {
		display: contents;
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
