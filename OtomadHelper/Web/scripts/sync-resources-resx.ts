import { readFile, writeFile } from "fs/promises";
import { dirname, resolve } from "path";
import { fileURLToPath } from "url";
const __dirname = dirname(fileURLToPath(import.meta.url));

import { create } from "xmlbuilder2";
import VariableName from "../src/classes/VariableName";

import allLanguages from "../src/locales/all";

const templateString = await readFile(resolve(__dirname, "Resources.resx.template.xml"), "utf-8");
const template = create(templateString);

function flattenObject(object: AnyObject, context: string[] = []) {
	const convertKey = (context: string[]) => context.map(key => new VariableName(key).pascal).join(".");
	const result = {} as Record<string, string>;
	for (const [key, value] of Object.entries(object)) {
		const newContext = [...context, key];
		if (value && typeof value === "object")
			Object.assign(result, flattenObject(value, newContext));
		else
			result[convertKey(newContext)] = value;
	}
	return result;
}

const languages = Object.entries(allLanguages).map(([code, lang]) => ({ code, culture: lang.javascript.metadata.culture, dictionary: flattenObject(lang.csharp) }));
for (const language of languages) {
	const xml = create(template.options).import(template);
	const root = xml.root();
	for (const [name, value] of Object.entries(language.dictionary))
		root.ele("data", { name, "xml:space": "preserve" })
			.ele("value").txt(value).up();
	const result = xml.end({
		prettyPrint: true,
		indent: "\t",
		spaceBeforeSlash: true,
	});
	writeFile(resolve(__dirname, "../../Strings", `${language.culture}.resw`), result, "utf-8");
}
