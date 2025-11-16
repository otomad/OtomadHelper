import type { EnumInit, EnumItemClass, EnumKey, EnumValue, PluginFunc, ValueTypeFromSingleInit } from "enum-plus";

const mapPlugin: PluginFunc = (_options, Enum) => {
	Enum.extends({
		map(callbackFn: (raw: object, index: number) => unknown) {
			return this.items.map(({ raw, ...others }, index) => callbackFn({ ...raw as object, ...others }, index));
		},
		// get all() {
		// 	return Object.fromEntries(this.items.map(({ raw, key, ...others }) => [key, { ...raw as object, ...others }]));
		// },
	});
	Object.defineProperties(Object.getPrototypeOf(Enum()), Object.getOwnPropertyDescriptors({
		get all() {
			return Object.fromEntries(this.items.map(({ raw, key, ...others }) => [key, { ...raw as object, ...others }]));
		},
		get array() {
			return this.items.map(({ raw, ...others }) => ({ ...raw as object, ...others }));
		},
	}));
};

declare module "enum-plus/extension" {
	export interface EnumExtension<
		T extends EnumInit<K, V>,
		K extends EnumKey<T> = EnumKey<T>,
		V extends EnumValue = ValueTypeFromSingleInit<T[K], K>,
	> {
		map<R>(callbackFn: (raw: EnumItemClass<T[K], K, V> & T[K], index: number) => R): R;
		readonly all: Record<K, EnumItemClass<T[K], K, V> & T[K]>;
		readonly array: (EnumItemClass<T[K], K, V> & T[K])[];
	}
}

export default mapPlugin;
