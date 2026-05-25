/**
 * An object that contains the proxy object and a key, used for subscribe store key.
 * @template TValue - For narrow type only, useless.
 */
export class StoreSubscribedProperty<TValue> {
	constructor(
		public readonly proxyObject: AnyObject,
		public readonly key: string,
	) { }

	get current(): TValue {
		return this.proxyObject[this.key];
	}

	set current(value: TValue) {
		this.proxyObject[this.key] = value;
	}

	use(): StatePropertyNonNull<TValue> {
		const { proxyObject, key } = this;
		const state = useSnapshot(proxyObject)[key] as TValue;
		const setState = setStateNarrow(newValue => proxyObject[key] = newValue, () => proxyObject[key]);
		return [state, setState];
	}
}

export class ComputedStoreSubscribeProperty<TValue, TTuple extends readonly Any[]> {
	constructor(
		public readonly keys: { [Index in keyof TTuple]: IStoreSubscribedProperty<TTuple[Index]> | undefined },
		private readonly getter: (...values: { [Index in keyof TTuple]: TTuple[Index] }) => TValue,
		private readonly setter: (newValue: TValue) => void = noop,
	) { }

	get current(): TValue {
		return this.getter(...this.keys.map(key => key?.current) as never);
	}

	set current(value: TValue) {
		this.setter(value);
	}

	use(): StatePropertyNonNull<TValue> {
		const keys = this.keys.map(key => key?.use()[0]);
		const result = this.getter(...keys as never);
		const setState = this.setter === noop ? noop as never : setStateNarrow(this.setter, () => this.current);
		return [result, setState];
	}
}

export type StoreSubscribedPropertiedObject<TState> = {
	[property in keyof TState]: StoreSubscribedProperty<TState[property]>;
};

export function currySubscribeStore<TState extends object>(state: TState | StoreSubscribedProperty<TState>): StoreSubscribedPropertiedObject<TState> {
	if (state instanceof StoreSubscribedProperty) state = state.current;
	return new Proxy(state as AnyObject, {
		get(state, property) {
			if (typeof property !== "string") return state[property];
			return new StoreSubscribedProperty(state, property);
		},
	}) as never;
}

type IStoreSubscribedProperty<T> = StoreSubscribedProperty<T> | ComputedStoreSubscribeProperty<T, Any>;

export const computedSubStore: <T, TTuple extends readonly Any[]>(...args: ConstructorParameters<typeof ComputedStoreSubscribeProperty<T, TTuple>>) => ComputedStoreSubscribeProperty<T, TTuple> = (keys, getter, setter) => new ComputedStoreSubscribeProperty(keys, getter, setter);

export type VariousState<T> = StateProperty<T> /* | StateGetterProperty<T> */ | IStoreSubscribedProperty<T>;
export type VariousStateWithSelf<T> = VariousState<T> | T;

export function useVariousState<T>(value?: VariousState<T>): StatePropertyNonNull<T>;
export function useVariousState<T>(value?: VariousStateWithSelf<T>, includeValueItself?: true): StatePropertyNonNull<T>;
export function useVariousState<T>(value: VariousState<T> = [], includeValueItself = false): StatePropertyNonNull<T> {
	"use no memo";
	if (value instanceof StoreSubscribedProperty || value instanceof ComputedStoreSubscribeProperty)
		return value.use();
	else if (isReadonlyArray(value))
		return [value[0] as T, value[1] ?? noop];
	else if (includeValueItself)
		return [value, noop];
	else
		throw new TypeError("The provided value is not supported by `useVariousState` function");
}

function getVariousState<T>(value: VariousStateWithSelf<T> = []): T {
	if (value instanceof StoreSubscribedProperty || value instanceof ComputedStoreSubscribeProperty)
		return value.current;
	else if (isReadonlyArray(value))
		return value[0] as T;
	else
		return value;
}

export function useReadonlyVariousState<T>(value?: VariousStateWithSelf<T>) {
	return useVariousState(value, true)[0];
}
