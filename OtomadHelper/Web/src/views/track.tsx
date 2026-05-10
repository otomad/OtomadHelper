const StyledDeactivateButton = styled(Button).attrs({
	icon: "arrow_reset",
	accent: true,
})`
	&.hidden {
		scale: 0.9;
		opacity: 0;
		content-visibility: hidden;
		pointer-events: none;
		transition-behavior: allow-discrete;
	}
`;

const DeactivateButton = ({ activated: [activated, setActivated] }: { activated: StatePropertyNonNull<boolean> }) => (
	<Tooltip placement="block" title={t.descriptions.track.deactivate}>
		<StyledDeactivateButton
			className={{ hidden: !activated }}
			tabIndex={activated ? 0 : -1}
			onClick={() => setActivated(false)}
		>
			{t.track.deactivate}
		</StyledDeactivateButton>
	</Tooltip>
);

export default function Track() {
	const { pushPage } = useSnapshot(pageStore);
	const [layoutEnabled, layoutEnabledCount, deactivateAll] = useLayoutEnabled();
	const meta = metas.track;
	const { thumbnail } = useThumbnail();

	return (
		<div className="container">
			<SettingsPageControl image={(<PreviewLayout thumbnail={thumbnail} />)} learnMoreLink="">{t.descriptions.track}</SettingsPageControl>

			<Subheader meta={meta.layout} />
			<SettingsCard
				title={t({ context: "full" }).titles.grid}
				type="button"
				icon="grid"
				onClick={() => pushPage("grid")}
			>
				<DeactivateButton activated={layoutEnabled.grid} />
			</SettingsCard>
			<SettingsCard
				title={t({ context: "full" }).titles.concentric}
				type="button"
				icon="concentric"
				onClick={() => pushPage("concentric")}
			>
				<DeactivateButton activated={layoutEnabled.concentric} />
			</SettingsCard>
			<SettingsCard
				title={t({ context: "full" }).titles.box3d}
				type="button"
				icon="cube"
				onClick={() => pushPage("box-3d")}
			>
				<DeactivateButton activated={layoutEnabled.box3d} />
			</SettingsCard>
			<SettingsCard
				title={t({ context: "full" }).titles.gradient}
				details={t.descriptions.track.gradient}
				type="button"
				icon="highlight"
				onClick={() => pushPage("gradient")}
			>
				<DeactivateButton activated={layoutEnabled.gradient} />
			</SettingsCard>

			<div>
				<Button icon="arrow_reset" disabled={layoutEnabledCount === 0} onClick={deactivateAll}>{t.track.deactivateAll}</Button>
			</div>

			<Subheader>{t.stream.legato}</Subheader>
			<ExpanderLegato stream="track" />

			<Subheader meta={meta.clear} />
			<div>
				<Button meta={meta.clear.motion} accent="critical" />
				<Button meta={meta.clear.effect} accent="critical" />
			</div>
		</div>
	);
}

function useLayoutEnabled() {
	const enabled = {
		grid: useSelectConfig(c => c.track.grid).enabled,
		concentric: useSelectConfig(c => c.track.concentric).enabled,
		box3d: useSelectConfig(c => c.track.box3d).enabled,
		gradient: useSelectConfig(c => c.track.gradient).enabled,
	};
	const states = Object.values(enabled);
	const count = states.filter(state => state[0]).length;
	const deactivateAll = () => states.forEach(state => state[1](false));
	return [enabled, count, deactivateAll] as const;
}

type AutoLayoutTrackType = keyof ReturnType<typeof useLayoutEnabled>[0];

function setLayoutEnabled(layout: AutoLayoutTrackType, enabled: boolean) {
	const layouts = configStore.track;
	const mutexLayouts = ["grid", "concentric", "box3d"] as const satisfies readonly AutoLayoutTrackType[];
	layouts[layout].enabled = enabled;
	if (mutexLayouts.includes(layout) && enabled)
		for (const mutexLayout of mutexLayouts)
			if (mutexLayout !== layout)
				layouts[mutexLayout].enabled = false;
}

export function useSetLayoutEnabledOnSave(layout: AutoLayoutTrackType, enabled: boolean) {
	pageStore.useOnSave(() => setLayoutEnabled(layout, enabled));
}
