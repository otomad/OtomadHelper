import { lstat, readdir, writeFile } from "fs/promises";
import { throttle } from "lodash-es";
import { parse, resolve } from "path";
import type { Plugin } from "vite";

export default (): Plugin => {
	const root = process.cwd().replaceAll("\\", "/");

	const registered = {
		icons: [] as string[],
		lotties: [] as string[],
	};

	const initIcons = async (path?: string[]) => {
		if (path) {
			if (path[2] === "icons")
				if (registered.icons.includes(path.slice(path.indexOf("icons") + 1).join("/"))) return;
			if (path[2] === "lotties")
				if (registered.lotties.includes(path.slice(path.indexOf("lotties") + 1).join("/"))) return;
		}
		const icons: string[] = [], lotties: string[] = [];
		registered.icons = []; registered.lotties = [];
		const fileDisplay = async (filePath: string, carrier: string[], reg: string[], parents: string[] = []) => {
			const files = await readdir(resolve(filePath));
			for (const filename of files) {
				const filedir = resolve(filePath, filename);
				const stats = await lstat(filedir);
				const basename = parse(filename).name;
				if (stats.isFile()) {
					carrier.push([...parents, basename].join("/"));
					reg.push([...parents, filename].join("/"));
				} else if (stats.isDirectory())
					await fileDisplay(filedir, carrier, reg, [...parents, filename]);
			}
		};
		await Promise.all([
			fileDisplay(resolve(root, "src/assets/icons"), icons, registered.icons),
			fileDisplay(resolve(root, "src/assets/lotties"), lotties, registered.lotties),
		]);
		const D_TS_NAME = "auto-icons.d.ts";
		await writeFile(resolve(root, "src/types/", D_TS_NAME), (() => {
			let result = 'declare global {\n\ttype DeclaredIcons = "';
			result += icons.join('" | "');
			result += '" | (string & {});\n\ttype DeclaredLotties = "';
			result += lotties.join('" | "');
			result += '" | (string & {});\n}\n\nexport { };\n';
			return result;
		})());
	};

	const throttledFn = throttle((path: string[]) => {
		if (path[1] === "assets" && ["icons", "lotties"].includes(path[2]))
			initIcons(path);
	}, 5000);

	return {
		name: "vite-plugin-globalized",
		apply: "serve",

		buildStart() {
			initIcons();
		},

		handleHotUpdate({ file }) {
			file = file.replace(root, "");
			const path = file.split("/").filter(folder => folder);
			if (path[0] !== "src" || !path[1]) return;
			throttledFn(path);
		},
	};
};
