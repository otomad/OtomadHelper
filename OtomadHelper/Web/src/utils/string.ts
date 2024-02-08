/**
 * 在每两个字符之间添加零宽连字 (Word Joiner / No Breaking)。
 * @param str - 字符串。
 * @returns 连字后的字符串。
 */
export function addWjBetweenEachChar(str: string) {
	const WJ = "\u2060";
	let prevChar: string | undefined;
	return Array.from(str).flatMap(curChar => {
		const isChar = (char: string) => prevChar === char || curChar === char;
		let valid = true;
		if (!prevChar || isChar(" ") || isChar(WJ)) valid = false;
		prevChar = curChar;
		return [valid && WJ, curChar];
	}).filter(char => char).join("");
}
