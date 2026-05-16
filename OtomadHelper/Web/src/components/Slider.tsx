import { inSettingsCardTrailing } from "./TextBox";

const THUMB_SIZE = 20;
export /* @internal */ const sliderThumbSize = THUMB_SIZE;
const TRACK_THICKNESS = 4;
const TRACK_CLICK_HEIGHT = 48;
const valueCalc = "calc(var(--value) * (100% - var(--thumb-size)))";
const thumbSizeHalf = "calc(var(--thumb-size) / 2)";

export /* @internal */ const SliderThumb = styled.div`
	${styles.mixins.square(`var(--thumb-size, ${THUMB_SIZE}px)`)};
	${styles.mixins.circle()};
	${styles.mixins.flexCenter()};
	position: absolute;
	background-color: ${c("fill-color-control-solid-default")};
	box-shadow:
		0 0 0 1px ${c("stroke-color-control-stroke-default")},
		0 1px 0 ${c("stroke-color-control-stroke-default")};
	transition: ${fallbackTransitions}, ${styles.effects.focusRingTransitions}, inset 0s !important;
	forced-color-adjust: none;

	&::after {
		content: "";
		${styles.mixins.square("100%")};
		${styles.mixins.circle()};
		display: block;
		background-color: ${c("accent-color")};
		scale: ${12 / 20};
		transition: ${fallbackTransitions}, scale ${eases.easeOutBackSmooth} 250ms !important;
	}

	&:hover::after,
	&.hover::after {
		scale: ${14 / 20};
	}

	.track:active ~ &::after,
	&:active::after {
		scale: ${10 / 20} !important;
	}

	[disabled] &::after {
		background-color: ${c("fill-color-accent-disabled")};
	}

	&::before { // Enlarge the click area of the thumb.
		content: "";
		position: absolute;
		inset: -4px;
		display: block;
	}
`;

const StyledSlider = styled.div`
	--value: 0;
	--buffered: 0;
	position: relative;
	touch-action: none;

	:where(&) {
		width: 100%;
	}

	${inSettingsCardTrailing} & {
		inline-size: 300px;
	}

	> * {
		--thumb-size: ${THUMB_SIZE}px;
		--track-thickness: ${TRACK_THICKNESS}px;
	}

	.track {
		height: calc(var(--thumb-size) + var(--track-thickness));
		padding-block: ${thumbSizeHalf};

		&::after {
			content: "";
			display: block;
			background-color: if(
				${ifColorScheme.contrast}: ${cc("FieldText")};
				else: ${c("fill-color-control-strong-default")};
			);
		}

		&::before { // Enlarge the click area of the track.
			content: "";
			position: absolute;
			inset-block-start: ${(TRACK_CLICK_HEIGHT - THUMB_SIZE - TRACK_THICKNESS) / -2}px;
			display: block;
			block-size: ${TRACK_CLICK_HEIGHT}px;
			inline-size: 100%;
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
		width: ${valueCalc};
		margin-block: ${thumbSizeHalf};
		background-color: ${c("accent-color")};
		pointer-events: none;
		transition: background-color ${eases.easeOutMax} 250ms;
	}

	${SliderThumb} {
		inset-block-start: calc(var(--track-thickness) / 2);
		inset-inline-start: ${valueCalc};
	}

	&:is([disabled], [disabled] *) {
		.track::after {
			background-color: ${c("fill-color-control-strong-disabled")};
		}

		.passed {
			background-color: ${c("fill-color-accent-disabled")};
		}
	}

	&:hover {
		.thumb {
			will-change: inset-inline-start;
		}

		.passed {
			will-change: width;
		}
	}

	${styles.mixins.forwardFocusRing(".thumb")};
`;

const StyledSliderWrapper = styled.div`
	display: flex;
	gap: 8px;
	align-items: center;
	align-self: stretch;
	max-inline-size: 100cqi;

	output {
		${styles.text.body};
		flex-shrink: 0;
		color: ${c("fill-color-text-secondary")};
		font-variant-numeric: tabular-nums;
	}
`;

export default function Slider({ value: _value, min = 0, max = 100, autoClampValue, defaultValue, step, keyStep = 1, keyBigStepMultiplier = 10, displayValueStep, smoothlyDisplayValue = true, disabled = false, displayValue: _displayValue = false, _disableSmooth: disableSmooth, onChanging, onChange/* , onDisplayValueChanged */ }: FCP<{
	/** Current value. */
	value: VariousState<number>;
	/** Slider minimum value. @default 0 */
	min?: number;
	/** Slider maximum value. @default 100 */
	max?: number;
	/** If `min` and `max` changed dynamically, should it automatically clamp the value that would not exceed the range? */
	autoClampValue?: boolean;
	/** Slider default value. Restore defaults when clicking the mouse middle button, right button, or touchscreen long press component. */
	defaultValue?: number;
	/** Slider effective increment value. Defaults to stepless. */
	step?: number;
	/** Specifies the value by which the slider adjusts once when a keyboard arrow key is pressed. @default 1 */
	keyStep?: number;
	/**
	 * According to the Accessibility feature, when user press PageUp and PageDown key, it will adjust a larger number than the `keyStep`.
	 * Please specify a number which will multiply by the `keyStep`.
	 * @default 10
	 */
	keyBigStepMultiplier?: number;
	/** The display value decimal places will accept it, or use `step` if it is undefined. Defaults same as `step` */
	displayValueStep?: number;
	/** Make the display value change smoothly? @default true */
	smoothlyDisplayValue?: boolean;
	/** Disabled? */
	disabled?: boolean;
	/** Show the text indicates the value? Or get the display text from the value. */
	displayValue?: boolean | ((value: number) => Readable) | Readable;
	/** @private Disable smooth value. */
	_disableSmooth?: boolean;
	/** Occurs when the slider is being dragged. */
	onChanging?(value: number): void;
	/** Occurs when the slider is lifted after being dragged. */
	onChange?(value: number): void;
	/** Occurs when you want to get the display value. */
	// onDisplayValueChanged?(value: Readable | undefined): void;
}>) {
	const [value, _setValue] = useVariousState(_value);
	const errorInfo = `The value range should be between [${min} ~ ${max}], with the current value being ${value}.`;
	if (value === undefined || Number.isNaN(value))
		throw new ReferenceError("value undefined");
	if (min > max)
		throw new RangeError(`Is the minimum value of Slider greater than the maximum value? The minimum value is ${min}, and the maximum value is ${max}`);
	if (value < min)
		if (autoClampValue) _setValue?.(min);
		else throw new RangeError("The value of Slider is lesser than the minimum value. " + errorInfo);
	if (value > max)
		if (autoClampValue) _setValue?.(max);
		else throw new RangeError("The value of Slider is greater than the maximum value. " + errorInfo);

	const restrict = useCallback((n: number | undefined, nanValue: number) => Number.isFinite(n) ? clamp(map(n!, min, max, 0, 1), 0, 1) : nanValue, [min, max]);
	const sharpValue = useMemo(() => restrict(value, 0), [value, restrict]);
	// Modify this parameter to adjust the smooth movement value of the slider.
	let smoothValue = useSmoothValue(sharpValue, 0.5);
	if (disableSmooth) smoothValue = sharpValue;
	const id = useId();

	const setValue = useCallback((value: number) => {
		value = clamp(value, min, max);
		if (step) value = value.toFixedNumber(step.countDecimals());
		_setValue(value);
	}, [_setValue, min, max, step]);

	function resetToDefault(e: MouseEvent) {
		e.preventDefault();
		if (defaultValue !== undefined && Number.isFinite(defaultValue)) {
			setValue(defaultValue);
			onChanging?.(defaultValue);
			onChange?.(defaultValue);
		}
	}

	const clampValue = useCallback((value: number) => {
		value = clamp(value, min, max);
		if (step !== undefined)
			value = Math.round((value - min) / step) * step + min;
		return value;
	}, [min, max, step]);

	const trackEl = useDomRef<"div">(), thumbEl = useDomRef<"div">();

	function onThumbDown(e: PointerEvent, triggerByTrack: boolean = false) {
		const track = trackEl.current, thumb = thumbEl.current;
		if (e.button || !track || !thumb) { e.preventDefault(); return; }
		const thumbSize = THUMB_SIZE;
		const { left, width } = track.getBoundingClientRect();
		const x = triggerByTrack ? thumbSize / 2 : e.clientX - left - thumb.offsetLeft * getUiScale1();
		const aborter = new AbortController();
		thumb.setPointerCapture(e.pointerId);
		thumb.addEventListener("pointermove", lodash.debounce((e: PointerEvent) => {
			const position = clamp(e.clientX - left - x, 0, width - thumbSize);
			let value = clampValue(map(position, 0, width - thumbSize, min, max));
			if (isRtl()) value = max - value + min;
			setValue(value);
			onChanging?.(value);
		}), { signal: aborter.signal });
		thumb.addEventListener("pointerup", () => {
			aborter.abort();
			thumb.releasePointerCapture(e.pointerId);
			onChange?.(value);
		}, { signal: aborter.signal });
	}

	const onTrackDown: PointerEventHandler = e => {
		if (e.button) { e.preventDefault(); return; }
		const thumbSizeHalf = THUMB_SIZE / 2;
		const track = e.currentTarget;
		const { width } = track.getBoundingClientRect();
		let value = clampValue(map(e.nativeEvent.offsetX, thumbSizeHalf, width - thumbSizeHalf, min, max));
		if (isRtl()) value = max - value + min;
		setValue(value);
		onChanging?.(value);
		onThumbDown(e, true); // Then call the dragging slider event.
	};

	const onKeyDown = useCallback<KeyboardEventHandler<HTMLDivElement>>(e => {
		if (e.code === "Space") { e.preventDefault(); return; }
		const increase = e.code.in("ArrowUp", "ArrowRight", "PageUp", "End");
		const decrease = e.code.in("ArrowDown", "ArrowLeft", "PageDown", "Home");
		const largeStep = e.code.in("PageUp", "PageDown");
		if (!decrease && !increase) return;
		stopEvent(e);
		e.currentTarget.focus(); // HACK: If slider is in the action part of checkbox, it can prevent use on form key down in the checkbox.
		const newValue = e.code === "Home" ? min : e.code === "End" ? max :
			clampValue(value + (decrease ? -1 : 1) * keyStep * (largeStep ? keyBigStepMultiplier : 1));
		setValue(newValue);
	}, [value, clampValue, keyBigStepMultiplier, keyStep, min, max, setValue]);

	const steppedSmoothValue = useMemo(() => {
		const smoothValue2 = map(smoothlyDisplayValue ? smoothValue : sharpValue, 0, 1, min, max);
		const step2 = displayValueStep ?? step;
		const steppedSmoothValue = step2 ? smoothValue2.toFixed(step2.countDecimals()) : smoothValue2;
		return steppedSmoothValue;
	}, [displayValueStep, max, min, sharpValue, smoothValue, smoothlyDisplayValue, step]);

	const displayValue = useMemo(() => {
		if (_displayValue === false || _displayValue === undefined) return undefined;
		else if (_displayValue === true) return steppedSmoothValue;
		else if (typeof _displayValue === "function") return _displayValue(+steppedSmoothValue);
		// It is possible to expose more types of values (such as the original value with long decimals, unclamped value, etc.), but it is unnecessary at the moment.
		else return _displayValue;
	}, [_displayValue, steppedSmoothValue]);

	// useEffect(() => { onDisplayValueChanged?.(displayValue); }, [displayValue, onDisplayValueChanged]);

	return (
		<StyledSliderWrapper onAuxClick={resetToDefault}>
			{hasValue(displayValue) /* && !onDisplayValueChanged */ && <output htmlFor={id} aria-hidden>{displayValue}</output>}
			<StyledSlider
				tabIndex={disabled ? -1 : 0}
				style={{
					"--value": smoothValue,
				}}
				disabled={disabled}
				aria-disabled={disabled}
				onKeyDown={onKeyDown}
				onContextMenu={stopEvent}
				id={id}
				role="slider"
				aria-valuemin={min}
				aria-valuemax={max}
				aria-valuenow={value}
				aria-valuetext={hasValue(displayValue) ? String(displayValue) : undefined}
			>
				<div className="track" ref={trackEl} onPointerDown={onTrackDown} />
				<div className="passed" />
				<SliderThumb className="thumb" ref={thumbEl} onPointerDown={onThumbDown} />{/* onDoubleClick={resetToDefault} */ /* Easy to touch by mistake */}
			</StyledSlider>
		</StyledSliderWrapper>
	);
}

/**
 * Ignore `undefined`, `null`, `NaN`, and empty string.
 * @param test - The value to test.
 * @returns Is the tested value not `undefined`, `null`, `NaN`, or empty string?
 */
function hasValue(test: Readable | undefined | null): test is Readable {
	return !!test || test === 0 || test === 0n;
}
