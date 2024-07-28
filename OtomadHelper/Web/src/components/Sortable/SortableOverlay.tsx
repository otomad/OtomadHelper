import type { DropAnimation } from "@dnd-kit/core";
import { DragOverlay } from "@dnd-kit/core";

export /* @internal */ const PRESSED_SORTABLE_ITEM_OPACITY = "0.25";

const StyledSortableOverlay = styled(DragOverlay)`
	* {
		transition: ${fallbackTransitions}, transform 0s, opacity 0s;
	}

	> * {
		block-size: inherit;
		inline-size: inherit;
	}
`;

export interface SortableOverlayEmits {
	/** Fired when the user release their pointer, the drag overlay started to move to the correct place. */
	onDrop?: DropAnimationSideEffects;
	/** Fired when the drag overlay moved to the correct place. */
	onDropped?: DropAnimationSideEffects;
	/** Fired after the drag overlay moved to the correct place. */
	onDropEnd?: DropAnimationSideEffects;
}

const dropAnimationConfig = (emits: SortableOverlayEmits): DropAnimation => ({
	duration: 250,
	easing: eases.easeOutMax,
	sideEffects: e => {
		const { active, dragOverlay } = e;
		active.node.style.opacity = PRESSED_SORTABLE_ITEM_OPACITY;
		active.node.classList.add("dragging", "dropping");
		dragOverlay.node.classList.add("dropping");
		emits.onDrop?.(e);

		return async () => {
			active.node.style.removeProperty("opacity");
			active.node.classList.remove("dragging");
			emits.onDropped?.(e);
			await delay(100);
			active.node.classList.remove("dropping");
			dragOverlay.node.classList.remove("dropping");
			emits.onDropEnd?.(e);
		};
	},
});

export /* @internal */ default function SortableOverlay({ children, ...emits }: FCP<SortableOverlayEmits>) {
	return (
		<Portal container={document.body}>
			<StyledSortableOverlay dropAnimation={dropAnimationConfig(emits)}>{children}</StyledSortableOverlay>
		</Portal>
	);
}
