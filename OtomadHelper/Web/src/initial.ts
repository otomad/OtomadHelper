import { initColorMode } from "helpers/color-mode";

const isProdMode = () => !useConfigStore.getState().settings.devMode;

/**
 * 在网页 DOM 加载之前执行。
 */
export default function initial() {
	// #region 配色方案
	initColorMode();
	// #endregion

	// #region 阻止网页单击右键菜单
	window.addEventListener("contextmenu", e => {
		if (isProdMode())
			e.preventDefault();
	});
	// #endregion

	// #region 阻止网页键盘按键和鼠标滚轮缩放
	window.addEventListener("keydown", e => {
		if (isProdMode())
			if (e.ctrlKey && ["Equal", "Minus", "NumpadAdd", "NumpadSubtract"].includes(e.code))
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
}
