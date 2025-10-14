import exampleThumbnail from "assets/images/ヨハネの氷.avif";

export /* @internal */ const trackLegatoModes = ["stacking", "stackingAllAfter", "stackingAllTracks", "limitStretch", "stretch", "lengthen", "increaseSpacing", "increaseSpacingAllTracks"] as const;

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
	const { mode: legatoMode, increaseSpacing, forClips: legatoForClips, includeGroup: legatoIncludeGroup, backwards: legatoBackwards } = useSelectConfig(c => c.track.legato);
	const meta = metas.track;

	useEffect(() => {
		if (!legatoForClips[0] && legatoMode[0] === "stackingAllAfter") legatoMode[1]("stacking");
	}, [legatoMode, legatoForClips]);

	return (
		<div className="container">
			<SettingsPageControl image={(<PreviewLayout thumbnail={exampleThumbnail} />)} learnMoreLink="">{t.descriptions.track}</SettingsPageControl>

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
			<Setting meta={meta.legato}>
				<ItemsView view="grid" current={legatoMode} itemWidth={320} key={String(legatoForClips[0])}>
					{/* When `legatoForClips` change, re-render the entire component to avoid the animation of the newly added item being out of sync with other items. */}
					{trackLegatoModes.map(mode => {
						if (mode === "stackingAllAfter" && !legatoForClips[0]) return;
						const displayMode = mode === "stacking" && legatoForClips[0] ? "stackingSelected" : mode;
						const multiline = t.track.legato[displayMode].split("\n");
						return (
							<ItemsView.Item
								id={mode}
								key={mode}
								details={multiline[1]}
								image={<PreviewTrackLegato mode={mode} />}
								withBorder
							>
								{multiline[0]}
							</ItemsView.Item>
						);
					})}
				</ItemsView>
				<Setting meta={meta.legato.forClips} on={legatoForClips} />
				<Setting meta={meta.legato.includeGroup} on={legatoIncludeGroup} />
				<Setting meta={meta.legato.backwards} on={legatoBackwards} />
				<Setting
					meta={meta.legato.increaseSpacing}
					disabled={!legatoMode[0].in("increaseSpacing", "increaseSpacingAllTracks")}
					actions={<TimecodeBox value={increaseSpacing} />}
				/>
				<Expander.ChildWrapper>
					<Button icon="checkmark">{t.apply}</Button>
				</Expander.ChildWrapper>
			</Setting>

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
		box3d: useSelectConfig(c => c.track.box3d).enabled,
		gradient: useSelectConfig(c => c.track.gradient).enabled,
	};
	const states = Object.values(enabled);
	const count = states.filter(state => state[0]).length;
	const deactivateAll = () => states.forEach(state => state[1](false));
	return [enabled, count, deactivateAll] as const;
}
