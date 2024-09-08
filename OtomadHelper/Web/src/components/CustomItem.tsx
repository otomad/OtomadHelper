const StyledCustomItem = styled.div`
	display: flex;
	flex-wrap: wrap;
	gap: 4px;
	justify-content: space-between;
	align-items: stretch;
	padding: 7px 35px;

	> .items-view-item {
		flex: 1;
		inline-size: 100%;
	}
`;

export default function CustomItem<T extends string = "custom">({ icon = "edit", title = t.custom, details, id = "custom" as T, current: [current, setCurrent], children, ...htmlAttrs }: FCP<{
	/** Icon. */
	icon?: DeclaredIcons | ReactNode;
	/** Title. */
	title?: ReactNode;
	/** Detailed description. */
	details?: ReactNode;
	/** Identifier. */
	id?: T;
	/** The identifier of the currently selected item. */
	current: StateProperty<T>;
	children?: ReactNode | ((setToCustom: () => void) => ReactNode);
}, "div">) {
	const setToCustom = () => setCurrent?.(id);

	return (
		<StyledCustomItem {...htmlAttrs}>
			<ItemsView.Item
				id={id}
				details={details}
				icon={icon}
				selected={current === id}
				onClick={setToCustom}
			>
				{title}
			</ItemsView.Item>
			{typeof children === "function" ? children(setToCustom) : children}
		</StyledCustomItem>
	);
}