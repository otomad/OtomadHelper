import "helpers/color-mode";
import "styles/fonts";
import "styles/properties";
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
		// 仅生产模式下禁用
		if (isProdMode())
			if (
				e.ctrlKey && ["Equal", "Minus", "NumpadAdd", "NumpadSubtract"].includes(e.code) ||
				e.code === "F12" ||
				e.ctrlKey && e.shiftKey && ["KeyC", "KeyI"].includes(e.code)
			)
				e.preventDefault();
		// 任何模式下均禁用
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

	// #region 修复当鼠标移至视窗外时无法响应鼠标弹起事件的问题
	document.addEventListener("pointerenter", e => {
		if (e.buttons === 0) {
			document.dispatchEvent(new Event("mouseup"));
			document.dispatchEvent(new Event("pointerup"));
			window.dispatchEvent(new Event("mouseup"));
			window.dispatchEvent(new Event("pointerup"));
		}
	});
	// #endregion

	// #region 页面已完全加载
	const observer = new MutationObserver(() => {
		window.chrome.webview.postMessage("initialized");
		observer.disconnect();
	});
	observer.observe(document.getElementById("root")!, { childList: true });
	// #endregion
}
