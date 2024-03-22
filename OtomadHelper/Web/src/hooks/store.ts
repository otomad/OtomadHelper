import type { StoreApi, UseBoundStore } from "zustand";

function getStorePath<Store extends UseBoundStore<StoreApi<Any>>, Field>(store: Store, path: (state: ZustandState<Store>) => Field) {
	type States = ZustandState<Store>;
	const pathContents = path.toString().split("=>").map(i => i.replaceAll(/\s/g, ""));
	const rootParamName = pathContents[0].replace(/^\(|\)$/g, "").split(",")[0];
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

export function useStoreSelector<Store extends UseBoundStore<StoreApi<Any>>, Field>(store: Store, path: (state: ZustandState<Store>) => Field) {
	type States = ZustandState<Store>;
	const { getParent, lastPath } = getStorePath(store, path);

	const getter = getParent(store())[lastPath];
	const setter = (value: unknown) => store.setState((root: States) => void (getParent(root)[lastPath] =
		typeof value === "function" ? value(getter) : value));
	return [getter, setter] as StatePropertyNonNull<Field>;
}

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
