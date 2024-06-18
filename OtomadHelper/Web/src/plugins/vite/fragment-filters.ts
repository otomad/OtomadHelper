/// <reference path="./fragment-filters.d.ts" />

import { lstat, readdir, readFile } from "fs/promises";
import { parse, resolve } from "path";
import type { Plugin } from "vite";
import loadShader from "../../../node_modules/vite-plugin-glsl/src/loadShader";

export type FragmentDefaults = Record<string, Record<string, { type: string; value: number | number[] }>>;

export default (): Plugin => {
	const pluginName = "fragment-filters";
	const virtualModuleId = "virtual:" + pluginName;
	const resolvedVirtualModuleId = "\0" + virtualModuleId;

	const root = process.cwd().replaceAll("\\", "/");
	// @ts-ignore
	let config: Parameters<Plugin["configResolved"]>[0];

	const excludes = ["main", "bgr", "chromatic-aberration"];

	const getFragments = async (path: string) => {
		const fragments = new Map<string, string>();
		const files = await readdir(resolve(root, path));
		for (const filename of files) {
			const filedir = resolve(path, filename);
			const stats = await lstat(filedir);
			const { name: basename, ext } = parse(filename);
			if (stats.isFile() && ext === ".frag" && !excludes.includes(basename)) {
				const file = await readFile(filedir, "utf-8");
				fragments.set(basename, file);
			}
		}
		return fragments;
	};

	const processFragments = (fragments: Map<string, string>) => {
		const defaults: FragmentDefaults = {};
		for (let [fragName, frag] of fragments) {
			const uniformNames: string[] = [];
			frag = frag.replaceAll(/(?<=(^|\n)\s*)uniform\s+(?<type>\w+)\s+(?<uniformName>\w+)(?<defaultPart>.*);/g, (match, ...args) => {
				const groups: Record<"type" | "uniformName" | "defaultPart", string> = args.at(-1);
				let { type, uniformName, defaultPart } = groups;
				uniformNames.push(uniformName);
				while (defaultPart) {
					defaultPart = defaultPart.replace(/\s*=\s*/, "").trim();
					if (!defaultPart) break;
					let value: number | number[];
					if (type.startsWith("vec")) {
						const vectorLength = parseInt(type.replace("vec", ""), 10);
						const csv = defaultPart.match(/\((.*)\)/)?.[1].trim();
						if (!csv) break;
						value = csv.split(",").map(str => parseFloat(str));
						if (value.length === 1)
							for (let i = 1; i < vectorLength; i++)
								value.push(value[0]);
					} else {
						value = parseFloat(defaultPart);
						if (!Number.isFinite(value)) break;
					}
					if (!(fragName in defaults))
						defaults[fragName] = {};
					const def = defaults[fragName];
					def[uniformName] = { type, value };
					break;
				}
				return `uniform ${groups.type} ${groups.uniformName};`;
			});
			for (const uniformName of uniformNames)
				frag = frag.replaceAll(new RegExp(`(?<![\\w\\.])${uniformName}(?![\\w])`, "g"), `${fragName}_${uniformName}`);
			frag = frag.replaceAll("vec4 frag()", `vec4 ${fragName}_frag()`);
			fragments.set(fragName, frag);
		}
		return [fragments, defaults] as const;
	};

	return {
		name: "vite-plugin-" + pluginName,

		configResolved(resolvedConfig) {
			config = resolvedConfig;
		},

		resolveId(id) {
			if (id === virtualModuleId)
				return resolvedVirtualModuleId;
		},

		async load(id) {
			if (id === resolvedVirtualModuleId) {
				const mainPath = "src/assets/glsl/main.frag";
				let main = await readFile(resolve(root, mainPath), "utf-8");
				const _fragments = await getFragments("src/assets/glsl");
				const [fragments, defaults] = processFragments(_fragments);
				const fragNames = [...fragments.keys()];

				main = main.replace("$fragments", [...fragments.values()].join("\n\n"));
				let switchCase = "switch (index) {\n";
				for (const [index, fragName] of fragNames.entries())
					switchCase += `\t\tcase ${index}: return ${fragName}_frag();\n`;
				switchCase += "\t}";
				main = main.replace("$switch", switchCase);

				const shader = loadShader(main, mainPath, { compress: config.command === "build" } as never);
				main = shader.outputShader;

				let result = `export default ${JSON.stringify(main)};\n\n`;
				result += `export const fragNames = ${JSON.stringify(fragNames)};\n\n`;
				result += `export const defaults = ${JSON.stringify(defaults)};\n`;
				return result;
			}
		},
	};
};
