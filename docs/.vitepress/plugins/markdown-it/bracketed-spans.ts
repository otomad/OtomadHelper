// See: https://github.com/mb21/markdown-it-bracketed-spans/issues/4

import type MarkdownIt from "markdown-it";

export default function bracketedSpansPlugin(md: MarkdownIt) {
	md.inline.ruler.push("bracketed-spans", state => {
		const max = state.posMax;

		if (state.src.charCodeAt(state.pos) !== 0x5b /* [ */) {
			// opening [
			return false;
		}

		const labelStart = state.pos + 1;
		const labelEnd = state.md.helpers.parseLinkLabel(state, state.pos, false);

		if (labelEnd < 0) {
			// parser failed to find closing ]
			return false;
		}

		const tokens = state.tokens;
		const tokens_len = state.tokens.length;

		if (
			(tokens_len === 0 && state.level > 0) ||
			(tokens_len > 0 &&
				((tokens[tokens_len - 1].nesting < 1 && tokens[tokens_len - 1].level !== state.level) ||
					(tokens[tokens_len - 1].nesting > 0 && tokens[tokens_len - 1].level >= state.level)))
		) {
			// parser failed to find closing ]
			return false;
		}

		const pos = labelEnd + 1;
		if (pos < max && state.src.charCodeAt(pos) === 0x7b /* { */) {
			// probably found span

			state.pos = labelStart;
			state.posMax = labelEnd;

			state.push("span_open", "span", 1);
			state.md.inline.tokenize(state);
			state.push("span_close", "span", -1);

			state.pos = pos;
			state.posMax = max;
			return true;
		} else {
			return false;
		}
	});
}
