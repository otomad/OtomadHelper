import ItemsViewItem from "./ItemsViewItem";

const StyledItemsView = styled.div<{
	/** 网格视图下，子元素图片的宽度。默认为 200px。 */
	$itemWidth?: number;
}>`
	:has(> &) {
		container: list-view / inline-size;
	}

	&.list {
		display: flex;
		flex-direction: column;
	}

	&.tile {
		display: grid;
		grid-template-columns: repeat(2, 1fr);

		@container list-view (width < 628px) {
			grid-template-columns: repeat(1, 1fr);
		}
	}

	&.grid {
		display: grid;
		grid-template-columns: repeat(auto-fill, ${({ $itemWidth = 200 }) => styles.toValue($itemWidth)});
		gap: 4px;
		justify-content: center;
	}

	.expander-child-items & {
		&.tile {
			padding: 7px 35px;
		}

		&.grid {
			padding: 7px;
		}
	}
`;

export default function ItemsView<T extends string = string>({ view, current: [current, setCurrent], $itemWidth, children }: FCP<{
	/** 显示方式：列表、平铺、网格。 */
	view: "list" | "tile" | "grid";
	/** 当前选中项标识符。 */
	current: StateProperty<T>;
	/** 网格视图下，子元素图片的宽度。 */
	$itemWidth?: number;
}>) {
	return (
		<StyledItemsView className={[view]} $itemWidth={$itemWidth}>
			{React.Children.map(children, child => {
				if (!isReactInstance(child, ItemsViewItem)) return child;
				const id = child.props.id as T;
				return (
					React.cloneElement(child, {
						selected: current === id,
						view,
						onClick: () => setCurrent?.(id),
					})
				);
			})}
		</StyledItemsView>
	);
}

ItemsView.Item = ItemsViewItem;
