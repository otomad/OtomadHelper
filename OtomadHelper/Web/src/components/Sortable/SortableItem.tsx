import type { DraggableSyntheticListeners, UniqueIdentifier } from "@dnd-kit/core";
import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import { PRESSED_SORTABLE_ITEM_OPACITY } from "./SortableOverlay";

const SortableItemContext = createContext({
	attributes: {} as AnyObject,
	listeners: undefined as DraggableSyntheticListeners,
	ref(_node: HTMLElement | null) { },
});

const StyledSortableItem = styled.li`
	display: flex;
	flex-grow: 1;
	justify-content: space-between;
	align-items: center;
	list-style: none;
	/* border-radius: calc(4px / var(--scale-x, 1));
	box-shadow:
		0 0 0 calc(1px / var(--scale-x, 1)) rgb(63 63 68 / 5%),
		0 1px calc(3px / var(--scale-x, 1)) 0 rgb(34 33 81 / 15%); */
	cursor: ns-resize;
	transition: ${fallbackTransitions}, transform 0s;

	> * {
		inline-size: 100%;
	}

	/* &:active {
		transition: ${fallbackTransitions}, transform 0s;
	} */
`;

export /* @internal */ function SortableItem({ children, id }: FCP<{
	/** Unique identifier. */
	id: UniqueIdentifier;
}>) {
	const [disabled, setDisabled] = useState(false);
	const { attributes, isDragging, listeners, setNodeRef, setActivatorNodeRef, transform, transition } = useSortable({ id, disabled, transition: {
		duration: 250, easing: eases.easeOutMax,
	} });
	const context = useMemo(() => ({ attributes, listeners, ref: setActivatorNodeRef }), [attributes, listeners, setActivatorNodeRef]);

	return (
		<SortableItemContext.Provider value={context}>
			<StyledSortableItem
				ref={setNodeRef}
				style={{
					opacity: isDragging ? PRESSED_SORTABLE_ITEM_OPACITY : undefined,
					transform: CSS.Translate.toString(transform),
					transition,
				}}
				{...attributes}
				{...listeners}
			>
				{children}
				{/* {(() => {
					// let disabled = false;
					const newChildren = React.Children.map(children, child => {
						console.log(child);
						return child;
					});
					// setDisabled(disabled);
					return newChildren;
				})()} */}
			</StyledSortableItem>
		</SortableItemContext.Provider>
	);
}
