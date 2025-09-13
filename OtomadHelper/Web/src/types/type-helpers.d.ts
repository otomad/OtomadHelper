export { };

declare global {
	/**
	 * Filter subset interfaces with values of the specified type from an interface.
	 *
	 * @template TSource - Source interface.
	 * @template TCondition - Filter the type of value.
	 */
	type FilterValueType<TSource, TCondition> = Pick<
		TSource,
		{
			[Key in keyof TSource]: TSource[Key] extends TCondition ? Key : never;
		}[keyof TSource]
	>;

	/**
	 * Remove read-only modifiers.
	 *
	 * @template T - Source object.
	 */
	type Writable<T> = { -readonly [Key in keyof T]: T[Key] };

	/**
	 * Deeply remove read-only modifiers.
	 *
	 * @template T - Source object.
	 */
	type DeepWritable<T> = { -readonly [Key in keyof T]: DeepWritable<T[Key]> };

	/**
	 * Non-null type. Similar to `!`.
	 *
	 * The functionality implementation of the built-in type helper `NonNullable` is inconsistent,
	 * and the results may be slightly different.
	 *
	 * @template T - Possible empty types.
	 */
	type NonNull<T> = Exclude<T, undefined | null | void>;

	/**
	 * Similar to `NonNull`, but also removes other falsy values such as false, "", ±0, ±0n.
	 *
	 * @template T - Possible falsy types.
	 */
	type NonFalsy<T> = Exclude<T, undefined | null | false | "" | 0 | 0n>;

	/**
	 * Override the type of partial field for a certain object.
	 *
	 * @template TSource - Source object.
	 * @template TOverrider - Overridden fields and their types.
	 */
	type Override<TSource, TOverrider> = Omit<TSource, keyof TOverrider> & TOverrider;

	/**
	 * Get the Props of the component.
	 *
	 * @template TVueComponent - Vue component.
	 */
	type ComponentProps<TVueComponent> = Omit<InstanceType<TVueComponent>["$props"], keyof VNodeProps>;

	/**
	 * Remove the type of Ref.
	 *
	 * @template TRef - Maybe a Ref type.
	 */
	type Unref<TRef> = TRef extends MaybeRef<infer Value> ? Value : TRef;

	/**
	 * Remove the index signature of T and only use known attribute key names.
	 *
	 * For example, removing `[x: string]` from an enumeration type.
	 *
	 * @template T - Source object.
	 */
	type KnownKeys<T> = keyof {
		[Key in keyof T]:
			string extends Key ? never :
			number extends Key ? never :
			symbol extends Key ? never :
			Key;
	};

	/**
	 * Capitalize all keys of an object.
	 *
	 * @template T - Source object.
	 */
	type CapitalizeObject<T extends object> = {
		[Key in keyof T as Capitalize<Key>]:
			T[Key] extends (infer U)[] | undefined | null ? CapitalizeObject<U>[] :
			CapitalizeObject<T[Key]>;
	} & T;

	/**
	 * Similar to the keyword 'keyof', it only returns a set of value types rather than a set of key types.
	 *
	 * @template T - Source object.
	 */
	type ValueOf<T extends object> =
		T extends ArrayLike<infer Value> ? Value :
		T extends Iterable<infer Value> ? Value :
		T[keyof T];

	/**
	 * Deep read-only object.
	 *
	 * @template T - Source object.
	 */
	type DeepReadonly<T> = Readonly<{
		[Key in keyof T]: DeepReadonly<T[Key]>;
	}>;

	/**
	 * Maybe the type object of the Ref packaging or its type itself.
	 *
	 * @template TRef - Maybe a Ref type.
	 */
	type MaybeRef<TRef> = RefObject<TRef> | RefObject<TRef> | TRef;

	/**
	 * Reference to HTML DOM element.
	 *
	 * @template TElement - HTML DOM element.
	 */
	type DomRef<TElement extends keyof ElementTagNameMap | Element> = RefObject<(TElement extends string ? TagNameToElement<TElement> : TElement) | null>;

	/**
	 * Get the type of a function based on the specified parameters and return value.
	 *
	 * @template TArgs - The tuple of the function parameters.
	 * @template TRet - The return value of a function, leaving blank indicates no return value `void`.
	 */
	type Func<
		TArgs extends Iterable<any> | ArrayLike<any> = [],
		TRet = void,
	> = (...args: TArgs) => TRet;

	/**
	 * Make all the parameters nullable in the function.
	 *
	 * @template TFunc - Source function.
	 */
	type PartialArgsFunc<TFunc extends AnyFunction> = Func<Partial<Parameters<TFunc>>, ReturnType<TFunc>>;

	/**
	 * Maybe the type object of Promise or its type itself.
	 *
	 * @template T - Maybe a Promise type.
	 */
	type MaybePromise<T> = T | Promise<T>;

	/**
	 * If the given type is already a Promise type, it returns itself,
	 * otherwise it returns this type wrapped by a Promise.
	 *
	 * There is no need for nesting Promise types much more times.
	 *
	 * @template T - Maybe a Promise type.
	 */
	type PromiseOnce<T> = T extends Promise<any> ? T : Promise<T>;

	/**
	 * Make all items in Array or Object T readonly.
	 *
	 * @template T - Source array or object.
	 */
	type ReadonlyArrayItems<T> = {
		[P in keyof T]: Readonly<T[P]>;
	};

	/**
	 * Converts a synchronous function into an asynchronous one by wrapping its return value in a Promise.
	 * If the given function already returns a Promise, it will be returned as is.
	 *
	 * @template TFunction - The type of the source function to be converted. It should be a synchronous function type.
	 */
	type MakeFunctionAsync<TFunction extends Function> = (...args: ReadonlyArrayItems<Parameters<TFunction>>) =>
		PromiseOnce<ReturnType<TFunction>>;

	/**
	 * Converts a set of synchronous functions into asynchronous ones by wrapping their return values in Promises.
	 * If a given function already returns a Promise, it will be returned as is.
	 *
	 * @template TFunctions - The type of the source object containing the functions to be converted.
	 * Each function in the object should be a synchronous function type.
	 *
	 * @returns An object with the same keys as the input object, but with each function value replaced by
	 * an asynchronous version of the function. The asynchronous version of a function will return a Promise
	 * that resolves with the same value as the original function.
	 */
	type MakeFunctionsAsync<TFunctions> = {
		[functionName in keyof TFunctions]: MakeFunctionAsync<TFunctions[functionName]>;
	};

	/**
	 * Remove properties with value type of `never` from a object.
	 *
	 * @template T - Source object.
	 */
	type OmitNevers<T> = Pick<T, {
		[K in keyof T]: T[K] extends never ? never : K;
	}[keyof T]>;

	/**
	 * Remove private properties which conventionally have keys beginning with underscores from a object.
	 *
	 * @template T - Source object.
	 */
	type OmitPrivates<T> = OmitNevers<{
		[key in keyof T]: key extends `_${string}` ? never : T[key];
	}>;

	/**
	 * Make all properties in T required and exclude null and undefined from them.
	 *
	 * @template T - Source object.
	 */
	type RequiredNonNullable<T> = {
		[P in keyof T]-?: T[P] & {};
	};

	/**
	 * Assign all custom properties to a specific default type except for known properties.
	 *
	 * @remarks
	 * If we want to declare a type where the value of `foo` is `() => void`, the value of `bar` is optional `number`,
	 * and all any other values are the types of string.
	 *
	 * We can easily write the following definition:
	 * ```typescript
	 * type Foo = {
	 *     foo: () => void;
	 *     bar?: number;
	 *     [x: string]: string;
	 * };
	 * ```
	 * However, this would result in an error. It says that the type `() => void` of `foo` and the type `number` of `bar`
	 * cannot be assigned to `string`.
	 *
	 * This requires us to change it to:
	 * ```typescript
	 * type Foo = {
	 *     foo: () => void;
	 *     bar?: number;
	 *     [x: string]: (() => void) | number | string;
	 * };
	 * ```
	 * This way, there will be no errors, but the type of any other properties has also been loosed from `string` to
	 * `(() => void) | number | string`, which is not what we want.
	 *
	 * Now we can use this type helper to solve this problem.
	 * ```typescript
	 * type Foo<T> = WithOtherProperties<{
	 *     foo: () => void;
	 *     bar?: number;
	 * }, string, T>;
	 * ```
	 * Applied to function or component parameters:
	 * ```typescript
	 * declare function MyComponent<T>({ foo, bar, ...otherProps }: WithOtherProperties<{
	 *     foo: () => void;
	 *     bar?: number;
	 * }, string, T>): React.JSX.Element;
	 * ```
	 *
	 * @template KnownProps - All known fixed properties types.
	 * @template DefaultValue - All any other properties values type.
	 * @template TProps - Pass the generic parameter of the function.
	 */
	type WithOtherProperties<KnownProps, DefaultValue, TProps> = KnownProps & {
		[key in keyof TProps]: key extends keyof KnownProps ? KnownProps[key] : DefaultValue;
	};

	/* eslint-disable @typescript-eslint/no-wrapper-object-types */
	/**
	 * Union the primitive value types with their wrapper object types.
	 * @template T - Primitive value types.
	 * @example
	 * ```typescript
	 * type foo = WithWrapperType<string | number | bigint>; // Expect type: string | number | bigint | String | Number | BigInt;
	 * ```
	 */
	type WithWrapperType<T> = T |
		(T extends object ? Object : never) |
		(T extends string ? String : never) |
		(T extends number ? Number : never) |
		(T extends boolean ? Boolean : never) |
		(T extends symbol ? Symbol : never) |
		(T extends bigint ? BigInt : never);
	/* eslint-enable @typescript-eslint/no-wrapper-object-types */

	/**
	 * Remove a wider type (`string | {}`) from an literal union (literal string union).
	 * @see https://stackoverflow.com/a/75080234/19553213
	 * @example
	 * ```typescript
	 * type Test = ObtainLiterals<"a" | "b" | string & {}>; // "a" | "b"
	 * ```
	 */
	type ObtainLiterals<T> = T extends infer R ? (R extends string ? (string extends R ? never : R) : never) : never;

	/**
	 * Creates a new type based on `TSource` where the specified properties `TRequiredProperties`
	 * are made required, while the rest of the properties retain their original optionality.
	 *
	 * @template TSource - Source object type.
	 * @template TRequiredProperties - The keys of `TSource` that should be required in the resulting type.
	 *
	 * @example
	 * ```typescript
	 * type Example = { a?: number; b?: string; c?: boolean; d: bigint };
	 * type Result = RequiredWith<Example, "a" | "b">;
	 * // Result is: { a: number; b: string; c?: boolean; d: bigint }
	 * ```
	 */
	type RequiredWith<TSource extends object, TRequiredProperties extends keyof TSource> = Override<TSource, Required<Pick<TSource, TRequiredProperties>>>;

	/**
	 * Makes all properties of a given object nullable.
	 *
	 * @template T - The object whose properties will be made nullable.
	 * @example
	 * ```typescript
	 * type User = { name: string; age: number };
	 * type NullableUser = Nullable<User>; // { name: string | null; age: number | null }
	 * ```
	 */
	type Nullable<T> = {
		[key in keyof T]: T[key] | null;
	};
}
