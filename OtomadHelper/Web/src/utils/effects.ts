const isReduceMotionOrTransparency = () => useMediaQuery.reduceMotion({ noHook: true }) || useMediaQuery.reduceTransparency({ noHook: true });
const isContrast = () => document.documentElement.dataset.scheme?.includes("contrast");

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
	ring.style.transition = "none";
	const computedStyle = getComputedStyle(el);
	if (borderRadius === undefined) borderRadius = computedStyle.getPropertyValue("--focus-border-radius") || computedStyle.borderRadius;
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
export async function makeFocusDiffusionEffect(element: TargetType, options: OverrideStyleOptions = {}) {
	options.portal ??= "#popovers";
	const el = targetToElement(element);
	const ring = createFocusRing(el, options);
	if (!el || !ring) return;
	const rect = el.getBoundingClientRect();
	for (const property of ["top", "left", "width", "height"] as const)
		ring.style[property] = rect[property] + "px";
	const duration = 500;
	if (!isReduceMotionOrTransparency() && !isContrast())
		await Promise.all([
			ring.animate({
				boxShadow: [`0 0 0 ${c("accent-color")}`, `0 0 50px ${c("accent-color")}`],
			}, { duration, easing: eases.easeOutQuad }).finished,
			ring.animate({
				opacity: [1, 0],
			}, { duration, easing: "linear" }).finished,
		]);
	else {
		if (isContrast())
			ring.style.backdropFilter = "invert(1)";
		else if (isReduceMotionOrTransparency())
			ring.style.outline = `4px solid ${c("accent-color")}`;
		await delay(duration / 2);
	}
	ring.remove();
}

/**
 * Sets the style position and size of an element based on its current offset values.
 * @param target - The target HTML element.
 * @param measure - The measure HTML element.
 */
function setElementRectFromOffset(target: HTMLElement, measure: HTMLElement) {
	target.style.top = measure.offsetTop + "px";
	target.style.left = measure.offsetLeft + "px";
	target.style.width = measure.offsetWidth + "px";
	target.style.height = measure.offsetHeight + "px";
}

const FOCUS_HIGHLIGHT_CLASS = "focus-highlight-effect";
const FOCUS_HIGHLIGHT_RING_CLASS = "focus-highlight-ring";
/**
 * A cleanup function for {@link makeFocusHighlightEffect}.
 */
const cleanupFocusHighlightEffect = () => {
	const reduceMotion = isReduceMotionOrTransparency();
	for (const ring of document.getElementsByClassName(FOCUS_HIGHLIGHT_RING_CLASS) as HTMLCollectionOf<HTMLElement>) {
		if (reduceMotion) {
			ring.remove();
			continue;
		}
		setElementRectFromOffset(ring, ring);
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
window.addEventListener("mouseup", e => e.isTrusted && cleanupFocusHighlightEffect(), true);
/**
 * Make a focus highlight effect around the target element.
 * @param element - Target HTML DOM element.
 * @param options - Override style options.
 * @returns Empty promise.
 */
export async function makeFocusHighlightEffect(element: TargetType, options?: OverrideStyleOptions) {
	cleanupFocusHighlightEffect();
	await nextAnimationTick();
	const el = targetToElement<HTMLElement>(element);
	const ring = createFocusRing(el, options);
	if (!el || !ring) return;
	el.classList.add(FOCUS_HIGHLIGHT_CLASS);
	const anchorName = CSS.escape("--" + crypto.randomUUID()); // CAUTION: DO NOT put `"--" +` outside of `CSS.escape`!
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
	const duration = 2000;
	try {
		if (!isReduceMotionOrTransparency() && !isContrast())
			await ring.animate([
				{ boxShadow: "none", easing: eases.easeOutMax },
				{ boxShadow: `0 0 8px 6px ${c("accent-color", 60)}`, easing: eases.easeInSmooth },
				{ boxShadow: "none" },
			], { duration, easing: "linear", iterations: 3 }).finished.catch(noop);
		else if (isContrast())
			await ring.animate([
				{ backdropFilter: "invert(1)", offset: 0, easing: "step-end" },
				{ backdropFilter: "none", offset: 0.5, easing: "step-end" },
			], { duration: duration / 2, easing: "linear", iterations: 3 }).finished.catch(noop);
		else if (isReduceMotionOrTransparency())
			await ring.animate([
				{ outline: `3px solid ${c("accent-color")}`, offset: 0, easing: "step-end" },
				{ outline: "none", offset: 0.5, easing: "step-end" },
			], { duration: duration / 2, easing: "linear", iterations: 3 }).finished.catch(noop);
	} finally {
		ring.remove();
		if (el.style.anchorName === anchorName) {
			el.classList.remove(FOCUS_HIGHLIGHT_CLASS);
			el.style.anchorName = null!;
		}
	}
}
