import { readFile, writeFile } from "fs/promises";
import checker from "license-checker";
import { resolve } from "path";

const webDirs = {
	web: "..",
	api: "../../API",
};

const allPackages: Record<string, checker.ModuleInfos> = {};

for (const [variableName, webRelativeDir] of Object.entries(webDirs)) {
	const webDir = resolve(import.meta.dirname, webRelativeDir);

	const packages = await new Promise<checker.ModuleInfos>((resolve, reject) => checker.init({
		start: webDir,
	}, (err, ret) => {
		if (err) reject(err);
		else resolve(ret);
	}));

	allPackages[variableName] = packages;
}

let md = await readFile(resolve(import.meta.dirname, "CREDITS.node.template.md"), "utf-8");

for (const [variableName, packages] of Object.entries(allPackages))
	md = md.replace("$" + variableName, Object.entries(packages).map(([packageName, { licenses, repository, publisher }]) => {
		licenses ??= "";
		publisher ??= "";
		if (Array.isArray(licenses)) licenses = licenses.join(" + ");
		packageName = packageName.replace(/@[^@]*$/, "");

		const project = repository ? `[${packageName}](${repository})` : packageName;
		return `${project} | ${publisher} | ${licenses}`;
	}).join("\n"));

writeFile(resolve(import.meta.dirname, "../../..", "CREDITS.node.md"), md, "utf-8");
