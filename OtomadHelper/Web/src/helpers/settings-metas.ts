/* eslint-disable @typescript-eslint/no-wrapper-object-types */
import type { I18nArgsFunction } from "locales/types";
import type { Trans } from "utils/i18n";
import { t as $$t } from "utils/i18n";
const t = new PathObject() as Trans;

type SettingsCardFormType = "container" | "button" | "expander" | "switch" | "link" | "radiogroup";

export interface SettingMeta {
	/** A unique identifier under the parent (global uniqueness is not required). */
	// id: string;
	/** Title must be referenced from an i18n locale string. If ignoring, it will auto concat from ancestor keys. */
	title?: string;
	/** Details must be referenced from an i18n locale string. If ignoring, it will auto get from `t.descriptions` namespace. */
	details?: string;
	/** Icon. */
	icon: DeclaredIcons;
	/** Settings card form type. @see {@link SettingsCard} */
	type?: SettingsCardFormType;
	/** Child settings if the type is an expander. */
	items?: Record<string, SettingMeta>;
	/** Aliases for this setting. It will auto inherit from `t.aliases` namespace. */
	aliases?: string;
	/** Click to jump at another link. */
	link?: string;
	/** Path of unique identifiers to it. Auto generated. CSS escaped. */
	path?: string;
}

const settingsMetasInput = {
	source: {
		trim: { icon: "aspect_ratio" },
		startTime: { icon: "start_point" },
		afterCompletion: {
			icon: "post_processing",
			items: {
				removeSourceClips: { icon: "delete_track_event" },
				removeSourceClipsWithTracks: { icon: "delete_layer" },
				selectSourceClips: { icon: "select_all" },
				selectGeneratedClips: { icon: undefined! },
			},
		},
		preferredTrack: {
			icon: "preferred_track",
			items: {
				index: {
					icon: "layer_number",
					details: t.descriptions.source.preferredTrack.fillingInstructions,
				},
				belowAdjustmentTracks: {
					icon: "layer_sparkle_add_below",
					details: undefined,
				},
			},
		},
		trackGroup: {
			icon: "group",
			items: {
				collapse: { icon: "chevron_down_up" },
			},
		},
		trackName: { icon: "rename" },
		secretBox: {
			icon: "dice",
			items: {
				limitToSelected: { icon: "video_clip_multiple_checkmark" },
				track: { icon: "layer" },
				marker: { icon: "flag" },
				barOrBeat: {
					icon: "music_bar",
					items: {
						period: { icon: "timer" },
						preparation: { icon: "hourglass" },
					},
				},
			},
		},
		consonant: { icon: "consonant" },
		matchCut: {
			icon: "flag_auto_beat",
			items: {
				order: {
					icon: "arrow_sort_horizontal",
					title: t.order,
				},
				loop: {
					icon: "arrow_repeat_all",
					title: t.stream.loop,
				},
				secretBox: {
					icon: "dice",
					title: t.source.secretBox,
				},
			},
		},
		linearMap: {
			icon: "launchpad",
			items: {
				descending: {
					icon: "descending",
					title: t.descending,
				},
			},
		},
	},
} as const satisfies Record<string, Record<string, SettingMeta>>;

type TranslateFromPath<TRoot, TPath> =
	TPath extends `${infer Parent}.${infer Child}` ? TranslateFromPath<TRoot[Parent & keyof TRoot], Child> :
	TRoot[TPath & keyof TRoot] extends { _: infer Title } ? Title : TRoot[TPath & keyof TRoot];
type HasTranslation<T> = string extends T ? never : T extends I18nArgsFunction ? never : T;
type DefaultMeta<TPath extends string> = OmitNevers<{
	path: /* Hyphenate< */ReplaceAll<ReplaceAll<Replace<TPath, ".", "#">, "_", "/">, ".", "/">/* > */;
	title: HasTranslation<TranslateFromPath<Trans, ReplaceAll<TPath, "_", ".">>>;
	details: HasTranslation<TranslateFromPath<Trans["descriptions"], ReplaceAll<TPath, "_", ".">>>;
}>;
type JoinDot<T, U> = T extends "" ? U & string : `${T & string}.${U & string}`;
type ConvertItem<TPage, TPath extends string> = {
	[item in keyof TPage]:
		{ meta: Override<DefaultMeta<JoinDot<TPath, item>>, Omit<TPage[item], "items">> } & (TPage[item] extends { items: Any } ? ConvertItem<TPage[item]["items"], JoinDot<TPath, item>> : {})
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
function convertItem(item: SettingMeta, path: string) {
	const { items: itemsInput, ...meta } = item;
	const items = itemsInput as AnyObject;
	if (!lodash.isEmpty(itemsInput))
		for (const [itemId, item] of Object.entries(itemsInput))
			items[itemId] = convertItem(item, `${path}.${itemId}`);
	meta.path = path.replace(".", ":").replaceAll("_", "/").replaceAll(".", "/");
	const dotJoined = path.replaceAll("_", ".");
	if (!("title" in meta)) meta.title = dotJoined;
	if (!("details" in meta)) meta.details = "descriptions." + dotJoined;
	meta.aliases ??= "aliases." + dotJoined;
	if (meta.title as Object instanceof PathObject) meta.title = meta.title?.toString();
	if (meta.details as Object instanceof PathObject) meta.details = meta.details?.toString();
	if (meta.aliases as Object instanceof PathObject) meta.aliases = meta.aliases?.toString();
	metas.push(meta);
	return { meta, ...items };
}
for (const [pageId, items] of Object.entries(settingsMetasInput as AnyObject))
	for (const [itemId, item] of Object.entries(items))
		items[itemId] = convertItem(item as SettingMeta, `${pageId}.${itemId}`);
export const settingsMetas = settingsMetasOutput as Nesting<ConvertPage<typeof settingsMetasInput>>;

export /* @internal */ function $t(key?: string) {
	if (!key) return;
	const keys = key.split(".");
	if (!i18nExists(key)) return;
	return keys.reduce<AnyObject>((root, key) => root[key], $$t).toString();
}

const settingsMetasSearchMapProperties = ["title", "alias", "details"] as const;
type SettingsMetasSearchMapProperty = typeof settingsMetasSearchMapProperties[number];
let settingsMetasSearchMap: [keyword: string, property: SettingsMetasSearchMapProperty, meta: SettingMeta, originalKeyword: string][] = [];
function updateSettingsMetasSearchMap() {
	settingsMetasSearchMap = [];
	const add = (keyword: string, property: SettingsMetasSearchMapProperty, meta: SettingMeta) => {
		const normalizedKeyword = keyword.toLowerCase().replaceAll(/[\r\n\u2008\p{VS}]/gu, "");
		settingsMetasSearchMap.push([normalizedKeyword, property, meta, keyword]);
	};
	for (const meta of metas) {
		let { title, details, aliases: _aliases } = meta;
		if ((title = $t(title))) add(title, "title", meta);
		if ((details = $t(details))) add(details, "details", meta);
		if ((_aliases = $t(_aliases))) {
			const aliases = _aliases?.split(/,\s*/).map(alias => alias.trim()).toCompacted() ?? [];
			for (const alias of aliases)
				add(alias, "alias", meta);
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

	return settingsMetasSearchMap
		.filter(([keyword]) => keyword.includes(query))
		.sort(([a], [b]) => {
			let score: number; const indexOfA = a.indexOf(query), indexOfB = b.indexOf(query);
			// Zerothly, sort by title → alias → details.
			// if ((score = settingsMetasSearchMapProperties.indexOf(propOfA) - settingsMetasSearchMapProperties.indexOf(propOfB))) return score;
			// Firstly, check if there are matches at the beginning boundary of the word, because generally no one will enter it from the inside of the word.
			if ((score = -(+matchWordBoundary(a) - +matchWordBoundary(b)))) return score;
			// Secondly, check if any keywords start with the query word.
			if (!indexOfA !== !indexOfB) return !indexOfA ? -1 : 1;
			// Thirdly, check if there are any most matched shorter string.
			if ((score = getSortScore(a) - getSortScore(b))) return score;
			// Fourthly, check the query word that are closer to the beginning.
			if ((score = indexOfA - indexOfB)) return score;
			// Fifthly, compare by alphabet.
			return a.localeCompare(b, locale);
		})
		.toUnique(([, , meta]) => meta);
}
