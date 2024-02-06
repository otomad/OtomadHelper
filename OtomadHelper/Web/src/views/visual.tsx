import legatoPortatoImage from "assets/images/legato_config/legato_portato.png";
import legatoUnlimitedImage from "assets/images/legato_config/legato_unlimited.png";
import legatoUpTo1BarImage from "assets/images/legato_config/legato_up_to_1bar.png";
import legatoUpTo1BeatImage from "assets/images/legato_config/legato_up_to_1beat.png";
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
	const mappingVelocity = selectConfig(c => c.visual.mappingVelocity);
	const transformOfx = selectConfig(c => c.visual.transformOfx);
	const enableStaffVisualizer = selectConfig(c => c.visual.enableStaffVisualizer);

	const { changePage, pushPage } = usePageStore();

	return (
		<div className="container">
			<SettingsPageControlMedia stream="visual" fileName="我的视频.mp4" enabled={[enabled, setEnabled]} thumbnail={exampleThumbnail} />

			{!enabled ? (
				<EmptyMessage
					key="disabled"
					icon="visual"
					heading={t.empty.disabled.heading({ name: t.titles.visual })}
					caption={t.empty.disabled.caption({ name: t.titles.visual })}
				>
					<Button onClick={() => setEnabled(true)} accent>{t.enable}</Button>
				</EmptyMessage>
			) : ytpEnabled ? (
				<EmptyMessage
					key="ytpEnabled"
					icon="ytp"
					heading={t.empty.ytpEnabled.heading}
					caption={t.empty.ytpEnabled.caption}
				>
					<StackPanel>
						<Button onClick={() => setYtpEnabled(false)}>{t.empty.ytpEnabled.disableYtp}</Button>
						<Button onClick={() => changePage(["ytp"])} accent>{t.empty.ytpEnabled.gotoYtp}</Button>
					</StackPanel>
				</EmptyMessage>
			) : (
				<>
					<ExpanderRadio
						heading={t.audioVisual.stretch}
						caption={t.descriptions.audioVisual.stretch}
						icon="stretch"
						items={stretches}
						value={stretch as StateProperty<string>}
						view="tile"
						idField="id"
						nameField={t.audioVisual.stretch}
						iconField="icon"
					/>
					<SettingsCardToggleSwitch heading={t.audioVisual.loop} caption={t.descriptions.audioVisual.loop} icon="loop" on={loop} />
					<SettingsCardToggleSwitch heading={t.audioVisual.staticVisual} caption={t.descriptions.audioVisual.staticVisual} icon="visual" on={staticVisual} />
					<SettingsCardToggleSwitch heading={t.audioVisual.noLengthening} caption={t.descriptions.audioVisual.noLengthening.visual} icon="no_lengthening" on={noLengthening} />
					<ExpanderRadio
						heading={t.audioVisual.legato}
						caption={t.descriptions.audioVisual.legato}
						icon="legato"
						items={legatos}
						value={legato as StateProperty<string>}
						view="grid"
						idField="id"
						nameField={t.audioVisual.legato}
						iconField="icon"
						imageField="image"
						$itemWidth={566 / 196 * GRID_VIEW_ITEM_HEIGHT}
					/>
					<SettingsCardToggleSwitch heading={t.audioVisual.multitrackForChords} caption={t.descriptions.audioVisual.multitrackForChords} icon="chords" on={multitrackForChords} />
					<SettingsCardToggleSwitch heading={t.audioVisual.createGroups} caption={t.descriptions.audioVisual.createGroups} icon="group_object" on={createGroups} />
					<Expander
						heading={t.audioVisual.glissando}
						caption={t.descriptions.audioVisual.glissando}
						icon="swirl"
						actions={
							<ToggleSwitch on={glissando} />
						}
					/>
					<Expander
						heading={t.audioVisual.mappingVelocity}
						caption={t.descriptions.audioVisual.mappingVelocity}
						icon="signal"
						type="button"
						actions={
							<ToggleSwitch on={mappingVelocity} />
						}
					/>
					<SettingsCardToggleSwitch heading={t.audioVisual.transformOfx} caption={t.descriptions.audioVisual.transformOfx} icon="zoom_fit" on={transformOfx} />

					<Subheader>{t.subheaders.effects}</Subheader>
					<SettingsCard heading={t.titles.prve} caption={t.descriptions.audioVisual.effects.prve} type="button" icon="sparkle" onClick={() => pushPage("prve")} />
					<SettingsCard heading={t.titles.staff} caption={t.descriptions.audioVisual.effects.staff} type="button" icon="g_clef">
						<ToggleSwitch on={enableStaffVisualizer} />
					</SettingsCard>

					<Subheader>{t.subheaders.parameters}</Subheader>
				</>
			)}
		</div>
	);
}
