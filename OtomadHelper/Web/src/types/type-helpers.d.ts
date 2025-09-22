export { };

declare global {
	/**
	 * Filter subset interfaces with values of the specified type from an interface.
	 *
	 * @template TSource - Source interface.
	 * @template TCondition - Filter the type of value.
	 *
	 * @example
	 * ```typescript
	 * type Keyframe = FilterValueType<{ width: string; height: string; offset: number }, string>;
	 * //   ^?
	 * type Keyframe = { width: string; height: string };
	 * ```
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
	 *
	 * @example
	 * ```typescript
	 * type Test = Writable<{ readonly foo: string; readonly bar: string }>;
	 * //   ^?
	 * type Test = { foo: string; bar: string };
	 * ```
	 */
	type Writable<T> = { -readonly [Key in keyof T]: T[Key] };

	/**
	 * Deeply remove read-only modifiers.
	 *
	 * @template T - Source object.
	 *
	 * @example
	 * ```typescript
	 * type Test = DeepWritable<{ readonly foo: { readonly bar: { readonly baz: Readonly<Element> } } }>;
	 * //   ^?
	 * type Test = { foo: { bar: { baz: Element } } };
	 * ```
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
	 *
	 * @example
	 * ```typescript
	 * type Character = Override<{ name: "Li Lei", age: 12 }, { name: "Han Meimei" }>;
	 * //   ^?
	 * type Character = { name: "Han Meimei", age: 12 };
	 * ```
	 */
	type Override<TSource, TOverrider> = Omit<TSource, keyof TOverrider> & TOverrider;

	/**
	 * Remove the type of Ref.
	 *
	 * @template TRef - Maybe a Ref type.
	 *
	 * @example
	 * ```typescript
	 * type T0 = Unref<Ref<string>>;
	 * //   ^?
	 * type T0 = string;
	 *
	 * type T1 = Unref<string>;
	 * //   ^?
	 * type T1 = string;
	 * ```
	 */
	type Unref<TRef> = TRef extends MaybeRef<infer Value> ? Value : TRef;

	/**
	 * Deep capitalize all keys of an object.
	 *
	 * @template T - Source object.
	 *
	 * @example
	 * ```typescript
	 * type Test = CapitalizeObject<{ foo: { bar: { baz: Element } } }>;
	 * //   ^?
	 * type Test = { Foo: { Bar: { Baz: Element } } };
	 * ```
	 */
	type CapitalizeObject<T extends object> = {
		[Key in keyof T as Capitalize<Key>]:
			T[Key] extends (infer U)[] | undefined | null ? CapitalizeObject<U>[] :
			CapitalizeObject<T[Key]>;
	} & T;

	/**
	 * Similar to the keyword `keyof`, but only returns a union of value types rather than a union of key types.
	 *
	 * @template T - Source object.
	 *
	 * @example
	 * ```typescript
	 * type T0 = ValueOf<number[]>;
	 * //   ^?
	 * type T0 = number;
	 *
	 * type T1 = ValueOf<{ foo: string; bar: number }>;
	 * //   ^?
	 * type T1 = string | number;
	 * ```
	 */
	type ValueOf<T extends object> =
		T extends ArrayLike<infer Value> ? Value :
		T extends Iterable<infer Value> ? Value :
		T[keyof T];

	/**
	 * Deep read-only object.
	 *
	 * @template T - Source object.
	 *
	 * @example
	 * ```typescript
	 * type Test = DeepReadonly<{ foo: { bar: { baz: Element } } }>;
	 * //   ^?
	 * type Test = { readonly foo: { readonly bar: { readonly baz: Readonly<Element> } } };
	 * ```
	 */
	type DeepReadonly<T> = Readonly<{
		[Key in keyof T]: DeepReadonly<T[Key]>;
	}>;

	/**
	 * Maybe the type object of the Ref packaging or its type itself.
	 *
	 * @template TRef - Maybe a Ref type.
	 *
	 * @example
	 * ```typescript
	 * type Test = MaybeRef<string>;
	 * //   ^?
	 * type Test = string | RefObject<string>;
	 * ```
	 */
	type MaybeRef<TRef> = RefObject<TRef> | TRef;

	/**
	 * Reference to HTML DOM element.
	 *
	 * @template TElement - HTML DOM element.
	 *
	 * @example
	 * ```typescript
	 * type El0 = DomRef<"div">;
	 * //   ^?
	 * type El0 = RefObject<HTMLDivElement | null>;
	 *
	 * type El1 = DomRef<HTMLDivElement>;
	 * //   ^?
	 * type El1 = RefObject<HTMLDivElement | null>;
	 */
	type DomRef<TElement extends keyof ElementTagNameMap | Element> = RefObject<(TElement extends string ? TagNameToElement<TElement> : TElement) | null>;

	/**
	 * Construct a function type with specified parameter types and return type.
	 *
	 * @template TArgs - A tuple of the function parameters.
	 * @template TRet - The return value of a function, leaving blank indicates no return value `void`.
	 *
	 * @example
	 * ```typescript
	 * type Function = Func<[arg1: string, arg2: boolean], number>;
	 * //   ^?
	 * type Function = (arg1: string, arg2: boolean) => number;
	 *
	 * type Consumer = Func<[arg1: string, arg2: boolean]>;
	 * //   ^?
	 * type Consumer = (arg1: string, arg2: boolean) => void;
	 *
	 * type Runnable = Func;
	 * //   ^?
	 * type Runnable = () => void;
	 * ```
	 */
	type Func<
		TArgs extends Iterable<any> | ArrayLike<any> = [],
		TRet = void,
	> = (...args: TArgs) => TRet;
	type a = Func<[f: string]>;

	/**
	 * Make all the parameters optional in the function.
	 *
	 * @template TFunc - Source function.
	 *
	 * @example
	 * ```typescript
	 * type Test = PartialArgsFunc<(arg1: string, arg2: boolean) => number>;
	 * //   ^?
	 * type Test = (arg1?: string, arg2?: boolean) => number;
	 * ```
	 */
	type PartialArgsFunc<TFunc extends AnyFunction> = Func<Partial<Parameters<TFunc>>, ReturnType<TFunc>>;

	/**
	 * Maybe the type object of Promise or its type itself.
	 *
	 * @template T - Maybe a Promise type.
	 *
	 * @example
	 * ```typescript
	 * type Test = MaybePromise<string>;
	 * //   ^?
	 * type Test = string | Promise<string>;
	 * ```
	 */
	type MaybePromise<T> = T | Promise<T>;

	/**
	 * If the given type is already a Promise type, it returns itself,
	 * otherwise it returns this type wrapped by a Promise.
	 *
	 * There is no need to nest Promise types much more times.
	 *
	 * @template T - Maybe a Promise type.
	 *
	 * @example
	 * ```typescript
	 * type T0 = PromiseOnce<string>;
	 * //   ^?
	 * type T0 = Promise<string>;
	 *
	 * type T1 = PromiseOnce<Promise<string>>;
	 * //   ^?
	 * type T1 = Promise<string>;
	 *
	 * type T2 = Promise<Promise<string>>;
	 * //   ^?
	 * type T2 = Promise<Promise<string>>;
	 * ```
	 */
	type PromiseOnce<T> = T extends Promise<any> ? T : Promise<T>;

	/**
	 * Make all items in Array or Object T readonly.
	 *
	 * @template T - Source array or object.
	 *
	 * @example
	 * ```typescript
	 * type T0 = ReadonlyArrayItems<Element[]>;
	 * //   ^?
	 * type T0 = Readonly<Element>[];
	 *
	 * type T1 = ReadonlyArrayItems<[Element, Node]>;
	 * //   ^?
	 * type T1 = [Readonly<Element>, Readonly<Node>];
	 * ```
	 */
	type ReadonlyArrayItems<T> = {
		[P in keyof T]: Readonly<T[P]>;
	};

	/**
	 * Converts a synchronous function into an asynchronous one by wrapping its return value in a Promise.
	 * If the given function already returns a Promise, it will be returned as is.
	 *
	 * @template TFunction - The type of the source function to be converted. It should be a synchronous function type.
	 *
	 * @example
	 * ```typescript
	 * type Asynchronous = MakeFunctionAsync<() => string>;
	 * //   ^?
	 * type Asynchronous = () => Promise<string>;
	 * ```
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
	 *
	 * @example
	 * ```typescript
	 * type Asynchronous = MakeFunctionAsync<{ foo: () => string }>;
	 * //   ^?
	 * type Asynchronous = { foo: () => Promise<string> };
	 * ```
	 */
	type MakeFunctionsAsync<TFunctions> = {
		[functionName in keyof TFunctions]: MakeFunctionAsync<TFunctions[functionName]>;
	};

	/**
	 * Remove properties with value type of `never` from a object.
	 *
	 * @template T - Source object.
	 *
	 * @example
	 * ```typescript
	 * type Test = OmitNevers<{ foo: string; bar: never }>;
	 * //   ^?
	 * type Test = { foo: string };
	 * ```
	 */
	type OmitNevers<T> = Pick<T, {
		[K in keyof T]: T[K] extends never ? never : K;
	}[keyof T]>;

	/**
	 * Remove private properties which conventionally have keys beginning with underscores from a object.
	 *
	 * @template T - Source object.
	 *
	 * @example
	 * ```typescript
	 * type MiddleAgedPerson = OmitConventionalPrivates<{ name: string; _age: number }>;
	 * //   ^?
	 * type MiddleAgedPerson = { name: string };
	 * ```
	 */
	type OmitConventionalPrivates<T> = OmitNevers<{
		[key in keyof T]: key extends `_${string}` ? never : T[key];
	}>;

	/**
	 * Make all properties in T required and exclude null and undefined from them.
	 * @note Built-in utility type `Required` is powerless for properties whose values are explicitly in a union with
	 * `undefined` without specifying `?:`.
	 *
	 * @template T - Source object.
	 *
	 * @example
	 * ```typescript
	 * type T0 = Required<{ foo?: string | undefined; bar: string | undefined }>;
	 * //   ^?
	 * type T0 = { foo: string; bar: string | undefined };
	 *
	 * type T1 = RequiredNonNullable<{ foo?: string | undefined; bar: string | undefined }>;
	 * //   ^?
	 * type T1 = { foo: string; bar: string };
	 * ```
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
	 *
	 * @example
	 * ```typescript
	 * type Test = WithWrapperType<string | number | bigint>;
	 * //   ^?
	 * type Test = string | number | bigint | String | Number | BigInt;
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
	 * type Test = ObtainLiterals<"a" | "b" | string & {}>;
	 * //   ^?
	 * type Test = "a" | "b";
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
	 * //   ^?
	 * type Result = { a: number; b: string; c?: boolean; d: bigint };
	 * ```
	 */
	type RequiredWith<TSource extends object, TRequiredProperties extends keyof TSource> = Override<TSource, Required<Pick<TSource, TRequiredProperties>>>;

	/**
	 * Makes all properties of a given object nullable.
	 * @remarks It is similar to `Partial` but replaces `undefined` with `null`.
	 *
	 * @template T - The object whose properties will be made nullable.
	 *
	 * @example
	 * ```typescript
	 * type User = { name: string; age: number };
	 * type NullableUser = Nullable<User>;
	 * //   ^?
	 * type NullableUser = { name: string | null; age: number | null };
	 * ```
	 */
	type Nullable<T> = {
		[key in keyof T]: T[key] | null;
	};

	/**
	 * Determines whether the given object type `TObject` contains the specified key `TKey`.
	 *
	 * @template TObject - The object type to check for the key.
	 * @template TKey - The property key to check for existence in `TObject`.
	 * @returns Does `TObject` have a property of key `TKey`?
	 *
	 * @example
	 * ```typescript
	 * type Example = HasKey<{ foo: number }, "foo">; // true
	 * type Example2 = HasKey<{ bar: string }, "baz">; // false
	 * ```
	 */
	type HasKey<TObject, TKey extends PropertyKey> = TObject extends Record<TKey, any> ? true : false;

	/**
	 * Replaces the first occurrence of a substring pattern within a string type with a replacement string type.
	 *
	 * @template TSource - The source string type to perform replacements on.
	 * @template TPattern - The substring pattern to search for and replace.
	 * @template TReplacement - The string type to replace the first occurrence of the pattern.
	 *
	 * @example
	 * ```typescript
	 * type Result = Replace<"foo_bar_bar", "bar", "baz">; // "foo_baz_bar"
	 * ```
	 */
	type Replace<TSource extends string, TPattern extends string, TReplacement extends string> =
		TSource extends `${infer Left}${TPattern}${infer Right}` ? `${Left}${TReplacement}${Right}` : TSource;

	/**
	 * Recursively replaces all occurrences of a substring pattern within a string type with a replacement string type.
	 *
	 * @template TSource - The source string type to perform replacements on.
	 * @template TPattern - The substring pattern to search for and replace.
	 * @template TReplacement - The string type to replace each occurrence of the pattern.
	 *
	 * @example
	 * ```typescript
	 * type Result = ReplaceAll<"foo_bar_bar", "bar", "baz">; // "foo_baz_baz"
	 * ```
	 */
	type ReplaceAll<TSource extends string, TPattern extends string, TReplacement extends string> =
		TSource extends `${infer Left}${TPattern}${infer Right}` ? `${Left}${TReplacement}${ReplaceAll<Right, TPattern, TReplacement>}` : TSource;
}
