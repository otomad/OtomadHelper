import ItemsViewItem from "./ItemsViewItem";

const isOtherOptionSymbol = Symbol("components.ItemsView.is_other_option");

export /* @internal */ const StyledItemsView = styled.div`
	:has(> &):not(${CONTAINER_CLASSNAMES}) {
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
		grid-template-columns: repeat(auto-fill, var(--grid-template-width));
		gap: 4px;
		justify-content: center;
		align-items: start;
		transition: ${fallbackTransitions}, --grid-template-width ${eases.easeOutMax} 250ms;

		&.auto-fill {
			grid-template-columns: repeat(auto-fill, minmax(var(--grid-template-width), 1fr));
		}
	}

	&:empty {
		display: none;
	}

	.expander-child-items & {
		&.tile {
			padding: 7px 32px;
		}

		&.grid {
			padding: 7px;
		}

		&.list .items-view-item > .base {
			padding-inline: 23.5px;
		}
	}

	@layer components {
		.items-view-item {
			${tgs()} {
				scale: 0.75;
				opacity: 0;
			}
		}
	}
`;

export default function ItemsView<
	T,
	TMultiple extends boolean = false,
>({ view, current: _current, itemWidth, multiple = false as TMultiple, indeterminatenesses = [], children, className, role, transition, style, inlineAlignment, autoFill, readOnly, emptyState, "aria-label": ariaLabel, selectAll, onItemCountChange, onItemEmptyChange, ...htmlAttrs }: FCP<{
	/** View mode: list, tile, grid. */
	view: ItemView;
	/**
	 * The identifier of the currently selected item.
	 *
	 * If it is `null`, items view will not select its items, and you can control them by yourself.
	 */
	current: StateProperty<TMultiple extends true ? T[] : T> | null;
	/**
	 * In grid view, the width of the child element image.
	 *
	 * If it is "square", it will result the image of the grid view item become square.
	 *
	 * @default 200px
	 */
	itemWidth?: number | "square";
	/** Multiple selection mode? */
	multiple?: TMultiple;
	/** Specifies which items are set to an indeterminate state. */
	indeterminatenesses?: T[];
	/**
	 * Override the default aria role attribute.
	 *
	 * Note that if you want to remove the role attribute, pass it `null` instead of `undefined`,
	 * or nothing will happened.
	 */
	role?: AriaRole | null;
	/** Enable transition group for items view items. Passing a string represents it as the transition name. */
	transition?: boolean | string;
	/** Inline alignment (justify content). */
	inlineAlignment?: CSSProperties["justifyContent"];
	/** Auto fill items (grid view only). */
	autoFill?: boolean;
	/** Make items cannot be selected? */
	readOnly?: boolean;
	/** Show something while nothing in the items. */
	emptyState?: ReactNode;
	/** Show select all and invert selection control. Only available when `multiple` is true. */
	selectAll?: TMultiple extends true ? Partial<PropsOf<typeof SelectAll>> | true : never;
	/** Occurs when the item count changes. */
	onItemCountChange?(length: number): void;
	/** Occurs when the item count toggles zero. */
	onItemEmptyChange?(empty: boolean): void;
}, "div">) {
	if (itemWidth === "square") itemWidth = GRID_VIEW_ITEM_HEIGHT;

	const [current, setCurrent] = _current ?? [];

	const allIds = (React.Children.toArray(children).map(child => {
		if (!isReactInstance(child, ItemsViewItem, "weakest")) return undefined;
		return child.props.id as T;
	}) ?? []).toCompacted();

	const isOther = multiple ? (current as T[] ?? []).every(id => !allIds.includesDeep(id)) : !allIds.includesDeep(current);

	const isSelected = (id: T) => {
		if (id === isOtherOptionSymbol) return isOther;
		else if (multiple)
			if (Array.isArray(current)) return current.includesDeep(id);
			else return false;
		else return lodash.isEqual(current, id);
	};

	const handleClick = (id: T) => {
		if (id === isOtherOptionSymbol) return;
		setCurrent?.((
			!multiple ? id : produce((draft: T[]) => {
				draft.toggleDeep(id);
			})
		) as never);
	};

	const items = React.Children.map(children, child => {
		if (!isReactInstance(child, ItemsViewItem, "weakest")) return child;
		const id = child.props.id as T;
		const key = child.key;
		const onParentClick = child.props.onClick;
		const item = React.cloneElement(child, {
			_view: view,
			_multiple: multiple,
			..._current !== null && {
				selected: !isSelected(id) ? "unchecked" : indeterminatenesses.includesDeep(id) ? "indeterminate" : "checked",
				onClick: (...e: Parameters<OnItemsViewItemClickEventHandler<unknown>>) => { handleClick(id); onParentClick?.(...e); },
			},
		});
		if (!transition) return item;
		else return (
			<CssTransition
				key={key}
				classNames={typeof transition === "string" ? transition : undefined}
				unmountOnExit
				maxTimeout={250}
			>
				{item}
			</CssTransition>
		);
	});

	const itemCount = useMemo(() => items?.length ?? 0, [items]);
	const isEmpty = useMemo(() => itemCount === 0, [itemCount]);
	useEffect(() => onItemCountChange?.(itemCount), [itemCount, onItemCountChange]);
	useEffect(() => onItemEmptyChange?.(isEmpty), [isEmpty, onItemEmptyChange]);

	return (
		<>
			{multiple && selectAll && !isEmpty && <SelectAll value={[current, setCurrent] as StateProperty<T[]>} all={allIds} {...selectAll === true ? {} : selectAll as never} />}
			<StyledItemsView
				className={[className, view, { autoFill }]}
				role={role === null ? undefined : role === undefined ? multiple ? "group" : "radiogroup" : role}
				aria-label={ariaLabel ?? (role === undefined && multiple ? t.aria.checkboxGroup : undefined)}
				aria-hidden={false}
				style={{
					...style,
					justifyContent: inlineAlignment,
					"--grid-template-width": view === "grid" ? styles.toValue(itemWidth) : undefined,
				}}
				inert={readOnly}
				data-is-other={isOther}
				aria-readonly={readOnly}
				{...htmlAttrs}
			>
				{(() => {
					if (isEmpty && emptyState) return emptyState;
					if (!transition) return items;
					else return <TransitionGroup component={null}>{items}</TransitionGroup>;
				})()}
			</StyledItemsView>
		</>
	);
}

ItemsView.Item = ItemsViewItem;
ItemsView.other = isOtherOptionSymbol;
