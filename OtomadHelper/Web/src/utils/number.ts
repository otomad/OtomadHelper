/**
 * Normalize the number. Reject stupid scientific notation.
 * @param num - Number.
 * @returns Normalized number.
 */
export function normalizeNumber(num: number | bigint | string) {
	let s = String(num);
	const regexp = (num: string) => num.trim().toLowerCase().replace(/\+/g, "").replace(/(?<=e|-|^)0*|(?<=\.[^e]*)0*(?=e|$)/g, "").replace(/(?<=-|^)\./, "0.").replace(/\.(?=e|$)/g, "");
	s = regexp(s);
	if (s.includes("e")) {
		let [base, exp_str] = s.split("e");
		const exp = +exp_str;
		const move = (float: string, direct: number) => {
			float += "";
			let dot = float.indexOf(".");
			if (dot === -1) dot = float.length;
			dot = direct > 0 ? dot + 1 : dot - 1;
			float = float.replace(".", "");
			if (dot === float.length) void 0;
			else if (dot > float.length) float += "0";
			else if (dot === 0) float = "0." + float;
			else float = float.slice(0, dot) + "." + float.slice(dot);
			return float;
		};
		for (let i = 0; i < Math.abs(exp); i++) base = move(base, exp);
		s = regexp(base);
	}
	if (s === "-" || s === "") s = "0";
	return s;
}