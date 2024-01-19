import type { StoreApi, UseBoundStore } from "zustand";

export function useStoreSelector<Store extends UseBoundStore<StoreApi<Any>>, Field>(store: Store, path: (state: ZustandState<Store>) => Field) {
	type States = ZustandState<Store>;
	const pathContents = path.toString().split("=>").map(i => i.replaceAll(/\s/g, ""));
	const rootParamName = pathContents[0].replace(/^\(|\)$/g, "").split(",")[0];
	const pathString = pathContents[1].replace(new RegExp(`^${rootParamName}\\.?`), "");

	const paths = pathString.split("."), lastPath = paths.at(-1)!;
	const getParent = (root: States) => {
		let parent: Any;
		for (let i = 0; i < paths.length - 1; i++)
			parent = root[paths[i]];
		return parent;
	};

	const getter = getParent(store())[lastPath];
	const setter = (value: unknown) => store.setState((root: States) => void (getParent(root)[lastPath] = value));
	return [getter, setter] as StatePropertyNonNull<Field>;
}
