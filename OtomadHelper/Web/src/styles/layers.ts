/*
 * 由于 styled-components 有全局样式加载在组件样式之后的 bug，临时使用提前加载样式的方法解决 layer 的问题。
 *
 * 跟踪链接：https://github.com/styled-components/styled-components/issues/3146
 */

export const layers = css`
	@layer base, theme, layout, props, utilities, components, special;
`;

// #region Init layers
const style = document.createElement("style");
style.textContent = layers.toString();
document.head.append(style);
// #endregion
