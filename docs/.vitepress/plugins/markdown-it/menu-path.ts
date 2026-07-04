import type MarkdownIt from "markdown-it";

const MARKER_OPEN = "[";
const MARKER_CLOSE = "]";
const ESCAPE_CHARACTER = "\\";
const TAG = "MenuPath";

type StateInline = InstanceType<InstanceType<typeof MarkdownIt>["inline"]["State"]>;

function tokenize(state: StateInline) {
	const start = state.pos;
	const max = state.posMax;
	let momChar = state.src.charAt(start);
	let nextChar = state.src.charAt(start + 1);
	let nextNextChar = state.src.charAt(start + 2);

	if (momChar !== MARKER_OPEN || nextChar !== MARKER_OPEN || nextNextChar !== MARKER_OPEN) {
		return false;
	}

	let openTagCount = 1;
	let end = -1;
	let skipNext: 0 | 1 | 2 = 0;
	for (let i = start + 1; i < max && end === -1; i++) {
		momChar = nextChar;
		nextChar = state.src.charAt(i + 1);
		nextNextChar = state.src.charAt(i + 2);
		if (skipNext) {
			i += skipNext - 1;
			skipNext = 0;
			continue;
		}
		if (momChar === MARKER_CLOSE && nextChar === MARKER_CLOSE && nextNextChar === MARKER_CLOSE) {
			openTagCount -= 1;
			if (openTagCount === 0) {
				end = i;
			}
			skipNext = 2;
		} else if (momChar === MARKER_OPEN && nextChar === MARKER_OPEN && nextNextChar === MARKER_OPEN) {
			openTagCount += 1;
			skipNext = 2;
		} else if (momChar === "\n") {
			return false;
		} else if (momChar === ESCAPE_CHARACTER) {
			skipNext = 1;
		}
	}

	if (end === -1) {
		return false;
	}

	const innerText = state.src.slice(start + 3, end).trim();

	const token = state.push("menu_path_open", TAG, 1);
	token.attrSet("items", innerText);

	state.pos = end + 3;
	state.posMax = max;

	state.push("menu_path_close", TAG, -1);

	return true;
}

export default function kbdPlugin(md: MarkdownIt): void {
	md.inline.ruler.before("kbd", "menu-path", tokenize);
}
