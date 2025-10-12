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
export /* @internal */ const prerenders = [
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
	const meta = metas.visual;
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
				<Setting
					meta={meta.preferredTrack}
					actions={(
						<StackPanel>
							<ComboBox current={preferredTrackIndex} ids={[...tracks.keys()]} options={tracks} />
							<QuicklySelectCurrentTrack />
						</StackPanel>
					)}
				/>
				<Setting meta={meta.createGroups} on={createGroups} />
				<ExpanderStreamPlaybackRate stream="visual" />
				<Setting
					meta={meta.loop}
					selectInfo={loop[0] === "auto" && t.descriptions.stream.loop.unset}
					actions={<ThreeStageSwitch current={loop} indetText={t.unset} indetIcon="line_horizontal" />}
				/>
				<ExpanderStreamPrerender stream="visual" />
				<EmptyMessage.YtpDisabled>
					<Setting
						meta={meta.stretch}
						selectInfo={isForceStretch && t(prveCount).descriptions.prve.forceStretch}
						selectValid={false}
						items={stretches}
						value={stretch}
						view="tile"
						idField="id"
						iconField="icon"
						nameField={t.stream.stretch}
						detailsField={t.descriptions.stream.stretch}
					/>
					<Setting
						meta={meta.truncate}
						items={truncates}
						value={truncate}
						view="tile"
						idField="id"
						iconField="icon"
						nameField={t.stream.truncate}
						detailsField={t.descriptions.stream.truncate}
					/>
					<Setting meta={meta.staticVisual} on={staticVisual} />
					<Setting
						meta={meta.legato}
						items={legatos}
						value={legato}
						view="grid"
						idField="id"
						nameField={t.stream.legato}
						iconField="icon"
						imageField="image"
						itemWidth={566 / 196 * GRID_VIEW_ITEM_HEIGHT}
					/>
					<Setting meta={meta.multitrackForChords} on={multitrackForChords} />
					<Setting meta={meta.stack} on={stack} />
					<Setting meta={meta.timeUnremapping} on={timeUnremapping} />
					<Setting meta={meta.mimical}>
						<InfoBar status="info">{t.descriptions.stream.tuning.mimical.auto}</InfoBar>
						<Setting meta={meta.mimical.resample} actions={<ThreeStageSwitch current={mimicalResample} />} />
						<Setting meta={meta.mimical.oscillator} actions={<ThreeStageSwitch current={mimicalOscillator} />} />
					</Setting>
					<Setting meta={meta.transition} on={transition}>
						<Setting
							meta={meta.transition.alignment}
							actions={(
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
							)}
						/>
						<Setting meta={meta.transition.duration} actions={<TimecodeBox value={transitionDuration} />} />
						<InfoBar status="info">{t.descriptions.stream.transition.crossfadeInfo}</InfoBar>
					</Setting>
					<Setting meta={meta.transformMethod} checkInfo={topPriorityTransformMethod && t.topPriority({ item: t.shared.plugins[topPriorityTransformMethod] })}>
						<SortableView items={transformMethod} fullyDraggable overlayEmits={{ onDrop: onSortableOverlayDrop }}>
							{(item, index) => (
								<Expander.Item
									title={t.shared.plugins[item]}
									icon={<div className="icon-placeholder"><Badge>{index + 1}</Badge></div>}
									clickable
									nonFocusable
								/>
							)}
						</SortableView>
					</Setting>
					{/* TODO: Change the integration method of TransformOFX into parameters, add an independent subheader and an info bar to tell user to download it. */}

					<Subheader meta={meta.effects} />
					<Setting meta={meta.prve} actions={prveCheckInfo} />
					<Setting meta={meta.staff} actions={<ToggleSwitch on={enableStaffVisualizer} />} />
					<Setting meta={meta.pixelScaling} actions={<ToggleSwitch on={enablePixelScaling} />} />

					<Subheader meta={meta.articulations} />
					<Setting meta={meta.articulations.glissando} on={glissando}>
						<Expander.Item icon="sparkle" title={t.titles.effect}>
							<Segmented current={glissandoEffect}>
								{glissandoEffects.map(({ id, name }) =>
									<Segmented.Item key={id} id={id} icon={id}>{name}</Segmented.Item>)}
							</Segmented>
						</Expander.Item>
						<Expander.Item title={t.stream.articulations.glissando.swirlAmount} details={t.descriptions.stream.articulations.glissando.swirlAmount}>
							<TextBox.Number value={glissandoAmount} min={-24} max={24} suffix={t.units.semitone} positiveSign />
						</Expander.Item>
					</Setting>
					<Setting meta={meta.articulations.appoggiatura} on={appoggiatura} />
					<Setting meta={meta.articulations.arpeggio} on={arpeggio}>
						<Setting meta={meta.articulations.arpeggio.negative} on={arpeggioNegative} />
						<Setting meta={meta.articulations.arpeggio.applyCustomPreset} actions={<Button>{t.unselected}</Button>} />
					</Setting>

					<Subheader meta={meta.mapping} />
					<Setting meta={meta.mapping.velocity} />
					<Setting meta={meta.mapping.pitch} />
					<Setting meta={meta.mapping.duration} />
					<Setting meta={meta.mapping.pan} />
					<Setting meta={meta.mapping.progress} />

					<Subheader meta={meta.parameters} />
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
					<SortableView items={activeParameterScheme} nonFocusableForSortableItems>
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
