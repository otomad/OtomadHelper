export type ColorScheme = "light" | "dark" | "auto";
export const highContrastMediaQuery = "(forced-colors: active) or (prefers-contrast: more)";
const lightModePreference = window.matchMedia("(prefers-color-scheme: light)");
const highContrastPreference = window.matchMedia(highContrastMediaQuery);

let lastClickMouseEvent: MouseEvent | undefined;
["mousedown", "mouseup", "mousemove", "mouseenter"].forEach(type => document.addEventListener(type as "mousedown", e => lastClickMouseEvent = e, true));
document.addEventListener("mouseleave", () => lastClickMouseEvent = undefined);

type ChangeColorSchemeMode = "initial" | "auto" | "manual" | "refresh";

const actualColorScheme = atom({
	scheme: "dark" as "light" | "dark",
	black: false,
	contrast: false,
});
export const useActualColorScheme = () => useAtomValue(actualColorScheme);

/**
 * Changes the color scheme of the website.
 * @param scheme - Whether to use the light color scheme or the dark color scheme.
 * If not specified, the color scheme will be automatically determined based on the user's system preferences.
 * @param amoledDark - Enable AMOLED dark scheme (pure black scheme)? Only available when the actual color scheme is dark.
 * @param contrast - Enable contrast scheme?
 * @param mode - The mode to use when changing the color scheme. Possible values are:
 * - `"initial"`: The color scheme will be updated immediately without any animation.
 * - `"auto"`: The color scheme will be updated based on the user's system preferences.
 * - `"manual"`: The color scheme will be updated with an animation.
 * - `"refresh"`: The color scheme will be updated with an animation, even if it is already set to the desired value.
 */
export function changeColorScheme(scheme?: ColorScheme, amoledDark?: boolean, contrast?: boolean, mode: ChangeColorSchemeMode = "manual") {
	const systemScheme = lightModePreference.matches ? "light" : "dark";
	scheme = scheme === "auto" || !scheme ? systemScheme : scheme;
	contrast ||= highContrastPreference.matches; // NOT `??=` !!!
	const { dataset } = document.documentElement;
	const updateThemeSettings = async () => {
		const black = scheme === "dark" && !!amoledDark;
		jotaiStore.set(actualColorScheme, { scheme, black, contrast });
		await delay(1);
		dataset.scheme = classNames(scheme, { black, contrast });
	};
	const IsColorSchemeChanged = (() => {
		const schemes = (dataset.scheme ?? "").split(" ");
		const prevAmoledDark = schemes.includes("black"), prevContrast = schemes.includes("contrast");
		const prevScheme = schemes.includes("light") ? "light" : "dark";
		if (prevScheme !== scheme) return scheme === "light" ? 1 : -1;
		else if (prevAmoledDark !== amoledDark) return !amoledDark ? 1 : -1;
		else if (prevContrast !== contrast) return contrast ? 1 : -1;
		return 0;
	})();
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
	if (IsColorSchemeChanged === 0 || mode === "refresh") {
		afterUpdateThemeSettings();
		return;
	}

	startCircleViewTransition(IsColorSchemeChanged > 0, updateThemeSettings).then(afterUpdateThemeSettings);
}

export function startCircleViewTransition(isSpread: boolean, changeFunc: () => MaybePromise<void | unknown>) {
	return new Promise<void>(resolve => {
		// It is difficult to get 100lvh (large viewport) height in JavaScript.
		const lvsEl = document.getElementById("large-viewport-size");
		const lvh = lvsEl?.offsetHeight ?? window.innerHeight, lvw = lvsEl?.offsetWidth ?? window.innerWidth;
		const { x, y } = lastClickMouseEvent ?? { x: window.innerWidth / 2, y: window.innerHeight / 2 };
		const endRadius = Math.hypot(Math.max(x, lvw - x), Math.max(y, lvh - y));
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
		], { cursor: "progress" }).then(() => {
			resolve();
		});
	});
}

{ // Init color mode
	const update = (mode: ChangeColorSchemeMode) => changeColorScheme(
		colorModeStore.scheme === "auto" ? lightModePreference.matches ? "light" : "dark" : colorModeStore.scheme,
		colorModeStore.amoledDark,
		colorModeStore.contrast || highContrastPreference.matches,
		mode,
	);
	[lightModePreference, highContrastPreference].forEach(media => media.addEventListener("change", () => update("auto")));
	changeColorScheme(colorModeStore.scheme, colorModeStore.amoledDark, colorModeStore.contrast, "initial");
	subscribeStore(colorModeStore, operations => {
		if (operations.flatMap(operation => operation[1]).every(path => path === "backgroundColor")) return;
		update("manual");
	});
}
