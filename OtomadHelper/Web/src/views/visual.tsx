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
						heading={t.stream.stretch}
						caption={t.descriptions.stream.stretch}
						icon="stretch"
						items={stretches}
						value={stretch as StateProperty<string>}
						view="tile"
						idField="id"
						nameField={t.stream.stretch}
						iconField="icon"
					/>
					<SettingsCardToggleSwitch heading={t.stream.loop} caption={t.descriptions.stream.loop} icon="loop" on={loop} />
					<SettingsCardToggleSwitch heading={t.stream.staticVisual} caption={t.descriptions.stream.staticVisual} icon="visual" on={staticVisual} />
					<SettingsCardToggleSwitch heading={t.stream.noLengthening} caption={t.descriptions.stream.noLengthening.visual} icon="no_lengthening" on={noLengthening} />
					<ExpanderRadio
						heading={t.stream.legato}
						caption={t.descriptions.stream.legato}
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
					<SettingsCardToggleSwitch heading={t.stream.multitrackForChords} caption={t.descriptions.stream.multitrackForChords} icon="chords" on={multitrackForChords} />
					<SettingsCardToggleSwitch heading={t.stream.createGroups} caption={t.descriptions.stream.createGroups} icon="group_object" on={createGroups} />
					<Expander
						heading={t.stream.glissando}
						caption={t.descriptions.stream.glissando}
						icon="swirl"
						actions={
							<ToggleSwitch on={glissando} />
						}
					/>
					<SettingsCardToggleSwitch heading={t.stream.transformOfx} caption={t.descriptions.stream.transformOfx} icon="zoom_fit" on={transformOfx} />

					<Subheader>{t.subheaders.effects}</Subheader>
					<SettingsCard heading={t.titles.prve} caption={t.descriptions.stream.effects.prve} type="button" icon="sparkle" onClick={() => pushPage("prve")} />
					<SettingsCard heading={t.titles.staff} caption={t.descriptions.stream.effects.staff} type="button" icon="g_clef" onClick={() => pushPage("staff")}>
						<ToggleSwitch on={enableStaffVisualizer} />
					</SettingsCard>

					<Subheader>{t.subheaders.parameters}</Subheader>
					<SettingsCard
						heading={t.stream.mapping}
						caption={t.descriptions.stream.mapping}
						icon="mapping"
						type="button"
					/>
				</>
			)}
		</div>
	);
}
