const DEV = true;

/**
 * 在网页 DOM 加载之前执行。
 */
export default function initial() {
	// #region 配色方案
	const changeColorScheme = (isLight: boolean) => document.documentElement.dataset.scheme = isLight ? "light" : "dark";
	const lightModePreference = window.matchMedia("(prefers-color-scheme: light)");
	lightModePreference.addEventListener("change", e => changeColorScheme(e.matches));
	changeColorScheme(lightModePreference.matches);
	// #endregion

	if (!DEV) {
		// #region 阻止网页单击右键菜单
		window.addEventListener("contextmenu", e => e.preventDefault());
		// #endregion

		// #region 阻止网页键盘按键和鼠标滚轮缩放。
		window.addEventListener("keydown", e => {
			if (e.ctrlKey && ["Equal", "Minus", "NumpadAdd", "NumpadSubtract"].includes(e.code))
				e.preventDefault();
		});
		document.addEventListener("wheel", function (e) {
			if (e.ctrlKey)
				e.preventDefault();
		}, {
			capture: false,
			passive: false,
		});
		// #endregion
	}
}
