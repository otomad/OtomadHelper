interface OverrideStyleOptions {
	/** Override the border-radius property for the focus ring. If not specified, it will auto inherit the value from the target element. */
	borderRadius?: CSSProperty.BorderRadius | null;
	/** Where the focus ring be placed? @default ["main.page", "#popovers", "body"] */
	portal?: string;
}

function createFocusRing(el: Element | null, { borderRadius, portal = "main.page" }: OverrideStyleOptions = {}) {
	const popovers = document.querySelector(portal) ?? document.getElementById("popovers") ?? document.body;
	if (!el || !popovers) return;
	const ring = document.createElement("div");
	ring.style.position = "fixed";
	ring.style.pointerEvents = "none";
	if (borderRadius === undefined) ({ borderRadius } = getComputedStyle(el));
	if (borderRadius && borderRadius !== "0px") ring.style.borderRadius = borderRadius;
	popovers.append(ring);
	return ring;
}

/**
 * Make a focus diffusion effect around the target element.
 * @param element - Target HTML DOM element.
 * @param options - Override style options.
 * @returns Empty promise.
 */
export async function makeFocusDiffusionEffect(element: TargetType, options?: OverrideStyleOptions) {
	const el = targetToElement(element);
	const ring = createFocusRing(el, options);
	if (!el || !ring) return;
	const rect = el.getBoundingClientRect();
	for (const property of ["top", "left", "width", "height"] as const)
		ring.style[property] = rect[property] + "px";
	const duration = 500;
	await Promise.all([
		ring.animate({
			boxShadow: [`0 0 0 ${c("accent-color")}`, `0 0 50px ${c("accent-color")}`],
		}, { duration, easing: eases.easeOutQuad }).finished,
		ring.animate({
			opacity: [1, 0],
		}, { duration, easing: "linear" }).finished,
	]);
	ring.remove();
}

const FOCUS_HIGHLIGHT_CLASS = "focus-highlight-effect";
const FOCUS_HIGHLIGHT_RING_CLASS = "focus-highlight-ring";
const clearFocusHighlightEffect = () => {
	for (const ring of document.getElementsByClassName(FOCUS_HIGHLIGHT_RING_CLASS) as HTMLCollectionOf<HTMLElement>) {
		ring.style.top = ring.offsetTop + "px";
		ring.style.left = ring.offsetLeft + "px";
		ring.style.width = ring.offsetWidth + "px";
		ring.style.height = ring.offsetHeight + "px";
		ring.animate({ opacity: [1, 0] }, { duration: 250, easing: eases.easeOutMax }).finished.catch(noop).then(() => {
			removeExistAnimations(ring);
			ring.remove();
		});
	}
	for (const el of document.getElementsByClassName(FOCUS_HIGHLIGHT_CLASS) as HTMLCollectionOf<HTMLElement>) {
		el.classList.remove(FOCUS_HIGHLIGHT_CLASS);
		el.style.anchorName = null!;
	}
};
window.addEventListener("mouseup", clearFocusHighlightEffect, true);
export async function makeFocusHighlightEffect(element: TargetType, options?: OverrideStyleOptions) {
	clearFocusHighlightEffect();
	await nextAnimationTick();
	const el = targetToElement<HTMLElement>(element);
	const ring = createFocusRing(el, options);
	if (!el || !ring) return;
	el.classList.add(FOCUS_HIGHLIGHT_CLASS);
	const anchorName = "--" + CSS.escape(crypto.randomUUID());
	el.style.anchorName = anchorName;
	assign(ring.style, {
		position: "fixed",
		positionAnchor: anchorName,
		top: "anchor(top, calc(infinity * -1px))",
		left: "anchor(left, calc(infinity * -1px))",
		width: "anchor-size(width, 0)",
		height: "anchor-size(height, 0)",
	});
	ring.classList.add(FOCUS_HIGHLIGHT_RING_CLASS);
	try {
		await ring.animate([
			{ boxShadow: "none", easing: eases.easeOutMax },
			{ boxShadow: `0 0 8px 6px ${c("accent-color", 60)}`, easing: eases.easeInSmooth },
			{ boxShadow: "none" },
		], { duration: 2000, easing: "linear", iterations: 3 }).finished.catch(noop);
	} finally {
		// ring.remove();
		// el.classList.remove(FOCUS_HIGHLIGHT_CLASS);
		// el.style.removeProperty("anchor-name");
	}
}
