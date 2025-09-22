import { t as $t } from "utils/i18n";
const { t } = new PathObject<typeof $t>();

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
	items?: Record<string, Omit<SettingMetaInput, "items">>;
	/** Aliases for this setting. It will auto inherit from `t.aliases` namespace. */
	aliases?: string[];
	/** Click to jump at another link. */
	link?: string;
}

const settingsMetasInput = {
	source: {
		trim: {
			icon: "aspect_ratio",
		},
		startTime: {
			icon: "start_point",
		},
		afterCompletion: {
			icon: "post_processing",
			items: {
				removeSourceClips: {
					icon: "delete_track_event",
				},
				removeSourceClipsWithTracks: {
					icon: "delete_layer",
				},
				selectSourceClips: {
					icon: "select_all",
				},
				selectGeneratedClips: {
					icon: undefined!,
				},
			},
		},
	},
} as const satisfies Record<string, Record<string, SettingMetaInput>>;

type TranslateFromPath<TRoot, TPath> =
	TPath extends `${infer Parent}.${infer Child}` ? TranslateFromPath<TRoot[Parent & keyof TRoot], Child> :
	TRoot[TPath & keyof TRoot] extends { _: infer Title } ? Title : TRoot[TPath & keyof TRoot];
type DefaultMeta<TPath extends string> = {
	path: ReplaceAll<ReplaceAll<Replace<TPath, ".", "#">, "_", "/">, ".", "/">;
	title: TranslateFromPath<typeof $t, ReplaceAll<TPath, "_", ".">>;
	details: TranslateFromPath<typeof $t.descriptions, ReplaceAll<TPath, "_", ".">>;
};
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

type B = Nesting<ConvertPage<typeof settingsMetasInput>>;
const b: B = undefined!;
b.source.afterCompletion.removeSourceClips.meta;
