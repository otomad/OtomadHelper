import type { RuleSet } from "styled-components";
import { setBorderRadius, type BorderRadiusPosition } from "./internal";

export default {
	/**
	 * 一键弹性布局加居中元素 (flex - center - center)。
	 */
	flexCenter: () => css`
		display: flex;
		justify-content: center;
		align-items: center;
	`,
	/**
	 * 一键弹性网格布局加居中元素 (grid - center)。
	 */
	gridCenter: () => css`
		display: grid;
		place-items: center;
	`,
	/**
	 * 变成正方形。长宽均相等。
	 * @param size - 长度。
	 * @param withSizeVar - 是否生成一个名为 `--size` 的 CSS 自定义属性以使其它组件更方便的修改尺寸？
	 */
	square: (size: string, withSizeVar: boolean = false) =>
		!withSizeVar ?
			css`
				width: ${size};
				height: ${size};
			` :
			css`
				--size: ${size};
				width: var(--size);
				height: var(--size);
			`,
	/**
	 * 变成椭圆形。
	 * 将 border-radius 设为一个非常大的值。
	 * @param position - 圆角的位置。
	 */
	oval: (position: BorderRadiusPosition = "full") =>
		setBorderRadius("9999rem", position),
	/**
	 * 变成圆形。
	 * 将 border-radius 设为 100%。
	 * @param position - 圆角的位置。
	 */
	circle: (position: BorderRadiusPosition = "full") =>
		setBorderRadius("100%", position),
	/**
	 * 创建一个渐变色的边框。
	 * @param gradient - 渐变色。
	 * @param borderWidth - 边框宽度。
	 */
	gradientBorder: (gradient: string | RuleSet<object>, borderWidth: number | string = "1px") => {
		if (typeof borderWidth === "number") borderWidth = borderWidth + "px";
		return css`
			position: relative;
			padding: ${borderWidth};

			&::before {
				content: "";
				position: absolute;
				inset: 0;
				background: ${gradient} border-box;
				border-width: ${borderWidth};
				border-style: solid;
				border-color: transparent;
				border-radius: inherit;
				pointer-events: none;
				-webkit-mask: linear-gradient(white 0 0) padding-box, linear-gradient(white 0 0);
				-webkit-mask-composite: xor;
				mask-composite: exclude;
			}
		`;
	},
	/**
	 * 取消父元素的聚焦环样式，转发到一个子元素上应用聚焦环样式。
	 * @param childClassName - 子元素类名。
	 */
	forwardFocusRing: (childClassName: string = "base") => css`
		&:focus-visible {
			box-shadow: none;

			.${childClassName} {
				${styles.effects.focus()};
			}
		}
	`,
	/**
	 * 将元素占满整个屏幕。
	 *
	 * 将长宽均设为 100% 视窗宽高度值。
	 * @param position - 指定元素的 position 属性。
	 */
	fullscreen: (position: "fixed" | "absolute" = "fixed") => css`
		position: ${position};
		top: 0;
		left: 0;
		width: 100dvw;
		height: 100dvh;
	`,
};
