import GridViewItem from "./GridViewItem";

const StyledGridView = styled.div<{
	/** 子元素图片的宽度。默认为 200px。 */
	$itemWidth?: number;
}>`
	display: grid;
	grid-template-columns: repeat(auto-fill, ${({ $itemWidth = 200 }) => styles.toValue($itemWidth)});
	justify-content: center;
	gap: 4px;

	.expander-child-items & {
		padding: 7px;
	}
`;

export default function GridView<T extends string = string>({ current: [current, setCurrent], $itemWidth, children }: FCP<{
	/** 当前选中项标识符。 */
	current: StateProperty<T>;
	/** 子元素图片的宽度。 */
	$itemWidth?: number;
}>) {
	return (
		<StyledGridView $itemWidth={$itemWidth}>
			{React.Children.map(children, child => {
				if (!isReactInstance(child, GridViewItem)) return child;
				const id = child.props.id as T;
				return (
					React.cloneElement(child, {
						active: current === id,
						onClick: () => setCurrent?.(id),
					})
				);
			})}
		</StyledGridView>
	);
}

GridView.Item = GridViewItem;
