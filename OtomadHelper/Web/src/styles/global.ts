import eases from "./eases";
import reset from "./reset";

const GlobalStyle = createGlobalStyle`
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
		transition: all ${eases.easeOutMax} 250ms, color ${eases.easeOutMax} 100ms, fill ${eases.easeOutMax} 100ms;
		font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Oxygen, Ubuntu, Cantarell, "Open Sans", "Helvetica Neue", "Microsoft YaHei UI", sans-serif, system-ui;
		user-select: none;
		box-sizing: border-box;
	}

	:focus,
	:focus-visible {
		outline: none !important;
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
	}

	#root {
		display: contents;
	}

	${reset}
`;

export default GlobalStyle;
