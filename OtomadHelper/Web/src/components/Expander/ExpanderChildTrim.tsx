const RangeDash = styled.div.attrs(() => ({
	children: t.rangeDash,
	"aria-hidden": true,
}))`
	${styles.text.body};
	margin-block-end: 1.5px;
	font-feature-settings: "case" on;
	speak-as: literal-punctuation;
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

	.timecodes.has-child-wrapped {
		${RangeDash} {
			font-feature-settings: "case" on, "vert" on;
			writing-mode: vertical-rl;
		}
	}
`;

function ExpanderChildTrimTimecode({ start, end }: FCP<{
	children?: never;
	/** Start time time code. */
	start: StateProperty<string>;
	/** End time time code. */
	end: StateProperty<string>;
}>) {
	function reset() {
		start[1]?.("0");
		end[1]?.("0");
	}

	return (
		<StyledExpanderChildTrim>
			<VerticalIfFlexWrap className="timecodes">
				<TimecodeBox value={start} aria-label={t.aria.trimTimecode.trimStart} />
				<RangeDash />
				<TimecodeBox value={end} aria-label={t.aria.trimTimecode.trimEnd} />
			</VerticalIfFlexWrap>
			<Button icon="arrow_reset" accent="critical" subtle extruded onClick={reset}>{t.reset}</Button>
		</StyledExpanderChildTrim>
	);
}

function ExpanderChildTrimValue<TUnit extends string>({ range: [[curStart, curEnd, curUnit], set], units = [], unitNames, decimalPlaces, min, max, spinnerStep }: FCP<{
	children?: never;
	/** Current range value: start, end, and unit. */
	range: StatePropertyNonNull<RangeUnit<TUnit>>;
	/** Value unit list. */
	units?: readonly TUnit[];
	/** Get the unit type names from the unit type (always plural). */
	unitNames(unit: TUnit, index: number, units: readonly TUnit[]): Readable;
	/** The number of decimal places, leaving blank means no limit. */
	decimalPlaces?: number;
	/** Limit of the minimum value. */
	min?: number;
	/** Limit of the maximum value. */
	max?: number;
	/** The value to increase or decrease each time the knob of numeric up down box is clicked. @default 1 */
	spinnerStep?: number;
}>) {
	const isStaticUnit = units.length <= 1;
	const staticUnit = isStaticUnit ? units[0] : undefined;
	const setStart = (newStart: React.SetStateAction<number>) => set?.(([, end, unit]) => [typeof newStart === "function" ? newStart(curStart) : newStart, end, unit]);
	const setEnd = (newEnd: React.SetStateAction<number>) => set?.(([start, , unit]) => [start, typeof newEnd === "function" ? newEnd(curEnd) : newEnd, unit]);
	const setUnit = (newUnit: React.SetStateAction<TUnit>) => set?.(([start, end]) => [start, end, typeof newUnit === "function" ? newUnit(curUnit) : newUnit]);
	const mappedUnitNames = !unitNames ? units : units.map((unit, index, units) => unitNames(unit, index, units));

	return (
		<StyledExpanderChildTrim>
			<VerticalIfFlexWrap className="timecodes">
				<TextBox.Number
					value={[curStart, setStart]}
					suffix={staticUnit}
					decimalPlaces={decimalPlaces}
					min={min}
					max={minWithUndefined(max, curEnd)}
					spinnerStep={spinnerStep}
				/>
				<RangeDash />
				<TextBox.Number
					value={[curEnd, setEnd]}
					suffix={staticUnit}
					decimalPlaces={decimalPlaces}
					min={maxWithUndefined(min, curStart)}
					max={max}
					spinnerStep={spinnerStep}
				/>
				{!isStaticUnit && <ComboBox current={[curUnit, setUnit]} ids={units} options={mappedUnitNames} />}
			</VerticalIfFlexWrap>
		</StyledExpanderChildTrim>
	);
}

function ExpanderChildTrimRoughTime({ units = roughTimeUnits, ...trimValueProps }: Override<PropsOf<typeof ExpanderChildTrimValue<RoughTimeUnit>>, {
	unitNames?: never;
	/** Filter the available rough time units. @default */
	units?: readonly RoughTimeUnit[];
}>) {
	return <ExpanderChildTrimValue {...trimValueProps} units={roughTimeUnits.filter(units)} unitNames={roughTimeUnits.names(2, units)} />;
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

namespace ExpanderChildTrim {
	export const Timecode = ExpanderChildTrimTimecode;
	export const Value = ExpanderChildTrimValue;
	export const RoughTime = ExpanderChildTrimRoughTime;
}

export default ExpanderChildTrim;
