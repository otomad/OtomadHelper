/* eslint-disable @typescript-eslint/no-wrapper-object-types */
import type { I18nArgsFunction } from "locales/types";
import { redirectIcon } from "src/ShellPage";
import type { Trans } from "utils/i18n";
import { tf as $$t } from "utils/i18n";
import { settingsMetasInput } from "./settings-metas_input";
const t = new PathObject() as Trans;

type SettingsCardFormType = "container" | "button" | "expander" | "switch" | "link" | "radiogroup";

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
}

export class SettingMeta implements ISettingMeta {
	title?: string;
	details?: string;
	icon?: DeclaredIcons;
	type?: SettingsCardFormType;
	items?: Record<string, ISettingMeta>;
	aliases?: string[];
	link?: string;

	constructor(meta: ISettingMeta, path: string) {
		Object.assign(this, meta);
		this.path = path;
	}

	/** Path of unique identifiers to it. Auto generated. */
	path: string;
	/** Path of unique identifiers to it. Auto generated. CSS escaped. */
	get cssPath() { return CSS_escape(this.path); }

	get translatedTitle() { return $t(this.title); }
	get translatedDetails() { return $t(this.details); }
	get translatedAliases() {
		const _aliases = this.aliases?.map(alias => $t(alias)).toCompacted();
		return _aliases?.join(", ").split(/,\s*/).map(alias => alias.trim()).filter(alias => alias !== this.translatedTitle).toCompacted() ?? [];
	}

	get translatedPath() {
		const path = this.path.replace(/(^|[:/])[^:/]*?$/, "");
		const [_page = "", _anchor = ""] = path.split(":");
		const pages = splitUnlessEmpty(_page, "/").map(subpage => $$t.titles[subpage]?.toString()).toCompacted();
		let metaRoot = splitUnlessEmpty(_page, "/").reduce<AnyObject>((root, subpage) => root[subpage], settingsMetas);
		const anchors = splitUnlessEmpty(_anchor, "/").map(anchor => { metaRoot = metaRoot?.[anchor]; return $t(metaRoot?.meta?.title); }).toCompacted();
		return { pages, anchors };
	}
}

function splitUnlessEmpty(source: string, sep: string) {
	return source ? source.split(sep) : [];
}

const redirectPath = {
	audio: "stream",
	visual: "stream",
	"visual.staff": "staff",
	"visual.prve": "prve",
	"visual.pixelScaling": "pixelScaling",
} as const;

type TranslateFromPath<TRoot, TPath> =
	TPath extends `${infer Parent}.${infer Child}` ? TranslateFromPath<TRoot[Parent & keyof TRoot], Child> :
	TRoot[TPath & keyof TRoot] extends { _: infer Title } ? Title : TRoot[TPath & keyof TRoot];
type HasTranslation<T> = string extends T ? never : T extends I18nArgsFunction ? never : T extends string ? T : never;
type DefaultMeta<TPath extends string> = OmitNevers<{
	path: /* Hyphenate< */ReplaceAll<ReplaceAll<Replace<TPath, ".", "#">, "_", "/">, ".", "/">/* > */;
	title: HasTranslation<TranslateFromPath<Trans, ReplaceAll<TPath, "_", ".">>>;
	details: HasTranslation<TranslateFromPath<Trans["descriptions"], ReplaceAll<TPath, "_", ".">>>;
}>;
type JoinDot<T, U> = T extends "" ? U & string : `${T & string}.${U & string}`;
type ConvertItem<TPage, TPath extends string> = {
	[item in keyof TPage]:
		{ meta: Override<DefaultMeta<JoinDot<TPath, item>>, Omit<TPage[item], "items">> & Omit<SettingMeta, keyof ISettingMeta> } &
		(TPage[item] extends { items: Any } ? ConvertItem<TPage[item]["items"], JoinDot<TPath, item>> : {})
};
type ConvertPage<TObject> = {
	[page in keyof TObject]: ConvertItem<TObject[page], page & string>;
};
type Nesting<TObject> = {
	[key in keyof TObject as key extends `${string}_${string}` ? never : key]:
		key extends `${string}_${string}` ? never : key extends string ? TObject[key] & Nesting<{
			[key2 in keyof TObject as key2 extends `${key}_${infer Child}` ? Child : never]:
				key2 extends `${key}_${string}` ? TObject[key2] : never;
		}> : never;
};

const metas: SettingMeta[] = [];
const settingsMetasOutput: AnyObject = settingsMetasInput;
function convertItem(item: ISettingMeta, path: string) {
	const { items: itemsInput, ...meta } = item;
	const items = itemsInput as AnyObject;
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
	if (!("title" in meta)) meta.title = dotJoined;
	if (!("details" in meta)) meta.details = "descriptions." + dotJoined;
	meta.aliases ??= [];
	meta.aliases.pushUniquely("aliases." + dotJoined);
	if (meta.title as Object instanceof PathObject) meta.title = meta.title?.toString();
	if (meta.details as Object instanceof PathObject) meta.details = meta.details?.toString();
	for (let i = 0; i < meta.aliases.length; i++)
		if (meta.aliases[i] as Object instanceof PathObject) meta.aliases[i] = meta.aliases[i]?.toString();
	const _meta = new SettingMeta(meta, _path);
	metas.push(_meta);
	return { meta: _meta, ...items };
}
for (let page of Object.keys(settingsMetasInput)) {
	page = page.replaceAll("_", "/"); const subpage = page.split("/").at(-1)!;
	const contexts = ["long", "full", "other", undefined];
	const context = contexts.firstDefined(ctx => i18nExists(t => t.titles[subpage], ctx) && ctx && "_" + ctx || undefined) ?? "";
	const meta = new SettingMeta({
		icon: redirectIcon(subpage),
		title: t.titles[subpage + context].toString(),
		aliases: [...contexts.map(ctx => t.titles[subpage + ctx].toString()), t.aliases.titles[subpage].toString()],
	}, page);
	metas.push(meta);
}
for (const [pageId, items] of Object.entries(settingsMetasInput as AnyObject))
	for (const [itemId, item] of Object.entries(items))
		items[itemId] = convertItem(item as ISettingMeta, `${pageId}.${itemId}`);
export const settingsMetas = settingsMetasOutput as Nesting<ConvertPage<typeof settingsMetasInput>>;

function $t(key?: string) {
	if (!key) return;
	const keys = key.split(".");
	if (!i18nExists(key, undefined, false)) return;
	return keys.reduce<AnyObject>((root, key) => root[key], $$t).toString();
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
function updateSettingsMetasSearchMap() {
	settingMetaSearchResults = [];
	const add = (keyword: string, prop: SettingMetaSearchResultProperty, meta: SettingMeta, index?: number) => {
		const normalized = keyword.toLowerCase().replaceAll(/[\r\n\u2008\p{VS}]/gu, "");
		settingMetaSearchResults.push({ normalized, prop, meta, keyword, index });
	};
	for (const meta of metas) {
		let { title, details, aliases: _aliases } = meta;
		if ((title = $t(title))) add(title, "title", meta);
		if ((details = $t(details))) add(details, "details", meta);
		if ((_aliases = _aliases?.map(alias => $t(alias)).toCompacted())) {
			const aliases = _aliases?.join(", ").split(/[,，、]\s*/).map(alias => alias.trim()).filter(alias => alias !== title).toCompacted() ?? [];
			for (const [i, alias] of aliases.entries())
				add(alias, "alias", meta, i);
		}
	}
}
updateSettingsMetasSearchMap();
i18n.on("languageChanged", updateSettingsMetasSearchMap);

export function search(query?: string) {
	if (!query?.trim()) return [];
	query = query.trim().replaceAll(/\s{2,}/g, " ").toLowerCase();
	const locale = i18n.language;
	const matchWordBoundary = (keyword: string) => !!keyword.match(new RegExp("\\b" + RegExp.escape(query)));
	const getSortScore = (keyword: string) => keyword.replace(query, "").replaceAll(query, "1").realLength;

	return settingMetaSearchResults
		.filter(({ normalized }) => normalized.includes(query))
		.sort(({ normalized: a, ...resultA }, { normalized: b, ...resultB }) => {
			let score: number; const indexOfA = a.indexOf(query), indexOfB = b.indexOf(query);
			// Firstly, check if there are matches at the beginning boundary of the word, because generally no one will enter it from the inside of the word.
			if ((score = -(+matchWordBoundary(a) - +matchWordBoundary(b)))) return score;
			// Secondly, check if any keywords start with the query word.
			if (!indexOfA !== !indexOfB) return !indexOfA ? -1 : 1;
			// Thirdly, sort by title → alias → details.
			if ((score = settingMetaSearchResultProperties.indexOf(resultA.prop) - settingMetaSearchResultProperties.indexOf(resultB.prop))) return score;
			// Fourthly, if they are same meta, sort with their prop collection declaration order.
			if (resultA.meta === resultB.meta && resultA.prop === resultB.prop && resultA.index !== undefined && resultB.index !== undefined) return resultA.index - resultB.index;
			// Fifthly, check if there are any most matched shorter string.
			if ((score = getSortScore(a) - getSortScore(b))) return score;
			// Sixthly, check the query word that are closer to the beginning.
			if ((score = indexOfA - indexOfB)) return score;
			// Seventhly, compare by alphabet.
			return a.localeCompare(b, locale);
		})
		.toUnique(({ meta }) => meta);
}
