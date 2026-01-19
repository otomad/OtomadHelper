function getContrastPaletteEvaluation() {
	const testElement = document.createElement("div");
	testElement.hidden = true;
	testElement.style.color = c("foreground-color");
	testElement.style.accentColor = c("accent-color");
	testElement.style.backgroundColor = c("background-color");
	document.body.append(testElement);
	// CAUTION: Directly use `getComputedStyle(document.documentElement)` will unexpectedly get `light-dark()` function value.
	const { color, accentColor, backgroundColor } = getComputedStyle(testElement);
	testElement.remove();
	try {
		const contrast = Math.min(
			Color.contrastWCAG21(accentColor, backgroundColor),
			Color.contrastWCAG21(color, backgroundColor),
		);
		return contrast >= 3 ? "high" : contrast >= 2 ? "low" : "very low";
	} catch {
		return "high";
	}
}

const contrastPaletteEvaluationAtom = atom<ReturnType<typeof getContrastPaletteEvaluation>>("high");

const updateContrastPaletteEvaluation = () => jotaiStore.set(contrastPaletteEvaluationAtom, getContrastPaletteEvaluation());

export const useContrastPaletteEvaluation = () => useAtom(contrastPaletteEvaluationAtom)[0];

useListen.on("app:evaluateContrastPalette", updateContrastPaletteEvaluation);
