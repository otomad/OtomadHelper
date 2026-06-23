import fs from "fs";
import path from "path";
import { defineRoutes, type RouteModule } from "vitepress";

const rootDir = path.resolve(import.meta.dirname, "..");

// Reference: https://vitepress.dev/guide/routing#dynamic-routes
export default function reusePages(dirname: string): RouteModule {
	const rootLangDirname = getRootLangPath(dirname);
	return defineRoutes({
		watch: [`${rootLangDirname}/*.md`, "./*.md"],
		paths: watchedFiles =>
			[
				...new Map(
					watchedFiles
						.filter(filePath => !filePath.includes("[")) // 本函数假定当前项目所在文件夹路径中一定不含带有方括号的文件夹，否则会产生异常。
						.map(filePath => [path.parse(filePath).name, filePath]),
				),
			].map(([pageName, filePath]) => ({
				params: { page: pageName },
				content: fs.readFileSync(path.resolve(rootLangDirname, filePath), "utf-8"),
			})),
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
