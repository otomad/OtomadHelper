/**
 * A utility class representing an Indexed DB store.
 *
 * @template T - The type of the objects stored in the IndexedDB store.
 */
export default class IndexedDBStore<T extends object> {
	/**
	 * The IndexedDB database instance.
	 */
	#database: IDBDatabase | null = null;

	/**
	 * Gets the opened IndexedDB database instance.
	 *
	 * @throws {ReferenceError} If the IndexedDB database hasn't been opened yet.
	 * @returns The opened IndexedDB database instance.
	 */
	private get database() {
		if (!this.#database) throw new ReferenceError("The IndexedDB database hasn't been opened");
		return this.#database;
	}

	/**
	 * Checks if the IndexedDB database is opened.
	 */
	get isDatabaseOpen() {
		return this.#database !== null;
	}

	/**
	 * Gets the readwrite IndexedDB object store associated with the current instance.
	 *
	 * @returns The readwrite IndexedDB object store associated with the current instance.
	 */
	private get store() {
		return this.database
			.transaction(this.objectStoreName, "readwrite")
			.objectStore(this.objectStoreName);
	}

	/**
	 * Gets the readonly IndexedDB object store associated with the current instance.
	 *
	 * @returns The readonly IndexedDB object store associated with the current instance.
	 */
	private get readonlyStore() {
		return this.database
			.transaction(this.objectStoreName, "readonly")
			.objectStore(this.objectStoreName);
	}

	/**
	 * Constructs a new IndexedDBStore instance.
	 *
	 * @param databaseName - The name of the IndexedDB database.
	 * @param databaseVersion - The version of the IndexedDB database.
	 * @param objectStoreName - The name of the object store within the database.
	 * @param objectStoreSchema - The schema of the object store within the database.
	 */
	constructor(
		public readonly databaseName: string,
		public readonly databaseVersion: number,
		public readonly objectStoreName: string,
		private readonly objectStoreSchema: {
			keyPath?: (string & keyof T) | (string & keyof T)[];
		} & {
			[key in keyof T]: IDBIndexParameters | null;
		},
	) { }

	/**
	 * Opens the IndexedDB database.
	 *
	 * @returns A Promise that resolves with the opened database instance or rejects with an error.
	 */
	open() {
		if (this.#database) return Promise.resolve(this.#database); // If it is already opened, resolve with the existing database instance.

		return new Promise<IDBDatabase>((resolve, reject) => {
			const request = indexedDB.open(this.databaseName, this.databaseVersion);

			request.onsuccess = () => resolve(this.#database = request.result);
			request.onerror = () => reject(request.error);

			request.onupgradeneeded = () => {
				if (!this.database.objectStoreNames.contains(this.objectStoreName)) {
					const { keyPath, ...keys } = this.objectStoreSchema;
					const objectStore = this.database.createObjectStore(this.objectStoreName, keyPath !== undefined ? { keyPath } : { autoIncrement: true });
					for (const [key, options] of Object.entries(keys))
						objectStore.createIndex(key, key, options ?? undefined);
				}
			};
		});
	}

	/**
	 * Resolves an IndexedDB request and returns a Promise.
	 *
	 * @template T - The type of the result of the IndexedDB request.
	 * @param request - The IndexedDB request to be resolved.
	 * @returns A Promise that resolves with the result of the IndexedDB request or rejects with an error.
	 *
	 * @remarks
	 * This method is a utility function that wraps the IndexedDB request's event handlers into a Promise.
	 * It simplifies the handling of IndexedDB requests by providing a consistent interface for handling success and error cases.
	 */
	private static getResult<T>(request: IDBRequest<T>): Promise<T> {
		return new Promise<T>((resolve, reject) => {
			request.onsuccess = () => resolve(request.result);
			request.onerror = () => reject(request.error);
		});
	}

	/**
	 * Adds a new item to the IndexedDB object store.
	 *
	 * @param item - The item to be added to the object store.
	 * @returns A Promise that resolves with the added item's key or rejects with an error.
	 */
	add(item: T) {
		return IndexedDBStore.getResult(this.store.add(item));
	}

	/**
	 * Retrieves an item from the IndexedDB object store.
	 *
	 * @param id - The key or key range of the item to retrieve.
	 * @returns A Promise that resolves with the retrieved item or rejects with an error.
	 */
	get(id: IDBValidKey | IDBKeyRange): Promise<T>;
	/**
	 * Retrieves an item from the IndexedDB object store based on a specific key or key range.
	 *
	 * @template TKey - The type of the key to retrieve. It must be a key of the object stored in the IndexedDB store.
	 * @param key - The key or name of the index to use for retrieving the item.
	 * @param value - The value of the index to match. If not provided, the function will retrieve an item using the key.
	 * @returns A Promise that resolves with the retrieved item or rejects with an error.
	 *
	 * @remarks
	 * If the `value` parameter is provided, the function will use the specified index to retrieve the item.
	 * If the `value` parameter is not provided, the function will retrieve the item using the provided key.
	 */
	get<TKey extends keyof T>(key: TKey, value: T[TKey]): Promise<T>;
	get(...args: unknown[]): Promise<T> {
		const request = args.length === 1 ?
			this.readonlyStore.get(args[0] as IDBValidKey) :
			this.readonlyStore.index(args[0] as string).get(args[1] as IDBValidKey);
		return IndexedDBStore.getResult<T>(request);
	}

	/**
	 * Sets a new item or updates an existing item in the IndexedDB object store.
	 *
	 * @param item - The item to be added or updated in the object store.
	 * @param id - (Optional) The key of the item to update. If not provided, a new item will be added.
	 * @returns A Promise that resolves with the added or updated item's key or rejects with an error.
	 */
	set(item: T, id?: IDBValidKey) {
		return IndexedDBStore.getResult(this.store.put(item, id));
	}

	/**
	 * Deletes an item from the IndexedDB object store.
	 *
	 * @param id - The key or key range of the item to delete.
	 * @returns A Promise that resolves when the item is deleted or rejects with an error.
	 */
	delete(id: IDBValidKey | IDBKeyRange): Promise<void> {
		return IndexedDBStore.getResult(this.store.delete(id));
	}

	/**
	 * Asynchronously iterates over all key-value pairs in the IndexedDB object store.
	 *
	 * @yields {Generator<[IDBValidKey, T]>} - Yields the key-value pair for each item in the object store.
	 */
	async *entries() {
		const request = this.store.openCursor();
		while (true) {
			const cursor = await IndexedDBStore.getResult(request);
			if (!cursor) break;
			yield [cursor.key, cursor.value] as [IDBValidKey, T];
			cursor.continue();
		}
	}

	/**
	 * Asynchronously iterates over all keys in the IndexedDB object store.
	 *
	 * @yields {Generator<IDBValidKey>} - Yields the key for each item in the object store.
	 */
	async *keys() {
		const request = this.store.openKeyCursor();
		while (true) {
			const cursor = await IndexedDBStore.getResult(request);
			if (!cursor) break;
			yield cursor.key;
			cursor.continue();
		}
	}

	/**
	 * Asynchronously iterates over all values in the IndexedDB object store.
	 *
	 * @yields {Generator<T>} - Yields the value for each item in the object store.
	 */
	async *values() {
		for await (const [_key, value] of this.entries())
			yield value;
	}

	/**
	 * Defines the async iterator for the IndexedDBStore class.
	 * Allows iterating over the values of the object store using the `for await...of` syntax.
	 */
	[Symbol.asyncIterator] = this.values;

	/**
	 * Retrieves all items from the IndexedDB object store.
	 *
	 * @note Use `for await...of` syntax for better performance.
	 * @returns A Promise that resolves with an array of all items in the object store or rejects with an error.
	 */
	all(): Promise<T[]> {
		return IndexedDBStore.getResult(this.store.getAll());
	}

	/**
	 * Applies a callback function to each item in the IndexedDB object store and returns a new array with the results.
	 *
	 * @template TOut - The type of the array elements returned by the callback function.
	 * @param callbackfn - A function to apply to each item in the object store.
	 * The callback function takes two arguments:
	 * - `value`: The current item being processed.
	 * - `key`: The key of the current item being processed.
	 *
	 * @returns A Promise that resolves with a new array containing the results of applying the callback function to each item in the object store.
	 *
	 * @remarks
	 * This method is asynchronous and uses the `for await...of` syntax to iterate over the object store's values.
	 * It is useful for transforming or filtering the items in the object store.
	 */
	async map<TOut>(callbackfn: (value: T, key: IDBValidKey) => TOut) {
		const result: TOut[] = [];
		for await (const [key, value] of this.entries())
			result.push(callbackfn(value, key));
		return result;
	}
}
