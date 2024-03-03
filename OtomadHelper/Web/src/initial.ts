import "helpers/color-mode";
import "styles/fonts";
import "utils/array";

const isProdMode = () => !useConfigStore.getState().settings.devMode;
const global = globalThis as AnyObject;

{ // Initial
	/**
	 * 在网页 DOM 加载之前执行。
	 */
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

	// #region 本机端代码向网页端通信事件
	window.chrome ??= {};
	window.chrome.webview?.addEventListener("message", e => {
		console.log(e);
	});
	// #endregion
}
