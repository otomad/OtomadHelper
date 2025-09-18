import { readFile, writeFile } from "fs/promises";
import { resolve } from "path";
import { IN_CONTEXT_LANGUAGE_CODE } from "helpers/jipt-activator_constants";
import VariableName from "variable-name-conversion";
import { create } from "xmlbuilder2";
import allLanguages from "../src/locales/all";

const template_string = await readFile(resolve(import.meta.dirname, "Resources.resx.template.xml"), "utf-8");
const template = create(template_string);

function flattenObject(object: AnyObject, context: string[] = []) {
	const convertKey = (context: string[]) => context.map(key => new VariableName(key, true).pascal).join(".");
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

const languages = Object.entries(allLanguages).map(([code, lang]) =>
	({ code, culture: lang.javascript.metadata.culture, dictionary: flattenObject({ ...lang.csharp, shared: lang.shared }) }));
for (const language of languages) {
	if (language.code === IN_CONTEXT_LANGUAGE_CODE) continue;
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
	writeFile(resolve(import.meta.dirname, "../../Strings", `${language.culture}.resw`), result, "utf-8");
}
