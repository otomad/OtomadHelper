import fs from "fs";
import path from "path";
import { defineRoutes, type RouteModule } from "vitepress";

// Reference: https://vitepress.dev/guide/routing#dynamic-routes
export default function reusePages(dirname: string): RouteModule {
	const rootLangDirname = getRootLangPath(dirname);
	return defineRoutes({
		watch: [`${rootLangDirname}/*.md`, "./*.md"],
		paths: watchedFiles => {
			console.log("watchedFiles​", watchedFiles);
			return [
				...new Map(
					watchedFiles
						.filter(filePath => !filePath.includes("[")) // 本函数假定当前项目所在文件夹路径中一定不含带有方括号的文件夹，否则会产生异常。
						.map(filePath => [path.parse(filePath).name, filePath]),
				),
			].map(([pageName, filePath]) => ({
				params: { page: pageName },
				content: fs.readFileSync(path.resolve(rootLangDirname, filePath), "utf-8"),
			}));
		},
	});
}

const ROOT_DIR = "docs";
function getRootLangPath(dirname: string) {
	console.log("dirname​", dirname);
	dirname = dirname.replaceAll("\\", "/");
	const slugs = dirname.split("/");
	const rootIndex = slugs.indexOf(ROOT_DIR); // 本函数假定当前项目所在文件夹路径中一定不含 docs 这一文件夹，否则会产生异常。
	if (rootIndex === -1) throw new Error("此时不应调用 reusePages");
	slugs.splice(rootIndex + 1, 1);
	const absolutePath = slugs.join("/");
	console.log("absolutePath", absolutePath);
	return path.relative(dirname, absolutePath);
}
