import legatoPortatoImage from "assets/images/tutorials/legato_config/legato_portato.png";
import legatoUnlimitedImage from "assets/images/tutorials/legato_config/legato_unlimited.png";
import legatoUpTo1BarImage from "assets/images/tutorials/legato_config/legato_up_to_1bar.png";
import legatoUpTo1BeatImage from "assets/images/tutorials/legato_config/legato_up_to_1beat.png";
import exampleThumbnail from "assets/images/ヨハネの氷.avif";
import { usePrveInfo } from "./visual/prve";

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
export /* @internal */ const truncates = [
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
export /* @internal */ const preRenders = [
	{ id: "instant", icon: "flash" },
	{ id: "media", icon: "media_forward" },
	{ id: "timeline", icon: "timeline_forward" },
] as const;
export /* @internal */ const glissandoEffects = [
	{ id: "swirl", name: t.shared.plugins.swirl },
	{ id: "wave", name: t.shared.plugins.wave },
	{ id: "tv", name: t({ context: "short" }).shared.plugins.tvSimulator },
	{ id: "pingpong", name: t.stream.articulations.glissando.pingpong },
] as const;

/** @deprecated */
const tracks = [t.source.preferredTrack.newTrack, "1: Lead"];

const buildInPresets = ["normal", "enter", "enterStaff", "fadeOut", "flashlight", "horizontalMovement", "verticalMovement", "ccwRotate", "cwRotate", "rainbowColor", "oversaturation", "highContrast", "thresholdChange"];

export default function Visual() {
	const {
		enabled, preferredTrack: preferredTrackIndex,
		stretch, loop, staticVisual, truncate, legato, multitrackForChords, transformMethod, currentPreset, stack, timeUnremapping,
		mimicalResample, mimicalOscillator, transition, transitionAlignment, transitionDuration,
		glissando, glissandoEffect, glissandoAmount, appoggiatura, arpeggio, arpeggioNegative, activeParameterScheme,
	} = useSelectConfig(c => c.visual);
	// const activeParameterScheme = useSelectConfigArray(c => c.visual.activeParameterScheme);
	const { enabled: enablePixelScaling } = useSelectConfig(c => c.visual.pixelScaling);
	const { enabled: enableStaffVisualizer } = useSelectConfig(c => c.visual.staff);
	const { createGroups } = useSelectConfig(c => c);
	const { prveCheckInfo, isForceStretch, prveCount } = usePrveInfo();
	const topPriorityTransformMethod = transformMethod[0][0];

	useEffect(() => { mimicalResample[0] === "true" && mimicalOscillator[0] === "true" && mimicalOscillator[1]("auto"); }, [mimicalResample[0]]);
	useEffect(() => { mimicalResample[0] === "true" && mimicalOscillator[0] === "true" && mimicalResample[1]("auto"); }, [mimicalOscillator[0]]);

	const { pushPage } = useSnapshot(pageStore);

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

			<EmptyMessage.Typical icon="image" title="visual" enabled={enabled}>
				<SettingsCard title={t.source.preferredTrack} details={t.descriptions.source.preferredTrack} icon="preferred_track">
					<StackPanel>
						<ComboBox current={preferredTrackIndex} ids={[...tracks.keys()]} options={tracks} />
						<QuicklySelectCurrentTrack />
					</StackPanel>
				</SettingsCard>
				<SettingsCardToggleSwitch title={t.stream.createGroups} details={t.descriptions.stream.createGroups} icon="group" on={createGroups} />
				<ExpanderStreamPlaybackRate stream="visual" />
				<SettingsCard
					title={t.stream.loop}
					details={t.descriptions.stream.loop}
					selectInfo={loop[0] === "auto" && t.descriptions.stream.loop.unset}
					icon="loop"
				>
					<ThreeStageSwitch current={loop} indetText={t.unset} indetIcon="line_horizontal" />
				</SettingsCard>
				<ExpanderStreamPreRender stream="visual" />
				<EmptyMessage.YtpDisabled>
					<ExpanderRadio
						title={t.stream.stretch}
						details={t.descriptions.stream.stretch}
						selectInfo={isForceStretch && t(prveCount).descriptions.prve.forceStretch}
						selectValid={false}
						icon="stretch"
						items={stretches}
						value={stretch}
						view="tile"
						idField="id"
						iconField="icon"
						nameField={t.stream.stretch}
						detailsField={t.descriptions.stream.stretch}
					/>
					<ExpanderRadio
						title={t.stream.truncate}
						details={t.descriptions.stream.truncate}
						icon="arrow_import_prohibited"
						items={truncates}
						value={truncate}
						view="tile"
						idField="id"
						iconField="icon"
						nameField={t.stream.truncate}
						detailsField={t.descriptions.stream.truncate}
					/>
					<SettingsCardToggleSwitch title={t.stream.staticVisual} details={t.descriptions.stream.staticVisual} icon="image" on={staticVisual} />
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
						itemWidth={566 / 196 * GRID_VIEW_ITEM_HEIGHT}
					/>
					<SettingsCardToggleSwitch
						title={t.stream.multitrackForChords}
						details={t.descriptions.stream.multitrackForChords}
						icon="chords"
						on={multitrackForChords}
					/>
					<SettingsCardToggleSwitch
						title={t.stream.stack}
						details={t.descriptions.stream.stack}
						icon="database_stack"
						on={stack}
					/>
					<SettingsCardToggleSwitch
						title={t.stream.timeUnremapping}
						details={t.descriptions.stream.timeUnremapping}
						icon="timer_off"
						on={timeUnremapping}
					/>
					<Expander
						title={t.stream.tuning.mimical}
						details={t.descriptions.stream.tuning.mimical}
						icon="tuning_image"
					>
						<InfoBar status="info">{t.descriptions.stream.tuning.mimical.auto}</InfoBar>
						<Expander.Item icon="link_multiple" title={t.stream.tuning.resample} details={t.descriptions.stream.tuning.mimical.resample}>
							<ThreeStageSwitch current={mimicalResample} />
						</Expander.Item>
						<Expander.Item icon="waveforms/triangle" title={t({ context: "full" }).stream.tuning.tuningMethod.oscillator} details={t.descriptions.stream.tuning.mimical.oscillator}>
							<ThreeStageSwitch current={mimicalOscillator} />
						</Expander.Item>
					</Expander>
					<SettingsCardToggleSwitch
						title={t.stream.transition}
						details={t.descriptions.stream.transition}
						icon="transition"
						on={transition}
					>
						<Expander.Item title={t.stream.transition.alignment} details={t.descriptions.stream.transition.alignment} icon="align_center_vertical">
							<Slider
								value={transitionAlignment}
								defaultValue={0}
								min={-100}
								max={100}
								displayValueStep={1}
								displayValue={value => ({
									"-100": t.stream.transition.alignment.end,
									0: t.stream.transition.alignment.center,
									100: t.stream.transition.alignment.start,
								})[value] ?? `${value > 0 ? "+" : ""}${value}%`}
							/>
						</Expander.Item>
						<Expander.Item title={t.duration} details={t.descriptions.stream.transition.duration} icon="timer">
							<TimecodeBox value={transitionDuration} />
						</Expander.Item>
						<InfoBar status="info">{t.descriptions.stream.transition.crossfadeInfo}</InfoBar>
					</SettingsCardToggleSwitch>
					<Expander
						title={t.stream.transformMethod}
						details={t.descriptions.stream.transformMethod}
						icon="zoom_fit"
						checkInfo={topPriorityTransformMethod && t.topPriority({ item: t.shared.plugins[topPriorityTransformMethod] })}
					>
						<SortableView items={transformMethod} fullyDraggable overlayEmits={{ onDrop: onSortableOverlayDrop }}>
							{(item, index) => (
								<Expander.Item
									title={t.shared.plugins[item]}
									icon={<div className="icon-placeholder"><Badge>{index + 1}</Badge></div>}
									clickable
								/>
							)}
						</SortableView>
					</Expander>
					{/* TODO: Change the integration method of TransformOFX into parameters, add an independent subheader and an info bar to tell user to download it. */}

					<Subheader>{t(2).titles.effect}</Subheader>
					<SettingsCard title={t.titles.prve} details={t.descriptions.stream.effects.prve} type="button" icon="sparkle" onClick={() => pushPage("prve")}>
						{prveCheckInfo}
					</SettingsCard>
					<SettingsCard title={t({ context: "full" }).titles.staff} details={t.descriptions.stream.effects.staff} type="button" icon="g_clef" onClick={() => pushPage("staff")}>
						<ToggleSwitch on={enableStaffVisualizer} />
					</SettingsCard>
					<SettingsCard title={t.titles.pixelScaling} details={t.descriptions.stream.effects.pixelScaling} type="button" icon="pixelate_zoom" onClick={() => pushPage("pixel-scaling")}>
						<ToggleSwitch on={enablePixelScaling} />
					</SettingsCard>

					<Subheader>{t.stream.articulations}</Subheader>
					<SettingsCardToggleSwitch
						title={t.stream.articulations.glissando}
						details={t.descriptions.stream.articulations.glissando}
						icon="slide_note"
						on={glissando}
					>
						<Expander.Item icon="sparkle" title={t.titles.effect}>
							<Segmented current={glissandoEffect}>
								{glissandoEffects.map(({ id, name }) =>
									<Segmented.Item key={id} id={id} icon={id}>{name}</Segmented.Item>)}
							</Segmented>
						</Expander.Item>
						<Expander.Item title={t.stream.articulations.glissando.swirlAmount} details={t.descriptions.stream.articulations.glissando.swirlAmount}>
							<TextBox.Number value={glissandoAmount} min={-24} max={24} suffix={t.units.semitone} positiveSign />
						</Expander.Item>
					</SettingsCardToggleSwitch>
					<SettingsCardToggleSwitch
						title={t.stream.articulations.appoggiatura}
						details={t.descriptions.stream.articulations.appoggiatura}
						icon="appoggiatura"
						on={appoggiatura}
					/>
					<SettingsCardToggleSwitch
						title={t.stream.articulations.arpeggio}
						details={t.descriptions.stream.articulations.arpeggio}
						icon="score"
						on={arpeggio}
					>
						<ToggleSwitch icon="invert_color" on={arpeggioNegative} details={t.descriptions.stream.articulations.arpeggio.negative}>{t.prve.effects.negative}</ToggleSwitch>
						<Expander.Item icon="preset" title={t.stream.articulations.applyCustomPreset}>
							<Button>{t.unselected}</Button>
						</Expander.Item>
					</SettingsCardToggleSwitch>

					<Subheader>{t.stream.mapping}</Subheader>
					<Expander title={t.stream.mapping.velocity} icon="signal" />
					<Expander title={t.stream.mapping.pitch} icon="music_note" />
					<Expander title={t.duration} icon="timer" />
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
						<Expander.ChildWrapper $tilePadding="tile view">
							<Button icon="add">{t.stream.preset.add}</Button>
						</Expander.ChildWrapper>
					</ExpanderRadio>
					<SortableView items={activeParameterScheme} unfocusableForSortableItems>
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
					</SortableView>
					<div>
						<Button icon="add">{t.new}</Button>
						{/* <Button icon="copy_add">{t.stream.parameters.copyAttributesFromSelectedClip}</Button> */}
					</div>
				</EmptyMessage.YtpDisabled>
			</EmptyMessage.Typical>
		</div>
	);
}
// TODO: 随机化正负角度 (±15°)
// 将在 -15° 至 15° 范围之间的非零角度随机浮动
// 0°时禁用
