type FieldType<T> = string | ((item: T) => string | undefined) | true;

export default function ExpanderRadio<TItem, TKey extends PropertyKey>({ items: _items, value: [value, setValue], checkInfoCondition = true, idField, nameField, iconField, imageField, detailsField, imageOverlayField, badgeField, view = "radio", details: _details, itemWidth, radioGroup, itemsViewItemAttrs, itemsViewAttrs, radioButtonAttrs, hideCustom = true, before, transition, readOnly, title, checkInfo: staticCheckInfo, children, onItemClick, onItemContextMenu, ...settingsCardProps }: FCP<Override<PropsOf<typeof Expander>, {
	/** List of options. */
	items: readonly TItem[];
	/** The identifier of the currently selected value. */
	value: Readonly<StateProperty<TKey>>;
	/**
	 * The conditions for displaying the currently selected state.
	 * - If it is a string, the constant string is displayed.
	 * - If it is true, it means to determine or fix the currently selected identifier based on the idField and nameField properties.
	 * - If it is `{ id: TKey; name: string }`, the required name value is looked up based on the current state of the object.
	 * - If it is a callback function, get the required value through the callback function.
	 */
	checkInfoCondition?: string | { id: TKey; name: string } | true | ((value: TKey | undefined, items: TItem[]) => ReactNode | undefined);
	/**
	 * The identifier field for the radio item.
	 * - If it is a string, it represents the value of the specified field name of the currently selected object.
	 * - If it is a callback function, get the required value through the callback function.
	 * - If it is true, it means that the selected item is a string, and the identifier can be used directly.
	 */
	idField: FieldType<TItem>;
	/**
	 * The name field for the radio item.
	 * - If it is a string, it represents the value of the specified field name of the currently selected object.
	 * - If it is a callback function, get the required value through the callback function.
	 * - If it is true, it means that the selected item is a string, and the name can be used directly.
	 * - If it is an i18n item object, it will get the value of the object from the value as the key.
	 */
	nameField?: FieldType<TItem> | object | ((item: TItem) => ReactNode) | ((item: TItem) => (ariaId: string) => ReactNode);
	/** The icon field for the radio item. */
	iconField?: FieldType<TItem> | ((item: TItem) => ReactNode) | DeclaredIcons | ReactNode;
	/** The image field for the radio item. */
	imageField?: FieldType<TItem> | ((item: TItem) => ReactNode);
	/** The detailed description field for the radio item. */
	detailsField?: FieldType<TItem> | object | ((item: TItem) => ReactNode);
	/** Other elements overlay the image field for the radio item. */
	imageOverlayField?: FieldType<TItem> | ((item: TItem) => ReactNode);
	/** The badge field for the radio item. You must get it by a callback which return a badge node. */
	badgeField?: (item: TItem) => BadgeValue | BadgeArgs;
	/** Use list/tile/grid view components instead of radio buttons. */
	view?: ItemView | "radio";
	/** Detailed description. */
	details?: ReactNode | ((value: TKey | undefined, items: TItem[]) => ReactNode);
	/**
	 * The width of the child element image when using the grid view component.
	 *
	 * If it is "square", it will result the image of the grid view item become square.
	 */
	itemWidth?: PropsOf<typeof ItemsView>["itemWidth"];
	/** Radio button group name, optional. */
	radioGroup?: string;
	/** Additional attributes for the items view item. */
	itemsViewItemAttrs?: Partial<PropsOf<typeof ItemsView.Item>> | false | ((item: TItem) => (Partial<PropsOf<typeof ItemsView.Item>> | undefined | false));
	/** Additional attributes for the items view. */
	itemsViewAttrs?: Partial<PropsOf<typeof ItemsView>>;
	/** Additional attributes for the radio button. */
	radioButtonAttrs?: Partial<Omit<PropsOf<typeof RadioButton>, "value">> | false | ((item: TItem) => (Partial<Omit<PropsOf<typeof RadioButton>, "value">> | undefined | false));
	/**
	 * Remove the "custom" option from the options so that you can customize the "custom" form control.
	 *
	 * Enabled by default, and the default assumption is that the identifier of the "custom" option is `"custom"`.
	 * If you pass a string to this parameter, the string will be used as the identifier of the "custom" option.
	 *
	 * If you don't want this feature, pass `false`.
	 */
	hideCustom?: boolean | string;
	/** The children before its items. */
	before?: ReactNode;
	/** Enable transition group for items view items. Passing a string represents it as the transition name. */
	transition?: boolean | string;
	/** Make items cannot be selected? */
	readOnly?: boolean;
	/** Occurs when the item left clicked. */
	onItemClick?(item: TItem, event: React.MouseEvent<HTMLElement>): void;
	/** Occurs when the item right clicked. */
	onItemContextMenu?(item: TItem, event: React.MouseEvent<HTMLElement>): void;
}>>) {
	const getItemField = (item: TItem, fieldName: "id" | "name" | "icon" | "image" | "details" | "imageOverlay"): Any => {
		const field = {
			name: nameField,
			id: idField,
			icon: iconField,
			image: imageField,
			details: detailsField,
			imageOverlay: imageOverlayField,
		}[fieldName];
		return !field ? undefined :
			isI18nItem(field) ? String(field[getItemField(item, "id")]) :
			typeof field === "string" ? (item as AnyObject)[field] :
			typeof field === "function" ? field(item) :
			item;
	};
	const items = _items as AnyObject[];
	const filteredItems = useMemo(() => hideCustom === false ? items : items.filter(item =>
		getItemField(item, "id") !== (typeof hideCustom === "string" ? hideCustom : "custom")), [_items, getItemField, hideCustom, items]);
	const checkInfo = staticCheckInfo || (!checkInfoCondition ? undefined :
		typeof checkInfoCondition === "string" ? checkInfoCondition :
		checkInfoCondition === true ? typeof idField === "string" && typeof nameField === "string" ?
			items.find(item => item[idField] === value)?.[nameField] :
			idField && isI18nItem(nameField) ? String(nameField[value as string]) : value :
		typeof checkInfoCondition === "function" ? checkInfoCondition(value, items) :
		items.find(item => item[checkInfoCondition.id] === value)?.[checkInfoCondition.name]);
	const details = typeof _details === "function" ? _details(value, items) : _details;

	return (
		<Expander
			{...settingsCardProps}
			childRole="radiogroup"
			title={title}
			checkInfo={checkInfo}
			details={details}
		>
			{before}
			{view === "radio" ? filteredItems.map(item => (
				<RadioButton
					value={[value, setValue]}
					id={getItemField(item, "id")}
					key={getItemField(item, "id")}
					icon={getItemField(item, "icon")}
					details={getItemField(item, "details")}
					radioGroup={radioGroup}
					readOnly={readOnly}
					onClick={e => onItemClick?.(item, e)}
					onContextMenu={e => onItemContextMenu?.(item, e)}
					{...typeof radioButtonAttrs === "function" ? radioButtonAttrs(item) : radioButtonAttrs}
				>
					{getItemField(item, "name")}
				</RadioButton>
			)) : (
				<ItemsView
					view={view}
					current={[value, setValue]}
					itemWidth={itemWidth}
					role={null}
					transition={transition}
					readOnly={readOnly}
					{...itemsViewAttrs as Any}
				>
					{filteredItems.map(item => (
						<ItemsView.Item
							id={getItemField(item, "id")}
							key={getItemField(item, "id")}
							image={getItemField(item, "image")}
							icon={getItemField(item, "icon")}
							details={getItemField(item, "details")}
							imageOverlay={getItemField(item, "imageOverlay")}
							badge={badgeField?.(item)}
							onClick={(_1, _2, e) => onItemClick?.(item, e)}
							onContextMenu={e => onItemContextMenu?.(item, e)}
							{...typeof itemsViewItemAttrs === "function" ? itemsViewItemAttrs(item) : itemsViewItemAttrs}
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

function ExpanderRadioEnum<T extends AnyEnum>({ items, ...otherProps }: Override<PropsOf<typeof ExpanderRadio<T["array"][0], T["keyType"]>>, {
	items: T;
	value: Readonly<StateProperty<T["keyType"]>>;
	idField?: never;
}>) {
	return (
		<ExpanderRadio
			items={items.array}
			idField="key"
			nameField="label"
			iconField="icon"
			checkInfoCondition={key => items.all[key]?.label}
			{...otherProps}
		/>
	);
}

ExpanderRadio.Enum = ExpanderRadioEnum;
