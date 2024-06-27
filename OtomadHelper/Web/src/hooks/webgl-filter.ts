import { fragNames } from "virtual:fragment-filters";
import filter from "./webgl/render";

type FilterBlobs = Record<string, string>;

const useSaved = createStore({
	imagePath: "",
	filters: {} as FilterBlobs,
});

export function useWebglFilters(imagePath: string) {
	const saved = useSnapshot(useSaved);

	const getCanvasFilter = useCallback(async (imagePath: string) => {
		if (imagePath === useSaved.imagePath) return saved.filters;
		else {
			useSaved.imagePath = imagePath;
			for (const url of Object.values(saved.filters))
				URL.revokeObjectURL(url);
		}

		const image = await createImageFromUrl(imagePath);
		// await delay(250); // Delay for the expander expanding duration, or the animation will be lost at the first time.
		filter.changeImage(image);
		for (const name of fragNames) { // Apply filter one by one
		// fragNames.forEach(async name => { // Apply filter simultaneously
			filter.changeFilter(name);
			filter.apply();
			const blob = await filter.canvas.toBlobURL();
			useSaved.filters[name] = blob;
		}

		return useSaved.filters;
	}, [imagePath]);

	useEffect(() => {
		getCanvasFilter(imagePath);
	}, [imagePath]);

	return saved.filters;
}
