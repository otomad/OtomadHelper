import { globalColors } from "./colors";
import eases from "./eases";
import reset from "./reset";

const readyDuration = (duration: number) => ({ $ready }: { $ready?: boolean }) => $ready ? `${duration}ms` : "0s";

const GlobalStyle = createGlobalStyle<{
	/** 页面已完成加载？ */
	$ready?: boolean;
}>`
	@layer base, theme, layout, props, utilities, components, special;

	:root {
		/* --background-color: #2d2d2d;
		--foreground-color: white;
		--accent-color: #14b2ff;
		--press-color: #636363;
		--border-color: #2a2a2a; */
		--background-color: #202020;
		--foreground-color: white;
		--accent-color: #60cdff;
	}

	:root${ifColorScheme.light} {
		--background-color: #f3f3f3;
		--foreground-color: #000000e5;
		--accent-color: #005fb8;
	}

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

	${reset}
`;

export default GlobalStyle;
