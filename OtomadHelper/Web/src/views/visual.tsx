import legatoPortatoImage from "assets/images/tutorials/legato_config/legato_portato.png";
import legatoUnlimitedImage from "assets/images/tutorials/legato_config/legato_unlimited.png";
import legatoUpTo1BarImage from "assets/images/tutorials/legato_config/legato_up_to_1bar.png";
import legatoUpTo1BeatImage from "assets/images/tutorials/legato_config/legato_up_to_1beat.png";
import loopImage from "assets/images/tutorials/visual/loop.png";
import noLengtheningImage from "assets/images/tutorials/visual/no_lengthening.png";
import staticVisualImage from "assets/images/tutorials/visual/static.png";
import stretchImage from "assets/images/tutorials/visual/stretch.png";
import exampleThumbnail from "assets/images/ヨハネの氷.png";

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
export /* internal */ const noLengthenings = [
	{ id: "lengthenable", icon: "lengthenable" },
	{ id: "freezeEndFrames", icon: "freeze_end_frames" },
	{ id: "trimEndFrames", icon: "trim_end_frames" },
	{ id: "splitThenFreeze", icon: "split_then_freeze" },
	{ id: "freezeToGray", icon: "freeze_to_gray" },
	{ id: "freezeToEffect", icon: "placeholder" },
] as const;
export /* internal */ const transformMethods = [
	"panCrop", "transformOfx",
] as const;

const tracks = [t.source.preferredTrack.newTrack, "1: Lead"];

const TooltipPartial = Tooltip.with({ placement: "y" });

export default function Visual() {
	const {
		enabled: [enabled, setEnabled],
		preferredTrack: [preferredTrackIndex, setPreferredTrackIndex],
		stretch, loop, staticVisual, noLengthening, legato, multitrackForChords, enableStaffVisualizer, enablePixelScaling, /* transformMethod */
	} = selectConfig(c => c.visual);
	const { enabled: glissando, amount: glissandoAmount } = selectConfig(c => c.visual.glissando);
	const { enabled: [ytpEnabled, setYtpEnabled] } = selectConfig(c => c.ytp);
	const { createGroups } = selectConfig(c => c);

	const { changePage, pushPage } = useSnapshot(pageStore);

	const preferredTrack = useMemo(() => {
		return [tracks[preferredTrackIndex], (item: string) => setPreferredTrackIndex(tracks.indexOf(item))] as StateProperty<string>;
	}, [preferredTrackIndex]);

	return (
		<div className="container">
			<SettingsPageControlMedia stream="visual" fileName="ヨハネの氷.mp4" enabled={[enabled, setEnabled]} thumbnail={exampleThumbnail} />

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
			) : (
				<>
					<SettingsCard title={t.source.preferredTrack} details={t.descriptions.source.preferredTrack} icon="preferred_track">
						<ComboBox current={preferredTrack} options={tracks} />
					</SettingsCard>
					<SettingsCardToggleSwitch title={t.stream.createGroups} details={t.descriptions.stream.createGroups} icon="group_object" on={createGroups} />
					{
						ytpEnabled ? (
							<EmptyMessage
								key="ytpEnabled"
								icon="ytp"
								title={t.empty.ytpEnabled.title}
								details={t.empty.ytpEnabled.details}
								noSideEffect
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
									<ExpanderRadio
										title={t.stream.noLengthening}
										details={t.descriptions.stream.noLengthening}
										icon="no_lengthening"
										items={noLengthenings}
										value={noLengthening as StateProperty<string>}
										view="tile"
										idField="id"
										nameField={t.stream.noLengthening}
										iconField="icon"
									/>
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
								<SettingsCardToggleSwitch
									title={t.stream.multitrackForChords}
									details={t.descriptions.stream.multitrackForChords}
									icon="chords"
									on={multitrackForChords}
								/>
								<Expander
									title={t.stream.glissando}
									details={t.descriptions.stream.glissando}
									icon="swirl"
									actions={
										<ToggleSwitch on={glissando} />
									}
								>
									<Expander.Item title={t.stream.glissando.amount} details={t.descriptions.stream.glissando.amount}>
										<TextBox.Number value={glissandoAmount} min={-24} max={24} suffix="key" />
									</Expander.Item>
								</Expander>
								{/* <ExpanderRadio
									title={t.stream.transformMethod}
									details={t.descriptions.stream.transformMethod}
									icon="zoom_fit"
									items={transformMethods}
									value={transformMethod}
									idField
									nameField={t.stream.transformMethod}
								/> // TODO: Change the integration method of TransformOFX into parameters, add an independent subheader and an info bar to tell user to download it.
								*/}

								<Subheader>{t.subheaders.effects}</Subheader>
								<SettingsCard title={t.titles.prve} details={t.descriptions.stream.effects.prve} type="button" icon="sparkle" onClick={() => pushPage("prve")} />
								<SettingsCard title={t.titles.staff} details={t.descriptions.stream.effects.staff} type="button" icon="g_clef" onClick={() => pushPage("staff")}>
									<ToggleSwitch on={enableStaffVisualizer} />
								</SettingsCard>
								<SettingsCard title={t.titles.pixelScaling} details={t.descriptions.stream.effects.pixelScaling} type="button" icon="miscz" onClick={() => pushPage("pixel-scaling")}>
									<ToggleSwitch on={enablePixelScaling} />
								</SettingsCard>
								<SettingsCard title={t.titles.customEffect} details={t.descriptions.stream.effects.customEffect} type="button" icon="placeholder" trailingIcon="open" />

								<Subheader>{t.subheaders.parameters}</Subheader>
								<SettingsCard
									title={t.stream.mapping}
									details={t.descriptions.stream.mapping}
									icon="mapping"
									type="button"
								/>
							</>
						)
					}
				</>
			)}
		</div>
	);
}
