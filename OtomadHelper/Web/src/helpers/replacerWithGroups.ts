type CompatibleReplacerCallback = Parameters<string["replace"]>[1];
type ReasonableReplacerCallback<TGroupNames extends string | undefined> = (
	matchedSubstring: string,
	namedGroups: GetNamedGroups<TGroupNames>,
	indexedGroups: string[],
	offset: number,
	wholeString: string,
) => string;
type GetNamedGroups<TGroupNames extends string | undefined> = Record<NonNull<TGroupNames>, string>;

/**
 * Because JavaScript needs damn compatibility, the very useful named grouping parameter is the last parameter of
 * the replacer callback function in `String.prototype.replace` / `String.prototype.replaceAll`.
 * Since the rest parameter feature must be at the end of the parameter list and not at the beginning or center,
 * it is very troublesome to obtain the named grouping parameter, and you also need to skip the damn index grouping
 * that is now meaningless from the parameters.
 *
 * @param replacer - New replacer callback. The arguments are:
 * ```typescript
 * [
 * 	matchedSubstring: string,
 * 	namedGroups: Record<TGroupNames, string>,
 * 	indexedGroups: string[],
 * 	offset: number,
 * 	wholeString: string,
 * ]
 * ```
 * @returns Compatible replacer callback.
 */
export default function replacerWithGroups<TGroupNames extends string | undefined = undefined>(replacer: ReasonableReplacerCallback<TGroupNames>): CompatibleReplacerCallback {
	return ((matchedSubstring, ...args) => {
		type NamedGroups = GetNamedGroups<TGroupNames>;
		let namedGroups: NamedGroups, wholeString: string;
		const wholeStringOrNamedGroups: string | NamedGroups = args.pop();
		if (typeof wholeStringOrNamedGroups === "object") { // Cannot use `isObject` function when it's imported outside the client application (such as plug-ins).
			namedGroups = wholeStringOrNamedGroups;
			wholeString = args.pop();
		} else
			wholeString = wholeStringOrNamedGroups;
		const offset: number = args.pop();
		return replacer(matchedSubstring, namedGroups!, args, offset, wholeString);
		// `namedGroups` maybe undefined here, however if it is undefined, you won't possibly use this function.
	}) as CompatibleReplacerCallback;
}
