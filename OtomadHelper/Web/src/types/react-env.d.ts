import _react from "react";

declare global {
	/**
	 * React Hook 风格函数式组件类型。
	 * @template P - 组件的 Props。
	 * @template E - 继承某个 HTML 原生元素的所有 Attrs。
	 */
	export type FC<P = {}, E extends Element | null = null> = React.FC<Override<
		E extends null ? { children?: ReactNode } : React.HTMLAttributes<E>, P>>;

	/**
	 * useState 的返回值，即包含表示当前值以及设定该值的函数的元组。可用于实现双向绑定。
	 * @template T - 参数类型。
	 */
	export type StateProperty<T> = [T, React.Dispatch<React.SetStateAction<string>>];
	// | (T extends unknown[] ? never : T)

	export type { CSSProperties, ReactElement, ReactNode } from "react";
}
