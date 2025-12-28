export default function ExpanderStreamPlaybackRate({ stream }: FCP<{
	/** Audio or visual? */
	stream: StreamKind;
}, "div">) {
	const { sync, audioRate, visualRate, audioBased, visualBased } = selectConfig(c => c.playbackRate);
	const isAudio = stream === "audio";
	const value = isAudio ? audioRate : visualRate;
	const meta = metas[stream];
	const playBackwards = useStateSelector(value, rate => rate < 0, (backward, rate) => (backward ? -1 : 1) * Math.abs(rate));

	useEffect(() => {
		if (!sync[0]) return;
		// NOTE: Do not put two of this component with both audio and visual stream kind in a same page, or they will fight.
		if (isAudio) {
			visualRate[1](audioRate[0]);
			visualBased[1](audioBased[0]);
		} else {
			audioRate[1](visualRate[0]);
			audioBased[1](visualBased[0]);
		}
	});

	return (
		<Setting
			meta={meta.playbackRate}
			trailingGap={8}
			actions={(
				<>
					<TextBox.Number
						value={value}
						min={-4}
						max={4}
						decimalPlaces={3}
						onValidate={value => value === "0" ? t.shared.exceptions.playbackRateCannotBeZero : undefined}
					/>
					<Tooltip title={t.reset} placement="block">
						<Button icon="arrow_reset" subtle minWidthUnbounded onClick={() => value[1](1)} />
					</Tooltip>
				</>
			)}
		>
			<Setting meta={meta.playbackRate.playBackwards} on={playBackwards} />
			<Setting meta={meta.playbackRate.based} on={isAudio ? audioBased : visualBased} />
			<Setting meta={meta.playbackRate.sync} on={sync} details={t.descriptions.stream.playbackRate[!sync[0] ? "sync" : "outSync"]({ stream: !isAudio ? t.titles.audio : t.titles.visual })} />
		</Setting>
	);
}
