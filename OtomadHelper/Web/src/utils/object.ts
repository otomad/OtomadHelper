/**
 * Returns an array whose elements are arrays corresponding to the enumerable string-keyed
 * property key-value pairs found directly upon `object`. This is the same as iterating with a `for...in` loop,
 * except that a `for...in` loop enumerates properties in the prototype chain as well. The order of the array
 * returned by `Object.entries()` is the same as that provided by a `for...in` loop.
 *
 * Compared to `Object.entries`, the types of values returned in TypeScript are less disgusting.
 *
 * @template TKey - The key enumeration type of the object.
 * @template TValue - The value type of the object.
 * @param obj - An object that can return key value pairs of its enumerable properties.
 * @returns
 * An array of the given object's own enumerable string-keyed property key-value pairs. Each key-value
 * pair is an array with two elements: the first element is the property key (which is always a string), and the
 * second element is the property value.
 */
export function entries<TKey extends string | number | symbol, TValue>(obj: { [s in TKey]?: TValue }) {
	return Object.entries(obj) as [TKey, TValue][];
}

/**
 * Returns an array of a given object's own enumerable string-keyed property names.
 *
 * Compared to `Object.keys`, the types of values returned in TypeScript are less disgusting.
 *
 * @param obj - An object.
 * @returns An array of strings representing the given object's own enumerable string-keyed property keys.
 */
export function keys<TKey extends object>(obj: TKey) {
	return Object.keys(obj) as (keyof TKey)[];
}

export function hasOwn<T extends object>(obj: T, key: PropertyKey): key is keyof T {
	return Object.hasOwn(obj, key);
}

/**
 * Copies all enumerable own properties from one or more source objects to a target object.
 * It returns the modified target object.
 *
 * Compared to `Object.assign`, when you are typing the source objects, you will enjoy the property type hints obtained
 * from the source object by TypeScript. In addition, since the TypeScript built-in library is implemented to merge the
 * type of the target object with the types of each source objects, in fact, we only need to maintain the original type
 * of the target object.
 *
 * @param target - The target object — what to apply the sources' properties to, which is returned after it is modified.
 * @param sources - The source object(s) — objects containing the properties you want to apply.
 * @return The target object.
 */
export function assign<TTarget extends object>(target: TTarget, ...sources: Partial<TTarget>[]): TTarget {
	return Object.assign(target, ...sources);
}

/**
 * Create an array of objects that repeat a specified number of times, such as for creating components in a loop.
 *
 * But we only need to care about the number of loops, not the content of the array.
 *
 * @template T - The object type to be repeated.
 * @param length - Number of loops.
 * @param callback - `map` callback.
 * @param flat - Flatten the array?
 * @returns An array of objects repeated a specified number of times.
 */
export function forMap<T>(length: number, callback: (index: number) => T, flat: boolean = false) {
	const mapAction = (flat ? "flatMap" : "map") as "map";
	return Array<void>(length).fill(undefined)[mapAction]((_, index) => callback(index + 1));
}

/**
 * Map an array from the given start value to the end value.
 * @param start - Start value.
 * @param end - End value.
 * @param callback - `map` callback.
 * @returns An array of objects repeated a specified number of times.
 */
export function forMapFromTo<T>(start: number, end: number, callback: (index: number) => T) {
	const result: T[] = [];
	for (let i = start; i <= end; i++)
		result.push(callback(i));
	return result;
}

/**
 * Determine whether the object contains the key, while also guarding the type.
 * @param obj - Object.
 * @param key - Key.
 * @returns Does the object contain the key?
 */
export const hasKey = <T extends object>(obj: T, key: keyof Any): key is keyof T => key in obj;

/**
 * Intercept the `setter` method in `useState`.
 * @template T - The type to set in the `setter`.
 * @param setter - The `setter` method in `useState`.
 * @param interceptor - Interceptor.
 * @returns The generated new `setter` method.
 */
export function setStateInterceptor<T>(setter: SetState<T>, interceptor: (userInput: Any, prevState: T) => T) {
	return (userInput: React.SetStateAction<T>) => {
		type PrevStateSetter = (value: (prevState: T) => void) => void;
		(setter as PrevStateSetter)(prevState => interceptor(userInput instanceof Function ? userInput(prevState) : userInput, prevState));
	};
}

/**
 * Map the old `useState` to a new `StateProperty`, such as its child property.
 * @template TOld - The old `StateProperty` type.
 * @template TNew - The new `StateProperty` type.
 * @param stateProperty - The old `useState`.
 * @param getter - The mapped new `getter`.
 * @param setter - The mapped new `setter`.
 * @returns The new `useState`.
 */
export function useStateSelector<TOld, TNew>(stateProperty: StateProperty<TOld>, getter: (original: TOld) => TNew, setter: (userInput: TNew, prevState: TOld) => TOld) {
	return [
		getter(stateProperty[0]!),
		setStateInterceptor(stateProperty[1]!, setter),
	] as StateProperty<TNew>;
}

/**
 * Checks whether the given value is a `RefObject`.
 *
 * This function, `isRef`, checks whether the given value is a `RefObject`. It returns `true` if the value is a `RefObject`,
 * otherwise `false`. A `RefObject` is a type of reference in React that allows you to access a DOM element or instance.
 * The function takes a single parameter, `ref`, which is the value to check. The function returns a boolean value
 * indicating whether the value is a `RefObject` or not.
 *
 * @param ref - The value to check.
 * @returns `true` if the value is a `RefObject`, otherwise `false`.
 *
 * @example
 * ```typescript
 * const ref: React.RefObject<HTMLElement> = React.useRef();
 * console.log(isRef(ref)); // Output: true
 *
 * const notRef: HTMLElement = document.getElementById("my-element")!;
 * console.log(isRef(notRef)); // Output: false
 * ```
 */
export function isRef<T>(ref: MaybeRef<T>): ref is RefObject<T> {
	return ref && Object.hasOwn(ref, "current") && Object.keys(ref).length === 1;
}

/**
 * Restore the actual value that may be a `RefObject` to itself or any other value.
 *
 * This function, `toValue`, takes a `MaybeRef` type parameter `T` and a `MaybeRef<T>` type parameter `ref` as input. It returns
 * the actual value of the input. If the input is a `RefObject`, it retrieves the current value of the `RefObject` using the
 * `.current` property. Otherwise, it simply returns the input value as it is. This function is useful when you want to use
 * the actual value of a `RefObject` or any other value in your code, rather than the reference itself.
 *
 * @param ref - The `RefObject` to convert.
 * @returns The actual value of the `RefObject`.
 *
 * @example
 * ```typescript
 * const ref: React.RefObject<HTMLElement> = React.useRef();
 * console.log(toValue(ref)); // Output: <div id="my-element"> or undefined or null
 *
 * const notRef: HTMLElement = document.getElementById("my-element")!;
 * console.log(toValue(notRef)); // Output: <div id="my-element">
 * ```
 */
export function toValue<T>(ref: MaybeRef<T>): T {
	return isRef(ref) ? ref.current! : ref;
}

/**
 * Creates a reference to an HTML DOM element without initializing it to null.
 *
 * This hook returns a reference to an HTML DOM element, which can be used to access the DOM element directly.
 * It is useful when you need to interact with the DOM element directly, rather than using React's controlled components.
 *
 * @remarks You must provide the `TElement` generic type.
 *
 * @template TElement - A tag name (e.g. `"div"`) or a subclass (e.g. `HTMLDivElement`) of the HTML DOM element class.
 * @param initialValue - The initial value of the reference. Usually to `null`.
 * @returns A reference to an HTML DOM element.
 *
 * @example
 * ```typescript
 * const ref = useDomRef<"div">();
 * // Or
 * const ref = useDomRef<HTMLDivElement>();
 * // Equivalent to
 * const ref = useRef<HTMLDivElement | null>(null);
 * ```
 */
export function useDomRef<TElement extends keyof ElementTagNameMap | Element>(initialValue: TagNameToElement<TElement> | null = null) {
	return useRef<TagNameToElement<TElement> | null>(initialValue);
}

/**
 * Creates a hook that returns a mutable reference to an HTML DOM element without initializing it to null.
 *
 * @remarks This will return a `StateProperty`, meaning that modifying the reference will also cause the component to re-render.
 *
 * @param initialValue - The initial value of the reference. Usually to `null`.
 * @returns A tuple containing the current value of the reference and a function to update it.
 *
 * @example
 * ```typescript
 * // Compare with `useDomRef`
 * const ref = useDomRef<"div">();
 * useEffect(() => {
 *     // This effect is only called during component initialization, regardless of whether the `ref` has changed.
 * }, [ref]);
 *
 * const [ref, setRef] = useDomRefState<"div">();
 * useEffect(() => {
 *     // This effect will be called as long as the `ref` changes.
 * }, [ref]);
 * ```
 */
export function useDomRefState<TElement extends keyof ElementTagNameMap | Element>(initialValue: TagNameToElement<TElement> | null = null) {
	return useState<TagNameToElement<TElement> | null>(initialValue);
}

export function useDomRefs<TElement extends keyof ElementTagNameMap | Element>() {
	type TElementOrNull = TagNameToElement<TElement> | null;
	const refs: MutableRefObject<TElementOrNull[]> = useRef([]);
	const setRef = (index: number) => (el: TElementOrNull) => refs.current[index] = el;
	return [refs, setRef] as const;
}

/**
 * Returns the global environment, used to define global variables.
 *
 * Also avoid warnings from TypeScript.
 */
export const globals = globalThis as AnyObject;

/**
 * Checks if `value` is object-like. A value is object-like if it's not `null`
 * and has a `typeof` result of "object".
 *
 * @category Lang
 * @param value - The value to check.
 * @returns Returns `true` if `value` is object-like, else `false`.
 * @example
 *
 * _.isObjectLike({});
 * // => true
 *
 * _.isObjectLike([1, 2, 3]);
 * // => true
 *
 * _.isObjectLike(_.noop);
 * // => false
 *
 * _.isObjectLike(null);
 * // => false
 */
export function isObject(value: unknown): value is object {
	return value !== null && typeof value === "object";
	// return lodash.isObjectLike(value);
}

/**
 * Checks if `value` is a literal object. A literal object is an object that is created using object literal syntax.
 *
 * @category Lang
 * @param value - The value to check.
 * @returns Returns `true` if `value` is a literal object, else `false`.
 *
 * @example
 * ```typescript
 * const obj1 = { prop: "value" };
 * console.log(isLiteralObject(obj1)); // Output: true
 *
 * const obj2 = new Object({ prop: "value" });
 * console.log(isLiteralObject(obj2)); // Output: true
 *
 * const obj3 = function () { };
 * console.log(isLiteralObject(obj3)); // Output: false
 *
 * const obj4 = { prop: "value" }.constructor;
 * console.log(isLiteralObject(obj4)); // Output: false
 *
 * const obj5 = document;
 * console.log(isLiteralObject(obj5)); // Output: false
 *
 * const obj6 = /(.*)/;
 * console.log(isLiteralObject(obj6)); // Output: false
 *
 * const obj7 = new Date();
 * console.log(isLiteralObject(obj7)); // Output: false
 * ```
 */
export function isLiteralObject(value: unknown): value is object {
	return Object.prototype.toString.call(value) === "[object Object]";
}

/**
 * Asserts that the provided object is of the specified type.
 *
 * In fact, if used correctly, it can force the type of a variable to suddenly change to another type.
 *
 * @remarks Due to the limitations of TypeScript, it can only shrink the type.
 *
 * @template T - The type that the object should be.
 * @param object - The object to be asserted.
 *
 * @remarks This function is a no-op and does not perform any actual assertion. It is used to provide type safety and ensure that the object is of the specified type.
 *
 * @example
 * ```typescript
 * let foo = "foo"; // Type: string
 * asserts<"foo" | "bar">(foo);
 * foo; // Type: "foo" | "bar"
 *
 * let element = document.getElementById("my-element")!.firstElementChild!; // Type: Element
 * asserts<HTMLInputElement>(element);
 * element; // Type: HTMLInputElement;
 *
 * let a = 123; // Type: number
 * asserts<string>(a);
 * a; // Type: never. Because type "number" is not assignable to type "string".
 *
 * let foo = "foo" as "foo" | "bar"; // Type: "foo" | "bar"
 * asserts<string>(foo);
 * foo; // Type is still "foo" | "bar", because it cannot increase the type.
 * ```
 */
export function asserts<T>(object: unknown): asserts object is T { }

/**
 * Checks if `value` is undefined, null, or NaN.
 *
 * @category Lang
 * @param value The value to check.
 * @returns Returns `true` if `value` is undefined, null, or NaN, else `false`.
 * @example
 *
 * isUndefinedNullNaN(undefined);
 * // => true
 *
 * isUndefinedNullNaN(null);
 * // => true
 *
 * isUndefinedNullNaN(NaN);
 * // => true
 *
 * isUndefinedNullNaN(123);
 * // => false
 */
export function isUndefinedNullNaN(object: unknown): object is undefined | null {
	return [undefined, null, NaN].includes(object as never);
}

/**
 * Makes the keys of the prototype non-enumerable.
 *
 * This function, `makePrototypeKeysNonEnumerable`, takes a constructor function as input.
 * It iterates through the keys of the constructor's prototype object and sets their enumerability to `false`.
 * This means that these keys will not be included when enumerating the properties of the prototype object using methods like `for...in` or `Object.keys()`.
 *
 * @param constructor - The constructor function whose prototype keys should be made non-enumerable.
 */
export function makePrototypeKeysNonEnumerable(constructor: AnyConstructor) {
	const protoKeys = Object.keys(constructor.prototype);
	for (const protoKey of protoKeys)
		Object.defineProperty(constructor.prototype, protoKey, {
			configurable: true, // Allowed to be overridden, if it has the same name as the ECMAScript method in the future.
			enumerable: false, // Do not print when being used in for in loops.
		});
}

/**
 * Defines a getter function on the prototype of a constructor function.
 *
 * This function is used to add a getter property to the prototype of a constructor function.
 * The getter function will be called when accessing the property on instances of the constructor.
 *
 * @template T - The type of the instances of the constructor.
 * @param constructor - The constructor function to which the getter property will be added.
 * @param protoKey - The name of the property to be added to the prototype.
 * @param getter - The function to be called when accessing the property.
 *
 * @example
 * ```typescript
 * class MyClass {
 *     private _value: number;
 *
 *     constructor(value: number) {
 *         this._value = value;
 *     }
 * }
 *
 * defineGetterInPrototype(MyClass, "value", function(this: MyClass) {
 *     return this._value;
 * });
 *
 * const instance = new MyClass(42);
 * console.log(instance.value); // Output: 42
 * ```
 */
export function defineGetterInPrototype<T>(constructor: new (...args: Any[]) => T, protoKey: string, getter: (this: T) => Any) {
	Object.defineProperty(constructor.prototype, protoKey, {
		get: getter,
		configurable: true,
		enumerable: false,
	});
}

/**
 * A no-operation function that returns undefined regardless of the arguments it receives.
 *
 * @return undefined
 */
export const noop = lodash.noop;

/**
 * Providing a list of boolean state properties (such as a list of toggle switches),
 * the logic of their setters will now be changed: when any toggle switch is turned on,
 * the other toggle switches in the list will be turned off. Achieve an effect similar to a radio button group.
 * This will make the toggle switches mutually exclusive.
 *
 * This function will return a new list of boolean state properties with the setters modified.
 * The original list of boolean status properties in the function parameters will also be modified.
 * As long as these tuples (boolean state properties) are not temporarily created when calling the function but existing variables,
 * you can reuse the original variables directly instead of the return values of the function.
 *
 * @param switches - S list of boolean state properties (such as a list of toggle switches).
 * @returns Same as parameter `switches`.
 */
export function mutexSwitches(...switches: (StateProperty<boolean> | StatePropertyNonNull<boolean> | SetState<boolean> | SetStateNarrow<boolean>)[]) {
	const originalSetStates: SetState<boolean>[] = [];
	const result: typeof originalSetStates = [];
	for (const switch_ of switches) {
		const originalSetState = (Array.isArray(switch_) ? switch_[1] : switch_)!;
		originalSetStates.push(originalSetState);
	}
	for (const [i, originalSetState] of originalSetStates.entries()) {
		const setState = setStateInterceptor(originalSetState, value => {
			if (value)
				for (const otherSetState of originalSetStates)
					if (otherSetState !== originalSetState)
						otherSetState(false);
			return value;
		});
		result[i] = setState;
		const switch_ = switches[i];
		if (Array.isArray(switch_))
			switch_[1] = setState;
	}
	return result;
}

/**
 * Filters the keys of an object based on a provided list of keys.
 * Creates a new object with only the filtered keys and their corresponding values.
 *
 * @param object - The object to filter keys from.
 * @param filteredKeys - An array of keys to filter from the object.
 * @returns A new object containing only the filtered keys and their corresponding values.
 *
 * @example
 * ```typescript
 * const originalObject = { a: 1, b: 2, c: 3 };
 * const filteredKeys = ['a', 'c'];
 * const result = objectFilterKeys(originalObject, filteredKeys);
 * // result: { a: 1, c: 3 }
 * ```
 */
export function objectFilterKeys(object: object, filteredKeys: PropertyKey[]) {
	const copiedObject: object = {};
	const ownedKeys = Reflect.ownKeys(object);
	for (const key of ownedKeys)
		if (filteredKeys.includes(key))
			Reflect.defineProperty(copiedObject, key, Reflect.getOwnPropertyDescriptor(object, key)!);
	return copiedObject;
}

/**
 * Replaces the keys of an object with new keys obtained from a provided function.
 * Creates a new object with the replaced keys and their corresponding values.
 *
 * @template T - The type of the input object. Must be an object type.
 * @param object - The object whose keys need to be replaced.
 * @param replacement - A function that takes an old key as input and returns the new key.
 * @returns A new object with the replaced keys and their corresponding values.
 *
 * @example
 * ```typescript
 * const originalObject = { a: 1, b: 2, c: 3 };
 * const replacementFunction = (oldKey: string) => oldKey.toUpperCase();
 * const result = objectReplaceKeys(originalObject, replacementFunction);
 * // result: { A: 1, B: 2, C: 3 }
 * ```
 */
export function objectReplaceKeys<T extends object>(object: T, replacement: (oldKey: string) => string) {
	return Object.fromEntries(Object.entries(object).map(([key, value]) => [replacement(key), value] as const)) as T;
}

/**
 * @deprecated // WARN: Await for React 19 released with a new feature **ref as a prop**, and then delete this function.
 */
export function functionModule<F extends Function, O>(
	func: F,
	object: O,
) {
	return Object.assign(func, object) as F & Readonly<O>;
}
