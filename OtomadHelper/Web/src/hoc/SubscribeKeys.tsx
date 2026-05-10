export function SubscribeKeys<TValue>({ children, keys }: {
	children(value: TValue, setValue: SetStateNarrow<TValue>): ReactNode;
	keys: StoreSubscribedProperty<TValue>;
}): ReactNode;
export function SubscribeKeys<TValue>({ children, keys }: {
	children(stateProperties: [value: TValue, setValue: SetStateNarrow<TValue>][]): ReactNode;
	keys: StoreSubscribedProperty<TValue>[];
}): ReactNode;
export function SubscribeKeys<TTuple extends Any[]>({ children, keys }: {
	children(stateProperties: { [Index in keyof TTuple]: [value: TTuple[Index], setValue: SetStateNarrow<TTuple[Index]>] } & { length: TTuple["length"] }): ReactNode;
	keys: { [Index in keyof TTuple]: StoreSubscribedProperty<TTuple[Index]> } & { length: TTuple["length"] };
}): ReactNode;
export function SubscribeKeys<TValue>({ children, keys }: {
	children: (...args: Any[]) => ReactNode;
	keys: StoreSubscribedProperty<TValue> | StoreSubscribedProperty<TValue>[];
}) {
	const singleMode = !Array.isArray(keys);
	keys = wrapIfNotArray(keys);
	const values = keys.map(key => useStoreSubscribedProperty(key));
	if (singleMode) {
		const [value, setValue] = values[0];
		return children(value, setValue);
	} else
		return children(values);
}
