import { convertAniBinaryToCSS } from "ani-cursor";
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

/**
 * 使用动态光标。
 * @param element - HTML DOM 元素的引用。
 * @param aniUrl - 动态光标的路径。
 */
export function useAniCursor(element: MutableRefObject<HTMLElement | null>, aniUrl: string) {
	useEffect(() => void (async () => {
		if (!document.head.querySelector(`style[data-ani-url="${aniUrl}"]`)) {
			const response = await fetch(aniUrl);
			const data = new Uint8Array(await response.arrayBuffer());

			const style = document.createElement("style");
			style.dataset.aniUrl = aniUrl;
			style.innerText = convertAniBinaryToCSS(`[data-anicursor="${aniUrl}"]`, data);

			document.head.appendChild(style);
		}
	})(), [aniUrl]);

	useEffect(() => {
		element.current && (element.current.dataset.anicursor = aniUrl);
	}, [aniUrl]);
}
