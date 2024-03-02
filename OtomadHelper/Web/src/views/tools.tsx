const StyledDescription = styled.p`
	margin-bottom: 10px;
`;

export default function Tools() {
	return (
		<div className="container">
			<StyledDescription>{t.descriptions.tools}</StyledDescription>
			<SettingsCard title={t.tools.selector} details={t.descriptions.tools.selector} type="button" icon="search" selectInfo={t(1).selectInfo.trackEvent} />
			<SettingsCard title={t.tools.replacer} details={t.descriptions.tools.replacer} type="button" icon="replace" />
			<SettingsCard
				title={t.tools.normalizer} details={t.descriptions.tools.normalizer} type="button" icon="spatial_volume" trailingIcon="open"
				selectInfo={t(1).selectInfo.audioEvent}
			/>
			<SettingsCard title={t.tools.subtitles} details={t.descriptions.tools.subtitles} type="button" icon="subtitles" selectInfo={t(1).selectInfo.audioEvent} />
			<SettingsCard title={t.tools.visualizer} details={t.descriptions.tools.visualizer} type="button" icon="sparkle" selectInfo={t(1).selectInfo.videoEvent} />
			<SettingsCard title={t.tools.fader} details={t.descriptions.tools.fader} type="button" icon="fade" selectInfo={t(1).selectInfo.trackEvent} />
			<SettingsCard title={t.tools.scoreExporter} details={t.descriptions.tools.scoreExporter} type="button" icon="export_score" />
			<SettingsCard title={t.tools.flow} details={t.descriptions.tools.flow} type="button" icon="flow" />

			<Subheader>{t.tools.converters}</Subheader>
			<SettingsCard title={t.stream.tuning.tuningMethod} details={t.descriptions.tools.converters.tuningMethod} type="button" icon="tuning" selectInfo={t(1).selectInfo.videoEvent} />
			<SettingsCard
				title={t.score.timeSignature} details={t.descriptions.tools.converters.timeSignature} type="button" icon="health" trailingIcon="open"
				selectInfo={t(1).selectInfo.audioEventOnlyOne}
			/>
			<SettingsCard title={t.stream.transformOfx} details={t.descriptions.tools.converters.transformOfx} type="button" icon="zoom_fit" selectInfo={t(1).selectInfo.videoEvent} />
		</div>
	);
}
