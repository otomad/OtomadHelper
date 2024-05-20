import "utils/array";
import "utils/string";

import "helpers/color-mode";
import "styles/fonts";
import "styles/properties";
import "utils/bridge";

const isProdMode = () => !useDevStore.getState().devMode;
const global = globalThis as AnyObject;

/**
 * Run before the web DOM is loading
 */
{ // Initial
	// #region Prevent context menu triggers by right click
	window.addEventListener("contextmenu", e => {
		if (!isInPath(e.target, 'input, [contenteditable="true"], [data-delete]') && isProdMode())
			e.preventDefault();
	});
	// #endregion

	// #region Prevent zoom by keyboard keys and mouse wheel
	window.addEventListener("keydown", e => {
		// Disabled only in prod mode
		if (isProdMode())
			if (
				e.ctrlKey && ["Equal", "Minus", "NumpadAdd", "NumpadSubtract"].includes(e.code) ||
				e.code === "F12" ||
				e.ctrlKey && e.shiftKey && ["KeyC", "KeyI"].includes(e.code)
			)
				e.preventDefault();
		// Disabled in any mode
		if (
			e.altKey && ["ArrowLeft", "ArrowRight"].includes(e.code)
		)
			e.preventDefault();
	});
	document.addEventListener("wheel", function (e) {
		if (isProdMode())
			if (e.ctrlKey)
				e.preventDefault();
	}, {
		capture: false,
		passive: false,
	});
	// #endregion

	// #region Prevent show scroll "compass" by mouse middle click
	window.addEventListener("mousedown", e => {
		if (e.button === 1 && isProdMode())
			e.preventDefault();
	});
	// #endregion

	// #region Prevent drag and drop by default
	addEventListeners(window, "dragover", "drop", e => {
		if (e.dataTransfer) e.dataTransfer.dropEffect = "none";
		e.preventDefault();
	});
	// #endregion

	// #region Find compiled CSS styles in dev mode
	if (import.meta.env.DEV)
		global.findCss = function (component: string) {
			return [...document.head.querySelector("style[data-styled]")?.childNodes as NodeListOf<Text> ?? []].find(rule => rule.textContent?.includes(component));
		};
	// #endregion

	// #region Fix no mouseup event responded when the mouse is moved outside the window
	document.addEventListener("pointerenter", e => {
		if (e.buttons === 0) {
			document.dispatchEvent(new Event("mouseup"));
			document.dispatchEvent(new Event("pointerup"));
			window.dispatchEvent(new Event("mouseup"));
			window.dispatchEvent(new Event("pointerup"));
		}
	});
	// #endregion

	// #region The page is fully loaded
	const observer = new MutationObserver(() => {
		postMessageToHost("initialized");
		observer.disconnect();
	});
	observer.observe(document.getElementById("root")!, { childList: true });
	// #endregion
}
