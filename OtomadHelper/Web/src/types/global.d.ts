import type { MenuOutput } from "utils/context-menu";

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
	 * I heard you want to use `any` without any warnings?
	 */
	type Any = Parameters<typeof alert>[0];
	/**
	 * Any object.
	 */
	type AnyObject = Record<PropertyKey, any>;
	/**
	 * Any function.
	 */
	type AnyFunction = (...args: any[]) => any;
	/**
	 * Any constructor of class.
	 */
	type AnyConstructor = new (...args: any[]) => any;
	/**
	 * Specify the value type of Record with any available object key types.
	 * @template TValue - The type of value.
	 */
	type RecordValue<TValue> = Record<PropertyKey, TValue>;
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
		 * Check if it is running in the WebView environment.
		 */
		isWebView: boolean;
		/**
		 * A global variable indicates the current context menu and its items.
		 */
		contextMenu?: MenuOutput;
	}
}
