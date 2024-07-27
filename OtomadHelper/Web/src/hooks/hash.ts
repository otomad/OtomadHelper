// cSpell:ignore uniqueid

/**
 * Due to React build-in hook `useId` will return a string that contains two special symbols - colons.\
 * You have to escape it then put it into `querySelector` function or CSS.\
 * But it seems cannot use it as a CSS custom property name.\
 * So I decided to remove the annoying colons.\
 * And then you can also add your own prefix and suffix to the generated ID.\
 * I've known `useId` does the best on the SSR and keeps consistency.\
 * But it's hard to remove that symbols.\
 * Unless React update `useId` rule, otherwise this function need to be updated.
 *
 * @param prefix - Prefix. Defaults to "uniqueid".
 * @param suffix - Suffix. Defaults to empty string.
 * @param connector - A string that connect the prefix, React ID, and the suffix.
 * @returns A unique ID without the special symbols - colons.
 */
export function useUniqueId(prefix = "uniqueid", suffix = "", connector = "-") {
	const reactId = useId();
	// Expect `reactId` to be ":r1:", ":r2:", ":r3:", etc. if react won't update.
	return useMemo(() => {
		const index = reactId.match(/:r(.+):/)![1];
		(prefix = prefix.trim()) && (prefix += connector);
		(suffix = suffix.trim()) && (suffix = connector + suffix);
		return prefix + index + suffix;
	}, [reactId, prefix, suffix, connector]);
}
