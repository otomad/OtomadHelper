import { inSettingsCardTrailing } from "./TextBox";

const THUMB_SIZE = 20;
const TRACK_THICKNESS = 4;
const valueCalc = "calc(var(--value) * (100% - var(--thumb-size)))";
const thumbSizeHalf = "calc(var(--thumb-size) / 2)";

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
			background-color: ${c("fill-color-control-strong-default")};
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

	.thumb {
		${styles.mixins.square("var(--thumb-size)")};
		${styles.mixins.circle()};
		${styles.mixins.flexCenter()};
		position: absolute;
		inset-block-start: calc(var(--track-thickness) / 2);
		inset-inline-start: ${valueCalc};
		background-color: ${c("fill-color-control-solid-default")};
		box-shadow:
			0 0 0 1px ${c("stroke-color-control-stroke-default")},
			0 1px 0 ${c("stroke-color-control-stroke-default")};
		transition: ${fallbackTransitions}, inset-inline-start 0s;

		&::after {
			content: "";
			${styles.mixins.square("100%")};
			${styles.mixins.circle()};
			display: block;
			background-color: ${c("accent-color")};
			scale: ${12 / 20};
			transition: ${fallbackTransitions}, scale ${eases.easeOutBackSmooth} 250ms;
		}

		&:hover::after {
			scale: ${14 / 20};
		}
	}

	.track:active ~ .thumb::after,
	.thumb:active::after,
	.thumb.pressed::after {
		scale: ${10 / 20} !important;
	}

	&[disabled] {
		.track::after {
			background-color: ${c("fill-color-control-strong-disabled")};
		}

		.passed,
		.thumb::after {
			background-color: ${c("fill-color-accent-disabled")};
		}
	}

	${styles.mixins.forwardFocusRing(".thumb")};
`;

const StyledSliderWrapper = styled.div`
	display: flex;
	gap: 8px;
	align-items: center;

	.display-value {
		${styles.effects.text.body};
		color: ${c("fill-color-text-secondary")};
		font-variant-numeric: tabular-nums;
	}
`;

export default function Slider({ value: [value, setValue], min = 0, max = 100, defaultValue, step, keyStep = 1, disabled = false, displayValue: _displayValue = false, onChanging, onChanged }: FCP<{
	/** Current value. */
	value: StateProperty<number>;
	/** Slider minimum value. */
	min?: number;
	/** Slider maximum value. */
	max?: number;
	/** Slider default value. Restore defaults when clicking the mouse middle button, right button, or touchscreen long press component. */
	defaultValue?: number;
	/** Slider effective increment value. */
	step?: number;
	/** Specifies the value by which the slider adjusts once when a keyboard direction key is pressed. */
	keyStep?: number;
	/** Disabled */
	disabled?: boolean;
	/** Show the text indicates the value? Or get the display text from the value. */
	displayValue?: boolean | ((value: number) => Readable) | Readable;
	/** The slider is dragging event. */
	onChanging?(value: number): void;
	/** The slider is lifted after being dragged event. */
	onChanged?(value: number): void;
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

	const restrict = (n: number | undefined, nanValue: number) => Number.isFinite(n) ? clamp(map(n!, min, max, 0, 1), 0, 1) : nanValue;
	const sharpValue = useMemo(() => restrict(value, 0), [value, min, max]);
	const smoothValue = useSmoothValue(sharpValue, 0.5); // Modify this parameter to adjust the smooth movement value of the slider.
	const [pressed, setPressed] = useState(false);

	function resetToDefault(e: MouseEvent) {
		e.preventDefault();
		if (defaultValue !== undefined && Number.isFinite(defaultValue)) {
			setValue?.(defaultValue);
			onChanging?.(defaultValue);
			onChanged?.(defaultValue);
		}
	}

	function clampValue(value: number) {
		value = clamp(value, min, max);
		if (step !== undefined)
			value = Math.floor((value - min) / step) * step + min;
		return value;
	}

	function onThumbDown(e: PointerEvent, triggerByTrack: boolean = false) {
		if (e.button) { resetToDefault(e); return; }
		setPressed(true);
		const thumb = (e.currentTarget as HTMLDivElement).parentElement!.querySelector(".thumb") as HTMLDivElement;
		const thumbSize = thumb.offsetWidth;
		const track = thumb.parentElement!.querySelector(".track")!;
		const { left, width } = track.getBoundingClientRect();
		const x = triggerByTrack ? thumbSize / 2 : e.clientX - left - thumb.offsetLeft;
		const pointerMove = lodash.debounce((e: PointerEvent) => {
			const position = clamp(e.clientX - left - x, 0, width - thumbSize);
			let value = clampValue(map(position, 0, width - thumbSize, min, max));
			if (isRtl()) value = max - value + min;
			setValue?.(value);
			onChanging?.(value);
		});
		const pointerUp = () => {
			document.removeEventListener("pointermove", pointerMove);
			document.removeEventListener("pointerup", pointerUp);
			onChanged?.(value!);
			nextAnimationTick().then(() => {
				setPressed(false);
			});
		};
		document.addEventListener("pointermove", pointerMove);
		document.addEventListener("pointerup", pointerUp);
	}

	const onTrackDown = useCallback<PointerEventHandler>(e => {
		if (e.button) { resetToDefault(e); return; }
		const track = e.currentTarget as HTMLDivElement;
		const thumb = track.parentElement!.querySelector(".thumb") as HTMLDivElement;
		const thumbSizeHalf = thumb.offsetWidth / 2;
		const { width } = track.getBoundingClientRect();
		let value = clampValue(map(e.nativeEvent.offsetX, thumbSizeHalf, width - thumbSizeHalf, min, max));
		if (isRtl()) value = max - value + min;
		setValue?.(value);
		onChanging?.(value);
		onThumbDown(e, true); // Then call the dragging slider event.
	}, []);

	const onKeyDown = useCallback<KeyboardEventHandler<HTMLDivElement>>(e => {
		const movePrev = e.code === "ArrowUp" || e.code === "ArrowLeft";
		const moveNext = e.code === "ArrowDown" || e.code === "ArrowRight";
		if (!movePrev && !moveNext) return;
		stopEvent(e);
		setValue?.(clampValue(value + (movePrev ? -1 : 1) * keyStep));
	}, [value]);

	const displayValue = (() => {
		const steppedSmoothValue = step ? smoothValue.toFixed(step.countDecimals()) : smoothValue;
		if (_displayValue === false || _displayValue === undefined) return undefined;
		else if (_displayValue === true) return steppedSmoothValue;
		else if (typeof _displayValue === "function") return _displayValue(+steppedSmoothValue);
		// It is possible to expose more types of values (such as the original value with long decimals, unclamped value, etc.), but it is unnecessary at the moment.
		else return _displayValue;
	})();

	return (
		<StyledSliderWrapper>
			<span className="display-value">{displayValue}</span>
			<StyledSlider
				tabIndex={0}
				style={{
					"--value": smoothValue,
				}}
				disabled={disabled}
				onKeyDown={onKeyDown}
				onAuxClick={resetToDefault}
				onContextMenu={stopEvent}
			>
				<div className="track" onPointerDown={onTrackDown} />
				<div className="passed" />
				<div className={["thumb", { pressed }]} onPointerDown={onThumbDown} />
			</StyledSlider>
		</StyledSliderWrapper>
	);
}
