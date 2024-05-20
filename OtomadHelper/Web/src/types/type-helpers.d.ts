export { };

declare global {
	/**
	 * Filter subset interfaces with values of the specified type from an interface.
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
	 * @template T - Source object.
	 */
	type Writeable<T> = { -readonly [Key in keyof T]: T[Key] };

	/**
	 * Deeply remove read-only modifiers.
	 * @template T - Source object.
	 */
	type DeepWriteable<T> = { -readonly [Key in keyof T]: DeepWriteable<T[Key]> };

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
	 * @template TSource - Source object.
	 * @template TOverrider - Overridden fields and their types.
	 */
	type Override<TSource, TOverrider> = Omit<TSource, keyof TOverrider> & TOverrider;

	/**
	 * Get the Props of the component.
	 * @template TVueComponent - Vue component.
	 */
	type ComponentProps<TVueComponent> = Omit<InstanceType<TVueComponent>["$props"], keyof VNodeProps>;

	/**
	 * Remove the type of Ref.
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
	 * @template T - Source object.
	 */
	type CapitalizeObject<T extends object> = {
		[Key in keyof T as Capitalize<Key>]:
		T[Key] extends (infer U)[] | undefined | null ? CapitalizeObject<U>[] :
		CapitalizeObject<T[Key]>;
	} & T;

	/**
	 * Similar to the keyword 'keyof', it only returns a set of value types rather than a set of key types.
	 * @template T - Source object.
	 */
	type ValueOf<T extends object> =
		T extends ArrayLike<infer Value> ? Value :
		T extends Iterable<infer Value> ? Value :
		T[keyof T];

	/**
	 * Deep read-only object.
	 * @template T - Source object.
	 */
	type DeepReadonly<T> = Readonly<{
		[Key in keyof T]: DeepReadonly<T[Key]>;
	}>;

	/**
	 * Maybe the type object of the Ref packaging or its type itself.
	 * @template TRef - Maybe a Ref type.
	 */
	type MaybeRef<TRef> = RefObject<TRef> | MutableRefObject<TRef> | TRef;

	/**
	 * Reference to HTML DOM element.
	 * @template TElement - HTML DOM element.
	 */
	type DomRef<TElement extends Element> = MutableRefObject<TElement | null>;

	/**
	 * Get the type of a function based on the specified parameters and return value.
	 * @template TArgs - The tuple of the function parameters.
	 * @template TRet - The return value of a function, leaving blank indicates no return value `void`.
	 */
	type Func<
		TArgs extends Iterable<any> | ArrayLike<any> = [],
		TRet = void,
	> = (...args: TArgs) => TRet;

	/**
	 * Make all the parameters nullable in the function.
	 * @template TFunc - Source function.
	 */
	type PartialArgsFunc<TFunc extends AnyFunction> = Func<Partial<Parameters<TFunc>>, ReturnType<TFunc>>;
}
