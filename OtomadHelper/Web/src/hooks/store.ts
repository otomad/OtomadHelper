import type { StoreApi, UseBoundStore } from "zustand";

/**
 * Function that extracts the path from a given function and returns the corresponding parts of the store's state.
 *
 * @param store - The store instance to select from.
 * @param path - A function that takes the store's state as an argument and returns the path to the desired state property.
 * @returns A function that returns the parent state object up to the specified path.
 */
function getStorePath<Store extends UseBoundStore<StoreApi<Any>>, Field>(store: Store, path: (state: ZustandState<Store>) => Field) {
	type States = ZustandState<Store>;
	const pathContents = path.toString().split("=>").map(i => i.replaceAll(/\s/g, ""));
	const rootParamName = pathContents[0].replace(/^\(|\)$/g, "").split(",")[0].replaceAll("$", "\\$");
	// The minifier (esbuild) will add the dollar symbol to the root param name,
	// so we must escape it to prevent the regexp treat it as a meta-character.
	const pathString = pathContents[1].replace(new RegExp(`^${rootParamName}\\.?`), "");

	const paths = pathString.split("."), lastPath = paths.last();
	const getParent = (root: States) => {
		let parent = root;
		for (let i = 0; i < paths.length - 1; i++)
			parent = parent[paths[i]];
		return parent;
	};

	return { getParent, lastPath };
}

/**
 * A hook that allows you to select a specific part of the store's state using a path function.
 *
 * @note The store must be used with zustand **immer** middleware!
 *
 * @param store - The store instance to select from.
 * @param path - A function that takes the store's state as an argument and returns the path to the desired state property.
 * @returns A tuple containing the selected state property and a setter function to update it.
 */
export function useStoreSelector<Store extends UseBoundStore<StoreApi<Any>>, Field>(store: Store, path: (state: ZustandState<Store>) => Field) {
	type States = ZustandState<Store>;
	const { getParent, lastPath } = getStorePath(store, path);

	const state: States = store();
	if (!state) throw new Error("You are using useStoreSelector hook with a zustand store that is not being used with zustand immer middleware");
	const getter = (root = state) => getParent(root)[lastPath];
	const setter = (value: unknown) => {
		store.setState((root: States) => {
			getParent(root)[lastPath] = typeof value === "function" ? value(getter(root)) : value;
		});
	};
	return [getter(), setter] as StatePropertyNonNull<Field>;
}

/**
 * Subscribes to a specific part of the store's state using a path function.
 *
 * @param store - The store instance to subscribe from.
 * @param path - A function that takes the store's state as an argument and returns the path to the desired state property.
 * @param listener - A function that will be called whenever the selected state property changes.
 */
export function subscribeStoreWithSelector<Store extends UseBoundStore<StoreApi<Any>>, Field>(store: Store, path: (state: ZustandState<Store>) => Field, listener: (prop: Field, prevProp: Field, state: ZustandState<Store>, prevState: ZustandState<Store>) => void) {
	type States = ZustandState<Store>;
	const { getParent, lastPath } = getStorePath(store, path);

	store.subscribe((state: States, prevState: States) => {
		const currentProp = getParent(state)[lastPath];
		const previousProp = getParent(prevState)[lastPath];
		if (currentProp !== previousProp)
			listener(currentProp, previousProp, state, prevState);
	});
}

/**
 * A hook that allows you to select a specific part of the store's state using a path function.
 * And then you can get or set the selected state property, just like use it in React `useState` hook.
 *
 * @param store - The store instance to select from.
 * @returns A proxy object that provides read and write access to the selected state property.
 */
export function useStoreState<Store extends UseBoundStore<StoreApi<Any>>>(store: Store) {
	type State = ZustandState<Store>;
	let state: State;
	try {
		state = store(); // In a React function component or a React hook
	} catch (error) {
		state = store.getState(); // In vanilla JavaScript
	}
	return new Proxy(state, {
		get(state, property) {
			if (!(property in state)) return [];
			return [state[property], (value: unknown) => store.setState((newState: State) => {
				const newValue = typeof value === "function" ? value(newState[property]) : value;
				return { [property]: newValue };
			})];
		},
	}) as {
		[property in keyof State]: StateProperty<State[property]>;
	};
}
