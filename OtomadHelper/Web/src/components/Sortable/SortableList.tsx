import type { Active, DragEndEvent, UniqueIdentifier } from "@dnd-kit/core";
import { DndContext, KeyboardSensor, PointerSensor, closestCenter, useSensor, useSensors } from "@dnd-kit/core";
import { restrictToParentElement, restrictToVerticalAxis } from "@dnd-kit/modifiers";
import { SortableContext, arrayMove, sortableKeyboardCoordinates, verticalListSortingStrategy } from "@dnd-kit/sortable";
import { SortableItem } from "./SortableItem";
import { SortableOverlay } from "./SortableOverlay";

const StyledSortableList = styled.ul.attrs({
	role: "application",
})`
	display: flex;
	flex-direction: column;
	padding: 0;
	list-style: none;
`;

type BaseItem = {
	/** Unique identifier. */
	id: UniqueIdentifier;
} | UniqueIdentifier;

const getItemId = (item: BaseItem) => isObject(item) ? item.id : item;

export function SortableList<T extends BaseItem>({ items: [items, setItems], children }: FCP<{
	/** List items. The item must have `id` property in it. */
	items: StateProperty<T[]>;
	/** Rendered item. */
	children(item: T, index: number, items: T[]): ReactNode;
}>) {
	items ??= [];
	const [active, setActive] = useState<Active | null>(null);
	const activeItem = useMemo(() => {
		const index = items.findIndex(item => getItemId(item) === active?.id);
		if (index === -1) return null;
		return [items[index], index, items] as const;
	}, [active, items]);
	const sensors = useSensors(
		useSensor(PointerSensor),
		useSensor(KeyboardSensor, { coordinateGetter: sortableKeyboardCoordinates }),
	);
	const onDragEnd = ({ active, over }: DragEndEvent) => {
		if (over && active.id !== over?.id)
			(setItems as SetStateNarrow<T[]>)?.(items => {
				const activeIndex = items.findIndex(item => getItemId(item) === active.id);
				const overIndex = items.findIndex(item => getItemId(item) === over.id);
				return arrayMove(items, activeIndex, overIndex);
			});
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
					{items.map((item, index, items) => {
						const id = getItemId(item);
						return (
							<SortableList.Item key={id} id={id}>
								{children(item, index, items)}
							</SortableList.Item>
						);
					})}
				</StyledSortableList>
			</SortableContext>
			<SortableOverlay>{activeItem?.[0] && children(...activeItem)}</SortableOverlay>
		</DndContext>
	);
}

SortableList.Item = SortableItem;
