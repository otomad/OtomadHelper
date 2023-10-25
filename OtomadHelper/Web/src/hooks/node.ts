type ReactElementType = string | React.JSXElementConstructor<Any>;

/**
 * 判断一个组件是否是某个组件类型的实例。
 * @param node - 组件实例。
 * @param element - 组件类、函数组件。
 * @returns - 是其实例。
 */
export function isReactInstance<T extends ReactElementType>(node: ReactNode, element: T):
	node is ReactElement<T extends React.FC<infer P> ? P : unknown, T> {
	return React.isValidElement(node) && node.type === element;
}

/**
 * 创建一个强制更新的函数。
 * @returns - 强制更新函数。
 */
export function useForceUpdate() {
	return React.useReducer(() => ({}), {})[1] as () => void;
}
