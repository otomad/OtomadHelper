import legatoPortatoImage from "assets/images/tutorials/legato_config/legato_portato.png";
import legatoUnlimitedImage from "assets/images/tutorials/legato_config/legato_unlimited.png";
import legatoUpTo1BarImage from "assets/images/tutorials/legato_config/legato_up_to_1bar.png";
import legatoUpTo1BeatImage from "assets/images/tutorials/legato_config/legato_up_to_1beat.png";
import loopImage from "assets/images/tutorials/visual/loop.png";
import staticVisualImage from "assets/images/tutorials/visual/static.png";
import stretchImage from "assets/images/tutorials/visual/stretch.png";
import unlengthenImage from "assets/images/tutorials/visual/unlengthen.png";
import exampleThumbnail from "assets/images/ヨハネの氷.png";
import { useIsForceStretch, usePrveCheckInfo } from "./visual/prve";

export /* @internal */ const stretches = [
	{ id: "noStretching", icon: "prohibited" },
	{ id: "flexingAndExtending", icon: "arrow_fit_both" },
	{ id: "extendingOnly", icon: "arrow_fit" },
	{ id: "flexingOnly", icon: "arrow_fit_in" },
] as const;
export /* @internal */ const legatos = [
	{ id: "portato", icon: "prohibited", image: legatoPortatoImage },
	{ id: "upToOneBeat", icon: "quarter_note", image: legatoUpTo1BeatImage },
	{ id: "upToOneBar", icon: "music_bar", image: legatoUpTo1BarImage },
	{ id: "unlimited", icon: "infinity", image: legatoUnlimitedImage },
] as const;
export /* @internal */ const unlengthens = [
	{ id: "lengthenable", icon: "lengthenable", availableInAudio: true },
	{ id: "freezeEndFrames", icon: "freeze_end_frames", availableInAudio: false },
	{ id: "trimEndFrames", icon: "trim_end_frames", availableInAudio: true },
	{ id: "splitThenFreeze", icon: "split_then_freeze", availableInAudio: false },
	{ id: "freezeToGray", icon: "freeze_to_gray", availableInAudio: false },
	{ id: "freezeToPreset", icon: "freeze_to_preset", availableInAudio: false },
] as const;
export /* @internal */ const transformMethods = [
	"panCrop", "pictureInPicture", "transformOfx",
] as const;

const tracks = [t.source.preferredTrack.newTrack, "1: Lead"];

const buildInPresets = ["normal", "enter", "enterStaff", "fadeOut", "flashlight", "horizontalMovement", "verticalMovement", "ccwRotate", "cwRotate", "rainbowColor", "oversaturation", "highContrast", "thresholdChange"];

const TooltipPartial = Tooltip.with({ placement: "y" });

export default function Visual() {
	const {
		enabled, preferredTrack: [preferredTrackIndex, setPreferredTrackIndex],
		stretch, loop, staticVisual, unlengthen, legato, multitrackForChords, enableStaffVisualizer, transformMethod, currentPreset, timeUnremapping, resampleImitatively,
		glissando, glissandoEffect, glissandoAmount, appoggiatura, arpeggio, arpeggioNegative, activeParameterScheme,
	} = selectConfig(c => c.visual);
	// const activeParameterScheme = selectConfigArray(c => c.visual.activeParameterScheme);
	const { enabled: enablePixelScaling } = selectConfig(c => c.visual.pixelScaling);
	const { createGroups } = selectConfig(c => c);
	const prveCheckInfo = usePrveCheckInfo();
	const isForceStretch = useIsForceStretch();
	const topPriorityTransformMethod = transformMethod[0][0];

	const { pushPage } = useSnapshot(pageStore);

	const preferredTrack = useMemo(() => {
		return [tracks[preferredTrackIndex], (item: string) => setPreferredTrackIndex(tracks.indexOf(item))] as StateProperty<string>;
	}, [preferredTrackIndex]);

	const onSortableOverlayDrop = useCallback<DropAnimationSideEffects>(({ dragOverlay: { node: dragOverlay } }) => {
		const expanderItem = dragOverlay.firstElementChild! as HTMLDivElement;
		const id = expanderItem.dataset.id!;
		const { transformMethod } = configStore.visual;
		const newIndex = transformMethod.indexOf(id as never);
		if (newIndex === -1) return;
		expanderItem.querySelector(".badge .text")!.textContent = String(newIndex + 1);
	}, []);

	return (
		<div className="container">
			<SettingsPageControlMedia stream="visual" fileName="ヨハネの氷.mp4" enabled={enabled} thumbnail={exampleThumbnail} />

			<EmptyMessage.Typical icon="visual" title="visual" enabled={enabled}>
				<SettingsCard title={t.source.preferredTrack} details={t.descriptions.source.preferredTrack} icon="preferred_track">
					<ComboBox current={preferredTrack} options={tracks} />
				</SettingsCard>
				<SettingsCardToggleSwitch title={t.stream.createGroups} details={t.descriptions.stream.createGroups} icon="group_object" on={createGroups} />
				<EmptyMessage.YtpDisabled>
					<TooltipPartial title={<Tooltip.Content image={stretchImage} />}>
						<ExpanderRadio
							title={t.stream.stretch}
							details={t.descriptions.stream.stretch}
							selectInfo={isForceStretch && t.descriptions.prve.forceStretch}
							selectValid={false}
							icon="stretch"
							items={stretches}
							value={stretch}
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
					<TooltipPartial title={<Tooltip.Content image={unlengthenImage} />}>
						<ExpanderRadio
							title={t.stream.unlengthen}
							details={t.descriptions.stream.unlengthen}
							icon="arrow_import_prohibited"
							items={unlengthens}
							value={unlengthen}
							view="tile"
							idField="id"
							nameField={t.stream.unlengthen}
							iconField="icon"
						/>
					</TooltipPartial>
					<ExpanderRadio
						title={t.stream.legato}
						details={t.descriptions.stream.legato}
						icon="legato"
						items={legatos}
						value={legato}
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
					<SettingsCardToggleSwitch
						title={t.stream.timeUnremapping}
						details={t.descriptions.stream.timeUnremapping}
						icon="timer_off"
						on={timeUnremapping}
					/>
					<SettingsCard
						title={t.stream.resampleImitatively}
						details={(
							<>
								{t.descriptions.stream.resampleImitatively}<br />
								{resampleImitatively[0] === "auto" && t.descriptions.stream.resampleImitatively.auto}
							</>
						)}
						icon="lock"
					>
						<ThreeStageSwitch current={resampleImitatively} />
					</SettingsCard>
					{/* <ExpanderRadio
						title={t.stream.transformMethod}
						details={t.descriptions.stream.transformMethod}
						icon="zoom_fit"
						items={transformMethods}
						value={transformMethod}
						idField
						nameField={t.stream.transformMethod}
					/> */}
					<Expander
						title={t.stream.transformMethod}
						details={t.descriptions.stream.transformMethod}
						icon="zoom_fit"
						checkInfo={topPriorityTransformMethod && t.topPriority({ item: t.stream.transformMethod[topPriorityTransformMethod] })}
					>
						<SortableList items={transformMethod} fullyDraggable overlayEmits={{ onDrop: onSortableOverlayDrop }}>
							{(item, index) => (
								<Expander.Item
									title={t.stream.transformMethod[item]}
									icon={<div className="icon-placeholder"><Badge>{index + 1}</Badge></div>}
									clickable
								/>
							)}
						</SortableList>
					</Expander>
					{/* TODO: Change the integration method of TransformOFX into parameters, add an independent subheader and an info bar to tell user to download it. */}

					<Subheader>{t(2).titles.effect}</Subheader>
					<SettingsCard title={t.titles.prve} details={t.descriptions.stream.effects.prve} type="button" icon="sparkle" onClick={() => pushPage("prve")}>
						{prveCheckInfo}
					</SettingsCard>
					<SettingsCard title={t({ context: "full" }).titles.staff} details={t.descriptions.stream.effects.staff} type="button" icon="g_clef" onClick={() => pushPage("staff")}>
						<ToggleSwitch on={enableStaffVisualizer} />
					</SettingsCard>
					<SettingsCard title={t.titles.pixelScaling} details={t.descriptions.stream.effects.pixelScaling} type="button" icon="miscz" onClick={() => pushPage("pixel-scaling")}>
						<ToggleSwitch on={enablePixelScaling} />
					</SettingsCard>

					<Subheader>{t.stream.playingTechniques}</Subheader>
					<Expander
						title={t.stream.playingTechniques.glissando}
						details={t.descriptions.stream.playingTechniques.glissando}
						icon="swirl"
						actions={<ToggleSwitch on={glissando} />}
					>
						<Expander.Item icon="sparkle" title={t.titles.effect}>
							<Segmented current={glissandoEffect}>
								<Segmented.Item icon="swirl" id="swirl">{t.stream.playingTechniques.glissando.swirl}</Segmented.Item>
								<Segmented.Item icon="pingpong" id="pingpong">{t.stream.playingTechniques.glissando.pingpong}</Segmented.Item>
							</Segmented>
						</Expander.Item>
						<Expander.Item title={t.stream.playingTechniques.glissando.swirlAmount} details={t.descriptions.stream.playingTechniques.glissando.swirlAmount}>
							<TextBox.Number value={glissandoAmount} min={-24} max={24} suffix={t.units.semitones} positiveSign />
						</Expander.Item>
					</Expander>
					<Expander
						title={t.stream.playingTechniques.appoggiatura}
						details={t.descriptions.stream.playingTechniques.appoggiatura}
						icon="appoggiatura"
						actions={<ToggleSwitch on={appoggiatura} />}
					/>
					<Expander
						title={t.stream.playingTechniques.arpeggio}
						details={t.descriptions.stream.playingTechniques.arpeggio}
						icon="arpeggio"
						actions={<ToggleSwitch on={arpeggio} />}
					>
						<ToggleSwitch icon="invert_color" on={arpeggioNegative} details={t.descriptions.stream.playingTechniques.arpeggio.negative}>{t.prve.effects.negative}</ToggleSwitch>
						<Expander.Item icon="preset" title={t.stream.playingTechniques.applyCustomPreset}>
							<Button>{t.unselected}</Button>
						</Expander.Item>
					</Expander>

					<Subheader>{t.stream.mapping}</Subheader>
					<Expander title={t.stream.mapping.velocity} icon="signal" />
					<Expander title={t.stream.mapping.pitch} icon="music_note" />
					<Expander title={t.stream.mapping.duration} icon="timer" />
					<Expander title={t.stream.mapping.pan} icon="stereo" />
					<Expander title={t.stream.mapping.progress} icon="progress_bar" />

					<Subheader>{t.subheaders.parameters}</Subheader>
					<ExpanderRadio
						title={t.preset}
						details={t.descriptions.stream.preset}
						icon="preset"
						items={buildInPresets}
						value={currentPreset}
						view="tile"
						idField
						nameField
					>
						<Expander.ChildWrapper>
							<Button icon="add">{t.stream.preset.add}</Button>
						</Expander.ChildWrapper>
					</ExpanderRadio>
					<SortableList items={activeParameterScheme}>
						{scheme => (
							<SettingsCard
								title={scheme.name[0]}
								details={listFormat(scheme.parameters[0], "conjunction", "narrow")}
								type="button"
								icon
								dragHandle
								onClick={() => pushPage("parameters")}
							>
								<ToggleSwitch on={scheme.enabled} />
								<Tooltip placement="y" title={t.descriptions.condition}>
									<Button subtle icon="filter" minWidthUnbounded />
								</Tooltip>
							</SettingsCard>
						)}
					</SortableList>
					<div>
						<Button icon="add">{t.new}</Button>
						<Button icon="copy_add">{t.stream.parameters.copyAttributesFromSelectedClip}</Button>
					</div>
				</EmptyMessage.YtpDisabled>
			</EmptyMessage.Typical>
		</div>
	);
}
