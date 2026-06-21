import type { Plugin } from "vitepress";
import type { SiteConfig } from "vitepress";
import path from "node:path";
import fs from "node:fs";

const targetLangs = ["zh-CN"];

export default (): Plugin => {
	const virtualIds = new Set<string>();
	let siteConfig!: SiteConfig;

	return {
		name: "vitepress:i18n-router",

		config(config) {
			siteConfig = (config as any).vitepress;
			const targetLangs = Object.keys(siteConfig.userConfig.locales!).filter(lang => lang !== "root");
			for (const lang of targetLangs)
				siteConfig.pages
					.filter(file => !file.startsWith(lang + "/"))
					.forEach(file => {
						siteConfig.pages.push(lang + "/" + file);
						virtualIds.add("/" + lang + "/" + file)
					});

			const input: Record<string, string> = Object.fromEntries(
				siteConfig.pages.map(file => [
					(siteConfig.rewrites.map[file] ?? file).replace(/[\\\/]/g, "_"),
					(() => {
						const targetPath = path.resolve(siteConfig.srcDir, file);
						return fs.existsSync(targetPath) ? targetPath : path.resolve(siteConfig.srcDir, file.replace(/^[\w-]+[\\\/]/, ""));
					})(),
				]),
			);

			return {
				build: { rollupOptions: { input } },
			};
		},

		resolveId(id) {
			// VitePress 在构建页面时，会 import 这个路径
			if (virtualIds.has(id)) {
				return id;
			}
		},

		load(id) {
			if (virtualIds.has(id)) {
				return fs.readFileSync(path.resolve(siteConfig.srcDir, id.replace(/^[\\\/][\w-]+[\\\/]/, "")), "utf-8");
			}
		},
	};
};
