import type { Cursor } from "styles/force-cursor";

/**
 * 强行指定鼠标光标样式。
 * @param cursor - 光标样式。如为 null 表示清除。
 */
export function forceCursor(cursor: Cursor | null) {
	if (!cursor)
		delete document.body.dataset.cursor;
	else
		document.body.dataset.cursor = cursor;
}
