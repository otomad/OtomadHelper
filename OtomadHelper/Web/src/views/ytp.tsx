import ytpImage from "assets/images/yoooo_a_boom.jpg";

export default function Ytp() {
	const [enabled, setEnabled] = selectConfig(c => c.ytp.enabled);

	return (
		<div className="container">
			<SettingsPageControl image={ytpImage} learnMoreLink="">{t.descriptions.ytp}</SettingsPageControl>
			<SettingsCardToggleSwitch heading={t.enabled} selectInfo="1 media source has been selected" icon="enabled" on={[enabled, setEnabled]} />

			{!enabled ? (
				<EmptyMessage
					key="disabled"
					icon="ytp"
					heading={t.empty.disabled.heading({ name: t.titles.ytp })}
					caption={t.empty.disabled.caption({ name: t.titles.ytp })}
				>
					<Button onClick={() => setEnabled(true)}>{t.enable}</Button>
				</EmptyMessage>
			) : (
				<>
					<Subheader>{t.subheader.parameters}</Subheader>
					<SettingsCard heading={t.ytp.constraint} caption={t.descriptions.ytp.constraint} icon="constraint" />
					<SettingsCard heading={t.ytp.clips} caption={t.descriptions.ytp.clips} icon="number" />
					<Subheader>{t.audioVisual.effects}</Subheader>
					<Expander heading={t.ytp.effects} caption={t.descriptions.ytp.effects} icon="sparkle" />
				</>
			)}
		</div>
	);
}
