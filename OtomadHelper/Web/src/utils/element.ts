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
 * 同时阻止事件冒泡和默认方法。
 * @param event - 事件。
 */
export function stopEvent(event?: Pick<Event, "preventDefault" | "stopPropagation">) {
	if (!event) return;
	// 池沼 React 自行声明的 Event 类型与内置 Event 类型不匹配。
	event.preventDefault();
	event.stopPropagation();
}

export function hasRefInReactNode(reactNode: unknown): reactNode is { ref: MutableRefObject<Element> } {
	return !!(reactNode && typeof reactNode === "object" && "ref" in reactNode && reactNode.ref);
}

export function cloneRef(children: ReactNode, nodeRef: MutableRefObject<Element | null>) {
	return h(
		Fragment,
		null,
		React.Children.map(children, child => {
			if (hasRefInReactNode(child))
				useImperativeHandle(child.ref, () => nodeRef.current!, []);
			return React.cloneElement(child as ReactElement, {
				ref: nodeRef,
			});
		}),
	);
}
