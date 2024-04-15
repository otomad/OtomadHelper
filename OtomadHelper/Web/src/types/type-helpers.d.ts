export { };

declare global {
	/**
	 * Filter subset interfaces with values of the specified type from an interface.
	 * @template Source - Source interface.
	 * @template Condition - Filter the type of value.
	 */
	type FilterValueType<Source, Condition> = Pick<
		Source,
		{
			[K in keyof Source]: Source[K] extends Condition ? K : never
		}[keyof Source]
	>;

	/**
	 * Remove read-only modifiers.
	 * @template T - Source object.
	 */
	type Writeable<T> = { -readonly [P in keyof T]: T[P] };

	/**
	 * Deeply remove read-only modifiers.
	 * @template T - Source object.
	 */
	type DeepWriteable<T> = { -readonly [P in keyof T]: DeepWriteable<T[P]> };

	/**
	 * Non-null type. Similar to `!`.
	 *
	 * The functionality implementation of the built-in type helper `NonNullable` is inconsistent,
	 * and the results may be slightly different.
	 * @template T - Possible empty types.
	 */
	type NonNull<T> = Exclude<T, undefined | null | void>;

	/**
	 * Similar to `NonNull`, but also removes other falsy values such as false, "", ±0, ±0n.
	 * @template T - Possible falsy types.
	 */
	type NonFalsy<T> = Exclude<T, undefined | null | false | "" | 0 | 0n>;

	/**
	 * Override the type of partial field for a certain object.
	 * @template T - Source object.
	 * @template U - Overridden fields and their types.
	 */
	type Override<T, U> = Omit<T, keyof U> & U;

	/**
	 * Get the Props of the component.
	 * @template T - Vue component.
	 */
	type ComponentProps<T> = Omit<InstanceType<T>["$props"], keyof VNodeProps>;

	/**
	 * Remove the type of Ref.
	 * @template R - May be of Ref type.
	 */
	type Unref<R> = R extends MaybeRefOrGetter<infer U> ? U : T;

	/**
	 * Remove the index signature of T and only use known attribute key names.
	 * For example, removing `[x: string]` from an enumeration type.
	 * @template T - Source object.
	 */
	type KnownKeys<T> = keyof {
		[K in keyof T]:
		string extends K ? never :
		number extends K ? never :
		symbol extends K ? never :
		K
	};

	/**
	 * Capitalize all keys of an object.
	 * @template T - Source object.
	 */
	type CapitalizeObject<T extends object> = {
		[key in keyof T as Capitalize<key>]:
		T[key] extends (infer U)[] | undefined | null ? CapitalizeObject<U>[] :
		CapitalizeObject<T[key]>;
	} & T;

	/**
	 * Similar to the keyword 'keyof', it only returns a set of value types rather than a set of key types.
	 * @template T - Source object.
	 */
	type ValueOf<T extends object> =
		T extends ArrayLike<infer U> ? U :
		T extends Iterable<infer U> ? U :
		T[keyof T];

	/**
	 * Deep read-only object.
	 * @template T - Source object.
	 */
	type DeepReadonly<T> = Readonly<{
		[key in keyof T]: DeepReadonly<T[key]>;
	}>;

	/**
	 * Maybe the type object of the Ref packaging or its type itself.
	 */
	type MaybeRef<T> = RefObject<T> | MutableRefObject<T> | T;

	/**
	 * Reference to HTML DOM element.
	 */
	type DomRef<E extends Element> = MutableRefObject<E | null>;

	/**
	 * Get the type of a function based on the specified parameters and return value.
	 * @template TArgs - The tuple of the function parameters.
	 * @template TRet - The return value of a function, leaving blank indicates no return value `void`.
	 */
	type Func<TArgs extends Iterable<any> | ArrayLike<any> = [], TRet = void> = (...args: TArgs) => TRet;

	/**
	 * Make all the parameters nullable in the function.
	 * @template TFunc - Source function.
	 */
	type PartialArgsFunc<TFunc extends AnyFunction> = Func<Partial<Parameters<TFunc>>, ReturnType<TFunc>>;
}
