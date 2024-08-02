import { readFile } from "fs/promises";
import { JSDOM } from "jsdom";
import type { Plugin } from "vite";

const svgCursorExt = /\.svg\?cursor$/i;
const getSvgCursorPath = (id: string) => svgCursorExt.test(id) ? id.replace(/\?.*/, "") : false;

export default (): Plugin => {
	return {
		name: "vite-plugin-svg-cursor",
		enforce: "pre",

		async load(id) {
			const filePath = getSvgCursorPath(id);
			if (!filePath) return;

			const svgString = await readFile(filePath, "utf-8");
			const svg = new JSDOM(svgString, { contentType: "text/xml" });
			const cursorAttrs = svg.window.document.documentElement.dataset as Record<"hotspotX" | "hotspotY" | "fallback", string>;
			const { hotspotX = "0", hotspotY = "0", fallback = "default" } = cursorAttrs;

			return `import svg from "${filePath}";\nexport default \`url(\${svg}) ${hotspotX} ${hotspotY}, ${fallback}\`;`;
		},
	};
};
