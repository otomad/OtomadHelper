import { readFile, writeFile } from "fs/promises";
import checker from "license-checker";
import { resolve } from "path";
const webDir = resolve(import.meta.dirname, "..");

const packages = await new Promise<checker.ModuleInfos>((resolve, reject) => checker.init({
	start: webDir,
}, (err, ret) => {
	if (err) reject(err);
	else resolve(ret);
}));

const mdTemplate = await readFile(resolve(import.meta.dirname, "CREDITS.node.template.md"), "utf-8");

const md = mdTemplate + Object.entries(packages).map(([packageName, { licenses, repository, publisher }]) => {
	licenses ??= "";
	publisher ??= "";
	if (Array.isArray(licenses)) licenses = licenses.join(" + ");
	packageName = packageName.split("@")[0];

	const project = repository ? `[${packageName}](${repository})` : packageName;
	return `${project} | ${publisher} | ${licenses}`;
}).join("\n");

writeFile(resolve(webDir, "../..", "CREDITS.node.md"), md, "utf-8");
