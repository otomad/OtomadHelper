/**
 * Initialize global variables that need to be declared in the console in the development environment.
 */
if (import.meta.env.DEV) {
	// #region Find compiled CSS styles
	globals.findCss = (component: string) => {
		return [...document.head.querySelector("style[data-styled]")?.childNodes as NodeListOf<Text> ?? []].find(rule => rule.textContent?.includes(component));
	};
	// #endregion

	// #region Lodash
	globals.lodash = lodash;
	// #endregion

	// #region Send toast
	globals.toast = (message: string, status?: Status) => {
		emit("app:toast", message, status);
	};
	// #endregion

	// #region Temporarily set theme
	globals.setTheme = (...themes: string[]) => {
		const html = document.documentElement;
		html.dataset.scheme = themes.join(" ");
	};
	// #endregion

	// #region Focus testing
	globals.focusTest = (enabled: boolean = true) => {
		document.documentElement.classList.toggle("focus-testing", enabled);
	};
	// #endregion

	// #region I18n translate
	globals.i18n = i18n;
	globals.t = t;
	// #endregion

	// #region Event bus
	globals.emit = emit;
	// #endregion
}
