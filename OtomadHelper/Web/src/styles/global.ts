import eases from "./eases";
import reset from "./reset";

const readyColorDuration = ({ $ready }: { $ready?: boolean }) => $ready ? "100ms" : "0s";

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

	*,
	::before,
	::after {
		transition: all ${eases.easeOutSmooth} 250ms, color ${eases.easeOutSmooth} ${readyColorDuration}, fill ${eases.easeOutSmooth} ${readyColorDuration};
		font-family: -apple-system, BlinkMacSystemFont, "Segoe UI Variable Text", "Segoe UI", Roboto, Oxygen, Ubuntu, Cantarell, "Open Sans", "Helvetica Neue", "Microsoft YaHei UI", sans-serif, system-ui;
		user-select: none;
		box-sizing: border-box;
	}

	:focus,
	:focus-visible {
		outline: none;
	}

	:focus-visible {
		${styles.effects.focus()};
	}

	html,
	body {
		background-color: var(--background-color);
		color: var(--foreground-color);
		margin: 0;
		padding: 0;
		height: 100vh;
		overflow: hidden;
		transition: none !important;
		color-scheme: dark;
	}

	#root {
		display: contents;
	}

	${reset}
`;

export default GlobalStyle;
