import type { DropAnimation } from "@dnd-kit/core";
import { DragOverlay } from "@dnd-kit/core";
import { restrictToParentElement, restrictToVerticalAxis } from "@dnd-kit/modifiers";
import { CSS } from "@dnd-kit/utilities";

export /* @internal */ const PRESSED_SORTABLE_ITEM_OPACITY = "0.25";
const DRAGGING_SCALE = 1.025;

const pop = forthBack_keyframes`
	from {
		scale: 1;
	}

	to {
		scale: ${DRAGGING_SCALE};
	}
`;

const StyledSortableOverlay = styled(DragOverlay)`
	* {
		transition: ${fallbackTransitions}, transform 0s, opacity 0s;
	}

	> * {
		block-size: inherit;
		inline-size: inherit;
		animation: ${pop[0]} 250ms cubic-bezier(0.18, 0.67, 0.6, 1.22) forwards;
	}

	&.dropping > * {
		animation-name: ${pop[1]};
	}
`;

const dropAnimationConfig = (emits: SortableOverlayEmits): DropAnimation => ({
	duration: 250,
	easing: eases.easeOutMax,
	keyframes: ({ transform }) => [
		{ transform: CSS.Transform.toString(transform.initial) },
		{ transform: CSS.Transform.toString({ ...transform.final, x: 0 }) }, // Don't adjust translate X.
	],
	sideEffects: e => {
		const { active, dragOverlay } = e;
		active.node.style.opacity = PRESSED_SORTABLE_ITEM_OPACITY;
		active.node.classList.add("dragging", "dropping");
		dragOverlay.node.classList.add("dropping");
		emits.onDrop?.(e);

		return async () => {
			active.node.style.opacity = null!;
			active.node.classList.remove("dragging");
			emits.onDropped?.(e);
			await delay(100);
			active.node.classList.remove("dropping");
			dragOverlay.node.classList.remove("dropping");
			emits.onDropEnd?.(e);
		};
	},
});

export interface SortableOverlayEmits {
	/** Fired when the user release their pointer, the drag overlay started to move to the correct place. */
	onDrop?: DropAnimationSideEffects;
	/** Fired when the drag overlay moved to the correct place. */
	onDropped?: DropAnimationSideEffects;
	/** Fired after the drag overlay moved to the correct place. */
	onDropEnd?: DropAnimationSideEffects;
}

export /* @internal */ default function SortableOverlay({ children, ...emits }: FCP<SortableOverlayEmits>) {
	return (
		<Portal container={document.body}>
			<StyledSortableOverlay dropAnimation={dropAnimationConfig(emits)} modifiers={[restrictToVerticalAxis, restrictToParentElement]}>{children}</StyledSortableOverlay>
		</Portal>
	);
}
