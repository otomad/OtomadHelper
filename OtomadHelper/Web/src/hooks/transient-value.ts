/**
 * Represents a value that is transient and associated with a timestamp.
 * Useful for tracking values that change over time and need to be associated with their change time.
 *
 * @template T - The type of the value being stored.
 */
export class TransientValue<T> {
	/**
	 * The current value.
	 */
	public value: T;

	/**
	 * The timestamp when the value was last set.
	 * @private
	 */
	// Unfortunately, the TransientValue class instance value has most likely been proxied,
	// so we can't use `#` private field, but instead use `private` keyword.
	private timestamp: number;

	/**
	 * Creates a new TransientValue instance.
	 *
	 * @param initialValue - The initial value to store.
	 */
	constructor(initialValue: T) {
		this.value = initialValue;
		this.timestamp = Date.now();
	}

	/**
	 * Creates a new TransientValue by computing a new value from an existing TransientValue.
	 * The new instance will inherit the timestamp from the source.
	 *
	 * @template TIn - The type of the source TransientValue.
	 * @template TOut - The type of the new TransientValue.
	 * @param source - The source TransientValue to compute from.
	 * @param getter - A function that computes the new value from the source value.
	 * @returns A new TransientValue with the computed value and inherited timestamp.
	 */
	static computed<TIn, TOut>(source: TransientValue<TIn> | undefined, getter: (oldValue: TIn) => TOut) {
		if (!source) return undefined!;
		const newValue = getter(source.value);
		const newInstance = new TransientValue(newValue);
		newInstance.timestamp = source.timestamp;
		return newInstance;
	}

	/**
	 * Runs a React effect whenever the transient value or its timestamp changes.
	 *
	 * @template T - The type of the transient value.
	 * @param transientValue - The TransientValue to watch for changes.
	 * @param effect - The effect function to run when the value or timestamp changes.
	 * @param deps - Additional dependencies for the effect.
	 */
	static useEffect<T>(transientValue: TransientValue<T> | undefined, effect: (value?: T) => ReturnType<React.EffectCallback>, deps: readonly unknown[] = []) {
		useEffect(() => effect(transientValue?.value), [transientValue?.value, transientValue?.timestamp, ...deps]);
	}
}

/**
 * Runs a React effect whenever the transient value or its timestamp changes.
 *
 * @template T - The type of the transient value.
 * @param transientValue - The TransientValue to watch for changes.
 * @param effect - The effect function to run when the value or timestamp changes.
 * @param deps - Additional dependencies for the effect.
 */
export const useTransientValue = TransientValue.useEffect;
