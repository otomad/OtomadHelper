import { readFile, writeFile } from "fs/promises";
import { dirname, resolve } from "path";
import { fileURLToPath } from "url";
const __dirname = dirname(fileURLToPath(import.meta.url));

import { create } from "xmlbuilder2";

import en from "../src/locales/English";
import ja from "../src/locales/Japanese";
import zhCN from "../src/locales/SChinese";
import vi from "../src/locales/Vietnamese";

const templateString = await readFile(resolve(__dirname, "Resources.resx.template.xml"), "utf-8");
const template = create(templateString);

const languages = Object.entries({ en, zhCN, ja, vi }).map(([code, lang]) => ({ code, culture: lang.javascript.metadata.culture, dictionary: lang.csharp }));
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
	writeFile(resolve(__dirname, "../../Properties", `Resources.${language.culture}.resx`), result, "utf-8");
}
