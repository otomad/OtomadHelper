declare interface String {
	/**
	 * 在字符串查找指定字符的数目。
	 *
	 * @param chars - 要查找的字符们。
	 * @returns 查找指定字符的数目。
	 */
	countChar(...chars: string[]): number;

	/**
	 * 颠倒字符串的顺序。
	 *
	 * 使用 `Array.from()` 可以避免 Unicode BMP 外的其它字符不能正常翻转的问题。
	 *
	 * @returns 颠倒后的字符串。
	 */
	reverse(): string;

	/**
	 * 字符串转布尔型。
	 *
	 * 用以解决类似 `"false" == false` 为 false 这样的问题。
	 *
	 * @returns 布尔型。
	 */
	toBoolean(): boolean;

	/**
	 * 字符串转类似 C 的字符数组，即数组的每一项对应字符串的每一个字符。
	 * @returns 包含源字符串中各字符的字符串形式的数组。
	 */
	toArray(): string[];

	/**
	 * 字符串两两字符间插一个字符。
	 * @param sep - 分隔符。默认为 `,`。
	 * @returns 处理后的字符串。
	 */
	inTwo(sep?: string): string;

	/**
	 * 字符串是否为参数列表中的某个字符串。
	 * @param list - 要测试是否包含的字符串数组。
	 * @returns 已包含。
	 */
	in<const T extends string>(...list: (T | undefined | null)[]): this is T;
}
