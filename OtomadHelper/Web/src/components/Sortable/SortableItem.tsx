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
	cursor: ns-resize;

	> * {
		inline-size: 100%;
	}

	&.dragging {
		pointer-events: none;
	}

	&:is(.dropping) * {
		transition: ${fallbackTransitions}, transform 0s, opacity 0s, background-color 0s;
	}
`;

export /* @internal */ default function SortableItem({ children, id }: FCP<{
	/** Unique identifier. */
	id: UniqueIdentifier;
}>) {
	const [disabled, setDisabled] = useState(false);
	const { attributes, isDragging, listeners, setNodeRef, setActivatorNodeRef, transform, transition } = useSortable({ id, disabled, transition: {
		duration: 250, easing: eases.easeOutMax,
	} });
	const context = useMemo(() => ({ attributes, listeners, ref: setActivatorNodeRef }), [attributes, listeners, setActivatorNodeRef]);

	useEffect(() => {
		setDisabled((React.Children.map(children, child =>
			React.isValidElement(child) ? !!child.props.disabled || ["true", true].includes(child.props["aria-disabled"]) : false,
		) as boolean[]).includes(true));
	}, [children]);

	return (
		<SortableItemContext.Provider value={context}>
			<StyledSortableItem
				ref={setNodeRef}
				className={{ dragging: isDragging }}
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
