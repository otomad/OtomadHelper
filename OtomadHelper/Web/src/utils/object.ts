import { match } from "@formatjs/intl-localematcher";
import lodash from "lodash";

/**
 * Returns an array whose elements are arrays corresponding to the enumerable string-keyed
 * property key-value pairs found directly upon `object`. This is the same as iterating with a `for...in` loop,
 * except that a `for...in` loop enumerates properties in the prototype chain as well. The order of the array
 * returned by `Object.entries()` is the same as that provided by a `for...in` loop.
 *
 * Compared to `Object.entries`, the types of values returned in TypeScript are less disgusting.
 *
 * @note
 * `Object.entries` will not get the entries with symbol keys, this function will also contain the entries with symbol keys.
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
	return Reflect.ownKeys(obj).map(key => [key, obj[key as TKey]] as [TKey, TValue]);
}

/**
 * Returns an array of a given object's own enumerable string-keyed property names.
 *
 * Compared to `Object.keys`, the types of values returned in TypeScript are less disgusting.
 *
 * @note
 * `Object.keys` will not get the symbol keys, this function will also contain the symbol keys.
 *
 * @template T - The object type.
 * @param obj - An object.
 * @returns An array of strings representing the given object's own enumerable string-keyed property keys.
 */
export function keys<T extends object>(obj: T) {
	return Reflect.ownKeys(obj) as (keyof T)[];
}

/**
 * Checks if the provided object has a specified own property.
 *
 * Compared to `Object.hasOwn`, the types of values returned in TypeScript are less disgusting.
 *
 * @template T - The type of the object. Must be an object type.
 * @param obj - The object to check.
 * @param key - The key to check for.
 * @returns Does the object have the specified own property?
 */
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
 * @template TTarget - The target object type.
 * @param target - The target object — what to apply the sources' properties to, which is returned after it is modified.
 * @param sources - Source object(s) — objects containing the properties you want to apply.
 * @returns The target object.
 */
export function assign<TTarget extends object>(target: TTarget, ...sources: Partial<TTarget>[]): TTarget {
	return Object.assign(target, ...sources);
}

{ // Init object extensions
	Object.pick = function <T>(object: T, predicate: ObjectPickOmitPredicate<T> = [], thisArg?: unknown) {
		return _objectPickOrOmit(true, object, predicate, thisArg);
	};

	Object.omit = function <T>(object: T, predicate: ObjectPickOmitPredicate<T> = [], thisArg?: unknown) {
		return _objectPickOrOmit(false, object, predicate, thisArg);
	};

	Object.replaceKeys = function (object, replacement) {
		return Object.create({}, Object.fromEntries(entries(Object.getOwnPropertyDescriptors(object)).map(([key, descriptor]) => [replacement(key as keyof typeof object), descriptor] as const)));
	};

	Object.clear = function (object) {
		for (const prop of Reflect.ownKeys(object))
			delete object[prop];
	};

	Object.indexOf = function (object, index) {
		const key = Reflect.ownKeys(object)[index] as keyof typeof object;
		return [key, object[key]];
	};

	Object.compactUndefined = function (object) {
		for (const key of Reflect.ownKeys(object))
			if (object[key] === undefined)
				delete object[key];
	};
}

type ObjectPickOmitPredicate<T> = (keyof T)[] | ((currentValue: T[keyof T], key: keyof T, object: T) => boolean);
// BUG: This function not work with DOMRect (Element.getBoundingClientRect()).
function _objectPickOrOmit<T>(isPick: boolean, object: T, predicate: ObjectPickOmitPredicate<T>, thisArg?: unknown): Partial<T> {
	if (object == null) return {};
	if (typeof predicate !== "function") {
		const keys = predicate;
		predicate = (_, key) => keys.includes(key);
	}
	if (thisArg != null) predicate = predicate.bind(thisArg);
	const descriptors = Object.getOwnPropertyDescriptors(object) as Record<string | symbol, PropertyDescriptor>;
	for (const key of Reflect.ownKeys(descriptors)) {
		const descriptor = descriptors[key];
		const value = "value" in descriptor ? descriptor.value : descriptor.get?.();
		const predicted = predicate(value, key as keyof T, object);
		if (isPick !== predicted) delete descriptors[key];
	}
	return Object.create(Object.getPrototypeOf(object), descriptors);
}

/**
 * Create an array of objects that repeat a specified number of times, such as for creating components in a loop.
 *
 * But we only need to care about the number of loops, not the content of the array.
 *
 * @template T - The object type to be repeated.
 * @param length - Number of loops.
 * @param callback - `map` callback.
 * @param startIndex - Initial index. Defaults to 0.
 * @param flat - Flatten the array?
 * @returns An array of objects repeated a specified number of times.
 */
export function forMap<T>(length: number, callback: (index: number, length: number) => T, startIndex: number = 0, flat: boolean = false) {
	const result = Array.from({ length }, (_, index) => callback(index + startIndex, length));
	return flat ? result.flat() : result;
}

/**
 * Map an array from the given start value to the end value.
 * @template T - The item type of array that will be returned.
 * @param start - Start value.
 * @param end - End value.
 * @param step - Step value.
 * @param callback - `map` callback.
 * @returns An array of objects repeated a specified number of times.
 */
export function forMapFromTo<T = number>(start: number, end: number, step: number = 1, callback: (index: number) => T = i => i as T) {
	const result: T[] = [];
	for (let i = start; i <= end; i += step)
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
 * @template TOld - The type to set in the `setter`.
 * @template TNew - `interceptor` and `getter` type.
 * @param setter - The `setter` method in `useState`.
 * @param interceptor - Interceptor.
 * @param subscribe - Do something after the value set.
 * @param getter - INTERNAL pass getter from `useStateSelector` hook.
 * @param immer - Use immer?
 * @returns The generated new `setter` method.
 */
export function setStateInterceptor<TOld, TNew = TOld>(
	setter: SetState<TOld>,
	interceptor?: (userInput: TNew, prevState: TOld) => TOld,
	subscribe?: (curState: TOld, prevState: TOld, userInput: TNew) => void,
	getter?: (original: TOld) => TNew,
	immer: boolean = false,
) {
	return (userInput: React.SetStateAction<TNew>) => {
		type PrevStateSetter = (value: (prevState: TOld) => void) => void;
		const getCurState = (prevState: TOld, userInputValue: TNew): TOld => interceptor ? interceptor(userInputValue, prevState) : userInputValue as unknown as TOld;
		(setter as PrevStateSetter)(prevOldState => {
			// if (immer) debugger;
			const prevNewState = getter ? getter(prevOldState) : prevOldState as unknown as TNew;
			const userInputValue: TNew = userInput instanceof Function ? userInput(prevNewState) : userInput;
			const curOldState = immer ? produce(prevOldState, draft => { getCurState(draft as TOld, userInputValue); }) : getCurState(prevOldState, userInputValue);
			if (curOldState !== prevOldState) subscribe?.(curOldState, prevOldState, userInputValue);
			return curOldState;
		});
	};
}

interface UseStateSelectorOptions {
	/** In the new setter, it will use the new getter to preprocess the previous state. */
	processPrevStateInSetterWithGetter?: boolean;
	/** Use immer? */
	immer?: boolean;
}

/**
 * Map the old `useState` to a new `StateProperty`, such as its child property.
 * @template TOld - The old `StateProperty` type.
 * @template TNew - The new `StateProperty` type.
 * @param stateProperty - The old `useState`.
 * @param getter - The mapped new `getter`.
 * @param setter - The mapped new `setter`.
 * @param options - Options.
 * @returns The new `useState`.
 */
export function useStateSelector<TOld, TNew>(
	stateProperty: StateProperty<TOld>,
	getter: (original: TOld) => TNew,
	setter: (userInput: TNew, prevState: TOld) => void,
	options?: UseStateSelectorOptions & { immer: true },
): StatePropertyNonNull<TNew>;
/**
 * Map the old `useState` to a new `StateProperty`, such as its child property.
 * @template TOld - The old `StateProperty` type.
 * @template TNew - The new `StateProperty` type.
 * @param stateProperty - The old `useState`.
 * @param getter - The mapped new `getter`.
 * @param setter - The mapped new `setter`.
 * @param options - Options.
 * @returns The new `useState`.
 */
export function useStateSelector<TOld, TNew>(
	stateProperty: StateProperty<TOld>,
	getter: (original: TOld) => TNew,
	setter: (userInput: TNew, prevState: TOld) => TOld,
	options?: UseStateSelectorOptions,
): StatePropertyNonNull<TNew>;
export function useStateSelector<TOld, TNew>(
	stateProperty: StateProperty<TOld>,
	getter: (original: TOld) => TNew,
	setter: (userInput: TNew, prevState: TOld) => TOld,
	{
		processPrevStateInSetterWithGetter = false,
		immer = false,
	}: UseStateSelectorOptions = {},
) {
	return [
		getter(stateProperty[0]!),
		setStateInterceptor(stateProperty[1]!, setter, undefined, processPrevStateInSetterWithGetter || immer ? getter : undefined, immer),
	] as StatePropertyNonNull<TNew>;
}

/**
 * Creates a narrow state setter that avoids checking if the input is an updater function and unnecessary updates.
 *
 * The returned setter accepts either a direct value or an updater function.\
 * If the new value equals the current value, the original setter is not invoked.
 *
 * @template T - The type of the state value.
 * @param setter - The original state setter function.
 * @param getter - A function that returns the current state value.
 * @returns A setter that only applies changes when the value differs.
 */
export function setStateNarrow<T>(setter: (newValue: T) => void, getter: () => T) {
	return ((value: unknown) => {
		const currentValue = getter();
		const newValue = typeof value === "function" ? value(currentValue) : value;
		if (currentValue !== newValue) // If the value is same as the previous value, do not set it again.
			setter(newValue);
		return newValue;
	}) as SetStateNarrow<T>;
}

/**
 * Checks whether the given value is a `RefObject`.
 *
 * This function, `isRefObject`, checks whether the given value is a `RefObject`. It returns `true` if the value is a `RefObject`,
 * otherwise `false`. A `RefObject` is a type of reference in React that allows you to access a DOM element or instance.
 * The function takes a single parameter, `ref`, which is the value to check. The function returns a boolean value
 * indicating whether the value is a `RefObject` or not.
 *
 * @template T - The value type that wrapped by the ref.
 * @param ref - The value to check.
 * @returns Is the value a `RefObject`?
 *
 * @example
 * ```typescript
 * const ref: React.RefObject<HTMLElement> = React.useRef();
 * console.log(isRefObject(ref)); // Output: true
 *
 * const notRef: HTMLElement = document.getElementById("my-element")!;
 * console.log(isRefObject(notRef)); // Output: false
 * ```
 */
export function isRefObject<T>(ref: unknown): ref is RefObject<T> {
	return isObject(ref) && Object.hasOwn(ref, "current") && Object.keys(ref).length === 1;
}

/**
 * Restore the actual value that may be a `RefObject` to itself or any other value.
 *
 * This function, `toValue`, takes a `MaybeRef` type parameter `T` and a `MaybeRef<T>` type parameter `ref` as input. It returns
 * the actual value of the input. If the input is a `RefObject`, it retrieves the current value of the `RefObject` using the
 * `.current` property. Otherwise, it simply returns the input value as it is. This function is useful when you want to use
 * the actual value of a `RefObject` or any other value in your code, rather than the reference itself.
 *
 * @template T - The value type that wrapped by the ref.
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
	return isRefObject(ref) ? ref.current : ref;
}

function useRefState<T>(initialValue: T) {
	const [state, setState] = useState<T>(initialValue);
	return [state, (state: T) => {
		setState(state);
		// return () => setState(null); // It seems that unmount effect is useless.
	}] as const;
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
 * @template TElement - HTML DOM element tag name of object.
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
	return useRefState(initialValue);
}

/**
 * Creates an array of references to an HTML DOM element.
 *
 * Useful when you want to use ref in a loop.
 *
 * @remarks You must provide the `TElement` generic type.
 *
 * @template TElement - A tag name (e.g. `"div"`) or a subclass (e.g. `HTMLDivElement`) of the HTML DOM element class.
 * @returns An array of references to an HTML DOM element.
 *
 * @example
 * ```tsx
 * const [refs, setRef] = useDomRefs<"p">();
 *
 * return array.map((item, index) => <p key={item} ref={setRef(index)}>{item}</p>);
 * ```
 */
export function useDomRefs<TElement extends keyof ElementTagNameMap | Element>() {
	type TElementOrNull = TagNameToElement<TElement> | null;
	const refs: RefObject<TElementOrNull[]> = useRef([]);
	const setRef = (index: number) => (el: TElementOrNull) => { refs.current[index] = el; };
	return [refs, setRef] as const;
}

/**
 * Creates a ref object that can get aria ID of a child element.
 * @returns A reference to a aria ID string.
 *
 * @example
 * ```tsx
 * // Parent component
 * const [ariaId, setAriaId, withAriaId] = useAriaIdRefState();
 * return <ChildComponent ariaIdRef={setAriaId} />
 *
 * // Child component
 * const ariaId = useId();
 * useImperativeHandleAriaId(ariaIdRef, ariaId);
 * ```
 */
export function useAriaIdRefState() {
	const [ariaId, setAriaId] = useRefState<string | undefined>(undefined);
	const withAriaId = (suffix: string) => ariaId && `${ariaId}-${suffix}`;
	return [ariaId, setAriaId, withAriaId] as const;
}

/**
 * Checks if `value` is object-like. A value is object-like if it's not `null`
 * and has a `typeof` result of "object".
 *
 * @category Lang
 * @param value - The value to check.
 * @returns Returns `true` if `value` is object-like, else `false`.
 * @example
 * ```javascript
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
 * ```
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
 * element; // Type: HTMLInputElement
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
// eslint-disable-next-line @typescript-eslint/no-unnecessary-type-parameters
export function asserts<T>(object: unknown): asserts object is T { }

/**
 * Checks if `value` is undefined, null, or NaN.
 *
 * @category Lang
 * @param object - The value to check.
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
export function makePrototypeKeysNonEnumerable(constructor: AnyConstructor | { prototype: Any }) {
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
 * @template TType - The type of the instances of the constructor.
 * @template TKey - The getter property key name of the type.
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
 * defineGetterInPrototype(MyClass, "value", function (this: MyClass) {
 *     return this._value;
 * });
 *
 * const instance = new MyClass(42);
 * console.log(instance.value); // Output: 42
 * ```
 */
export function defineGetterInPrototype<TType, TKey extends keyof TType>(constructor: { prototype: TType }, protoKey: TKey, getter: (this: TType) => TType[TKey]) {
	Object.defineProperty(constructor.prototype, protoKey, {
		get: getter,
		configurable: true,
		enumerable: false,
	});
}

/**
 * A no-operation function that returns undefined regardless of the arguments it receives.
 *
 * @returns undefined
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
		const setState = setStateInterceptor(originalSetState, (value: boolean) => {
			if (value)
				for (const otherSetState of originalSetStates)
					if (otherSetState !== originalSetState)
						otherSetState(false);
			return value;
		});
		result[i] = setState;
		const switch_ = switches[i] as Writable<typeof switches[number]>;
		if (Array.isArray(switch_))
			switch_[1] = setState;
	}
	return result;
}

/**
 * Like JavaScript `with` syntax, but safer.
 *
 * @template TObject - Source object type.
 * @template TReturn - The type returned.
 * @param object - A long name object.
 * @param getter - Rename that object to a short name, then get the result.
 * @returns The return value from getter.
 *
 * @example
 * ```typescript
 * // With
 * console.log(foo.bar.baz, b => b * (b + 1));
 *
 * // Without
 * console.log(foo.bar.baz * (foo.bar.baz + 1));
 * ```
 */
export function withObject<TObject, TReturn>(object: TObject, getter: (object: TObject) => TReturn) {
	return getter(object);
}

/**
 * Determine whether the current context is in the top layer of a component or a hook function.
 * If so, it means that the "use" hooks can be called now.
 *
 * @warn This function use an unstable API, which may become invalid after a React update in the future.
 * @warn Cannot use this function when enable React Compiler.
 *
 * @returns Can use hook here?
 */
export function canUseHook() {
	// React 18.0
	// const internal = React.__SECRET_INTERNALS_DO_NOT_USE_OR_YOU_WILL_BE_FIRED;
	// return !!internal.ReactCurrentDispatcher.current;

	// @ts-expect-error
	const internal = React.__CLIENT_INTERNALS_DO_NOT_USE_OR_WARN_USERS_THEY_CANNOT_UPGRADE;
	return !!internal.H;
}

/**
 * The ultimate method for determining the type of an element.
 * @param object - The element to be determined.
 * @param lowerCase - Should convert to all lowercase automatically? If false, preserve the case of the original name. Defaults to false.
 * @returns The type name of this element, which is case preserved by default, and custom types can also support type names instead of returning "Object".
 * @note This function does not support type guarding in TypeScript, it is recommended to use it only in the JavaScript level.
 */
// eslint-disable-next-line @typescript-eslint/no-wrapper-object-types
export function type(object: Object | undefined | null, lowerCase: boolean = false) {
	const type = object != null ? object.constructor.name : Object.prototype.toString.call(object).slice(8, -1);
	return lowerCase ? type.toLowerCase() : type;
}

/**
 * Creates a proxy for the given object that provides a fallback value when accessing
 * properties that do not exist on the object. If a missing property is accessed,
 * the value of the specified fallback key is returned instead.
 *
 * @template T - The type of the target object, which must be an object.
 * @param target - The target object to wrap with the fallback proxy.
 * @param fallbackKey - The key in the object whose value will be returned for missing properties.
 * @throws {TypeError} If the number of arguments is incorrect, if `target` is not an object, if `target` is empty,
 * or if `fallbackLocale` is not a valid property key type.
 * @throws {RangeError} If the fallback key does not exist in the object.
 * @returns A proxy of the original object that returns the fallback value for missing properties.
 */
export function fallbackWithKey<const T extends Record<PropertyKey, Any>>(target: T, fallbackKey: keyof T): T & Record<PropertyKey, T[keyof T]> {
	if (import.meta.env.DEV) {
		const errorMsgHeader = `Failed to execute "${fallbackWithKey.name}": `;
		if (arguments.length !== fallbackWithKey.length) throw new TypeError(`${errorMsgHeader}${fallbackWithKey.length} argument required, but only ${arguments.length} present.`);
		if (!isObject(target)) throw new TypeError(`${errorMsgHeader}parameter 1 is not of type "object".`);
		if (Object.values(target).length === 0) throw new TypeError(`${errorMsgHeader}parameter 1 target object is empty.`);
		if (!["string", "symbol", "number", "bigint"].includes(typeof fallbackKey)) throw new TypeError(`${errorMsgHeader}parameter 2 is not of type "string", "symbol", "number", or "bigint".`);
		if (!(fallbackKey in target)) throw new RangeError(`${errorMsgHeader}The fallback key "${String(fallbackKey)}" is not in the target object, it is likely that you spelled it incorrectly.`);
	}

	return new Proxy(target, {
		get(target, property) {
			if (property in target) return target[property];
			else return target[fallbackKey];
		},
	});
}

/**
 * Creates a proxy object that provides locale-based fallback behavior for accessing properties.
 *
 * When accessing a property by locale identifier, if the property does not exist on the target object,
 * the function attempts to find the best matching locale from the available keys using `Intl.LocaleMatcher.match`.
 * If no match is found, it falls back to the specified fallback locale.
 *
 * @template T - An object type whose keys are valid BCP 47 locale identifiers.
 * @param target - The object containing locale-specific values.
 * @param fallbackLocale - The locale key to use as a fallback if a requested locale is not found. Defaults to English (en).
 * @throws {TypeError} If the number of arguments is incorrect, if `target` is not an object, if `target` is empty,
 * or if `fallbackLocale` is not a string.
 * @throws {RangeError} If `fallbackLocale` is not a valid locale identifier.
 * @returns A proxy object that provides locale-based property access with fallback logic.
 */
export function fallbackWithLocale<const T extends Record<Intl.UnicodeBCP47LocaleIdentifier, Any>>(target: T, fallbackLocale: keyof T = "en"): T & Record<Intl.UnicodeBCP47LocaleIdentifier, T[keyof T]> {
	if (import.meta.env.DEV) {
		const errorMsgHeader = `Failed to execute "${fallbackWithLocale.name}": `;
		if (arguments.length !== fallbackWithLocale.length) throw new TypeError(`${errorMsgHeader}${fallbackWithLocale.length} argument required, but only ${arguments.length} present.`);
		if (!isObject(target)) throw new TypeError(`${errorMsgHeader}parameter 1 is not of type "object".`);
		if (Object.values(target).length === 0) throw new TypeError(`${errorMsgHeader}parameter 1 target object is empty.`);
		if (typeof fallbackLocale !== "string") throw new TypeError(`${errorMsgHeader}parameter 2 is not of type "string".`);
		if (!isValidLocale(fallbackLocale)) throw new RangeError(`${errorMsgHeader}The fallback locale "${String(fallbackLocale)}" is not a valid locale.`);
	}

	return new Proxy(target, {
		get(target, locale) {
			if (typeof locale === "symbol") return;
			if (locale in target) return target[locale];
			const bestMatchLocale = match([locale, fallbackLocale as string], Object.keys(target), undefined!) ?? Object.keys(target)[0];
			return target[bestMatchLocale];
		},
	});
}

/**
 * Supplements the target object with properties from one or more source objects.
 * Only properties that do not already exist or the property values are undefined in the target object are added.
 * This function does not overwrite existing properties in the target.
 *
 * This function has the same function signature as `Object.assign`, but the operation is the opposite.
 *
 * @param target - The object to be supplemented.
 * @param sources - One or more source objects whose properties will be added to the target.
 * @returns The supplemented target object.
 *
 * @example
 * ```typescript
 * const bar = { a: true, c: true };
 * {
 *     const foo = { a: false, b: false };
 *     Object.assign(foo, bar);
 *     console.log(foo); // { a: true, b: false, c: true };
 * }
 * {
 *     const foo = { a: false, b: false };
 *     Object.supplement(foo, bar);
 *     console.log(foo); // { a: false, b: false, c: true };
 * }
 * ```
 */
export const supplement: typeof Object["assign"] = (target: object, ...sources: object[]) => {
	for (const source of sources)
		for (const [key, value] of Object.entries(source))
			if ((target as AnyObject)[key] === undefined)
				(target as AnyObject)[key] = value;
	return target;
};

/**
 * Deep freeze the object recursively.
 * @template T - Object type.
 * @param obj - Object on which to lock the attributes.
 * @returns Deep freezed object.
 */
export function deepFreeze<T extends AnyObject>(obj: T): DeepReadonly<T> {
	Object.freeze(obj);
	if (obj === undefined) return obj;

	Object.getOwnPropertyNames(obj).forEach(prop => {
		const value = obj[prop];
		if (
			value !== null &&
			(typeof value === "object" || typeof value === "function") &&
			(() => {
				try {
					return !Object.isFrozen(value);
				} catch {
					return false;
				}
			})()
		)
			deepFreeze(value);
	});

	return obj;
}

/**
 * Creates an object with the specified keys, all assigned the same value.
 * @template TKey - The type of the object keys (must be string, number, or symbol type).
 * @template TValue - The type of the value to assign to all keys.
 * @param args - A spread of keys followed by the value to assign to each key.
 * @returns An object where each key from the input is mapped to the provided value.
 * @example
 * ```javascript
 * const obj = keysWithSameValue("a", "b", "c", 0); // Result: { a: 0, b: 0, c: 0 }
 * ```
 */
export function keysWithSameValue<const TKey extends PropertyKey, TValue>(...args: [...keys: TKey[], value: TValue]) {
	const value = args.pop() as TValue, keys = args as TKey[];
	return Object.fromEntries(keys.map(key => [key, value])) as {
		[key in TKey]: TValue;
	};
}

export function deconstructState<TState, TSelected = TState>(state: StateProperty<TState>, selector: (state: TState) => TSelected = state => state as unknown as TSelected) {
	return new Proxy(state, {
		get(target, property) {
			return useStateSelector(
				target,
				state => (selector(state) as AnyObject)[property],
				(value, state) => (selector(state) as AnyObject)[property] = value,
				{ immer: true },
			);
		},
	}) as {
		[key in keyof TSelected]: StatePropertyNonNull<TSelected[key]>
	};
}
