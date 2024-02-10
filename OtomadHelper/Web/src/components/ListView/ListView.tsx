import ListViewItem from "./ListViewItem";

const StyledListView = styled.div`
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

	.expander-child-items &.tile {
		padding: 7px 35px;
	}
`;

export default function ListView<T extends string = string>({ view, current: [current, setCurrent], children }: FCP<{
	/** 显示方式：平铺、列表。 */
	view: "list" | "tile";
	/** 当前选中项标识符。 */
	current: StateProperty<T>;
}>) {
	return (
		<StyledListView className={[view]}>
			{React.Children.map(children, child => {
				if (!isReactInstance(child, ListViewItem)) return child;
				const id = child.props.id as T;
				return (
					React.cloneElement(child, {
						active: current === id,
						onClick: () => setCurrent?.(id),
					})
				);
			})}
		</StyledListView>
	);
}

ListView.Item = ListViewItem;
