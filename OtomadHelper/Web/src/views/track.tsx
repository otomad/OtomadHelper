export default function Track() {
	const { pushPage } = usePageStore();

	return (
		<div className="container">
			<Subheader>{t.track.layout}</Subheader>
			<SettingsCard title={t.track.grid} type="button" icon="grid" onClick={() => pushPage("home")} />
			<SettingsCard title={t.track.box3d} type="button" icon="cube" />
			<SettingsCard title={t.track.gradient} details={t.descriptions.track.gradient} type="button" icon="highlight" />

			<Subheader>{t.stream.legato}</Subheader>
			<Expander title={t.track.legato} details={t.descriptions.track.legato} icon="legato" />

			<Subheader>{t.track.clear}</Subheader>
			<StackPanel $direction="horizontal">
				<Button icon="clear_motion" accent="critical">{t.track.clear.motion}</Button>
				<Button icon="clear_plugin" accent="critical">{t.track.clear.effect}</Button>
			</StackPanel>
		</div>
	);
}
