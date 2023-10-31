export type ColorScheme = "light" | "dark" | "auto";
const lightModePreference = window.matchMedia("(prefers-color-scheme: light)");
const changeColorScheme = (isLight?: boolean | ColorScheme) => {
	if (typeof isLight === "string")
		isLight = isLight === "light" ? true : isLight === "dark" ? false : undefined;
	if (isLight === undefined) isLight = lightModePreference.matches;
	document.documentElement.dataset.scheme = isLight ? "light" : "dark";
};
const getUserColorScheme = () => useColorMode.getState().scheme;

export function initColorMode() {
	lightModePreference.addEventListener("change", e => getUserColorScheme() === "auto" && changeColorScheme(e.matches));
	changeColorScheme(getUserColorScheme());
	useColorMode.subscribe(({ scheme }) => changeColorScheme(scheme));
}
