export { };

declare global {
	/**
	 * React Hook 风格函数式组件类型。
	 * @template P - 组件的 Props。
	 */
	type FC<P = {}> = React.FC<Override<React.HTMLAttributes<HTMLElement>, P>>;
}
