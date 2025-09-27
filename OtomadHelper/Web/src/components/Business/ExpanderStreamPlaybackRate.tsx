export default function ExpanderStreamPlaybackRate({ stream }: FCP<{
	/** Audio or visual? */
	stream: StreamKind;
}, "div">) {
	const { sync, audioRate, visualRate, audioBased, visualBased } = useSelectConfig(c => c.playbackRate);
	const value = stream === "audio" ? audioRate : visualRate;

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
			meta={metas.audio.playbackRate}
			title={t.stream.playbackRate}
			details={t.descriptions.stream.playbackRate}
			icon="play_circle_hint_half"
			trailingGap={8}
			actions={(
				<>
					<TextBox.Number
						value={value}
						min={0}
						max={16}
						decimalPlaces={3}
					/>
					<Tooltip title={t.reset} placement="y">
						<Button icon="arrow_reset" subtle minWidthUnbounded onClick={() => value[1](1)} />
					</Tooltip>
				</>
			)}
		>
			<Setting meta={metas.audio.playbackRate.based} on={stream === "audio" ? audioBased : visualBased} />
			<Setting meta={metas.audio.playbackRate.sync} on={sync} details={t.descriptions.stream.playbackRate[!sync[0] ? "sync" : "outSync"]({ stream: stream !== "audio" ? t.titles.audio : t.titles.visual })} />
		</Setting>
	);
}
