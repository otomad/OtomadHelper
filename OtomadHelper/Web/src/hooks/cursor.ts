import { convertAniBinaryToCSS } from "ani-cursor";
import type { Cursor } from "styles/force-cursor";

/**
 * Forcefully specify the mouse cursor style.
 * @param cursor - Cursor style. If it is null, it will restore to the default.
 */
export function forceCursor(cursor: Cursor | null) {
	if (!cursor)
		delete document.body.dataset.cursor;
	else
		document.body.dataset.cursor = cursor;
}

/**
 * Use animated mouse cursor (.ani).
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

	useEffect(() => {
		element.current && (element.current.dataset.anicursor = aniUrl);
	}, [aniUrl]);
}
