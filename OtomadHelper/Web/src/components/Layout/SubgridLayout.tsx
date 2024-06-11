const SPAN_TO_END = "span-to-end";

const subgridChildren = createStore(new class {
	subgrids = proxyMap<string, Set<Element>>();
	resizeObserver: ResizeObserver;

	constructor() {
		this.resizeObserver = valtioRef(new ResizeObserver(this.updateLayout));
	}

	addChildren(groupName: string, children: Iterable<Element>) {
		const { subgrids } = subgridChildren;
		if (!subgrids.has(groupName))
			subgrids.set(groupName, new Set());
		const subgrid = subgrids.get(groupName)!;
		for (const child of children)
			if (!child.classList.contains(SPAN_TO_END))
				subgrid.add(child);
	}

	removeChildren(groupName: string, children: Iterable<Element>) {
		const { subgrids } = subgridChildren;
		if (!subgrids.has(groupName))
			return;
		const subgrid = subgrids.get(groupName)!;
		for (const child of children)
			subgrid.delete(child);
	}

	updateLayout = lodash.debounce(() => {
		const { subgrids } = subgridChildren;
		for (const [_groupName, subgrid] of subgrids) {
			if (!subgrid?.size) continue;
			const ghostElements: Element[] = [];
			const grouped = Map.groupBy(subgrid, element => getElementIndex(element));
			for (const [_index, elements] of grouped) {
				let maxWidth = 0;
				for (const element of elements as HTMLElement[]) {
					if (!element.parentNode) ghostElements.push(element);
					element.style.width = "auto";
					if (element.offsetWidth > maxWidth) maxWidth = element.offsetWidth;
				}
				if (elements.length <= 1) continue;
				for (const element of elements as HTMLElement[])
					element.style.width = maxWidth + "px";
			}
			for (const element of ghostElements)
				subgrid.delete(element);
		}
	});
}());

export default forwardRef(function SubgridLayout({ name: groupName, children, ...htmlAttrs }: FCP<{
	/** Subgrid group name. */
	name: string;
}, "div">, ref: ForwardedRef<"div">) {
	const containerEl = useDomRef<"div">();
	useImperativeHandle(ref, () => containerEl.current!);

	const { addChildren, removeChildren, updateLayout, resizeObserver } = useSnapshot(subgridChildren);
	const getContainerAndChildren = () => {
		const container = containerEl.current;
		const children = container?.children;
		return { container, children: children?.length ? children : null };
	};

	useEffect(() => {
		const { container, children } = getContainerAndChildren();
		if (container) resizeObserver.observe(container);
		if (children) addChildren(groupName, children);
		updateLayout();

		return () => {
			if (container) resizeObserver.unobserve(container);
			if (children) removeChildren(groupName, children);
		};
	}, [children]);

	return <Contents ref={containerEl} {...htmlAttrs}>{children}</Contents>;
});
