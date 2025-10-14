export default function ExpanderStreamPlaybackRate({ stream }: FCP<{
	/** Audio or visual? */
	stream: StreamKind;
}, "div">) {
	const { sync, audioRate, visualRate, audioBased, visualBased } = useSelectConfig(c => c.playbackRate);
	const value = stream === "audio" ? audioRate : visualRate;
	const meta = metas[stream];

	useEffect(() => {
		if (!sync[0]) return;
		// NOTE: Do not put two of this component with both audio and visual stream kind in a same page, or they will fight.
		if (stream === "audio") {
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
						min={0}
						max={16}
						decimalPlaces={3}
					/>
					<Tooltip title={t.reset} placement="block">
						<Button icon="arrow_reset" subtle minWidthUnbounded onClick={() => value[1](1)} />
					</Tooltip>
				</>
			)}
		>
			<Setting meta={meta.playbackRate.based} on={stream === "audio" ? audioBased : visualBased} />
			<Setting meta={meta.playbackRate.sync} on={sync} details={t.descriptions.stream.playbackRate[!sync[0] ? "sync" : "outSync"]({ stream: stream !== "audio" ? t.titles.audio : t.titles.visual })} />
		</Setting>
	);
}
