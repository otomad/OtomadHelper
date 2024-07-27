import type { DropAnimation } from "@dnd-kit/core";
import { DragOverlay, defaultDropAnimationSideEffects } from "@dnd-kit/core";

export /* @internal */ const PRESSED_SORTABLE_ITEM_OPACITY = "0.25";

const dropAnimationConfig: DropAnimation = {
	duration: 250,
	easing: eases.easeOutMax,
	sideEffects: defaultDropAnimationSideEffects({
		styles: {
			active: {
				opacity: "0",
			},
		},
	}),
};

export /* @internal */ function SortableOverlay({ children }: FCP) {
	return (
		<Portal container={document.body}>
			<DragOverlay className="drag-overlay" dropAnimation={dropAnimationConfig} style={{ transition: "none" }}>{children}</DragOverlay>
		</Portal>
	);
}
