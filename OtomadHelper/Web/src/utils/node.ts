type ReactElementType = string | React.JSXElementConstructor<Any>;

export function isReactInstance<T extends ReactElementType>(node: ReactNode, element: T):
	node is ReactElement<T extends React.FC<infer P> ? P : unknown, T> {
	return React.isValidElement(node) && node.type === element;
}
