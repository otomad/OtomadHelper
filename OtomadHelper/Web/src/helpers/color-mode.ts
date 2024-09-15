export type ColorScheme = "light" | "dark" | "auto";
const lightModePreference = window.matchMedia("(prefers-color-scheme: light)");

let lastClickMouseEvent: MouseEvent | undefined;
document.addEventListener("click", e => lastClickMouseEvent = e, true);

/**
 * Changes the color scheme of the website.
 * @param isLight - Whether to use the light color scheme or the dark color scheme. If not specified, the color scheme will be automatically determined based on the user's system preferences.
 * @param mode - The mode to use when changing the color scheme. Possible values are:
 * - `"initial"`: The color scheme will be updated immediately without any animation.
 * - `"auto"`: The color scheme will be updated based on the user's system preferences.
 * - `"manual"`: The color scheme will be updated with an animation.
 * - `"refresh"`: The color scheme will be updated with an animation, even if it is already set to the desired value.
 */
export function changeColorScheme(isLight?: boolean | ColorScheme, mode: "initial" | "auto" | "manual" | "refresh" = "manual") {
	if (typeof isLight === "string")
		isLight = isLight === "light" ? true : isLight === "dark" ? false : undefined;
	if (isLight === undefined) isLight = lightModePreference.matches;
	const isPreviousLight = (() => {
		const { scheme } = document.documentElement.dataset;
		return ["light", "dark"].includes(scheme!) ? scheme === "light" : undefined;
	})();
	const updateThemeSettings = () => document.documentElement.dataset.scheme = isLight ? "light" : "dark";
	const afterUpdateThemeSettings = () => {
		const { backgroundColor } = getComputedStyle(document.body);
		if (backgroundColor !== "rgba(0, 0, 0, 0)")
			colorModeStore.backgroundColor = backgroundColor;
	};

	if (mode === "initial") {
		updateThemeSettings();
		return;
	} else if (mode === "auto")
		lastClickMouseEvent = undefined;
	if (isPreviousLight === isLight || mode === "refresh") {
		afterUpdateThemeSettings();
		return;
	}

	startCircleViewTransition(isLight, updateThemeSettings).then(afterUpdateThemeSettings);
}

export function startCircleViewTransition(isSpread: boolean, changeFunc: () => MaybePromise<void | unknown>) {
	return new Promise<void>(resolve => {
		const { x, y } = lastClickMouseEvent ?? { x: window.innerWidth / 2, y: window.innerHeight / 2 };
		const endRadius = Math.hypot(Math.max(x, window.innerWidth - x), Math.max(y, window.innerHeight - y));
		const clipPath = [
			`circle(0px at ${x}px ${y}px)`,
			`circle(${endRadius}px at ${x}px ${y}px)`,
		];
		startColorViewTransition(changeFunc, [
			[{
				clipPath: isSpread ? clipPath : clipPath.toReversed(),
			}, {
				pseudoElement: isSpread ? "::view-transition-new(root)" : "::view-transition-old(root)",
			}],
			[{
				zIndex: ["1", "1"],
			}, {
				pseudoElement: !isSpread ? "::view-transition-new(root)" : "::view-transition-old(root)",
			}],
			[{
				zIndex: ["calc(infinity)", "calc(infinity)"],
			}, {
				pseudoElement: isSpread ? "::view-transition-new(root)" : "::view-transition-old(root)",
			}],
		]).then(() => {
			resolve();
		});
	});
}

{ // Init color mode
	lightModePreference.addEventListener("change", e => colorModeStore.scheme === "auto" && changeColorScheme(e.matches, "auto"));
	changeColorScheme(colorModeStore.scheme, "initial");
	subscribeStoreKey(colorModeStore, "scheme", scheme => changeColorScheme(scheme));
}
