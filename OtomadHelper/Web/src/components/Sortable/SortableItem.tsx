import type { DraggableSyntheticListeners, UniqueIdentifier } from "@dnd-kit/core";
import { useSortable } from "@dnd-kit/sortable";
import { CSS } from "@dnd-kit/utilities";
import moveCur from "assets/cursors/move.svg?cursor";
import nsResizeCur from "assets/cursors/ns_resize.svg?cursor";
import { PRESSED_SORTABLE_ITEM_OPACITY } from "./SortableOverlay";

const SortableItemContext = createContext({
	attributes: {} as AnyObject,
	listeners: undefined as DraggableSyntheticListeners,
	ref(_node: HTMLElement | null) { },
	isDragging: false,
});

const StyledSortableItem = styled.li<{
	/** Is there no drag handle and you can drag it the whole element? */
	$fullyDraggable?: boolean;
	/** View mode: list, grid. */
	$view?: "list" | "grid";
}>`
	display: flex;
	flex-grow: 1;
	justify-content: space-between;
	align-items: center;
	list-style: none;

	&:not(.pinned) {
		${({ $fullyDraggable, $view }) => $fullyDraggable && css`
			cursor: ${$view === "list" ? nsResizeCur : moveCur};
		`}
	}


	> * {
		inline-size: 100%;
	}

	&.dragging {
		pointer-events: none;
	}

	&:is(.dropping) * {
		transition: ${fallbackTransitions}, transform 0s, opacity 0s, background-color 0s;
	}

	:has(> .dragging) > &:not(.dragging) {
		cursor: not-allowed;

		> * {
			pointer-events: none;
		}
	}

	${styles.mixins.forwardFocusRing("> *", true)};
	&:focus-visible,
	&:focus-visible > * {
		transition: ${fallbackTransitions}, ${styles.effects.focusRingTransitions}, transform 0s, opacity 0s;
	}
`;

export /* @internal */ default function SortableItem({ children, id, fullyDraggable, _view: view, _pin: pin, nonFocusable }: FCP<{
	/** Unique identifier. */
	id: UniqueIdentifier;
	/** Is there no drag handle and you can drag it the whole element? */
	fullyDraggable?: boolean;
	/** @private View mode: list, grid. */
	_view?: "list" | "grid";
	/** Should the item pin to the top or bottom of the list? */
	_pin?: PinTo;
	/** Apply tabIndex = -1? */
	nonFocusable?: boolean;
}>) {
	const [disabled, setDisabled] = useState(false);
	const { attributes, isDragging, listeners, setNodeRef, setActivatorNodeRef, transform, transition } = useSortable({ id, disabled, transition: {
		duration: 250, easing: eases.easeOutMax,
	} });
	const context = useMemo(() => ({ attributes, listeners, ref: setActivatorNodeRef, isDragging }), [attributes, listeners, setActivatorNodeRef, isDragging]);
	const liEl = useDomRef<"li">();
	useOnFormKeyDown(liEl, { disabled: isDragging });

	useEffect(() => {
		setDisabled((React.Children.map(children, child =>
			isValidElement<{
				disabled?: boolean;
				"aria-disabled"?: boolean;
			}>(child) ? !!child.props.disabled || (["true", true] as unknown[]).includes(child.props["aria-disabled"]) : false,
		) as boolean[]).includes(true));
	}, [children]);

	return (
		<SortableItemContext value={context}>
			<StyledSortableItem
				ref={el => { liEl.current = el; setNodeRef(el); }}
				className={{ dragging: isDragging, view, pinned: !!pin }}
				style={{
					opacity: isDragging ? PRESSED_SORTABLE_ITEM_OPACITY : undefined,
					transform: CSS.Translate.toString(transform),
					transition,
				}}
				$fullyDraggable={fullyDraggable}
				$view={view}
				{...fullyDraggable && { ...attributes, ...listeners }}
				tabIndex={nonFocusable ? -1 : 0}
			>
				{children}
			</StyledSortableItem>
		</SortableItemContext>
	);
}

SortableItem.Context = SortableItemContext;
SortableItem.verticalDragHandleCursor = nsResizeCur;
