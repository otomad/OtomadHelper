import { readFile } from "fs/promises";
import { DOMParser } from "linkedom";

const CURSOR_BASE_SIZE = 32;
const CURSOR_MAX_SIZE = 128; // Chromium (maybe) has max custom cursor size limit, exceed it will use default cursor.

const getSvgPath = (id: string, ext: RegExp) => ext.test(id) ? id.replace(/\?.*/, "") : false;

const svgCursorExt = /\.svg\?cursor$/i;
export const svgCursor = (): VitePlugin => {
	return {
		name: "vite-plugin-svg-cursor",
		enforce: "pre",

		async load(id) {
			const filePath = getSvgPath(id, svgCursorExt);
			if (!filePath) return;

			const svgString = await readFile(filePath, "utf-8");
			const cursorAttrs = getSvgDataset<"hotspotX" | "hotspotY" | "fallback" | "baseWidth" | "baseHeight">(svgString);
			const { hotspotX = "0", hotspotY = "0", fallback = "default", baseWidth = "32" } = cursorAttrs;

			return (
				`import svg from "${filePath}";\n` +
				`export default ({ theme: { systemCursorConfig: { cursorSize = ${CURSOR_BASE_SIZE}, cursorFill = "white" } } }) =>\n` +
				`\`url("\${svg}?cursor=&size=\${Math.min(cursorSize, ${CURSOR_MAX_SIZE})}&fill=\${encodeURIComponent(cursorFill)}") \${${hotspotX} / ${baseWidth} * Math.min(cursorSize, ${CURSOR_MAX_SIZE})} \${${hotspotY} / ${baseWidth} * Math.min(cursorSize, ${CURSOR_MAX_SIZE})}, ${fallback}\`;`
			);
		},
	};
};

const svgDatasetExt = /\.svg\?dataset$/i;
export const svgDataset = (): VitePlugin => {
	return {
		name: "vite-plugin-svg-dataset",
		enforce: "pre",

		async load(id) {
			const filePath = getSvgPath(id, svgDatasetExt);
			if (!filePath) return;

			const svgString = await readFile(filePath, "utf-8");
			const dataset = getSvgDataset(svgString);

			return `export default ${JSON.stringify(dataset)};`;
		},
	};
};

const parser = new DOMParser();

function getSvgDataset(svgString: string): DOMStringMap;
function getSvgDataset<TKeys extends string>(svgString: string): Record<TKeys, string>;
function getSvgDataset(svgString: string) {
	const svg = parser.parseFromString(svgString, "image/svg+xml");
	const root = svg.documentElement as unknown as SVGSVGElement;
	return {
		...root.dataset,
		baseWidth: root.getAttribute("width")!,
		baseHeight: root.getAttribute("height")!,
	};
}
