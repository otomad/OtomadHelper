/* eslint-disable react/no-children-prop */

const StyledResetButton = styled(Button).attrs({
	icon: "arrow_reset",
	subtle: true,
	accent: true,
	onClick: e => stopEvent(e),
})`
	min-width: unset;
`;
const ResetButton = Tooltip.wrap(StyledResetButton, { placement: "y", title: t.descriptions.track.resetLayout });

export default function Track() {
	const { pushPage } = useSnapshot(pageStore);

	return (
		<div className="container">
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
