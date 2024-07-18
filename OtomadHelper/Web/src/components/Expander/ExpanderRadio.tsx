type FieldType<T> = string | ((item: T) => string | undefined) | true;

export default function ExpanderRadio<T>({ items: _items, value: [value, setValue], checkInfoCondition = true, idField, nameField, iconField, imageField, detailsField, view = false, details: _details, $itemWidth, radioGroup, onItemClick, children, itemsViewItemAttrs, ...settingsCardProps }: FCP<Override<PropsOf<typeof Expander>, {
	/** List of options. */
	items: readonly T[];
	/** The identifier of the currently selected value. */
	value: T extends string ? StateProperty<T> : StateProperty<string>;
	/**
	 * The conditions for displaying the currently selected state.
	 * - If it is a string, the constant string is displayed.
	 * - If it is true, it means to determine or fix the currently selected identifier based on the idField and nameField properties.
	 * - If it is `{ id: string; name: string }`, the required name value is looked up based on the current state of the object.
	 * - If it is a callback function, get the required value through the callback function.
	 */
	checkInfoCondition?: string | { id: string; name: string } | true | ((value: string | undefined, items: T[]) => string | undefined);
	/**
	 * The identifier field for the radio item.
	 * - If it is a string, it represents the value of the specified field name of the currently selected object.
	 * - If it is a callback function, get the required value through the callback function.
	 * - If it is true, it means that the selected item is a string, and the identifier can be used directly.
	 */
	idField: FieldType<T>;
	/**
	 * The name field for the radio item.
	 * - If it is a string, it represents the value of the specified field name of the currently selected object.
	 * - If it is a callback function, get the required value through the callback function.
	 * - If it is true, it means that the selected item is a string, and the name can be used directly.
	 */
	nameField: FieldType<T> | object;
	/** The icon field for the radio item. */
	iconField?: FieldType<T> | ((item: T) => ReactNode);
	/** The image field for the radio item. */
	imageField?: FieldType<T> | ((item: T) => ReactNode);
	/** The detailed description field for the radio item. */
	detailsField?: FieldType<T>;
	/** Use list/tile/grid view components instead of radio buttons. */
	view?: ItemView | false;
	/** Detailed description. */
	details?: ReactNode | ((value: string | undefined, items: T[]) => ReactNode);
	/** The width of the child element image when using the grid view component. */
	$itemWidth?: number;
	/** Radio button group name, optional. */
	radioGroup?: string;
	/** Additional attributes for the items view item. */
	itemsViewItemAttrs?: Partial<PropsOf<typeof ItemsView.Item>>;
	/** Fired when the item is clicked. */
	onItemClick?: MouseEventHandler<HTMLElement>;
}>>) {
	const items = _items as AnyObject[];
	const getItemField = (item: T, fieldName: "id" | "name" | "icon" | "image" | "details"): Any => {
		const field = {
			name: nameField,
			id: idField,
			icon: iconField,
			image: imageField,
			details: detailsField,
		}[fieldName];
		return !field ? undefined :
			isI18nItem(field) ? field[getItemField(item, "id")] :
			typeof field === "string" ? (item as AnyObject)[field] :
			typeof field === "function" ? field(item) :
			item;
	};
	const checkInfo = !checkInfoCondition ? undefined :
		typeof checkInfoCondition === "string" ? checkInfoCondition :
		checkInfoCondition === true ? typeof idField === "string" && typeof nameField === "string" ?
			items.find(item => item[idField] === value)?.[nameField] :
			idField && isI18nItem(nameField) ? nameField[value!] : value :
		typeof checkInfoCondition === "function" ? checkInfoCondition(value, items) :
		items.find(item => item[checkInfoCondition.id] === value)?.[checkInfoCondition.name];
	const details = typeof _details === "function" ? _details(value, items) : _details;
	return (
		<Expander {...settingsCardProps} checkInfo={checkInfo} details={details}>
			{!view ? items.map(item => (
				<RadioButton
					value={[value as T, setValue]}
					id={getItemField(item, "id")}
					key={getItemField(item, "id")}
					details={getItemField(item, "details")}
					radioGroup={radioGroup}
					onClick={onItemClick}
				>
					{getItemField(item, "name")}
				</RadioButton>
			)) : (
				<ItemsView view={view} current={[value as T, setValue]} $itemWidth={$itemWidth}>
					{items.map(item => (
						<ItemsView.Item
							id={getItemField(item, "id")}
							key={getItemField(item, "id")}
							image={getItemField(item, "image")}
							icon={getItemField(item, "icon")}
							details={getItemField(item, "details")}
							onClick={onItemClick}
							{...itemsViewItemAttrs}
						>
							{getItemField(item, "name")}
						</ItemsView.Item>
					))}
				</ItemsView>
			)}
			{children}
		</Expander>
	);
}
