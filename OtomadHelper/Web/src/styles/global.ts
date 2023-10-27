import eases from "./eases";
import reset from "./reset";

const readyColorDuration = ({ $ready }: { $ready?: boolean }) => $ready ? "100ms" : "0s";

const GlobalStyle = createGlobalStyle<{
	/** 页面已完成加载？ */
	$ready?: boolean;
}>`
	@layer base, theme, layout, props, utilities, components, special;

	:root {
		--background-color: #2d2d2d;
		--foreground-color: white;
		/* --press-color: #636363; */
		--accent-color: #14b2ff;
		/* --border-color: #2a2a2a; */
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
		outline: none !important;
	}

	:focus-visible {
		box-shadow: 0 0 0 3px white;
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
