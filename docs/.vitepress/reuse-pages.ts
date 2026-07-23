import fs from "fs";
import path from "path";
import { defineRoutes, type RouteModule } from "vitepress";

const rootDir = path.resolve(import.meta.dirname, "..");

// Reference: https://vitepress.dev/guide/routing#dynamic-routes
export default function reusePages(dirname: string): RouteModule {
	const rootLangDirname = getRootLangPath(dirname);
	return defineRoutes({
		watch: [`${rootLangDirname}/*.md`],
		paths: watchedFiles =>
			watchedFiles
				.map(filePath => [path.parse(filePath).name, filePath])
				.map(([pageName, filePath]) => ({
					params: { page: pageName },
					content: fs.readFileSync(path.resolve(rootLangDirname, filePath), "utf-8"),
				})),
		// See: https://github.com/angelespejo/vitepress-plugin-llmstxt/issues/8#issuecomment-4973106751
		options: { globOptions: { ignore: ["**/index.md", "**/v4/revise.md"] } },
	});
}

function getRootLangPath(dirname: string) {
	const relativeLangPath = path.relative(rootDir, dirname).replaceAll("\\", "/");
	const slugs = relativeLangPath.split("/");
	slugs.shift();
	const relativeSourcePath = slugs.join("/");
	const absoluteSourcePath = path.resolve(rootDir, relativeSourcePath);
	return path.relative(dirname, absoluteSourcePath);
}
