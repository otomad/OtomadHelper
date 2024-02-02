const THUMB_SIZE = 20;
const TRACK_THICKNESS = 4;
const valueCalc = "calc(var(--value) * (100% - var(--thumb-size)))";
const thumbSizeHalf = "calc(var(--thumb-size) / 2)";

const StyledSlider = styled.div`
	--value: 0;
	--buffered: 0;
	position: relative;
	touch-action: none;

	> * {
		--thumb-size: ${THUMB_SIZE}px;
		--track-thickness: ${TRACK_THICKNESS}px;
	}

	.track {
		height: var(--thumb-size);
		padding: ${thumbSizeHalf} 0;

		&::after {
			display: block;
			background-color: ${c("fill-color-control-strong-default")};
			content: "";
		}
	}

	.track::after,
	.passed {
		${styles.mixins.oval()};
		height: var(--track-thickness);
	}

	.passed {
		position: absolute;
		top: 0;
		margin-top: 0;
		transition: none;
		pointer-events: none;
		margin: ${thumbSizeHalf} 0;
	}

	.passed {
		width: ${valueCalc};
		background-color: ${c("accent-color")};
	}

	.thumb {
		${styles.mixins.square("var(--thumb-size)")};
		${styles.mixins.circle()};
		${styles.mixins.flexCenter()};
		position: absolute;
		top: calc(var(--track-thickness) / 2);
		left: ${valueCalc};
		background-color: ${c("fill-color-control-solid-default")};
		transition: all ${eases.easeOutMax} 250ms, left 0s;
		box-shadow:
			0 0 0 1px ${c("stroke-color-control-stroke-default")},
			0 1px 0 ${c("stroke-color-control-stroke-default")};

		&::after {
			${styles.mixins.square("100%")};
			${styles.mixins.circle()};
			display: block;
			background-color: ${c("accent-color")};
			transition: all ${eases.easeOutMax} 250ms, scale ${eases.easeOutBack} 250ms;
			content: "";
			scale: ${12 / 20};
		}

		&:hover::after {
			scale: ${14 / 20};
		}
	}

	.track:active ~ .thumb::after,
	.thumb:active::after {
		scale: ${10 / 20} !important;
	}

	${styles.mixins.forwardFocusRing("thumb")};
`;

export default function Slider({ value: [value, setValue], min = 0, max = 100, defaultValue, step, keyStep = 1, onChanging, onChanged }: FCP<{
	/** 当前值。 */
	value: StateProperty<number>;
	/** 滑块最小值。 */
	min?: number;
	/** 滑块最大值。 */
	max?: number;
	/** 滑块默认值。当单击鼠标中键或触摸屏长按组件时还原默认值。 */
	defaultValue?: number;
	/** 滑块有效的增量值。 */
	step?: number;
	/** 指定当按下键盘方向按键时，滑块单次调整的值。 */
	keyStep?: number;
	/** 当滑块拖动时即触发事件。 */
	onChanging?: (value: number) => void;
	/** 当滑块拖动完成抬起后才触发事件。 */
	onChanged?: (value: number) => void;
}>) {
	const errorInfo = `The value range should be between [${min} ~ ${max}], with the current value being ${value}.`;
	if (value === undefined)
		throw new ReferenceError("value undefined");
	if (min > max)
		throw new RangeError(`Is the minimum value of Slider greater than the maximum value? The minimum value is ${min}, and the maximum value is ${max}`);
	if (value < min)
		throw new RangeError("The value of Slider is lesser than the minimum value. " + errorInfo);
	if (value > max)
		throw new RangeError("The value of Slider is greater than the maximum value. " + errorInfo);

	const restrict = useCallback((n: number | undefined, nanValue: number) => Number.isFinite(n) ? clamp(map(n!, min, max, 0, 1), 0, 1) : nanValue, [min, max]);
	const sharpValue = useMemo(() => restrict(value, 0), [value, min, max]);
	const smoothValue = useSmoothValue(sharpValue, 0.5); // 修改这个参数可以调整滑动条的平滑移动值。

	const resetDefault = useCallback((e: MouseEvent) => {
		e.preventDefault();
		if (defaultValue !== undefined && Number.isFinite(defaultValue)) {
			setValue?.(defaultValue);
			onChanging?.(defaultValue);
			onChanged?.(defaultValue);
		}
	}, []);

	const clampValue = useCallback((value: number) => {
		value = clamp(value, min, max);
		if (step !== undefined)
			value = Math.floor((value - min) / step) * step + min;
		return value;
	}, [min, max, step]);

	const onThumbDown = useCallback((e: PointerEvent, triggerByTrack: boolean = false) => {
		if (e.button) { resetDefault(e); return; }
		const thumb = (e.currentTarget as HTMLDivElement).parentElement!.querySelector(".thumb") as HTMLDivElement;
		const thumbSize = thumb.offsetWidth;
		const track = thumb.parentElement!.querySelector(".track")!;
		const { left, width } = track.getBoundingClientRect();
		const x = triggerByTrack ? 0 : e.clientX - left - thumb.offsetLeft;
		const pointerMove = lodash.debounce((e: PointerEvent) => {
			const position = clamp(e.clientX - left - x, 0, width - thumbSize);
			const value = clampValue(map(position, 0, width - thumbSize, min, max));
			setValue?.(value);
			onChanging?.(value);
		});
		const pointerUp = () => {
			document.removeEventListener("pointermove", pointerMove);
			document.removeEventListener("pointerup", pointerUp);
			onChanged?.(value!);
		};
		document.addEventListener("pointermove", pointerMove);
		document.addEventListener("pointerup", pointerUp);
	}, []);

	const onTrackDown = useCallback<PointerEventHandler>(e => {
		if (e.button) { resetDefault(e); return; }
		const track = e.currentTarget as HTMLDivElement;
		const thumb = track.parentElement!.querySelector(".thumb") as HTMLDivElement;
		const thumbSizeHalf = thumb.offsetWidth / 2;
		const { width } = track.getBoundingClientRect();
		const value = clampValue(map(e.nativeEvent.offsetX, thumbSizeHalf, width - thumbSizeHalf, min, max));
		setValue?.(value);
		onChanging?.(value);
		onThumbDown(e, true); // 再去调用拖拽滑块的事件。
	}, []);

	const onKeyDown = useCallback<KeyboardEventHandler<HTMLDivElement>>(e => {
		const movePrev = e.code === "ArrowUp" || e.code === "ArrowLeft";
		const moveNext = e.code === "ArrowDown" || e.code === "ArrowRight";
		if (!movePrev && !moveNext) return;
		stopEvent(e);
		setValue?.(clampValue(value + (movePrev ? -1 : 1) * keyStep));
	}, [value]);

	return (
		<div>
			<StyledSlider
				tabIndex={0}
				style={{
					"--value": smoothValue,
				}}
				onKeyDown={onKeyDown}
				onAuxClick={resetDefault}
				onContextMenu={stopEvent}
			>
				<div className="track" onPointerDown={onTrackDown} />
				<div className="passed" />
				<div className="thumb" onPointerDown={onThumbDown} />
			</StyledSlider>
		</div>
	);
}
