import type { DropAnimation } from "@dnd-kit/core";
import { DragOverlay, defaultDropAnimationSideEffects } from "@dnd-kit/core";

const dropAnimationConfig: DropAnimation = {
	sideEffects: defaultDropAnimationSideEffects({
		styles: {
			active: {
				opacity: c("pressed-text-opacity"),
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
