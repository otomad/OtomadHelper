import type { PluginSimple } from "markdown-it";
import type { RenderRule } from "markdown-it/lib/renderer.mjs";

/** True for ASCII letters (A-Z, a-z) and digits (0-9). */
function isASCIIAlphanumeric(code: number): boolean {
	return (
		(code >= 0x30 && code <= 0x39) || // 0-9
		(code >= 0x41 && code <= 0x5a) || // A-Z
		(code >= 0x61 && code <= 0x7a)    // a-z
	);
}

const markdownItUnderline: PluginSimple = md => {
	// Custom inline rule: handle _text_ cases that the built-in emphasis
	// rule cannot close (i.e. when the closing _ has can_close=false in
	// CommonMark's flanking rules). This is the case for Chinese text like
	// _你好_世界！ where the closing _ before 世 is both left- and
	// right-flanking — CommonMark forbids such a _ from closing emphasis.
	//
	// We ONLY open underline for _ that:
	// - The emphasis rule would also open (can_open=true), OR
	// - Is NOT between two ASCII alphanumeric chars (not snake_case)
	//
	// This gives us:
	// - Normal English _text_ (can_open=true) → emphasis handles close,
	//   or we handle if emphasis can't close
	// - Chinese _你好_世界！ (can_open=true for opening, can_close=false
	//   for closing) → we handle the close
	// - Chinese 你_好_世界！ (can_open=false, but not between ASCII
	//   alnum) → we handle both open and close
	// - Intra-word snake_case (can_open=false AND between ASCII alnum)
	//   → skipped, remains literal
	// - Mixed *foo_bar*baz_ (can_open=false, between ASCII alnum) → skipped
	md.inline.ruler.before("emphasis", "underline", (state, silent) => {
		const src = state.src;
		const pos = state.pos;
		const posMax = state.posMax;

		// Must start with _
		if (src.charCodeAt(pos) !== 0x5f) return false;

		// __ belongs to strong emphasis — don't touch it
		if (pos + 1 < posMax && src.charCodeAt(pos + 1) === 0x5f) return false;

		// Decide whether this _ can open an underline.
		// Allow if emphasis can open it (can_open=true), OR if it's not an
		// intra-word ASCII underscore (where both neighbors are [a-zA-Z0-9]).
		const openDelim = state.scanDelims(pos, false);
		if (!openDelim.can_open) {
			// Still allow if NOT between two ASCII alphanumeric chars.
			// This catches Chinese text like 你_好_世界！ where the _
			// is between CJK chars (both are Unicode letters, not ASCII).
			if (pos <= 0 || pos + 1 >= posMax) return false;
			const prev = src.charCodeAt(pos - 1);
			const next = src.charCodeAt(pos + 1);
			if (isASCIIAlphanumeric(prev) && isASCIIAlphanumeric(next)) {
				return false; // snake_case — literal underscore
			}
		}

		// Find matching closing single _. Unlike the opening check, we do
		// NOT gate on scanDelims here: once an underline is opened, any
		// non-escaped single _ can close it. This is essential for Chinese
		// text where the closing _ may have can_close=true (emphasis
		// could close it) but emphasis has no matching opener.
		let endPos = -1;
		for (let i = pos + 1; i < posMax; i++) {
			if (src.charCodeAt(i) !== 0x5f) continue;

			// Skip underscores that are part of __
			if (i + 1 < posMax && src.charCodeAt(i + 1) === 0x5f) continue;
			if (i - 1 > pos && src.charCodeAt(i - 1) === 0x5f) continue;

			// Escaped underscore: preceded by an odd number of backslashes.
			// E.g. \_ is literal, \\_ is a literal backslash + delimiter.
			let backslashCount = 0;
			for (let j = i - 1; j >= 0 && src.charCodeAt(j) === 0x5c; j--) {
				backslashCount++;
			}
			if (backslashCount % 2 === 1) continue;

			// Empty content (_ immediately followed by _) is invalid
			if (i === pos + 1) return false;

			endPos = i;
			break;
		}

		if (endPos === -1) return false;

		if (!silent) {
			const oldPosMax = state.posMax;
			state.posMax = endPos;

			const openToken = state.push("em_open", "u", 1);
			openToken.markup = "_";

			// Tokenize inner content so that nested inline markup
			// (e.g. *italic*, **bold**) is parsed correctly
			state.pos = pos + 1;
			state.md.inline.tokenize(state);

			const closeToken = state.push("em_close", "u", -1);
			closeToken.markup = "_";

			state.posMax = oldPosMax;
		}

		state.pos = endPos + 1;
		return true;
	});

	// Renderer rule: convert em_open/em_close with _ markup to <u> tags.
	// This handles both tokens from our custom rule above and any em
	// tokens that the built-in emphasis rule may still generate for _.
	const renderEm: RenderRule = (tokens, index, options, _, self) => {
		const token = tokens[index];
		if (token.markup === "_") token.tag = "u";
		return self.renderToken(tokens, index, options);
	};

	md.renderer.rules.em_open = renderEm;
	md.renderer.rules.em_close = renderEm;
};

export default markdownItUnderline;
