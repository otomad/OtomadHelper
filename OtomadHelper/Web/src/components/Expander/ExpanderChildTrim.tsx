const RangeDash = styled.div.attrs({
	children: t.rangeDash,
})`
	${styles.effects.text.body};
	margin-block-end: 1.5px;
	font-feature-settings: "case" on;
	transition: ${fallbackTransitions}, margin 0s;
`;

const StyledExpanderChildTrim = styled(Expander.ChildWrapper)`
	justify-content: space-between;

	&,
	.timecodes {
		display: flex;
		flex-wrap: wrap;
		gap: 12px;
		align-items: center;
	}

	.timecodes.has-child-wrapped ${RangeDash} {
		font-feature-settings: "case" on, "vert" on;
		writing-mode: vertical-rl;
	}
`;

function ExpanderChildTrimTimecode({ start, end }: FCP<{
	children?: never;
	/** Start time time code. */
	start: StateProperty<string>;
	/** End time time code. */
	end: StateProperty<string>;
}, "div">) {
	function reset() {
		start[1]?.("0");
		end[1]?.("0");
	}

	return (
		<StyledExpanderChildTrim>
			<VerticalIfFlexWrap className="timecodes">
				<TimecodeBox value={start} />
				<RangeDash />
				<TimecodeBox value={end} />
			</VerticalIfFlexWrap>
			<Button icon="arrow_reset" accent="critical" subtle extruded onClick={reset}>{t.reset}</Button>
		</StyledExpanderChildTrim>
	);
}

function ExpanderChildTrimValue({ start, end, unit = t.units.millisecond, decimalPlaces, min, max, spinnerStep }: FCP<{
	children?: never;
	/** Start time value. */
	start: StateProperty<number>;
	/** End time value. */
	end: StateProperty<number>;
	/** Value unit. */
	unit?: string;
	/** The number of decimal places, leaving blank means no limit. */
	decimalPlaces?: number;
	/** Limit of the minimum value. */
	min?: number;
	/** Limit of the maximum value. */
	max?: number;
	/** The value to increase or decrease each time the knob of numeric up down box is clicked. @default 1 */
	spinnerStep?: number;
}, "div">) {
	return (
		<StyledExpanderChildTrim>
			<VerticalIfFlexWrap className="timecodes">
				<TextBox.Number
					value={start as StatePropertyNonNull<number>}
					suffix={unit}
					decimalPlaces={decimalPlaces}
					min={min}
					max={minWithUndefined(max, end[0])}
					spinnerStep={spinnerStep}
				/>
				<RangeDash />
				<TextBox.Number
					value={end as StatePropertyNonNull<number>}
					suffix={unit}
					decimalPlaces={decimalPlaces}
					min={maxWithUndefined(min, start[0])}
					max={max}
					spinnerStep={spinnerStep}
				/>
			</VerticalIfFlexWrap>
		</StyledExpanderChildTrim>
	);
}

/**
 * Calculates the minimum value from an array of numbers, excluding any undefined values.
 * @param values - An array of numbers and/or undefined values.
 * @returns The minimum number from the input array, excluding undefined values.
 */
function minWithUndefined(...values: (number | undefined)[]) {
	return Math.min(...values.toTrimmed());
}
/**
 * Calculates the maximum value from an array of numbers, excluding any undefined values.
 * @param values - An array of numbers and/or undefined values.
 * @returns The maximum number from the input array, excluding undefined values.
 */
function maxWithUndefined(...values: (number | undefined)[]) {
	return Math.max(...values.toTrimmed());
}

const ExpanderChildTrim = {
	Timecode: ExpanderChildTrimTimecode,
	Value: ExpanderChildTrimValue,
};

export default ExpanderChildTrim;
