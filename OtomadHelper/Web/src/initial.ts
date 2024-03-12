import "helpers/color-mode";
import "styles/fonts";
import "utils/array";
import "utils/bridge";

const isProdMode = () => !useConfigStore.getState().settings.devMode;
const global = globalThis as AnyObject;

/**
 * 在网页 DOM 加载之前执行。
 */
{ // Initial
	// #region 阻止网页单击右键菜单
	window.addEventListener("contextmenu", e => {
		if (isProdMode())
			e.preventDefault();
	});
	// #endregion

	// #region 阻止网页键盘按键和鼠标滚轮缩放
	window.addEventListener("keydown", e => {
		if (isProdMode())
			if (
				e.ctrlKey && ["Equal", "Minus", "NumpadAdd", "NumpadSubtract"].includes(e.code) ||
				e.code === "F12" ||
				e.ctrlKey && e.shiftKey && ["KeyC", "KeyI"].includes(e.code)
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

	// #region 默认阻止将文件拖放到网页
	addEventListeners(window, "dragover", "drop", e => {
		if (e.dataTransfer) e.dataTransfer.dropEffect = "none";
		e.preventDefault();
	});
	// #endregion

	// #region 开发环境找编译后样式
	if (import.meta.env.DEV)
		global.findCss = function (component: string) {
			return [...document.head.querySelector("style[data-styled]")?.childNodes as NodeListOf<Text> ?? []].find(rule => rule.textContent?.includes(component));
		};
	// #endregion

	// #region 页面已完全加载
	const observer = new MutationObserver(() => {
		window.chrome.webview.postMessage("initialized");
		const splash = document.getElementById("splash")!;
		const TRANSITION_DURATION = 250;
		splash.animate({ opacity: [1, 0] }, { duration: TRANSITION_DURATION, easing: eases.easeInOutQuad }).finished
			.then(() => splash.hidden = true);
		splash.firstElementChild?.animate({ scale: [1, 0.75] }, { duration: TRANSITION_DURATION, easing: eases.easeInMax });
		observer.disconnect();
	});
	observer.observe(document.getElementById("root")!, { childList: true });
	// #endregion
}
