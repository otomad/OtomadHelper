import { setBorderRadius, type BorderRadiusPosition } from "./internal";

export default {
	/**
	 * 一键弹性布局加居中元素 (flex - center - center)。
	 */
	flexCenter: () => css`
		display: flex;
		align-items: center;
		justify-content: center;
	`,
	/**
	 * 变成正方形。长宽均相等。
	 * @param size - 长度。
	 * @param withSizeVar - 是否生成一个名为 `--size` 的 CSS 自定义属性以使其它组件更方便的修改尺寸？
	 */
	square: (size: string, withSizeVar: boolean = false) => (
		!withSizeVar ?
			css`
				width: ${size};
				height: ${size};
			` :
			css`
				--size: ${size};
				width: var(--size);
				height: var(--size);
			`
	),
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
};
