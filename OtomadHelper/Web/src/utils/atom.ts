import { type ExtractAtomValue as ExtractJotaiAtomValue, getDefaultStore } from "jotai";
import { atomWithStorage as _atomWithStorage } from "jotai/utils";
import type { SyncStorage } from "jotai/vanilla/utils/atomWithStorage";
import { withImmer } from "jotai-immer";

type UnknownMap = Map<unknown, unknown>;
// The serialize and deserialize methods of map.
const serializeMap = (map: UnknownMap) => JSON.stringify(Array.from(map.entries()));
const deserializeMap = (str: string) => new Map(JSON.parse(str));

/** The method to store map in local storage. */
const localStorageWithMap: SyncStorage<UnknownMap> = {
	getItem: (key, _initialValue) => {
		const item = localStorage.getItem(key);
		return item ? deserializeMap(item) : new Map();
	},
	setItem: (key, newValue) => {
		localStorage.setItem(key, serializeMap(newValue));
	},
	removeItem: key => {
		localStorage.removeItem(key);
	},
};

/**
 * The `atomWithStorage` function creates an atom with a value persisted in `localStorage` or `sessionStorage` for React.
 * @param key - A unique string used as the key when syncing state with localStorage, sessionStorage, or AsyncStorage.
 * @param initialValue - The initial value of the atom.
 * @param storage - An object with the following methods:
 * - `getItem(key, initialValue)` (required): Reads an item from storage, or falls back to the `initialValue`;
 * - `setItem(key, value)` (required): Saves an item to storage;
 * - `removeItem(key)` (required): Deletes the item from storage;
 * - `subscribe(key, callback, initialValue)` (optional): A method which subscribes to external storage updates.
 *
 * Note that if `storage` is not provided and the `initialValue` is a `Map` object,
 * a `localStorageWithMap` method will be auto provided.
 * @param options - An object with the following properties:
 * - `getOnInit` (optional, by default `false`): A boolean value indicating whether to get item from storage on initialization.
 * Note that in an SPA with `getOnInit` either not set or `false` you will always get the initial value instead of the stored
 * value on initialization. If the stored value is preferred set `getOnInit` to `true`.
 * @returns A writable atom.
 */
export const atomWithStorage: typeof _atomWithStorage = (key, initialValue, storage, options) => {
	if (initialValue instanceof Map && !storage)
		storage = localStorageWithMap as SyncStorage<Any>;
	return _atomWithStorage(key, initialValue, storage as never, options) as never;
};

/**
 * The `atomWithStorage` function creates an atom with a value persisted in `localStorage` or `sessionStorage` for React.
 * @param key - A unique string used as the key when syncing state with localStorage, sessionStorage, or AsyncStorage.
 * @param initialValue - The initial value of the atom.
 * @param storage - An object with the following methods:
 * - `getItem(key, initialValue)` (required): Reads an item from storage, or falls back to the `initialValue`;
 * - `setItem(key, value)` (required): Saves an item to storage;
 * - `removeItem(key)` (required): Deletes the item from storage;
 * - `subscribe(key, callback, initialValue)` (optional): A method which subscribes to external storage updates.
 *
 * Note that if `storage` is not provided and the `initialValue` is a `Map` object,
 * a `localStorageWithMap` method will be auto provided.
 * @param options - An object with the following properties:
 * - `getOnInit` (optional, by default `false`): A boolean value indicating whether to get item from storage on initialization.
 * Note that in an SPA with `getOnInit` either not set or `false` you will always get the initial value instead of the stored
 * value on initialization. If the stored value is preferred set `getOnInit` to `true`.
 * @returns A writable atom with immer.
 */
export const atomWithStorageAndImmer = <Value>(key: string, initialValue: Value, storage?: SyncStorage<Value>, options?: { getOnInit?: boolean }) => {
	return withImmer(atomWithStorage(key, initialValue, storage, options));
};

export const jotaiStore = getDefaultStore();

export type ExtractAtomValue<AtomType> = ExtractJotaiAtomValue<AtomType>;
