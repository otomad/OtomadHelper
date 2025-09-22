import type { I18nArgsFunction } from "locales/types";
import type { Trans } from "utils/i18n";
const t = new PathObject() as Trans;

type SettingsCardFormType = "container" | "button" | "expander" | "switch" | "link";

interface SettingMetaInput {
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
	items?: Record<string, SettingMetaInput>;
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
				belowAdjustmentTracks: { icon: "layer_sparkle_add_below" },
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
} as const satisfies Record<string, Record<string, SettingMetaInput>>;

type Hyphenate<T extends string> = T extends `${infer Char}${infer Subsequent}` ?
	`${Char extends Uppercase<Char> ? Char extends Lowercase<Char> ? Char : `-${Lowercase<Char>}` : Char}${Hyphenate<Subsequent>}` : T;
type TranslateFromPath<TRoot, TPath> =
	TPath extends `${infer Parent}.${infer Child}` ? TranslateFromPath<TRoot[Parent & keyof TRoot], Child> :
	TRoot[TPath & keyof TRoot] extends { _: infer Title } ? Title : TRoot[TPath & keyof TRoot];
type HasTranslation<T> = string extends T ? never : T extends I18nArgsFunction ? never : T;
type DefaultMeta<TPath extends string> = OmitNevers<{
	path: Hyphenate<ReplaceAll<ReplaceAll<Replace<TPath, ".", "#">, "_", "/">, ".", "/">>;
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

const settingsMetasOutput: AnyObject = settingsMetasInput;
function convertItem(item: SettingMetaInput, path: string) {
	const { items: itemsInput, ...meta } = item;
	const items = itemsInput as AnyObject;
	if (!lodash.isEmpty(itemsInput))
		for (const [itemId, item] of Object.entries(itemsInput))
			items[itemId] = convertItem(item, `${path}.${itemId}`);
	meta.path = CSS.escape(path.replace(".", ":").replaceAll("_", "/").replaceAll(".", "/").replaceAll(/[A-Z]/g, letter => "-" + letter.toLowerCase()));
	const dotJoined = path.replaceAll("_", ".");
	meta.title ??= dotJoined;
	meta.details ??= "descriptions." + dotJoined;
	meta.aliases ??= "aliases." + dotJoined;
	return { meta, ...items };
}
for (const [pageId, items] of Object.entries(settingsMetasInput as AnyObject))
	for (const [itemId, item] of Object.entries(items))
		items[itemId] = convertItem(item as SettingMetaInput, `${pageId}.${itemId}`);
export const settingsMetas = settingsMetasOutput as Nesting<ConvertPage<typeof settingsMetasInput>>;
