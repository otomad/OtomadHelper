import type { PluginSimple } from "markdown-it";
import type { RenderRule } from "markdown-it/lib/renderer.mjs";

const markdownItUnderline: PluginSimple = md => {
	// Custom inline rule: handle _text_ cases that the built-in emphasis
	// rule cannot close (i.e. when the closing _ has can_close=false in
	// CommonMark's flanking rules). This is the case for Chinese text like
	// _你好_世界！ where the closing _ before 世 is both left- and
	// right-flanking — CommonMark forbids such a _ from closing emphasis.
	//
	// We ONLY open underline for _ that the emphasis rule would also open
	// (can_open=true). Then we search for a closing _ that emphasis CANNOT
	// close (can_close=false). This way:
	// - Normal English _text_ (both can_open/can_close=true) → emphasis
	// - Chinese _你好_世界！ (open can_open, close can_close=false) → us
	// - Intra-word _ (neither can_open nor can_close) → skipped (literal)
	// - Mixed *foo_bar*baz_ → _ can't open inside * emphasis → skipped
	md.inline.ruler.before("emphasis", "underline", (state, silent) => {
		const src = state.src;
		const pos = state.pos;
		const posMax = state.posMax;

		// Must start with _
		if (src.charCodeAt(pos) !== 0x5f) return false;

		// __ belongs to strong emphasis — don't touch it
		if (pos + 1 < posMax && src.charCodeAt(pos + 1) === 0x5f) return false;

		// Only open if the emphasis rule would also be able to open this _
		// (CommonMark left-flanking). Intra-word underscores in ASCII text
		// and underscores inside * emphasis are skipped here.
		const openDelim = state.scanDelims(pos, false);
		if (!openDelim.can_open) return false;

		// Find matching closing single _ that the emphasis rule CANNOT close
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

			// If emphasis can close this _, let the emphasis rule handle
			// the pair. We only step in when emphasis CANNOT close.
			const closeDelim = state.scanDelims(i, false);
			if (closeDelim.can_close) continue;

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
