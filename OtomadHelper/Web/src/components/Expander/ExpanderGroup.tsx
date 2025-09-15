const INITIAL_CURRENT_INDEX = -1;

export /* @internal */ default function ExpanderGroup({ autoCollapse = true, children }: FCP<{
	/** Should collapse other expanded expanders automatically when expanding a expander? @default true */
	autoCollapse?: boolean;
}>) {
	const [currentIndex, setCurrentIndex] = useState(INITIAL_CURRENT_INDEX);
	return (
		<>
			{React.Children.map(children, (child, index) => {
				if (!isReactInstance(child, Expander) && !isReactInstance(child, ExpanderRadio)) return child;
				asserts<ReactElementOf<typeof Expander>>(child);
				if (!autoCollapse) return React.cloneElement(child);
				const { expanded, onToggle } = child.props;
				if (autoCollapse && currentIndex === INITIAL_CURRENT_INDEX && expanded)
					setCurrentIndex(index);
				return React.cloneElement(child, {
					expanded: currentIndex === index,
					onToggle(expanded) {
						onToggle?.(expanded);
						if (expanded) setCurrentIndex(index);
					},
				});
			})}
		</>
	);
}
