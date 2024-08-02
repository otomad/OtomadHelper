import type { Active, DragEndEvent, UniqueIdentifier } from "@dnd-kit/core";
import { DndContext, KeyboardSensor, PointerSensor, closestCenter, useSensor, useSensors } from "@dnd-kit/core";
import { restrictToParentElement, restrictToVerticalAxis } from "@dnd-kit/modifiers";
import { SortableContext, arrayMove, sortableKeyboardCoordinates, verticalListSortingStrategy } from "@dnd-kit/sortable";
import nsResizeDraggingCur from "assets/cursors/ns_resize_dragging.svg?cursor";
import SortableItem from "./SortableItem";
import SortableOverlay, { type SortableOverlayEmits } from "./SortableOverlay";

const StyledSortableList = styled.ul.attrs({
	role: "application",
})`
	display: flex;
	flex-direction: column;
	gap: inherit;
	padding: 0;
	list-style: none;

	* {
		transition: ${fallbackTransitions}, transform 0s, opacity 0s;
	}
`;

type BaseItem = {
	/** Unique identifier. */
	id: UniqueIdentifier;
} | UniqueIdentifier;

const getItemId = (item: BaseItem) =>
	isObject(item) ?
		// isStateProperty<UniqueIdentifier>(item.id) ?
		// item.id[0]! :
		item.id :
		item;

const addDatasets = (children: ReactNode, id: UniqueIdentifier, index: number) => React.Children.map(children, child =>
	React.isValidElement(child) ? React.cloneElement(child as never, { "data-id": id, "data-index": index }) : child);

export function SortableList<T extends BaseItem>({ items: itemsStateProperty, overlayEmits, fullyDraggable, children }: FCP<{
	/** List items. The item must have `id` property in it. */
	items: StateProperty<T[]>;
	/** Rendered item. */
	children(states: StatePropertiedObject<T>, index: number, item: T): ReactNode;
	/** Sortable overlay drop animation side effects event handlers. */
	overlayEmits?: SortableOverlayEmits;
	/** Is there no drag handle and you can drag it the whole element? */
	fullyDraggable?: boolean;
}>) {
	if (isStatePropertyPremium(itemsStateProperty))
		itemsStateProperty = itemsStateProperty.useState();
	let [items, setItems] = itemsStateProperty;
	items ??= [];
	const states = useStoreStateArray(itemsStateProperty[0] as never) as StatePropertiedObject<T>[];

	const [active, _setActive] = useState<Active | null>(null);
	const setActive = setStateInterceptor(_setActive, undefined, active => forceCursor(active ? nsResizeDraggingCur : null));
	const activeItem = useMemo(() => {
		const index = items.findIndex(item => getItemId(item) === active?.id);
		if (index === -1) return null;
		return [states[index], index, items[index]] as const;
	}, [active, items]);
	const sensors = useSensors(
		useSensor(PointerSensor),
		useSensor(KeyboardSensor, { coordinateGetter: sortableKeyboardCoordinates }),
	);
	const onDragEnd = ({ active, over }: DragEndEvent) => {
		if (over && active.id !== over?.id) {
			const activeIndex = items.findIndex(item => getItemId(item) === active.id);
			const overIndex = items.findIndex(item => getItemId(item) === over.id);
			setItems?.(arrayMove(items, activeIndex, overIndex));
		}
		setActive(null);
	};

	return (
		<DndContext
			sensors={sensors}
			collisionDetection={closestCenter}
			onDragStart={({ active }) => setActive(active)}
			onDragEnd={onDragEnd}
			onDragCancel={() => setActive(null)}
			modifiers={[restrictToVerticalAxis, restrictToParentElement]}
		>
			<SortableContext items={items} strategy={verticalListSortingStrategy}>
				<StyledSortableList>
					{items.map((item, index) => {
						const id = getItemId(item);
						return (
							<SortableList.Item key={id} id={id} fullyDraggable={fullyDraggable}>
								{addDatasets(children(states[index], index, item), id, index)}
							</SortableList.Item>
						);
					})}
				</StyledSortableList>
			</SortableContext>
			<SortableOverlay {...overlayEmits}>
				{activeItem?.[2] && addDatasets(children(...activeItem), getItemId(activeItem[2]), activeItem[1])}
			</SortableOverlay>
		</DndContext>
	);
}

SortableList.Item = SortableItem;
SortableList.Overlay = SortableOverlay;
