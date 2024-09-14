import { convertAniBinaryToCSS } from "ani-cursor";
import type { Property } from "csstype";
type Cursor = Property.Cursor;

/**
 * Forcefully specify the mouse cursor style.
 *
 * [MDN Reference](https://developer.mozilla.org/zh-CN/docs/Web/CSS/cursor)
 *
 * @param cursor - Cursor style. If it is null, it will restore to the default.
 */
export function forceCursor(cursor: Cursor | null) {
	if (!cursor)
		document.body.style.removeProperty("--cursor");
	else
		document.body.style.setProperty("--cursor", cursor);
}

/**
 * Use animated mouse cursor (.ani).
 *
 * @param element - HTML DOM element reference.
 * @param aniUrl - The path of the animated mouse cursor.
 */
export function useAniCursor(element: MutableRefObject<HTMLElement | null>, aniUrl: string) {
	useAsyncEffect(async () => {
		const isCreated = () => !!document.head.querySelector(`style[data-ani-url="${aniUrl}"]`);
		if (!isCreated()) {
			const response = await fetch(aniUrl);
			const data = new Uint8Array(await response.arrayBuffer());
			if (isCreated()) return;

			const style = document.createElement("style");
			style.dataset.aniUrl = aniUrl;
			style.innerText = convertAniBinaryToCSS(`[data-anicursor="${aniUrl}"]`, data);

			document.head.appendChild(style);
		}
	}, [aniUrl]);

	useEffect(() => void (
		element.current && (element.current.dataset.anicursor = aniUrl)
	), [aniUrl]);
}
