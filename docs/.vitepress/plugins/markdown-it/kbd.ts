import type { PluginSimple } from "markdown-it";
import type StateInline from "markdown-it/lib/rules_inline/state_inline.mjs";

const MARKER_OPEN = "[";
const MARKER_CLOSE = "]";
const ESCAPE_CHARACTER = "\\";
const SINGLE_TAG = "kbd";
const WRAPPED_TAG = "KeyShortcuts";

/*
 * Add delimiters for double occurrences of MARKER_SYMBOL.
 */
function tokenize(state: StateInline, silent: boolean) {
	if (silent) {
		return false;
	}

	const start = state.pos;
	const max = state.posMax;
	let momChar = state.src.charAt(start);
	let nextChar = state.src.charAt(start + 1);

	// We are looking for two times the open symbol.
	if (momChar !== MARKER_OPEN || nextChar !== MARKER_OPEN) {
		return false;
	}

	// Find the end sequence
	let openTagCount = 1;
	let end = -1;
	let skipNext = false;
	for (let i = start + 1; i < max && end === -1; i++) {
		momChar = nextChar;
		nextChar = state.src.charAt(i + 1);
		if (skipNext) {
			skipNext = false;
			continue;
		}
		if (momChar === MARKER_CLOSE && nextChar === MARKER_CLOSE) {
			openTagCount -= 1;
			if (openTagCount === 0) {
				// Found the end!
				end = i;
			}
			// Skip second marker char, it is already counted.
			skipNext = true;
		} else if (momChar === MARKER_OPEN && nextChar === MARKER_OPEN) {
			openTagCount += 1;
			// Skip second marker char, it is already counted.
			skipNext = true;
		} else if (momChar === "\n") {
			// Found end of line before the end sequence. Thus, ignore our start sequence!
			return false;
		} else if (momChar === ESCAPE_CHARACTER) {
			skipNext = true;
		}
	}

	// Input ended before closing sequence.
	if (end === -1) {
		return false;
	}

	// Extract inner text for data-key attribute
	const innerText = state.src.slice(start + 2, end).trim();
	const isWrapped = /\s[+>]\s/.test(innerText);

	const tag = !isWrapped ? SINGLE_TAG : WRAPPED_TAG;
	// start tag
	const token = state.push("kbd_open", tag, 1);
	if (!isWrapped) {
		token.attrSet("aria-keyshortcuts", innerText.replaceAll(/\s/g, ""));
		// parse inner
		state.pos += 2;
		state.posMax = end;
		state.md.inline.tokenize(state);
	} else {
		token.attrSet("path", innerText);
	}
	state.pos = end + 2;
	state.posMax = max;
	// end tag
	state.push("kbd_close", tag, -1);

	return true;
}

const kbdPlugin: PluginSimple = md => {
	md.inline.ruler.before("link", "kbd", tokenize);
};

export default kbdPlugin;
