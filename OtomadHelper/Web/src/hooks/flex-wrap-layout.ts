/*
 * @see https://github.com/tkrotoff/flex-wrap-layout
 */

const defaultClassName = {
	wrapChildren: "wrap-children",
	nextIsWrapped: "next-is-wrapped",
	hasChildWrapped: "has-child-wrapped",
};

/**
 * Detects flex-wrap via JavaScript ([unfortunately not possible in CSS](https://stackoverflow.com/q/40012428)).
 *
 * `useDetectWrappedElements()` are JavaScript functions that detect when elements are wrapped and let you *define the CSS* that goes with it.
 *
 * This allows for responsive UIs without hardcoded CSS (width, min-width, media queries...) using the "natural" width of elements instead.
 *
 * @see [How to detect CSS flex wrap event](https://stackoverflow.com/q/40012428)
 * @see [flex wrap layout](https://github.com/tkrotoff/flex-wrap-layout)
 */
export function useDetectWrappedElements(ref: RefObject<HTMLElement>, {
	nextIsWrapped: nextIsWrappedClassName = defaultClassName.nextIsWrapped,
	hasChildWrapped: hasChildWrappedClassName = defaultClassName.hasChildWrapped,
} = {}) {
	function detectWrappedElements() {
		const parent = ref.current;
		if (parent == null) return;

		setStyleTemporarily(parent, {
			flexDirection: "row",
			// alignItems: "stretch",
			flexWrap: "wrap",
		}, () => {
			let hasChildWrapped = false;
			for (let i = 0; i < parent.children.length; i++) {
				const child = parent.children[i], prevChild = parent.children[i - 1];
				const isWrapped = isElementWrapped(child, prevChild);
				if (isWrapped) hasChildWrapped = true;
				prevChild?.classList.toggle(nextIsWrappedClassName, isWrapped);
				if (child?.nextElementSibling === null) child.classList.remove(nextIsWrappedClassName);
			}
			parent.classList.toggle(hasChildWrappedClassName, hasChildWrapped);
		});
	}

	useEventListener(window, "resize", () => detectWrappedElements(), { immediate: true });
	// new ResizeObserver() // No usage temporarily
}

/**
 * @see [jQuery.position() equivalent is wrong](https://github.com/HubSpot/youmightnotneedjquery/issues/172)
 */
function getTopPosition(el: Element) {
	const { top } = el.getBoundingClientRect();
	const { marginBlockStart } = getComputedStyle(el);
	return top - parseInt(marginBlockStart, 10);
}

function getLeftPosition(el: Element, isRtl: boolean = false) {
	const { left, right } = el.getBoundingClientRect();
	const insetInlineStart = isRtl ? right : left;
	const { marginInlineStart } = getComputedStyle(el);
	return insetInlineStart - parseInt(marginInlineStart, 10);
}

function isElementWrapped(el?: Element | null, prevEl?: Element | null) {
	if (el == null || prevEl == null) return false;
	const rtl = isRtl(el.parentElement);
	return (
		getTopPosition(el) > getTopPosition(prevEl) &&
		(getLeftPosition(el, rtl) - getLeftPosition(prevEl, rtl)) * (rtl ? -1 : 1) <= 0
	);
}
