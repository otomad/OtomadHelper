import exampleThumbnail from "assets/images/ヨハネの氷.png";

const StyledDeactivateButton = styled(Button).attrs({
	icon: "arrow_reset",
	accent: true,
})`
	&.hidden {
		scale: 0.9;
		opacity: 0;
		pointer-events: none;
	}
`;

const DeactivateButton = ({ deactivated: [deactivated, setDeactivated] }: { deactivated: StatePropertyNonNull<boolean> }) => (
	<Tooltip placement="y" title={t.descriptions.track.deactivate}>
		<StyledDeactivateButton className={{ hidden: !deactivated }} onClick={() => setDeactivated(false)}>{t.track.deactivate}</StyledDeactivateButton>
	</Tooltip>
);

export default function Track() {
	const { pushPage } = useSnapshot(pageStore);
	const [layoutEnabled, layoutEnabledCount, deactivateAll] = useLayoutEnabled();

	return (
		<div className="container">
			<SettingsPageControl image={(<PreviewLayout thumbnail={exampleThumbnail} />)} learnMoreLink="">{t.descriptions.track}</SettingsPageControl>

			<Subheader>{t.track.layout}</Subheader>
			<SettingsCard
				title={t.track.grid}
				type="button"
				icon="grid"
				onClick={() => pushPage("home")}
			>
				<DeactivateButton deactivated={layoutEnabled.grid} />
			</SettingsCard>
			<SettingsCard
				title={t.track.box3d}
				type="button"
				icon="cube"
			>
				<DeactivateButton deactivated={layoutEnabled.box3d} />
			</SettingsCard>
			<SettingsCard
				title={t.track.gradient}
				details={t.descriptions.track.gradient}
				type="button"
				icon="highlight"
			>
				<DeactivateButton deactivated={layoutEnabled.gradient} />
			</SettingsCard>

			<div>
				<Button icon="arrow_reset" disabled={layoutEnabledCount === 0} onClick={deactivateAll}>{t.track.deactivateAll}</Button>
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

function useLayoutEnabled() {
	const enabled = {
		grid: selectConfig(c => c.track.grid).enabled,
		box3d: selectConfig(c => c.track.box3d).enabled,
		gradient: selectConfig(c => c.track.gradient).enabled,
	};
	const states = Object.values(enabled);
	const count = states.filter(state => state[0]).length;
	const deactivateAll = () => states.forEach(state => state[1](false));
	return [enabled, count, deactivateAll] as const;
}
