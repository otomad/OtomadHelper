/* eslint-disable jsdoc/require-returns */
import { type BorderRadiusPosition, setBorderRadius } from "./internal";

type ResponsiveUnit = "v" | "dv" | "lv" | "sv" | "cq";
type OutOfFlowPositions = "absolute" | "fixed";

// #region keyframes
const overflowGradientScrollStartMaskTransparencyChangeKeyframes = keyframes`
	from {
		--scroll-start-mask-transparency: 1;
	}

	to {
		--scroll-start-mask-transparency: 0;
	}
`;
const overflowGradientScrollEndMaskTransparencyChangeKeyframes = keyframes`
	from {
		--scroll-end-mask-transparency: 0;
	}

	to {
		--scroll-end-mask-transparency: 1;
	}
`;
// #endregion

export default {
	/**
	 * Center the element with the **flex** layout (flex - center - center).
	 */
	flexCenter: () => css`
		display: flex;
		justify-content: center;
		align-items: center;
	`,
	/**
	 * Center the element with the **grid** layout (grid - center).
	 */
	gridCenter: () => css`
		display: grid;
		place-items: center;
	`,
	/**
	 * Center an element with **unknown size** that positioning out of flow (absolute or fixed).
	 * @param position - Specify the `position` property of the element. Must be `absolute` or `fixed`.
	 */
	absoluteCenter: (position: OutOfFlowPositions = "absolute") => css`
		position: ${position};
		top: 50%;
		left: 50%;
		place-self: anchor-center;
	`,
	/**
	 * Center an element with **known size** that positioning out of flow (absolute or fixed).
	 * @note You have to specify the width and height while using the mixin.
	 * @param position - Specify the `position` property of the element. Must be `absolute` or `fixed`.
	 */
	absoluteCenterSized: (position: OutOfFlowPositions = "absolute") => css`
		position: ${position};
		inset: 0;
		margin: auto;
	`,
	/**
	 * Become a square.
	 *
	 * Equal in width and height.
	 * @param size - Side length.
	 * @param withSizeVar - Create a CSS custom property named `--size` to make it easier for other components to modify its size?
	 * @param responsiveUnitOrLogicalProperties - Use `inline-size` and `block-size` properties instead of `width` and `height`?
	 */
	square: ((size: string, withSizeVar: boolean = false, responsiveUnitOrLogicalProperties?: ResponsiveUnit | true) =>
		responsiveUnitOrLogicalProperties === true ?
			!withSizeVar ?
				css`
					block-size: ${size};
					inline-size: ${size};
				` :
				css`
					--size: ${size};
					block-size: var(--size);
					inline-size: var(--size);
				` :
			!responsiveUnitOrLogicalProperties ?
				!withSizeVar ?
					css`
						width: ${size};
						height: ${size};
					` :
					css`
						--size: ${size};
						width: var(--size);
						height: var(--size);
					` :
				!withSizeVar ?
					css`
						width: ${size}${responsiveUnitOrLogicalProperties}w;
						height: ${size}${responsiveUnitOrLogicalProperties}h;
					` :
					css`
						--size: ${size};
						width: calc(var(--size) * 1${responsiveUnitOrLogicalProperties}w);
						height: calc(var(--size) * 1${responsiveUnitOrLogicalProperties}h);
					`
	) as {
		(size: string, withSizeVar?: boolean): RuleSet;
		(size: number, withSizeVar: boolean, responsiveUnit?: ResponsiveUnit): RuleSet;
		(size: string, withSizeVar: boolean, logicalProperties?: boolean): RuleSet;
	},
	/**
	 * Become a oval.
	 *
	 * Set border radius to a very large value.
	 * @param position - The position of the rounded corners.
	 */
	oval: (position: BorderRadiusPosition = "full") =>
		setBorderRadius("calc(infinity * 1px)", position),
	/**
	 * Become a circle.
	 *
	 * Set border radius to 100%.
	 * @param position - The position of the rounded corners.
	 */
	circle: (position: BorderRadiusPosition = "full") =>
		setBorderRadius("100%", position),
	/**
	 * Cancel the focus ring style of the parent element, and forward it to a child element to apply the focus ring style.
	 * @param childSelector - Child element selector.
	 * @param inset - Makes focus ring inside the element.
	 */
	forwardFocusRing: (childSelector: string = ".base", inset: boolean = false) => css`
		&:focus-visible {
			box-shadow: none;
			transition: inherit;

			${childSelector} {
				${styles.effects.focus(inset)};
			}
		}
	`,
	/**
	 * Fill the element to the entire screen.
	 *
	 * Set both width and height to 100% of the window width and height value.
	 * @param position - Specify the `position` property of the element.
	 */
	fullscreen: (position: OutOfFlowPositions = "fixed") => css`
		position: ${position};
		top: 0;
		left: 0;
		width: 100dvw;
		height: 100dvh;
	`,
	/**
	 * Hide the element when it is empty.
	 */
	hideIfEmpty: () => css`
		&:empty {
			display: none;
		}
	`,
	/**
	 * Enable hardware 3D. Used to solve certain problems that may cause flickering during transitions.
	 * @remarks This will replace or be replaced with existing transform properties.
	 */
	enableHardware3d: () => css`
		transform: translateZ(1px);
	`,
	/**
	 * Effectively hide the scroll bar. However, the elements are still scrollable.
	 *
	 * Support both modern and legacy declaration simultaneously.
	 */
	noScrollbar: () => css`
		@supports (scrollbar-width: auto) {
			scrollbar-width: none;
		}

		@supports selector(::-webkit-scrollbar) {
			&::-webkit-scrollbar {
				width: 0;
				height: 0;
			}
		}
	`,
	/**
	 * Inherit multiple CSS properties from parent.
	 * @param properties - CSS properties name in hyphen case.
	 */
	inherit: ((...properties: (keyof CSSPropertiesHyphen)[]) => {
		let important = false;
		if (typeof properties.last() === "boolean")
			important = !!properties.pop();
		return properties.map(property => css`${property}: inherit ${important ? "!important" : ""};`);
	}) as {
		(...properties: (keyof CSSPropertiesHyphen)[]): RuleSet;
		(...args: [...properties: (keyof CSSPropertiesHyphen)[], important: boolean]): RuleSet;
	},
	/**
	 * Fading out text on overflow if the text is bigger than allowed.
	 * @param axis - Overflow x (horizontally) or y (vertically).
	 * @param scrollMaskThickness - Specify the fading gradient width or height.
	 * @param staticEffect - Always show the fading gradient, instead of auto detecting.
	 */
	overflowGradient: (axis: "x" | "y", scrollMaskThickness: "1em" | "1.25lh" | string & {}, staticEffect: boolean = false) => css`
		--scroll-mask-thickness: ${scrollMaskThickness};
		mask: linear-gradient(
			to ${axis === "x" ? "right" : "bottom"},
			rgb(0 0 0 / var(--scroll-start-mask-transparency)) 0%,
			black var(--scroll-mask-thickness) calc(100% - var(--scroll-mask-thickness)),
			rgb(0 0 0 / var(--scroll-end-mask-transparency)) 100%
		);

		${staticEffect ? css`
			--scroll-start-mask-transparency: 0;
			--scroll-end-mask-transparency: 0;
		` : css`
			animation:
				${overflowGradientScrollStartMaskTransparencyChangeKeyframes} 1s linear forwards,
				${overflowGradientScrollEndMaskTransparencyChangeKeyframes} 1s linear backwards;
			animation-timeline: scroll(self ${axis});
			animation-range: 0 1em, calc(100% - 1em) 100%;
			overflow-${axis}: auto;
			overscroll-behavior-${axis}: auto;

			${ifColorScheme.at.reduceTransparency} {
				mask: none;
				animation: none;
			}
		`}
	`,
};
