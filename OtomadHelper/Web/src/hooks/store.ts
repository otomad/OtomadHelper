/**
 * Creates a persistent *Valtio* store using the provided name and initial object.
 * The store is persisted in the browser's local storage.
 *
 * @template TState - The type of the initial object. It should be an object type.
 * @param name - The name of the store. This will be used as the key in the local storage.
 * @param initialObject - The initial object to be stored in the store.
 * If not provided, the store will be initialized with an empty object.
 * @returns A valtio store instance that is persisted in the local storage.
 */
export function createPersistStore<TState extends object>(name: string, initialObject: TState = {} as TState, options: PersistOptions<TState> = {}) {
	const store = createStore(Object.assign(initialObject, JSON.parse(localStorage.getItem(name)!) as TState));
	subscribeStore(store, () => {
		let partialStore: object = store;
		if (options.partialize)
			partialStore = typeof options.partialize === "function" ? options.partialize(store) : objectFilterKeys(store, options.partialize);
		localStorage.setItem(name, JSON.stringify(partialStore));
	});
	return store;
}

interface PersistOptions<TState extends object> {
	/**
	 * Filter the persisted value.
	 *
	 * @params state The state's value
	 */
	partialize?: (keyof TState)[] | ((state: TState) => object);
}

/**
 * A hook that allows you to select a specific part of the store's state using a path function.
 * And then you can get or set the selected state property, just like use it in React `useState` hook.
 *
 * @param state - The store instance to select from.
 * @returns A proxy object that provides read and write access to the selected state property.
 */
export function useStoreState<TState extends object>(state: TState): StatePropertiedObject<TState> {
	if (!isObject(state)) return state;
	return new Proxy(state as AnyObject, {
		get(state, property) {
			if (!(property in state)) return [];
			let snapshot: Any;
			try {
				snapshot = useSnapshot(state)[property];
			} catch { // Not in hook lifecycle.
				snapshot = state[property];
			}
			const stateProperty = [snapshot, (value: unknown) => {
				const newValue = typeof value === "function" ? value(state[property]) : value;
				if (state[property] === newValue) return newValue; // If the value is same as the previous value, do not reassign it.
				return state[property] = newValue;
			}];
			Object.assign(stateProperty, {
				subscribe: (callback: (value: unknown) => void) => subscribeStoreKey(state, property, callback),
				useState() {
					const [getState, setStateOriginal] = useState(state[property]);
					const setState = setStateInterceptor(setStateOriginal, undefined, value => state[property] = value);
					useMountEffect(() => {
						(stateProperty as StatePropertyPremium<unknown>).subscribe(value => setStateOriginal(value));
					});
					return [getState, setState];
				},
			});
			return stateProperty;
		},
	}) as never;
}

/**
 * Check if the StateProperty is StatePropertyPremium.
 */
export function isStatePropertyPremium<T>(stateProperty?: StateProperty<T> | null): stateProperty is StatePropertyPremium<T> {
	return !!stateProperty && "subscribe" in stateProperty;
}

export function useStoreStateArray<T extends object>(array: T[]): StatePropertiedObject<T>[] {
	return array.map(item => useStoreState(item));
}
