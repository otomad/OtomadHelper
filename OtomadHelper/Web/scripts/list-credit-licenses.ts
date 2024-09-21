import { exec as execSync } from "child_process";
import { readFile, writeFile } from "fs/promises";
import checker from "license-checker";
import { resolve } from "path";

const exec = (command: string) => new Promise<string>((resolve, reject) => execSync(command, (_err, stdout, stderr) => {
	if (stderr) reject(stderr);
	resolve(stdout);
}));

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

interface NugetModuleInfo {
	PackageId: string;
	PackageVersion: string;
	PackageProjectUrl: string;
	Copyright: string;
	Authors: string;
	License: string;
	LicenseUrl: string;
	LicenseInformationOrigin: number;
}

// NOTE: You have to install this nuget tool globally before run the script:
// dotnet tool install --global nuget-license
const csJson = await exec("nuget-license -i ../../OtomadHelper.sln -t -o Json");
const cs = JSON.parse(csJson) as NugetModuleInfo[];
allPackages.cs = Object.fromEntries(cs.map(pack => [pack.PackageId, {
	licenses: pack.License,
	repository: pack.PackageProjectUrl,
	publisher: pack.Authors,
} satisfies checker.ModuleInfo]));

let md = await readFile(resolve(import.meta.dirname, "CREDITS.gen.template.md"), "utf-8");

for (const [variableName, packages] of Object.entries(allPackages))
	md = md.replace("$" + variableName, Object.entries(packages).map(([packageName, { licenses, repository, publisher }]) => {
		licenses ??= "";
		publisher ??= "";
		if (Array.isArray(licenses)) licenses = licenses.join(" + ");
		packageName = packageName.replace(/@[^@]*$/, "");

		const project = repository ? `[${packageName}](${repository})` : packageName;
		return `${project} | ${publisher} | ${licenses}`;
	}).join("\n"));

writeFile(resolve(import.meta.dirname, "../../..", "CREDITS.gen.md"), md, "utf-8");
