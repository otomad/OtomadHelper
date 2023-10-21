/**
 * 等待下一时刻 CSS 动画更新刷新。
 * @returns 空承诺。
 */
export function nextAnimationTick() {
	return new Promise<void>(resolve => {
		window.requestAnimationFrame(() => {
			window.requestAnimationFrame(() => {
				resolve();
			});
		});
	});
}

/**
 * 为元素设定样式时**暂时**禁用过渡动画。
 * @param element - HTML DOM 元素。
 * @param style - CSS 样式。
 */
export async function setStyleWithoutTransition(element: HTMLElement, style: CSSProperties) {
	Object.assign(element.style, style);
	element.style.transition = "none";
	await nextAnimationTick();
	element.style.removeProperty("transition");
}

/**
 * 没错，就是大名鼎鼎的延迟函数。
 * 叫 sleep 也行。
 * @param ms - 毫秒值。
 * @returns 空返回值。
 */
export function delay(ms: number): Promise<void> {
	return new Promise(resolve => setTimeout(resolve, ms));
}
