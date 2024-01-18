import { globalColors } from "./colors";
import eases from "./eases";
import forceCursors from "./force-cursor";
import reset from "./reset";

const readyDuration = (duration: number) => ({ $ready }: { $ready?: boolean }) => $ready ? `${duration}ms` : "0s";

const GlobalStyle = createGlobalStyle<{
	/** 页面已完成加载？ */
	$ready?: boolean;
}>`
	@layer base, theme, layout, props, utilities, components, special;

	${globalColors()};

	*,
	::before,
	::after {
		transition: all ${eases.easeOutMax} 250ms, color ${eases.easeOutMax} ${readyDuration(100)}, fill ${eases.easeOutMax} ${readyDuration(100)};
		font-family: -apple-system, BlinkMacSystemFont, "Segoe UI Variable Text", "Segoe UI", Roboto, Oxygen, Ubuntu, Cantarell, "Open Sans", "Helvetica Neue", "Microsoft YaHei UI", sans-serif, system-ui;
		user-select: none;
		box-sizing: border-box;
		-webkit-tap-highlight-color: transparent;
	}

	:focus,
	:focus-visible {
		outline: none;
	}

	:focus-visible {
		${styles.effects.focus()};
	}

	html {
		font-size: 14px;
	}

	body {
		background-color: var(--background-color);
		color: var(--foreground-color);
		margin: 0;
		padding: 0;
		height: 100dvh;
		overflow: hidden;
		transition: background-color ${eases.easeOutSmooth} ${readyDuration(250)};
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
