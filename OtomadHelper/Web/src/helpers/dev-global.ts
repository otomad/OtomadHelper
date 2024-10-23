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
	globals.toast = (message: string) => {
		useEvent("app:toast", message);
	};
	// #endregion
}
