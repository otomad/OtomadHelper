import type { PluginSimple } from "markdown-it";
import type { RenderRule } from "markdown-it/lib/renderer.mjs";

const markdownItUnderline: PluginSimple = md => {
	// Custom inline rule: handle _text_ with permissive rules (like *text*)
	// instead of CommonMark's restrictive rules for _. Without this, Chinese
	// text like _你好_世界 fails because the closing _ before 世 is both
	// left-flanking and right-flanking — CommonMark forbids a _ from closing
	// emphasis in that case unless followed by punctuation.
	md.inline.ruler.before("emphasis", "underline", (state, silent) => {
		const src = state.src;
		const pos = state.pos;
		const posMax = state.posMax;

		// Must start with _
		if (src.charCodeAt(pos) !== 0x5f) return false;

		// __ belongs to strong emphasis — don't touch it
		if (pos + 1 < posMax && src.charCodeAt(pos + 1) === 0x5f) return false;

		// Find matching closing single _
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
