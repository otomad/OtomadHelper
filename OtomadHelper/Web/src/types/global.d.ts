export { };

declare global {
	/**
	 * Booleans and their string forms.
	 */
	type Booleanish = boolean | "true" | "false";
	/**
	 * Numbers and their string forms.
	 */
	type Numberish = number | string;
	/**
	 * Any type that can be directly read by humans, including String, Number, and BigInt.
	 */
	type Readable = string | number | bigint;
	/**
	 * Any type that can be used as a JavaScript object key, including String, Number, Symbol.
	 */
	type ObjectKey = string | number | symbol;
	/**
	 * I heard you want to use `any` without any warnings?
	 */
	type Any = Parameters<typeof alert>[0];
	/**
	 * Any object.
	 */
	type AnyObject = Record<ObjectKey, any>;
	/**
	 * Anu function.
	 */
	type AnyFunction = (...args: any[]) => any;
	/**
	 * Any constructor of class.
	 */
	type AnyConstructor = new (...args: any[]) => any;
	/**
	 * Specify the value type of Record with any available object key types.
	 * @template T - The type of value.
	 */
	type RecordValue<T> = Record<ObjectKey, T>;
	/**
	 * A tuple representing a two-dimensional point.
	 */
	type TwoD = [number, number];
	/**
	 * A triple representing a three-dimensional point.
	 */
	type ThreeD = [number, number, number];
	/**
	 * A quadruple representing a four-dimensional point.
	 */
	type FourD = [number, number, number, number];
	/**
	 * A specified type or an parameterless function returns the specified type.
	 * @template T - The specified type or an parameterless function returns the specified type.
	 */
	type TypeOrReturnToType<T> = T | (() => T);
	/**
	 * When called, requests that the Node.js event loop _not_ exit so long as the`Timeout` is active. Calling `timeout.ref()` multiple times will have no effect.
	 *
	 * By default, all `Timeout` objects are "ref'ed", making it normally unnecessary
	 * to call `timeout.ref()` unless `timeout.unref()` had been called previously.
	 *
	 * @return a reference to `timeout`
	 */
	interface Timeout extends NodeJS.Timeout { }
	/**
	 * Built in timer object.
	 */
	interface Timer extends NodeJS.Timer { }
	interface Window {
		/**
		 * The ActiveX object that is only available in Internet Explorer, higher versions of browsers will return undefined.
		 */
		ActiveXObject: undefined;
	}
	interface Document {
		/**
		 * Text selection objects available in Internet Explorer.
		 *
		 * [MDN Reference](https://developer.mozilla.org/docs/Web/API/Window/getSelection)
		 */
		selection: Selection | null;
	}
	interface ScreenOrientation {
		/**
		 * The `lock()` property of the `ScreenOrientation` interface locks the orientation of the containing document to the specified orientation.
		 * @note I don't know why VSCode removes this property definition.
		 * @param type - An orientation lock type.
		 * @returns A Promise that resolves after locking succeeds.
		 */
		lock(type: "any" | "natural" | "landscape" | "portrait" | OrientationType): Promise<void>;
	}
}
