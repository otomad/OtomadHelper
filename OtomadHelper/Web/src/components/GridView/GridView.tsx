const StyledGridView = styled.div`
	display: grid;
	grid-template-columns: repeat(auto-fill, 200px);
	justify-content: center;
	gap: 4px;

	.expander-child-items & {
		padding: 7px;
	}
`;

export default function GridView<T extends string = string>({ current: [current, setCurrent], children }: FCP<{
	/** 当前选中项标识符。 */
	current: StateProperty<T>;
}>) {
	return (
		<StyledGridView>
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
