import type SettingsCard from "components/Settings/SettingsCard/SettingsCard";
import type { I18nArgsFunction } from "locales/types";
import type { Trans } from "utils/i18n";
import { tf as $$t } from "utils/i18n";
import { redirectIcon } from "../ShellPage";
import { languageNode, settingsMetaInput } from "./settings-meta_input";

type SettingsCardFormType = "container" | "button" | "expander" | "switch" | "link" | "radiogroup" | "subheader";

export interface ISettingMeta {
	/** A unique identifier under the parent (global uniqueness is not required). */
	// id: string;
	/** Title must be referenced from an i18n locale string. If ignoring, it will auto concat from ancestor keys. */
	title?: string;
	/** Details must be referenced from an i18n locale string. If ignoring, it will auto get from `t.descriptions` namespace. */
	details?: string;
	/** Icon. */
	icon?: DeclaredIcons;
	/** Settings card form type. @see {@link SettingsCard} */
	type?: SettingsCardFormType;
	/** Child settings if the type is an expander. */
	items?: Record<string, ISettingMeta>;
	/** Aliases for this setting. It will auto inherit from `t.aliases` namespace. */
	aliases?: string[];
	/** Click to jump at another link. */
	link?: string;
	/** Exclude it (and its descendants) from search results? */
	unsearchable?: boolean;
}

export class SettingMeta implements ISettingMeta {
	title?: string;
	details?: string;
	icon?: DeclaredIcons;
	type?: SettingsCardFormType;
	items?: Record<string, ISettingMeta>;
	aliases?: string[];
	link?: string;
	unsearchable?: boolean = false;

	constructor(meta: ISettingMeta, path: string) {
		Object.assign(this, meta);
		this.path = path;
	}

	/** Path of unique identifiers to it. Auto generated. */
	path: string;
	/** Path of unique identifiers to it. Auto generated. CSS escaped. */
	get cssPath() { return this.path !== undefined ? CSS.escape(this.path) : undefined!; }

	get translatedTitle() { return $t(this.title); }
	get translatedDetails() { return $t(this.details); }
	get translatedAliases() {
		if (this.aliases?.includes(languageNode)) return [...languageInAllLanguages];
		const _aliases = this.aliases?.map(alias => $t(alias, false)).toCompacted();
		return _aliases?.join(", ").split(/,\s*/).map(alias => alias.trim()).filter(alias => alias !== this.translatedTitle).toCompacted() ?? [];
	}

	get translatedPath() {
		const path = this.path.replace(/(^|[:/])[^:/]*?$/, "");
		const [_page = "", _anchor = ""] = path.split(":");
		const pages = splitUnlessEmpty(_page, "/").map(subpage => $$t.titles[subpage]?.toString()).toCompacted();
		let metaRoot = splitUnlessEmpty(_page, "/").reduce<AnyObject>((root, subpage) => root[subpage], settingsMeta);
		const anchors = splitUnlessEmpty(_anchor, "/").map(anchor => { metaRoot = metaRoot?.[anchor]; return $t(metaRoot?.meta?.title); }).toCompacted();
		return { pages, anchors };
	}
}

function splitUnlessEmpty(source: string, sep: string) {
	return source ? source.split(sep) : [];
}

const redirectPath = {
	"visual.staff": "staff",
	"visual.prve": "prve",
	"visual.pixelScaling": "pixelScaling",
	audio: "stream",
	visual: "stream",
} as const;

type RedirectPath<Root extends AnyObject> = Root & {
	audio: Root["stream"];
	visual: Root["stream"] & {
		staff: Root["staff"];
		prve: Root["prve"];
		pixelScaling: Root["pixelScaling"];
	};
};

export type RedirectedTrans = RedirectPath<Trans> & {
	descriptions: RedirectPath<Trans["descriptions"]>;
	aliases: RedirectPath<Trans["aliases"]>;
};

type TranslateFromPath<TRoot, TPath> =
	TPath extends `${infer Parent}.${infer Child}` ? TranslateFromPath<TRoot[Parent & keyof TRoot], Child> :
	TRoot[TPath & keyof TRoot] extends { _: infer Title } ? Title : TRoot[TPath & keyof TRoot];
type HasTranslation<T> = string extends T ? never : T extends I18nArgsFunction ? never : T extends string ? T : never;
type DefaultMeta<TPath extends string> = OmitNevers<{
	path: /* Hyphenate< */ReplaceAll<ReplaceAll<Replace<TPath, ".", "#">, "_", "/">, ".", "/">/* > */;
	title: HasTranslation<TranslateFromPath<RedirectedTrans, ReplaceAll<TPath, "_", ".">>>;
	details: HasTranslation<TranslateFromPath<RedirectedTrans["descriptions"], ReplaceAll<TPath, "_", ".">>>;
}>;
type JoinDot<T, U> = T extends "" ? U & string : `${T & string}.${U & string}`;
type ConvertItem<TPage, TPath extends string> = {
	[item in keyof TPage]:
		{ meta: Override<DefaultMeta<JoinDot<TPath, item>>, Omit<TPage[item], "items">> & Omit<SettingMeta, keyof ISettingMeta> } &
		(TPage[item] extends { items: Any } ? ConvertItem<TPage[item]["items"], JoinDot<TPath, item>> : {})
};
type ConvertPage<TObject> = {
	[page in keyof TObject]: ConvertItem<Omit<TObject[page], "meta">, page & string> &
		{ meta: Override<DefaultMeta<page & string>, TObject[page] extends { meta: Any } ? TObject[page]["meta"] : {}> & Omit<SettingMeta, keyof ISettingMeta> };
};
type Nesting<TObject> = {
	[key in keyof TObject as key extends `${string}_${string}` ? never : key]:
		key extends `${string}_${string}` ? never : key extends string ? TObject[key] & Nesting<{
			[key2 in keyof TObject as key2 extends `${key}_${infer Child}` ? Child : never]:
				key2 extends `${key}_${string}` ? TObject[key2] : never;
		}> : never;
};

const languageInAllLanguages = Object.freeze(getAllLanguageTags().map(lang => i18n.t("settings.language._", { lng: lang, fallbackLng: false })));
const metas: SettingMeta[] = [];
const settingsMetaOutput: AnyObject = {};
const pathToI18nItem = (path: string) => path.split(".").reduce<Any>((parent, key) => parent?.[key], tf) as string;
function convertItem(item: ISettingMeta, path: string, isPageMeta: boolean = false) {
	if (!item) return undefined!;
	const { items: itemsInput, ...meta } = item;
	const items: AnyObject = {};
	if (!lodash.isEmpty(itemsInput))
		for (const [itemId, item] of Object.entries(itemsInput))
			items[itemId] = convertItem(item, `${path}.${itemId}`);
	const _path = path.replace(".", ":").replaceAll("_", "/").replaceAll(".", "/");
	let dotJoined = path.replaceAll("_", ".");
	for (const [old, new_] of Object.entries(redirectPath))
		if (dotJoined.startsWith(old)) {
			dotJoined = dotJoined.replaceStart(old, new_);
			break;
		}
	meta.aliases = [...meta.aliases ?? []];
	if (!isPageMeta) {
		if (!("title" in meta)) meta.title = pathToI18nItem(dotJoined);
		if (!("details" in meta)) meta.details = pathToI18nItem("descriptions." + dotJoined);
		meta.aliases.pushUniquely(pathToI18nItem("aliases." + dotJoined));
		if (!("icon" in meta) && meta.type === "subheader") meta.icon = "subheader";
	} else {
		const subpage = _path.split("/").at(-1)!;
		const contexts = ["long", "full", "other", undefined];
		const context = contexts.filter(ctx => ctx !== "other").firstDefined(ctx => i18nExists(t.titles[subpage], ctx) && ctx && `_${ctx}` || undefined) ?? "";
		if (!("title" in meta)) meta.title = t.titles[subpage + context];
		if (!("details" in meta)) meta.details = pathToI18nItem("descriptions." + dotJoined + ".caption");
		meta.aliases.pushUniquely(...contexts.map(ctx => tf.titles[`${subpage}${ctx ? `_${ctx}` : ""}`]), tf.aliases.titles[subpage]);
		if (!("icon" in meta)) meta.icon = redirectIcon(subpage);
		meta.type ??= "link";
		meta.link ??= _path;
	}
	const _meta = new SettingMeta(meta, _path);
	metas.push(_meta);
	return { meta: _meta, ...items };
}
for (const [pageId, items] of Object.entries(settingsMetaInput as AnyObject)) {
	settingsMetaOutput[pageId] = {};
	settingsMetaOutput[pageId].meta = convertItem(items.meta ?? {}, pageId, true).meta;
	for (const [itemId, item] of Object.entries(items))
		settingsMetaOutput[pageId][itemId] = convertItem(item as ISettingMeta, `${pageId}.${itemId}`);
}
for (const [pageId, items] of Object.entries(settingsMetaOutput))
	if (pageId.includes("_")) {
		accessPath(settingsMetaOutput, pageId.replaceAll("_", "."), items);
		delete settingsMetaOutput[pageId as never];
	}
export const settingsMeta = settingsMetaOutput as Nesting<ConvertPage<typeof settingsMetaInput>>;

function accessPath(root: AnyObject, path: string, overwrite: unknown) {
	return path.split(".").reduce((parent, layer, i, { length }) => {
		const nullish = !parent[layer];
		if (nullish) parent[layer] = {};
		if (overwrite && i === length - 1) parent[layer] = nullish ? overwrite : Object.assign(parent[layer], overwrite);
		return parent[layer];
	}, root);
}

function $t(key: string | undefined, enableFallbackLang: boolean = true) {
	if (!key) return;
	if (typeof (key as unknown) === "string") throw new TypeError(`Unexpectedly got string key: ${key}`);
	if (!i18nExists(key, undefined, enableFallbackLang)) return;
	return key.toString();
}

const settingMetaSearchResultProperties = ["title", "alias", "details"] as const;
type SettingMetaSearchResultProperty = typeof settingMetaSearchResultProperties[number];
interface SettingMetaSearchResult {
	/** Normalized keyword. */
	normalized: string;
	prop: SettingMetaSearchResultProperty;
	meta: SettingMeta;
	/** Original keyword. */
	keyword: string;
	/** Index in the collection of the property. */
	index?: number;
}
let settingMetaSearchResults: SettingMetaSearchResult[] = [];
function updateSettingsMetaSearchMap() {
	settingMetaSearchResults = [];
	const add = (keyword: string, prop: SettingMetaSearchResultProperty, meta: SettingMeta, index?: number) => {
		const normalized = keyword.toLowerCase().replaceAll(/[\r\n\u2008\p{VS}]/gu, "");
		settingMetaSearchResults.push({ normalized, prop, meta, keyword, index });
	};
	for (const meta of metas) {
		let { title, details, aliases: _aliases, unsearchable } = meta;
		if (unsearchable) continue;
		if ((title = $t(title, false))) add(title, "title", meta);
		if ((details = $t(details, false))) add(details, "details", meta);
		if (_aliases?.includes(languageNode))
			languageInAllLanguages.forEach((lang, i) => add(lang, "alias", meta, i));
		else if ((_aliases = _aliases?.map(alias => $t(alias, false)).toCompacted())) {
			const aliases = _aliases?.join(", ").split(/[,，、]\s*/).map(alias => alias.trim()).filter(alias => alias !== title).toCompacted() ?? [];
			for (const [i, alias] of aliases.entries())
				add(alias, "alias", meta, i);
		}
	}
}
updateSettingsMetaSearchMap();
i18n.on("languageChanged", updateSettingsMetaSearchMap);

export function search(query?: string) {
	if (!query?.trim()) return [];
	query = query.trim().replaceAll(/\s{2,}/g, " ").toLowerCase();
	const locale = i18n.language;
	const matchWordBoundary = (keyword: string) => !!keyword.match(new RegExp("\\b" + RegExp.escape(query)));
	const getSortScore = (keyword: string) => keyword.replace(query, "").replaceAll(query, "1").realLength;
	const compareIfBestMatch = (a: string, b: string) => +(b === query) - +(a === query);

	return settingMetaSearchResults
		.filter(({ normalized }) => normalized.includes(query))
		.sort(({ normalized: a, ...resultA }, { normalized: b, ...resultB }) => {
			let score: number; const indexOfA = a.indexOf(query), indexOfB = b.indexOf(query);
			// Firstly, check to see if any of them best match the query.
			if ((score = compareIfBestMatch(a, b))) return score;
			// Secondly, check if there are matches at the beginning boundary of the word, because generally no one will enter it from the inside of the word.
			if ((score = -(+matchWordBoundary(a) - +matchWordBoundary(b)))) return score;
			// Thirdly, check if any keywords start with the query word.
			if (!indexOfA !== !indexOfB) return !indexOfA ? -1 : 1;
			// Fourthly, sort by title → alias → details.
			if ((score = settingMetaSearchResultProperties.indexOf(resultA.prop) - settingMetaSearchResultProperties.indexOf(resultB.prop))) return score;
			// Fifthly, if they are same meta, sort with their prop collection declaration order.
			if (resultA.meta === resultB.meta && resultA.prop === resultB.prop && resultA.index !== undefined && resultB.index !== undefined) return resultA.index - resultB.index;
			// Sixthly, check if there are any most matched shorter string.
			if ((score = getSortScore(a) - getSortScore(b))) return score;
			// Seventhly, check the query word that are closer to the beginning.
			if ((score = indexOfA - indexOfB)) return score;
			// Eighthly, compare by alphabet.
			if ((score = a.localeCompare(b, locale))) return score;
			// Ninthly, compare their path length, prioritize short circuit.
			return resultA.meta.path.length - resultB.meta.path.length;
		})
		.toUnique(({ meta }) => meta);
}
