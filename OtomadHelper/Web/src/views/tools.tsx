const StyledDescription = styled.p`
	margin-bottom: 10px;
`;

export default function Tools() {
	return (
		<div className="container">
			<StyledDescription>{t.descriptions.tools}</StyledDescription>
			<SettingsCard heading={t.tools.selector} caption={t.descriptions.tools.selector} type="button" icon="search" selectInfo={t(1).selectInfo.trackEvent} />
			<SettingsCard heading={t.tools.replacer} caption={t.descriptions.tools.replacer} type="button" icon="replace" />
			<SettingsCard
				heading={t.tools.normalizer} caption={t.descriptions.tools.normalizer} type="button" icon="spatial_volume" trailingIcon="open"
				selectInfo={t(1).selectInfo.audioEvent}
			/>
			<SettingsCard heading={t.tools.subtitles} caption={t.descriptions.tools.subtitles} type="button" icon="subtitles" selectInfo={t(1).selectInfo.audioEvent} />
			<SettingsCard heading={t.tools.visualizer} caption={t.descriptions.tools.visualizer} type="button" icon="sparkle" selectInfo={t(1).selectInfo.videoEvent} />
			<SettingsCard heading={t.tools.fader} caption={t.descriptions.tools.fader} type="button" icon="fade" selectInfo={t(1).selectInfo.trackEvent} />
			<SettingsCard heading={t.tools.scoreExporter} caption={t.descriptions.tools.scoreExporter} type="button" icon="export_score" />
			<SettingsCard heading={t.tools.flow} caption={t.descriptions.tools.flow} type="button" icon="flow" />

			<Subheader>{t.tools.converters}</Subheader>
			<SettingsCard heading={t.stream.tuning.tuningMethod} caption={t.descriptions.tools.converters.tuningMethod} type="button" icon="tuning" selectInfo={t(1).selectInfo.videoEvent} />
			<SettingsCard
				heading={t.score.timeSignature} caption={t.descriptions.tools.converters.timeSignature} type="button" icon="health" trailingIcon="open"
				selectInfo={t(1).selectInfo.audioEventOnlyOne}
			/>
			<SettingsCard heading={t.stream.transformOfx} caption={t.descriptions.tools.converters.transformOfx} type="button" icon="zoom_fit" selectInfo={t(1).selectInfo.videoEvent} />
		</div>
	);
}
