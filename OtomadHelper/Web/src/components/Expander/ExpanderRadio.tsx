type FieldType<T> = string | ((item: T) => string) | true;

export default function ExpanderRadio<T>({ items: _items, value: [value, setValue], checkInfoCondition = true, idField, nameField, iconField, captionField, view = false, children, ...settingsCardProps }: FCP<PropsOf<typeof Expander> & {
	/** 选项列表。 */
	items: readonly T[];
	/** 当前选中值的 ID。 */
	value: T extends string ? StateProperty<T> : StateProperty<string>;
	/**
	 * 当前选中状态的显示的条件。
	 * - 如果为字符串，则显示该固定字符串。
	 * - 如果为 true，表示根据 idField 和 nameField 属性来判断或固定当前选中的 ID。
	 * - 如果为 `{ id: string; name: string }`，则根据当前状态的对象查找所需的名称值。
	 * - 如果为回调函数，则通过回调函数获取所需的值。
	 */
	checkInfoCondition?: string | { id: string; name: string } | true | ((value: string | undefined, items: T[]) => string);
	/**
	 * 单选项目的 ID 字段。
	 * - 如果为字符串，表示为当前选中对象的指定字段名称的值。
	 * - 如果为回调函数，则通过回调函数获取所需的值。
	 * - 如果为 true，表示选中项目就是字符串，则 ID 可直接使用之。
	 */
	idField: FieldType<T>;
	/**
	 * 单选项目的名称字段。
	 * - 如果为字符串，表示为当前选中对象的指定字段名称的值。
	 * - 如果为回调函数，则通过回调函数获取所需的值。
	 * - 如果为 true，表示选中项目就是字符串，则名称可直接使用之。
	 */
	nameField: FieldType<T> | object;
	/** 单选项目的图标字段。 */
	iconField?: FieldType<T>;
	/** 单选项目的详细描述字段。 */
	captionField?: FieldType<T>;
	/** 使用列表视图组件而不是单选框。 */
	view?: "list" | "tile" | false;
}>) {
	const items = _items as AnyObject[];
	const getItemField = (item: T, fieldName: "id" | "name" | "icon" | "caption"): Any => {
		const field = {
			name: nameField,
			id: idField,
			icon: iconField,
			caption: captionField,
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
	return (
		<Expander {...settingsCardProps} checkInfo={checkInfo}>
			{!view ? items.map(item =>
				<RadioButton value={[value as T, setValue]} id={getItemField(item, "id")} key={getItemField(item, "id")}>{getItemField(item, "name")}</RadioButton>) :
			(
				<ListView view="tile" current={[value as T, setValue]}>
					{items.map(item =>
						<ListViewItem id={getItemField(item, "id")} key={getItemField(item, "id")} icon={getItemField(item, "icon")} caption={getItemField(item, "caption")}>{getItemField(item, "name")}</ListViewItem>)}
				</ListView>
			)}
			{children}
		</Expander>
	);
}
