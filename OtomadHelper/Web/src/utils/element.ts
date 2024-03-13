type ReactElementType = string | React.JSXElementConstructor<Any>;

/**
 * 判断一个组件是否是某个组件类型的实例。
 * @param node - 组件实例。
 * @param element - 组件类、函数组件。
 * @returns - 是其实例。
 */
export function isReactInstance<T extends ReactElementType>(node: ReactNode, element: T):
	node is ReactElement<T extends React.FC<infer P> ? P : unknown, T> {
	return React.isValidElement(node) && isObject(node) && "type" in node && node.type === element;
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

export function hasRefInReactNode(reactNode: unknown): reactNode is { ref: MutableRefObject<Element | null> } {
	return !!(reactNode && typeof reactNode === "object" && "ref" in reactNode && reactNode.ref);
}

export function cloneRef(children: ReactNode, nodeRef: MutableRefObject<Element | null>) {
	return h(
		Fragment,
		null,
		React.Children.map(children, (child: ReactNode) => {
			if (hasRefInReactNode(child)) {
				// useImperativeHandle(child.ref, () => nodeRef.current!, []);
				// child.ref.current = nodeRef.current;
				delete (child.ref as Partial<DomRef<Element>>).current;
				Object.defineProperty(child.ref, "current", {
					configurable: true,
					enumerable: true,
					get: () => nodeRef.current,
					set: value => nodeRef.current = value,
				});
			}
			return React.cloneElement(child as ReactElement, {
				ref: nodeRef,
			});
		}),
	);
}

type TargetType = Node | Element | MutableRefObject<Element | null | undefined> | Event | EventTarget | null | undefined;
type DetectInPathType = Node | Element | MutableRefObject<Element | null | undefined> | Event | EventTarget | string | null | undefined;

/**
 * 获取从指定元素节点，一直追溯到根元素的数组。
 * 用于查找事件目标并向上冒泡，找到需要的元素。
 * @param target - HTML DOM 节点。
 * @returns 从指定元素节点，一直追溯到根元素的数组。
 */
export function getPath(target: TargetType): Element[] {
	if (isRef(target)) target = toValue(target);
	if (isObject(target) && "target" in target) target = target.target;
	if (!(target instanceof Element)) return [];
	const path: Element[] = [];
	while (target instanceof Element) {
		path.push(target);
		target = target.parentElement;
	}
	return path;
}

/**
 * 根据鼠标事件的目标节点，查找要查询的元素是否是或是其祖先节点。比如查找元素是否被点击等。
 * @param target - 点击事件中的目标 HTML DOM 节点。
 * @param element - 要查找的冒泡 HTML DOM 节点。如果为字符串则表示要查询的类名。
 * @returns 要查询的元素是或是其祖先节点。
 */
export function isInPath(target: TargetType, element: DetectInPathType): boolean {
	const path = getPath(target);
	if (isRef(element)) element = toValue(element);
	if (isObject(element) && "target" in element) element = element.target;
	if (typeof element === "string") {
		for (const el of path)
			if (el.classList.contains(element))
				return true;
		return false;
	}
	if (!(element instanceof Element)) return false;
	return path.includes(element);
}
