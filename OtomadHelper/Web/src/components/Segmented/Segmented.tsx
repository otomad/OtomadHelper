import SegmentedItem from "./SegmentedItem";

const THUMB_BORDER_WIDTH = 1;
const ITEM_BASE_MARGIN_X_WIDTH = 3;
const ITEM_BASE_PADDING_X_WIDTH = 11.5;
const ITEM_BASE_ADJUSTED_PADDING_X_WIDTH = ITEM_BASE_PADDING_X_WIDTH + ITEM_BASE_MARGIN_X_WIDTH;
const THUMB_TRANSITION_OPTION = `${eases.easeOutSmooth} 350ms`;

const StyledSegmented = styled.div<{
	/** The child item count. */
	$itemCount?: number;
	/** The selected item index. */
	$selectedIndex?: number;
}>`
	position: relative;
	background-color: ${c("fill-color-control-alt-secondary")};
	border: 1px solid ${c("stroke-color-control-stroke-default")};
	border-radius: 4px;

	@layer props {
		width: fit-content;
	}

	.items,
	.thumb-content {
		display: grid;
		grid-auto-columns: 1fr;
		grid-auto-flow: column;
		width: inherit;

		.item {
			padding: ${ITEM_BASE_MARGIN_X_WIDTH}px;

			> .base {
				${styles.mixins.flexCenter()};
				gap: 10px;
				padding: 7px ${ITEM_BASE_PADDING_X_WIDTH}px;
				border-radius: 2px;
			}

			p {
				${styles.effects.text.body};
				text-align: center;
			}
		}
	}

	.items .item {
		&:hover > .base {
			background-color: ${c("fill-color-subtle-secondary")};
		}

		&:active > .base {
			background-color: ${c("fill-color-subtle-tertiary")};

			> * {
				opacity: ${c("pressed-text-opacity")};
			}
		}

		&.selected ~ .item:not(:last-child),
		&:not(:has(+ .selected)):has(~ .item.selected) {
			padding-inline-end: 0;

			> .base {
				padding-inline-end: ${ITEM_BASE_ADJUSTED_PADDING_X_WIDTH}px;
			}
		}

		&.selected ~ .item:not(.selected + .item),
		&:not(:first-child):has(~ .item.selected) {
			padding-inline-start: 0;

			> .base {
				padding-inline-start: ${ITEM_BASE_ADJUSTED_PADDING_X_WIDTH}px;
			}
		}
	}

	.thumb {
		position: absolute;
		inset-block-start: 0;
		display: flex;
		justify-content: center;
		align-items: flex-end;
		height: calc(100% + ${2 * THUMB_BORDER_WIDTH}px);
		margin: ${-THUMB_BORDER_WIDTH}px;
		background-color: ${c("fill-color-control-default")};
		border: 1px solid ${c("stroke-color-control-stroke-default")};
		border-radius: inherit;
		transition: ${fallbackTransitions}, inset-inline-start ${THUMB_TRANSITION_OPTION};

		${({ $itemCount = 0, $selectedIndex = -1 }) => !$itemCount ? css`
			inset-inline-start: 0;
			width: 0;
		` : css`
			inset-inline-start: calc((100% + ${2 * THUMB_BORDER_WIDTH}px) / ${$itemCount} * ${$selectedIndex});
			width: calc((100% + ${2 * THUMB_BORDER_WIDTH}px) / ${$itemCount});
			${$selectedIndex === -1 && css`opacity: 0;`}
		`}

		${ifColorScheme.dark} &:not(:active, [disabled]) {
			border-top-color: ${c("stroke-color-control-stroke-secondary")};
		}

		${ifColorScheme.light} &:not(:active, [disabled]) {
			border-bottom-color: ${c("stroke-color-control-stroke-secondary")};
		}

		&::after {
			${styles.mixins.oval()};
			content: "";
			display: block;
			width: 16px;
			height: 3px;
			background-color: ${c("accent-color")};
		}

		&:active {
			&::after {
				width: 10px;
			}

			& + .thumb-content {
				opacity: 0.5;
			}
		}
	}

	.thumb-content {
		position: absolute;
		top: 0;
		pointer-events: none;
		clip-path: inset(${({ $itemCount = 0, $selectedIndex = -1 }) => !$itemCount || $selectedIndex === -1 ? "0 100% 0 0" : !isRtl() ?
			`0 calc((1 - (${$selectedIndex} + 1) / ${$itemCount}) * 100%) 0 calc(${$selectedIndex} / ${$itemCount} * 100%)` :
			`0 calc(${$selectedIndex} / ${$itemCount} * 100%) 0 calc((1 - (${$selectedIndex} + 1) / ${$itemCount}) * 100%)`} round 4px);
		transition: ${fallbackTransitions}, clip-path ${THUMB_TRANSITION_OPTION};

		${ifColorScheme.dark} & {
			display: none;
		}
	}
`;

export default function Segmented<T extends string = string>({ current: [current, setCurrent], children }: FCP<{
	/** The identifier of the selected segmented item. */
	current: StateProperty<T>;
}>) {
	const items = React.Children.toArray(children).filter(child => isReactInstance(child, SegmentedItem)) as
		GetReactElementFromFC<typeof SegmentedItem>[];
	const itemCount = items.length;
	const selectedIndex = items.findIndex(item => item.props.id === current);

	const handleDrag = useCallback<PointerEventHandler<HTMLDivElement>>(e => {
		const thumb = e.currentTarget;
		if (!thumb) return;
		const track = thumb.parentElement as HTMLDivElement;
		const pointerMove = (e: PointerEvent) => {
			const { left: trackLeft, width: trackWidth } = track.getBoundingClientRect();
			let index = Math.floor((e.clientX - trackLeft) / trackWidth * itemCount);
			if (isRtl()) index = itemCount - 1 - index;
			if (items[index]) setCurrent?.(items[index].props.id as T);
		};
		const pointerUp = () => {
			document.removeEventListener("pointermove", pointerMove);
			document.removeEventListener("pointerup", pointerUp);
		};
		document.addEventListener("pointermove", pointerMove);
		document.addEventListener("pointerup", pointerUp);
	}, [children]);

	return (
		<StyledSegmented $itemCount={itemCount} $selectedIndex={selectedIndex}>
			<div className="items">
				{items.map(child => {
					const id = child.props.id as T ?? child.props.children;
					return React.cloneElement(child, {
						className: { selected: id === current },
						onClick: () => setCurrent?.(id),
					});
				})}
			</div>
			<div className="thumb" onPointerDown={handleDrag} />
			<div className="thumb-content">
				{items.map(child => React.cloneElement(child))}
			</div>
		</StyledSegmented>
	);
}

Segmented.Item = SegmentedItem;
