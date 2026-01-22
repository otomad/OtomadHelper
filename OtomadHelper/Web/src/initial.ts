import "utils/array";
import "utils/misc";
import "utils/number";
import "utils/string";

import "helpers/color-mode";
import "helpers/dev-global";
import "helpers/host-messages_listen";
import "styles/fonts";
import "styles/properties";
import "utils/bridge";

import { Enum } from "enum-plus";
import { enableMapSet } from "immer";
import mapPlugin from "plugins/enum-plus/map";
import { config as transitionGroupConfig } from "react-transition-group-fc";

/**
 * Run before the web DOM is loading.
 */
{ // Initial
	const isFocusOnInputField = (e: Event) => isInPath(e.target, 'input[type="text"], textarea, [contenteditable]:not([contenteditable="false"])');

	// #region Prevent context menu triggers by right click
	window.addEventListener("contextmenu", e => {
		window.contextMenu = undefined;
		if (!isFocusOnInputField(e) && isProdMode())
			e.preventDefault();
	});
	// #endregion

	// #region Prevent zoom by keyboard keys and mouse wheel
	window.addEventListener("keydown", e => {
		// Disabled only in prod mode
		if (isProdMode())
			if (
				// Zoom (Ctrl + + / Ctrl + -)
				e.ctrlKey && ["Equal", "Minus", "NumpadAdd", "NumpadSubtract"].includes(e.code) ||
				// Open DevTools (F12 / Ctrl + Shift + I / Ctrl + Shift + C)
				e.code === "F12" ||
				e.ctrlKey && e.shiftKey && ["KeyC", "KeyI"].includes(e.code) ||
				// Select all (Ctrl + A)
				!isFocusOnInputField(e) && e.ctrlKey && e.code === "KeyA"
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

	// #region Fix no mouseup event responded when the mouse is moved outside the window
	document.addEventListener("pointerenter", e => {
		if (e.buttons === 0) {
			document.dispatchEvent(new MouseEvent("mouseup"));
			document.dispatchEvent(new PointerEvent("pointerup"));
			window.dispatchEvent(new MouseEvent("mouseup"));
			window.dispatchEvent(new PointerEvent("pointerup"));
		}
	});
	// #endregion

	// #region Prevent space bar from scrolling page in any time
	window.addEventListener("keydown", e => {
		if (e.code === "Space" && e.target === document.body)
			e.preventDefault();
	});
	// #endregion

	// #region The page is fully loaded
	const observer = new MutationObserver(() => {
		postMessageToHost("initialized");
		observer.disconnect();
	});
	observer.observe(document.getElementById("root")!, { childList: true });
	// #endregion

	// #region Listen page focus event
	window.addEventListener("focus", () => bridges.bridge.setPageFocus(true));
	window.addEventListener("blur", () => bridges.bridge.setPageFocus(false));
	// #endregion

	// #region Enable Map and Set support for Immer
	enableMapSet();
	// #endregion

	// #region Dispatch global transition exit event
	transitionGroupConfig.onExit = node => window.dispatchEvent(createCustomEvent("transitionExitCapture", { detail: { target: node } }));
	// #endregion

	// #region Init enum plus localization method
	Enum.localize = (label: unknown) => typeof label === "function" ? label() : label?.toString();
	Enum.config.autoLabel = ({ item: { key, raw: { label } }, labelPrefix }) => label ||
		(typeof labelPrefix === "string" ? `${labelPrefix}.${key}` : isI18nItem(labelPrefix) ? labelPrefix[key] : undefined!);
	Enum.install(mapPlugin);
	// #endregion
}
