import legatoPortatoImage from "assets/images/tutorials/legato_config/legato_portato.png";
import legatoUnlimitedImage from "assets/images/tutorials/legato_config/legato_unlimited.png";
import legatoUpTo1BarImage from "assets/images/tutorials/legato_config/legato_up_to_1bar.png";
import legatoUpTo1BeatImage from "assets/images/tutorials/legato_config/legato_up_to_1beat.png";
import loopImage from "assets/images/tutorials/visual/loop.png";
import noLengtheningImage from "assets/images/tutorials/visual/no_lengthening.png";
import staticVisualImage from "assets/images/tutorials/visual/static.png";
import stretchImage from "assets/images/tutorials/visual/stretch.png";
import exampleThumbnail from "assets/images/ヨハネの氷.jpg";

export /* internal */ const stretches = [
	{ id: "noStretching", icon: "prohibited" },
	{ id: "flexingAndExtending", icon: "arrow_fit_both" },
	{ id: "extendingOnly", icon: "arrow_fit" },
	{ id: "flexingOnly", icon: "arrow_fit_in" },
] as const;
export /* internal */ const legatos = [
	{ id: "portato", icon: "prohibited", image: legatoPortatoImage },
	{ id: "upToOneBeat", icon: "quarter_note", image: legatoUpTo1BeatImage },
	{ id: "upToOneBar", icon: "music_bar", image: legatoUpTo1BarImage },
	{ id: "unlimited", icon: "infinity", image: legatoUnlimitedImage },
] as const;

export default function Visual() {
	const [enabled, setEnabled] = selectConfig(c => c.visual.enabled);
	const [ytpEnabled, setYtpEnabled] = selectConfig(c => c.ytp.enabled);
	const stretch = selectConfig(c => c.visual.stretch);
	const loop = selectConfig(c => c.visual.loop);
	const staticVisual = selectConfig(c => c.visual.staticVisual);
	const noLengthening = selectConfig(c => c.visual.noLengthening);
	const legato = selectConfig(c => c.visual.legato);
	const multitrackForChords = selectConfig(c => c.visual.multitrackForChords);
	const createGroups = selectConfig(c => c.createGroups);
	const glissando = selectConfig(c => c.visual.glissando);
	const transformOfx = selectConfig(c => c.visual.transformOfx);
	const enableStaffVisualizer = selectConfig(c => c.visual.enableStaffVisualizer);

	const { changePage, pushPage } = usePageStore();
	const TooltipPartial = Tooltip.with({ placement: "y" });

	return (
		<div className="container">
			<SettingsPageControlMedia stream="visual" fileName="我的视频.mp4" enabled={[enabled, setEnabled]} thumbnail={exampleThumbnail} />

			{!enabled ? (
				<EmptyMessage
					key="disabled"
					icon="visual"
					title={t.empty.disabled.title({ name: t.titles.visual })}
					details={t.empty.disabled.details({ name: t.titles.visual })}
					iconOff
				>
					<Button onClick={() => setEnabled(true)} accent>{t.enable}</Button>
				</EmptyMessage>
			) : ytpEnabled ? (
				<EmptyMessage
					key="ytpEnabled"
					icon="ytp"
					title={t.empty.ytpEnabled.title}
					details={t.empty.ytpEnabled.details}
				>
					<StackPanel>
						<Button onClick={() => setYtpEnabled(false)}>{t.empty.ytpEnabled.disableYtp}</Button>
						<Button onClick={() => changePage(["ytp"])} accent>{t.empty.ytpEnabled.gotoYtp}</Button>
					</StackPanel>
				</EmptyMessage>
			) : (
				<>
					<TooltipPartial title={<Tooltip.Content image={stretchImage} />}>
						<ExpanderRadio
							title={t.stream.stretch}
							details={t.descriptions.stream.stretch}
							icon="stretch"
							items={stretches}
							value={stretch as StateProperty<string>}
							view="tile"
							idField="id"
							nameField={t.stream.stretch}
							iconField="icon"
						/>
					</TooltipPartial>
					<TooltipPartial title={<Tooltip.Content image={loopImage} />}>
						<SettingsCardToggleSwitch title={t.stream.loop} details={t.descriptions.stream.loop} icon="loop" on={loop} />
					</TooltipPartial>
					<TooltipPartial title={<Tooltip.Content image={staticVisualImage} />}>
						<SettingsCardToggleSwitch title={t.stream.staticVisual} details={t.descriptions.stream.staticVisual} icon="visual" on={staticVisual} />
					</TooltipPartial>
					<TooltipPartial title={<Tooltip.Content image={noLengtheningImage} />}>
						<SettingsCardToggleSwitch title={t.stream.noLengthening} details={t.descriptions.stream.noLengthening.visual} icon="no_lengthening" on={noLengthening} />
					</TooltipPartial>
					<ExpanderRadio
						title={t.stream.legato}
						details={t.descriptions.stream.legato}
						icon="legato"
						items={legatos}
						value={legato as StateProperty<string>}
						view="grid"
						idField="id"
						nameField={t.stream.legato}
						iconField="icon"
						imageField="image"
						$itemWidth={566 / 196 * GRID_VIEW_ITEM_HEIGHT}
					/>
					<SettingsCardToggleSwitch title={t.stream.multitrackForChords} details={t.descriptions.stream.multitrackForChords} icon="chords" on={multitrackForChords} />
					<SettingsCardToggleSwitch title={t.stream.createGroups} details={t.descriptions.stream.createGroups} icon="group_object" on={createGroups} />
					<Expander
						title={t.stream.glissando}
						details={t.descriptions.stream.glissando}
						icon="swirl"
						actions={
							<ToggleSwitch on={glissando} />
						}
					/>
					<SettingsCardToggleSwitch title={t.stream.transformOfx} details={t.descriptions.stream.transformOfx} icon="zoom_fit" on={transformOfx} />

					<Subheader>{t.subheaders.effects}</Subheader>
					<SettingsCard title={t.titles.prve} details={t.descriptions.stream.effects.prve} type="button" icon="sparkle" onClick={() => pushPage("prve")} />
					<SettingsCard title={t.titles.staff} details={t.descriptions.stream.effects.staff} type="button" icon="g_clef" onClick={() => pushPage("staff")}>
						<ToggleSwitch on={enableStaffVisualizer} />
					</SettingsCard>

					<Subheader>{t.subheaders.parameters}</Subheader>
					<SettingsCard
						title={t.stream.mapping}
						details={t.descriptions.stream.mapping}
						icon="mapping"
						type="button"
					/>
				</>
			)}
		</div>
	);
}
