import PillButton from "./PillButton";

const StyledFilter = styled(HorizontalScroll)`
	${styles.mixins.noScrollbar()};
	container: filter / scroll-state inline-size;
	display: flex;
	flex-shrink: 0;
	gap: 0;
	align-items: center;
	padding-block: 4px;
	overflow: auto visible;

	* {
		flex-shrink: 0;
	}

	.pills {
		${styles.mixins.inherit("display", "align-items")};
		position: relative;
		gap: 4px;
	}
`;

export default function Filter<T extends PropertyKey>({ current: [current, setCurrent], children, ...htmlAttrs }: FCP<{
	/** The identifier of the currently selected item. */
	current: StateProperty<T>;
}, "div">) {
	return (
		<StyledFilter role="radiogroup" {...htmlAttrs}>
			<Flipper arrow="left" />
			<div className="pills">
				{React.Children.map(children, child => {
					if (!isReactInstance(child, PillButton)) return child;
					const id = child.props.id as T;
					return React.cloneElement(child, {
						selected: current === id,
						onClick: () => setCurrent?.(id),
					});
				})}
			</div>
			<Flipper arrow="right" />
		</StyledFilter>
	);
}

Filter.Item = PillButton;
