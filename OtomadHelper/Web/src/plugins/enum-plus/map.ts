import type { EnumInit, EnumItemClass, EnumKey, EnumValue, PluginFunc, ValueTypeFromSingleInit } from "enum-plus";

const mapPlugin: PluginFunc = (_options, Enum) => {
	Enum.extends({
		map(callbackFn: (raw: object, index: number) => unknown) {
			return this.items.map(({ raw, ...others }, index) => callbackFn({ ...raw as object, ...others }, index));
		},
		get allKeyed() {
			// @ts-expect-error
			return Object.fromEntries(this.items.map(({ raw, key, ...others }) => [key, { ...raw as object, key, ...others }]));
		},
		get array() {
			// @ts-expect-error
			return this.items.map(({ raw, ...others }) => ({ ...raw as object, ...others }));
		},
		/*
		 * CAUTION: Set a invalid useless setter to avoid conflict with same name of enum key.
		 * ```
		 * enum-collection.ts:86  Uncaught TypeError: Cannot set property `all` of #<EnumExtensionClass> which has only a getter
		 *     at enum-collection.ts:86:20
		 *     at EnumItemsArray.forEach (<anonymous>)
		 *     at new EnumCollectionClass (enum-collection.ts:84:11)
		 *     at Enum (enum.ts:39:12)
		 * ```
		 */
		set allKeyed(_) { },
		set array(_) { },
	});
};

declare module "enum-plus/extension" {
	export interface EnumExtension<
		T extends EnumInit<K, V>,
		K extends EnumKey<T> = EnumKey<T>,
		V extends EnumValue = ValueTypeFromSingleInit<T[K], K>,
	> {
		map<R>(callbackFn: (raw: EnumItemClass<T[K], K, V> & T[K], index: number) => R): R[];
		readonly allKeyed: Record<K, EnumItemClass<T[K], K, V> & T[K]>;
		readonly array: (EnumItemClass<T[K], K, V> & T[K])[];
	}
}

export default mapPlugin;
