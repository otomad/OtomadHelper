export type ColorScheme = "light" | "dark" | "auto";
const lightModePreference = window.matchMedia("(prefers-color-scheme: light)");

let lastClickMouseEvent: MouseEvent | undefined;
document.addEventListener("click", e => lastClickMouseEvent = e, true);

export const changeColorScheme = (isLight?: boolean | ColorScheme, mode: "initial" | "auto" | "manual" | "refresh" = "manual") => {
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
			useColorModeStore.setState(() => ({ backgroundColor }));
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

	const { x, y } = lastClickMouseEvent ?? { x: window.innerWidth / 2, y: window.innerHeight / 2 };
	const endRadius = Math.hypot(Math.max(x, window.innerWidth - x), Math.max(y, window.innerHeight - y));
	const clipPath = [
		`circle(0px at ${x}px ${y}px)`,
		`circle(${endRadius}px at ${x}px ${y}px)`,
	];
	const CHANGING_COLOR_SCHEME_CLASS = "changing-color-scheme";
	document.documentElement.classList.add(CHANGING_COLOR_SCHEME_CLASS);
	startViewTransition(updateThemeSettings, {
		clipPath: isLight ? clipPath : clipPath.toReversed(),
	}, {
		duration: 300,
		easing: eases.easeInOutSmooth,
		pseudoElement: isLight ? "::view-transition-new(root)" : "::view-transition-old(root)",
	}).then(() => {
		document.documentElement.classList.remove(CHANGING_COLOR_SCHEME_CLASS);
		afterUpdateThemeSettings();
	});
};
const getUserColorScheme = () => useColorModeStore.getState().scheme;

{ // Init color mode
	lightModePreference.addEventListener("change", e => getUserColorScheme() === "auto" && changeColorScheme(e.matches, "auto"));
	changeColorScheme(getUserColorScheme(), "initial");
	useColorModeStore.subscribe(state => state.scheme, scheme => changeColorScheme(scheme));
}
