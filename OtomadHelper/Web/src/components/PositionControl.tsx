import { SliderThumb, sliderThumbSize } from "./Slider";

const BUTTON_SIZE = 32;
const KEY_PERCENT = [0, 50, 100] as const;
const childrenStates = ["hover", "pressed"] as const;
const thumbMoveOffset = BUTTON_SIZE * 3 - 8 - sliderThumbSize;
const POINTER_MOVE_THRESHOLD = 5;

const StyledPositionControl = styled.div`
	${styles.mixins.square(BUTTON_SIZE * 3 + "px")}
	position: relative;
	direction: ltr;
	writing-mode: horizontal-tb;
	background-color: ${c("background-fill-color-card-background-secondary")};
	background-clip: padding-box;
	border: 1px solid ${c("stroke-color-control-stroke-default")};
	border-radius: 13px;

	&[disabled] {
		background-color: ${c("background-fill-color-card-background-secondary")};
		border-color: ${c("stroke-color-control-stroke-disabled")};
	}

	${SliderThumb} {
		top: calc(2px + var(--y) / 100 * ${thumbMoveOffset}px);
		left: calc(2px + var(--x) / 100 * ${thumbMoveOffset}px);
		margin: 1px;
	}

	&::after { // Intercept pointer events.
		content: "";
		position: absolute;
		inset: 0;
		display: block;
	}

	.buttons {
		position: absolute;
		inset: 2px;
		display: grid;
		grid-template-columns: repeat(3, 1fr);
		gap: 2px;

		button {
			position: relative;
			border-radius: 4px;

			&::after {
				content: "";
				position: absolute;
				inset: -1px;
				display: block;
			}

			&.hover {
				background-color: ${c("fill-color-subtle-secondary")};
			}

			&.pressed {
				background-color: ${c("fill-color-subtle-tertiary")};
			}

			${Object.entries({
				"top-left": 1,
				"top-right": 3,
				"bottom-left": 7,
				"bottom-right": 9,
			}).map(([corner, nth]) => css`
				&:nth-of-type(${nth}) {
					border-${corner}-radius: 10px;

					&::after {
						border-${corner}-radius: 10px;
					}
				}
			`)}

			${Object.entries({
				left: "3n + 1",
				right: "3n + 3",
				top: "-n + 3",
				bottom: "n + 7",
			}).map(([side, nth]) => css`
				&:nth-of-type(${nth})::after {
					${side}: -3px;
				}
			`)}
		}
	}
`;

export default function PositionControl({ value, disabled, defaultValue = [50, 50], onChanging, onChanged }: FCP<{
	/** Position. */
	value: TwoD;
	/** Disabled? */
	disabled?: boolean;
	/** Default value. Restore defaults when clicking the mouse middle button, right button, or touchscreen long press component. @default [50, 50] */
	defaultValue?: TwoD;
	/** Occurs when the thumb is being dragged. */
	onChanging?(value: TwoD): void;
	/** Occurs when the thumb is lifted after being dragged. */
	onChanged?(value: TwoD): void;
	children?: never;
}>) {
	const thumbEl = useDomRef<"div">(), buttonsEl = useDomRef<"div">();
	const lastPointerAction = useRef<"move" | "down" | "down move" | "up">("up");
	const smoothValue = useSmoothValue(value, 0.5);

	const getHoveredElements = (e: PointerEvent) => document.elementsFromPoint(e.pageX, e.pageY);

	const setChildrenState = (targetElements: Element[], state: typeof childrenStates[number]) => {
		if (!buttonsEl.current || !thumbEl.current) return;
		[thumbEl.current, ...buttonsEl.current.children].forEach(el => childrenStates.forEach(curState =>
			el.classList.toggle(curState, state !== curState ? false : targetElements.includes(el))));
	};

	const handlePointerLeave = () => setChildrenState([], "hover");

	const handlePointerMove: PointerEventHandler<HTMLDivElement> = e => {
		if (lastPointerAction.current.in("down", "down move")) return;
		lastPointerAction.current = "move";
		const hoveredElements = !e.buttons ? getHoveredElements(e) : [];
		setChildrenState(hoveredElements, "hover");
	};

	const handlePointerDown: PointerEventHandler<HTMLDivElement> = e => {
		if (e.button) { e.preventDefault(); return; }
		const thumb = thumbEl.current, target = e.currentTarget as HTMLDivElement, buttons = buttonsEl.current;
		if (!thumb || !buttons) return;
		const thumbRadius = sliderThumbSize / 2;
		const targetLeft = buttons.offsetLeft + thumbRadius,
			targetTop = buttons.offsetTop + thumbRadius,
			targetRight = buttons.offsetLeft + buttons.offsetWidth - thumbRadius,
			targetBottom = buttons.offsetTop + buttons.offsetHeight - thumbRadius;
		// const { offsetLeft: targetLeft, offsetTop: targetTop, offsetWidth: targetWidth, offsetHeight: targetHeight } = buttonsEl.current;
		lastPointerAction.current = "down";
		const hoveredElements = getHoveredElements(e);
		setChildrenState(hoveredElements, "pressed");
		const aborter = new AbortController();
		target.setPointerCapture(e.pointerId);
		const eDown = e;
		let lastPointerMoveEvent: PointerEvent;
		let changingValue: TwoD | undefined;
		const pointerMove = lodash.debounce((e?: PointerEvent, shiftKey?: boolean) => {
			if (e) lastPointerMoveEvent = e;
			e ??= lastPointerMoveEvent;
			if (lastPointerAction.current === "down" && Math.hypot(e.pageX - eDown.pageX, e.pageY - eDown.pageY) <= POINTER_MOVE_THRESHOLD || !lastPointerAction.current.includes("down")) return;
			lastPointerAction.current = "down move";
			setChildrenState([thumb], "pressed");
			changingValue = withShiftKey([
				clampMap(e.offsetX, targetLeft, targetRight, 0, 100),
				clampMap(e.offsetY, targetTop, targetBottom, 0, 100),
			], value, shiftKey ?? e.shiftKey);
			onChanging?.(changingValue);
		});
		target.addEventListener("pointermove", pointerMove, { signal: aborter.signal });
		target.addEventListener("pointerup", () => {
			aborter.abort();
			target.releasePointerCapture(e.pointerId);
			if (changingValue) onChanged?.(changingValue);
		}, { signal: aborter.signal });
		(["keydown", "keyup"] as const).forEach(type => window.addEventListener(type, e => {
			if (e.key === "Shift") pointerMove(undefined, type === "keydown");
		}, { signal: aborter.signal }));
	};

	const handlePointerUp: PointerEventHandler<HTMLDivElement> = e => {
		if (e.button) { e.preventDefault(); return; }
		handlePointerLeave();
		if (lastPointerAction.current === "down" && buttonsEl.current) {
			const hoveredElements = getHoveredElements(e);
			([...buttonsEl.current.children].find(el => hoveredElements.includes(el)) as HTMLButtonElement)?.click();
			setChildrenState(hoveredElements, "hover");
		}
		lastPointerAction.current = "up";
	};

	return (
		<StyledPositionControl
			onPointerMove={handlePointerMove}
			onPointerLeave={handlePointerLeave}
			onPointerUp={handlePointerUp}
			onPointerDown={handlePointerDown}
			onAuxClick={e => { e.preventDefault(); onChanged?.(defaultValue); }}
			onContextMenu={stopEvent}
			disabled={disabled}
		>
			<div ref={buttonsEl} className="buttons">
				{KEY_PERCENT.map(y => KEY_PERCENT.map(x => {
					const position = `${x}% ${y}%`;
					return <button key={position} type="button" tabIndex={-1} onClick={() => onChanged?.([x, y])} />;
				}))}
			</div>
			<SliderThumb ref={thumbEl} style={{ "--x": smoothValue[0], "--y": smoothValue[1] }} />
		</StyledPositionControl>
	);
}

function withShiftKey([x, y]: TwoD, [oldX, oldY]: TwoD, shiftKey: boolean): TwoD {
	if (!shiftKey) return [x, y];
	else if (Math.abs(y - oldY) <= Math.abs(x - oldX)) return [x, oldY];
	else return [oldX, y];
}
