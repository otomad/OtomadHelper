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

const lastFocusHighlightEffectElAtom = atom<[el: HTMLElement, ring: HTMLDivElement]>();
export async function makeFocusHighlightEffect(element: TargetType, options?: OverrideStyleOptions) {
	const remove = (el?: HTMLElement, ring?: HTMLDivElement) => {
		jotaiStore.set(lastFocusHighlightEffectElAtom, undefined);
		el?.style.removeProperty("anchor-name");
		ring?.remove();
	};
	const lastEl = jotaiStore.get(lastFocusHighlightEffectElAtom);
	remove(...lastEl ?? []);
	const el = targetToElement<HTMLElement>(element);
	const ring = createFocusRing(el, options);
	if (!el || !ring) return;
	el.scrollIntoView({ block: "center" });
	jotaiStore.set(lastFocusHighlightEffectElAtom, [el, ring]);
	const anchorName = "--" + CSS.escape(crypto.randomUUID());
	console.log("​ ​ anchorName​", anchorName);
	el.style.anchorName = anchorName;
	assign(ring.style, { position: "fixed", positionAnchor: anchorName, positionArea: "center", inlineSize: "100%", blockSize: "100%" });
	await ring.animate([
		{ boxShadow: "none", easing: eases.easeOutMax },
		{ boxShadow: `0 0 8px 6px ${c("accent-color", 60)}`, easing: eases.easeInSmooth },
		{ boxShadow: "none" },
	], { duration: 2000, easing: "linear", iterations: 3 }).finished.catch(() => remove(el, ring)).finally(() => remove(el, ring));
}
