import type { DropAnimation } from "@dnd-kit/core";
import { DragOverlay } from "@dnd-kit/core";

export /* @internal */ const PRESSED_SORTABLE_ITEM_OPACITY = "0.25";

const GlobalStyle = createGlobalStyle`
	.sortable-list *,
	.sortable-overlay,
	.sortable-overlay * {
		transition: ${fallbackTransitions}, transform 0s, opacity 0s;
	}

	.sortable-overlay * {
		transition: ${fallbackTransitions}, transform 0s, opacity 0s, background-color 0s;
	}
`;

const dropAnimationConfig: DropAnimation = {
	duration: 250,
	easing: eases.easeOutMax,
	sideEffects: ({ active, dragOverlay }) => {
		active.node.style.opacity = PRESSED_SORTABLE_ITEM_OPACITY;
		active.node.classList.add("dragging", "dropping");
		dragOverlay.node.classList.add("dropping");

		return async () => {
			active.node.style.removeProperty("opacity");
			active.node.classList.remove("dragging");
			dragOverlay.node.classList.remove("dropping");
			await delay(300);
			active.node.classList.remove("dropping");
		};
	},
};

export /* @internal */ default function SortableOverlay({ children }: FCP) {
	return (
		<Portal container={document.body}>
			<GlobalStyle />
			<DragOverlay className="sortable-overlay" dropAnimation={dropAnimationConfig}>{children}</DragOverlay>
		</Portal>
	);
}
