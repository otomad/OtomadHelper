/* eslint-disable react/no-children-prop */
import exampleThumbnail from "assets/images/ヨハネの氷.png";

const ResetButton = () => (
	<Tooltip placement="y" title={t.descriptions.track.resetLayout}>
		<Button icon="arrow_reset" subtle accent minWidthUnbounded />
	</Tooltip>
);

export default function Track() {
	const { pushPage } = useSnapshot(pageStore);

	return (
		<div className="container">
			<SettingsPageControl image={(<PreviewLayout thumbnail={exampleThumbnail} />)} learnMoreLink="">{t.descriptions.track}</SettingsPageControl>

			<Subheader>{t.track.layout}</Subheader>
			<SettingsCard title={t.track.grid} type="button" icon="grid" onClick={() => pushPage("home")} children={<ResetButton />} />
			<SettingsCard title={t.track.box3d} type="button" icon="cube" children={<ResetButton />} />
			<SettingsCard title={t.track.gradient} details={t.descriptions.track.gradient} type="button" icon="highlight" children={<ResetButton />} />
			<div>
				<Button icon="arrow_reset">{t.track.resetAllLayouts}</Button>
			</div>

			<Subheader>{t.stream.legato}</Subheader>
			<Expander title={t.track.legato} details={t.descriptions.track.legato} icon="legato" />

			<Subheader>{t.track.clear}</Subheader>
			<div>
				<Button icon="clear_motion" accent="critical">{t.track.clear.motion}</Button>
				<Button icon="clear_plugin" accent="critical">{t.track.clear.effect}</Button>
			</div>
		</div>
	);
}
