import type { Active, DragEndEvent, Modifiers, UniqueIdentifier } from "@dnd-kit/core";
import { DndContext, KeyboardSensor, PointerSensor, useSensor, useSensors } from "@dnd-kit/core";
import { restrictToParentElement, restrictToVerticalAxis } from "@dnd-kit/modifiers";
import { SortableContext, arrayMove, sortableKeyboardCoordinates, verticalListSortingStrategy } from "@dnd-kit/sortable";
import moveDraggingCur from "assets/cursors/move_dragging.svg?cursor";
import nsResizeDraggingCur from "assets/cursors/ns_resize_dragging.svg?cursor";
import { StyledItemsView } from "components/ItemsView/ItemsView";
import ItemsViewItem from "components/ItemsView/ItemsViewItem";
import { useTheme } from "styled-components";
import SortableItem from "./SortableItem";
import SortableOverlay from "./SortableOverlay";

const StyledSortableView = styled(StyledItemsView).attrs({
	role: "application",
	as: "ul",
})`
	list-style: none;

	&.list {
		gap: inherit;
		padding: 0;
	}

	* {
		transition: ${fallbackTransitions}, transform 0s, opacity 0s;
	}
`;

export type PinTo = "top" | "bottom" | undefined;

type BaseItem = {
	/** Unique identifier. */
	id: UniqueIdentifier;
	/** Should the item pin to the top or bottom of the list? */
	pin?: PinTo;
} | UniqueIdentifier;

const getItemId = (item: BaseItem) => isObject(item) ? item.id : item;
const getItemPin = (item: BaseItem) => isObject(item) ? item.pin : undefined;

const addDatasets = (children: ReactNode, id: UniqueIdentifier, index: number, view?: ItemView) => React.Children.map(children, child =>
	isValidElement<AnyObject>(child) ? React.cloneElement(child, {
		"data-id": id,
		"data-index": index,
		...view && isReactInstance(child, ItemsViewItem, "weakest") ? { _view: view } : null,
	}) : child);

const minimumDistanceActivationConstraint = { distance: 15 };

export function SortableView<TItem extends BaseItem>({ items: itemsStateProperty, overlayEmits, fullyDraggable, view = "list", minDistance, nonFocusableForSortableItems, disableKeyboardSensor, children, onReorder }: FCP<{
	/** List items. The item must have `id` property in it. */
	items: StateProperty<TItem[]>;
	/** Rendered item. */
	children(states: StatePropertiedObject<TItem>, index: number, item: TItem): ReactNode;
	/** Sortable overlay drop animation side effects event handlers. */
	overlayEmits?: SortableOverlayEmits;
	/** Is there no drag handle and you can drag it the whole element? */
	fullyDraggable?: boolean;
	/** View mode: list, grid. */
	view?: /* ItemView */ "list" | "grid";
	/** Should user move a little distance before moving the item? Aka static friction. */
	minDistance?: boolean;
	/** Occurs after user reorder an item. */
	onReorder?(fromIndex: number, toIndex: number, items: TItem[]): MaybePromise<void>;
	/** Apply tabIndex = -1 to sortable items? */
	nonFocusableForSortableItems?: boolean;
	/** Stop do sorting when press space bar key? */
	disableKeyboardSensor?: boolean;
}>) {
	if (isStatePropertyPremium(itemsStateProperty))
		itemsStateProperty = itemsStateProperty.useState();
	let [items, setItems] = itemsStateProperty;
	items ??= [];
	const states = useStoreStateArray(itemsStateProperty[0] as never) as StatePropertiedObject<TItem>[];
	const verticalDragOnly = view === "list";
	const modifiers: Modifiers = [
		verticalDragOnly && restrictToVerticalAxis,
		restrictToParentElement,
	].toCompacted();
	const theme = useTheme();

	const [active, _setActive] = useState<Active | null>(null);
	const setActive = setStateInterceptor(_setActive, undefined, active => forceCursor(active ? (verticalDragOnly ? nsResizeDraggingCur : moveDraggingCur)({ theme }) : null));
	const activeIndex = useMemo(() => items.findIndex(item => getItemId(item) === active?.id), [active, items]);
	const sensors = useSensors(
		useSensor(PointerSensor, { activationConstraint: minDistance ? minimumDistanceActivationConstraint : undefined }),
		disableKeyboardSensor ? undefined : useSensor(KeyboardSensor, { coordinateGetter: sortableKeyboardCoordinates }),
	);
	const onDragEnd = ({ active, over }: DragEndEvent) => {
		if (over && active.id !== over?.id) {
			const activeIndex = items.findIndex(item => getItemId(item) === active.id);
			const overIndex = items.findIndex(item => getItemId(item) === over.id);
			setItems?.(arrayMove(items, activeIndex, overIndex));
			onReorder?.(activeIndex, overIndex, items);
		}
		setActive(null);
	};
	const getSortableItems = (expectedPin?: PinTo) => items.map((item, index) => {
		const id = getItemId(item), pin = getItemPin(item);
		if (expectedPin !== pin) return;
		return (
			<SortableItem
				key={id}
				id={id}
				fullyDraggable={fullyDraggable}
				nonFocusable={nonFocusableForSortableItems}
				_view={view}
				_pin={pin}
			>
				{addDatasets(children(states[index], index, item), id, index, view)}
			</SortableItem>
		);
	});
	const sortableItems = getSortableItems();

	return (
		<StyledSortableView className={view}>
			{getSortableItems("top")}
			<DndContext
				sensors={sensors}
				onDragStart={({ active }) => setActive(active)}
				onDragEnd={onDragEnd}
				onDragCancel={() => setActive(null)}
				modifiers={modifiers}
			>
				<SortableContext items={items} strategy={verticalDragOnly ? verticalListSortingStrategy : undefined}>
					{sortableItems}
				</SortableContext>
				<SortableOverlay {...overlayEmits} modifiers={modifiers}>
					{sortableItems[activeIndex]}
					{/* {addDatasets(children(states[activeIndex], activeIndex, items[activeIndex]), getItemId(items[activeIndex]), activeIndex, view)?.[activeIndex]} */}
					{/* { activeItem?.[2] && addDatasets(children(...activeItem!), getItemId(activeItem![2]), activeItem![1], view)} */}
				</SortableOverlay>
			</DndContext>
			{getSortableItems("bottom")}
		</StyledSortableView>
	);
}

SortableView.Item = SortableItem;
SortableView.Overlay = SortableOverlay;
