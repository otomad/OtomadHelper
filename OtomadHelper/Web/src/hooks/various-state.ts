const STORE_SUBSCRIBED_PROPERTY_VALUE = Symbol("store-subscribed-property.value_and_is_type");

/**
 * An object that contains the proxy object and a key, used for subscribe store key.
 * @template TValue - For narrow type only, useless.
 */
export class StoreSubscribedProperty<TValue> {
	constructor(
		public proxyObject: AnyObject,
		public key: string,
	) { }

	/**
	 * This symbol valued property plays two roles:
	 * 1. In TypeScript part, it plays the role of *indicating the type of the value*. This is because if you don't reference
	 * the generic parameter `TValue` inside the class body, TypeScript could treat `StoreSubscribedProperty<int>`
	 * and `StoreSubscribedProperty<string>` as the same type.
	 * 2. In JavaScript part, it plays the role of *checking the type*. Other places can check this property to examine
	 * the object is the `StoreSubscribedProperty`.
	 */
	[STORE_SUBSCRIBED_PROPERTY_VALUE]: TValue = true as never;

	static [Symbol.hasInstance](value: Any) {
		return defaultInstanceOf(StoreSubscribedProperty, value) || !!value?.[STORE_SUBSCRIBED_PROPERTY_VALUE];
	}
}

export type StoreSubscribedPropertiedObject<TState> = {
	[property in keyof TState]: StoreSubscribedProperty<TState[property]>;
};

export function currySubscribeStore<TState extends object>(state: TState): StoreSubscribedPropertiedObject<TState> {
	return new Proxy(state as AnyObject, {
		get(state, property) {
			if (typeof property !== "string") return state[property];
			return new StoreSubscribedProperty(state, property);
		},
	}) as never;
}

export function useStoreSubscribedProperty<T>(value: StoreSubscribedProperty<T>): StatePropertyNonNull<T> {
	const { proxyObject, key } = value;
	const state = useSnapshot(proxyObject)[key] as T;
	const setState = setStateNarrow(newValue => proxyObject[key] = newValue, () => proxyObject[key]);
	return [state, setState];
}

export type VariousState<T> = StateProperty<T> /* | StateGetterProperty<T> */ | StoreSubscribedProperty<T>;
export type VariousStateWithSelf<T> = VariousState<T> | T;

export function useVariousState<T>(value?: VariousState<T>): StatePropertyNonNull<T>;
export function useVariousState<T>(value?: VariousStateWithSelf<T>, includeValueItself?: true): StatePropertyNonNull<T>;
export function useVariousState<T>(value: VariousState<T> = [], includeValueItself = false): StatePropertyNonNull<T> {
	"use no memo";
	if (value instanceof StoreSubscribedProperty)
		return useStoreSubscribedProperty(value);
	else if (isReadonlyArray(value))
		return [value[0] as T, value[1] ?? noop];
	else if (includeValueItself)
		return [value, noop];
	else
		throw new TypeError("The provided value is not supported by `useVariousState` function");
}
