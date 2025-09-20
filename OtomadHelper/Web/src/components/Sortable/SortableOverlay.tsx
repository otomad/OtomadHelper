import type { DropAnimation, Modifiers } from "@dnd-kit/core";
import { DragOverlay } from "@dnd-kit/core";

export /* @internal */ const PRESSED_SORTABLE_ITEM_OPACITY = "0.25";
const DRAGGING_SCALE = 1.025;

const StyledSortableOverlay = styled(DragOverlay)`
	--sortable-overlay-scale: ${DRAGGING_SCALE};

	&,
	* {
		transition: ${fallbackTransitions}, transform 0s;
	}

	> * {
		${styles.mixins.inherit("block-size", "inline-size", true)};
		scale: var(--sortable-overlay-scale);
		transition: ${fallbackTransitions}, scale 250ms cubic-bezier(0.18, 0.67, 0.6, 1.22), opacity 100ms 250ms;

		${ifColorScheme.reduceMotion} {
			transition: scale 1ms step-end !important;
		}

		@starting-style {
			scale: 1;
		}
	}
`;

const dropAnimationConfig = (emits: SortableOverlayEmits): DropAnimation => async e => {
	if (useMediaQuery.reduceMotion({ noHook: true })) {
		emits.onDrop?.(e);
		emits.onDropped?.(e);
		emits.onDropEnd?.(e);
		return;
	}

	const { transform, active, dragOverlay } = e;
	active.node.style.opacity = PRESSED_SORTABLE_ITEM_OPACITY;
	await nextAnimationTick();
	active.node.classList.add("dragging", "dropping");
	dragOverlay.node.classList.add("dropping");
	emits.onDrop?.(e);

	active.node.animate([{}, { opacity: 0 }], { easing: eases.easeOutQuad, duration: 250 });
	const transformAnimation = dragOverlay.node.animate([
		{ },
		{
			transform: [
				`translateX(${transform.x - dragOverlay.rect.left + active.rect.left - dragOverlay.rect.width * (DRAGGING_SCALE - 1) / 2}px)`,
				`translateY(${transform.y - dragOverlay.rect.top + active.rect.top - dragOverlay.rect.height * (DRAGGING_SCALE - 1) / 2}px)`,
			].join(" "),
			"--sortable-overlay-scale": 1,
		},
	], { easing: eases.easeOutMax, duration: 250, fill: "forwards" });
	await transformAnimation.finished;
	transformAnimation.commitStyles();

	active.node.style.opacity = null!;
	dragOverlay.node.hidden = true;
	active.node.classList.remove("dragging");
	emits.onDropped?.(e);
	await delay(100);
	active.node.classList.remove("dropping");
	dragOverlay.node.classList.remove("dropping");
	emits.onDropEnd?.(e);
};

export interface SortableOverlayEmits {
	/** Occurs when user release their pointer, the drag overlay started to move to the correct place. */
	onDrop?: DropAnimationSideEffects;
	/** Occurs when the drag overlay moved to the correct place. */
	onDropped?: DropAnimationSideEffects;
	/** Occurs after the drag overlay moved to the correct place. */
	onDropEnd?: DropAnimationSideEffects;
}

export /* @internal */ default function SortableOverlay({ modifiers, children, ...emits }: FCP<SortableOverlayEmits & {
	/**
	 * Modifiers let you dynamically modify the movement coordinates that are detected by sensors.
	 * They can be used for a wide range of use cases, for example:
	 * - Restricting motion to a single axis;
	 * - Restricting motion to the draggable node container's bounding rectangle;
	 * - Restricting motion to the draggable node's scroll container bounding rectangle;
	 * - Applying resistance or clamping the motion.
	 */
	modifiers?: Modifiers;
}>) {
	const reduceMotion = useMediaQuery.reduceMotion();
	return (
		<Portal container={document.body}>
			<StyledSortableOverlay
				className={ifColorScheme.forceMotion}
				adjustScale
				dropAnimation={dropAnimationConfig(emits)}
				modifiers={modifiers}
				transition={reduceMotion ? "none" : undefined}
			>
				{children}
			</StyledSortableOverlay>
		</Portal>
	);
}
// FIXME: 开启 minimumDistanceActivationConstraint 后，如果鼠标一开始快速拖动元素，则有可能导致元素突然飞出去且定位错误。
