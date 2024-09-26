/* eslint-disable react/no-children-prop */
import exampleThumbnail from "assets/images/ヨハネの氷.png";

const DeactivateButton = () => (
	<Tooltip placement="y" title={t.descriptions.track.deactivate}>
		<Button icon="arrow_reset" accent>{t.track.deactivate}</Button>
	</Tooltip>
);

export default function Track() {
	const { pushPage } = useSnapshot(pageStore);

	return (
		<div className="container">
			<SettingsPageControl image={(<PreviewLayout thumbnail={exampleThumbnail} />)} learnMoreLink="">{t.descriptions.track}</SettingsPageControl>

			<Subheader>{t.track.layout}</Subheader>
			<SettingsCard title={t.track.grid} type="button" icon="grid" onClick={() => pushPage("home")} children={<DeactivateButton />} />
			<SettingsCard title={t.track.box3d} type="button" icon="cube" children={<DeactivateButton />} />
			<SettingsCard title={t.track.gradient} details={t.descriptions.track.gradient} type="button" icon="highlight" children={<DeactivateButton />} />
			<div>
				<Button icon="arrow_reset">{t.track.deactivateAll}</Button>
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
