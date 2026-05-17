interface PropsSingle<TValue> {
	keys: StoreSubscribedProperty<TValue>;
	children(value: TValue, setValue: SetStateNarrow<TValue>): ReactNode;
}
interface PropsArray<TValue> {
	keys: StoreSubscribedProperty<TValue>[];
	children(...stateProperties: [value: TValue, setValue: SetStateNarrow<TValue>][]): ReactNode;
}
interface PropsTuple<TTuple extends readonly Any[]> {
	keys: { [Index in keyof TTuple]: StoreSubscribedProperty<TTuple[Index]> };
	children(...stateProperties: { [Index in keyof TTuple]: [value: TTuple[Index], setValue: SetStateNarrow<TTuple[Index]>] }): ReactNode;
}
interface PropsImplement<TValue> {
	keys: StoreSubscribedProperty<TValue> | StoreSubscribedProperty<TValue>[];
	children: (...args: Any[]) => ReactNode;
}

function UnmemoizedSubscribeKeys<TValue>({ children, keys }: PropsSingle<TValue>): ReactNode;
function UnmemoizedSubscribeKeys<TValue>({ children, keys }: PropsArray<TValue>): ReactNode;
function UnmemoizedSubscribeKeys<TTuple extends readonly Any[]>({ children, keys }: PropsTuple<TTuple>): ReactNode;
function UnmemoizedSubscribeKeys<TValue>({ children, keys }: PropsImplement<TValue>): ReactNode {
	const singleMode = useMemo(() => !Array.isArray(keys), [keys]);
	keys = wrapIfNotArray(keys);
	const values = keys.map(key => useStoreSubscribedProperty(key));
	if (singleMode) {
		const [value, setValue] = values[0];
		return children(value, setValue);
	} else
		return children(...values);
}

const SubscribeKeys = memo(UnmemoizedSubscribeKeys) as unknown as typeof UnmemoizedSubscribeKeys;

export function subKeys<TValue>(key: PropsSingle<TValue>["keys"], render: PropsSingle<TValue>["children"]): React.JSX.Element;
export function subKeys<TValue>(keys: PropsArray<TValue>["keys"], render: PropsArray<TValue>["children"]): React.JSX.Element;
export function subKeys<TTuple extends readonly Any[]>(keys: PropsTuple<TTuple>["keys"], render: PropsTuple<TTuple>["children"]): React.JSX.Element;
export function subKeys<TValue>(keys: PropsImplement<TValue>["keys"], render: PropsImplement<TValue>["children"]): React.JSX.Element {
	return <SubscribeKeys keys={keys as PropsArray<TValue>["keys"]}>{render}</SubscribeKeys>;
}
