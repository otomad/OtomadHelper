import { fragNames } from "virtual:fragment-filters";
import filter, { type WebGLFilter } from "./webgl/render";

type FilterBlobs = Record<string, string>;

const useSaved = createStore({
	imagePath: "",
	filters: {} as FilterBlobs,
});

async function saveFilter(filter: WebGLFilter, name: string) {
	const blob = await filter.canvas.toBlobURL();
	useSaved.filters[name] = blob;
}

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
		// fragNames.forEach(async name => { // Apply filter simultaneously, it will be massy, do not use it.
			filter.changeFilter(name);
			filter.apply();
			await saveFilter(filter, name);
		}

		// #region Special saved filters
		filter.changeFilter("twist");
		filter.uniform("1f", "twist_angle", 5);
		filter.apply();
		await saveFilter(filter, "twist_ccw");
		// #endregion

		return useSaved.filters;
	}, [imagePath]);

	useEffect(() => {
		getCanvasFilter(imagePath);
	}, [imagePath, getCanvasFilter]);

	return saved.filters;
}
