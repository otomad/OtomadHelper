import render from "./webgl/render";

const fragments = (() => {
	const globs = import.meta.glob<string>([
		"/src/assets/glsl/**/*.frag", "!/src/assets/glsl/**/main.frag",
	], { import: "default", eager: true });
	return Object.fromEntries(Object.entries(globs).map(([path, fragment]) => [new VariableName(path.match(/\/src\/assets\/glsl\/(.*)\.frag/)![1]).camel, fragment]));
})();

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

		const canvas = document.createElement("canvas");
		for (const [name, fragment] of Object.entries(fragments)) {
			const filter = await render(imagePath, fragment, undefined, canvas);
			const blob = await filter.canvas.toBlobUrl();
			useSaved.filters[name] = blob;
		}

		return useSaved.filters;
	}, [imagePath]);

	useEffect(() => {
		getCanvasFilter(imagePath);
	}, [imagePath]);

	return saved.filters;
}
