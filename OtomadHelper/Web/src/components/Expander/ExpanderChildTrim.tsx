const StyledExpanderChildTrim = styled(Expander.ChildWrapper)`
	justify-content: space-between;

	&,
	.timecodes {
		display: flex;
		flex-wrap: wrap;
		gap: 12px;
		align-items: center;
	}

	.tilde {
		margin: 0 auto;
	}
`;

export default function ExpanderChildTrim({ start, end }: FCP<{
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
			<div className="timecodes">
				<TimecodeBox timecode={start} />
				<div className="tilde">~</div>
				<TimecodeBox timecode={end} />
			</div>
			<Button icon="arrow_reset" accent="critical" subtle extruded onClick={reset}>{t.reset}</Button>
		</StyledExpanderChildTrim>
	);
}
