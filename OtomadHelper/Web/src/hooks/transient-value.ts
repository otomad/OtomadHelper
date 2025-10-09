export class TransientValue<T> {
	public value: T;
	// Unfortunately, the TransientValue class instance value has most likely been proxied, so we can't use "#", but instead use "private".
	private timestamp: number;

	constructor(initialValue: T) {
		this.value = initialValue;
		this.timestamp = Date.now();
	}

	static computed<TIn, TOut>(source: TransientValue<TIn> | undefined, getter: (oldValue: TIn) => TOut) {
		if (!source) return undefined!;
		const newValue = getter(source.value);
		const newInstance = new TransientValue(newValue);
		newInstance.timestamp = source.timestamp;
		return newInstance;
	}

	static useEffect<T>(transientValue: TransientValue<T> | undefined, effect: (value?: T) => ReturnType<React.EffectCallback>, deps: readonly unknown[] = []) {
		useEffect(() => effect(transientValue?.value), [transientValue?.value, transientValue?.timestamp, ...deps]);
	}
}

export const useTransientValue = TransientValue.useEffect;
