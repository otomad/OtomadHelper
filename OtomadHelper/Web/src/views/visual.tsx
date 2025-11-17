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
	{ id: "lengthenable", icon: "lengthenable", availableInAudio: true, idleEffectApplicable: false },
	{ id: "freezeEndFrames", icon: "freeze_end_frames", availableInAudio: false, idleEffectApplicable: false },
	{ id: "trimEndFrames", icon: "trim_end_frames", availableInAudio: true, idleEffectApplicable: false },
	{ id: "splitThenFreeze", icon: "split_then_freeze", availableInAudio: false, idleEffectApplicable: true },
	{ id: "splitThenResume", icon: "split_then_resume", availableInAudio: false, idleEffectApplicable: true },
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

const builtInPresets = ["normal", "enter", "enterStaff", "exit", "fadeOut", "flashlight", "floatLeft", "floatRight", "floatUp", "floatDown", "ccwRotate", "cwRotate", "colorful", "oversaturation", "highContrast", "lumaFade"];
const asteriskBuiltInPresets = ["floatLeft", "floatRight", "floatUp", "floatDown", "ccwRotate", "cwRotate"];

export default function Visual() {
	const {
		enabled, preferredTrack: preferredTrackIndex,
		stretch, loop, staticVisual, truncate, truncateIdleEffect,
		legato, multitrackForChords, transformMethod, currentPreset, stack, timeUnremapping, presetPreviewIdeality,
		mimicalResample, mimicalOscillator, transition, transitionAlignment, transitionDuration, transitionCrossfadeCurve,
		glissando, glissandoEffect, glissandoAmount, appoggiatura, arpeggio, arpeggioIdleEffect, activeParameterScheme,
	} = useSelectConfig(c => c.visual);
	// const activeParameterScheme = useSelectConfigArray(c => c.visual.activeParameterScheme);
	const { enabled: enablePixelScaling } = useSelectConfig(c => c.visual.pixelScaling);
	const { enabled: enableStaffVisualizer } = useSelectConfig(c => c.visual.staff);
	const { createGroups } = useSelectConfig(c => c);
	const { prveCheckInfo, isForceStretch, prveCount } = usePrveInfo();
	const meta = metas.visual;
	const topPriorityTransformMethod = transformMethod[0][0];

	useEffect(() => { mimicalResample[0] && mimicalOscillator[0] && mimicalOscillator[1](null); }, [mimicalResample[0]]);
	useEffect(() => { mimicalResample[0] && mimicalOscillator[0] && mimicalResample[1](null); }, [mimicalOscillator[0]]);

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
					selectInfo={loop[0] === null && t.descriptions.stream.loop.unset}
					actions={<TriStateSwitch current={loop} indetText={t.unset} indetIcon="subtract" />}
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
					>
						<IdleEffectSettings
							value={truncateIdleEffect}
							disabled={!truncates.find(({ id }) => id === truncate[0])?.idleEffectApplicable}
							pinToTop="monochrome"
							details={t.descriptions.stream.truncate.idleEffect}
							disabledInfo={t.descriptions.stream.truncate.idleEffectUnavailable({ modes: truncates.filter(({ idleEffectApplicable }) => idleEffectApplicable).map(({ id }) => t.stream.truncate[id]) })}
						/>
					</Setting>
					<Setting
						meta={meta.prologue}
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
						<InfoBar>{t.descriptions.stream.tuning.mimical.auto}</InfoBar>
						<Setting meta={meta.mimical.resample} actions={<TriStateSwitch current={mimicalResample} indetText={t.auto} indetIcon="auto" />} />
						<Setting meta={meta.mimical.oscillator} actions={<TriStateSwitch current={mimicalOscillator} indetText={t.auto} indetIcon="auto" />} />
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
						<Expander.Item.CrossfadeCurve curve={transitionCrossfadeCurve} subset="exceptHold" />
						<InfoBar>{t.descriptions.stream.transition.crossfadeInfo}</InfoBar>
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
						<Setting
							meta={meta.articulations.glissando.amplitude}
							details={t.descriptions.amplitude({ effect: glissandoEffects.find(({ id }) => id === glissandoEffect[0])?.name })}
							actions={<TextBox.Number value={glissandoAmount} min={-24} max={24} suffix={t.units.semitone} positiveSign />}
						/>
					</Setting>
					<Setting meta={meta.articulations.appoggiatura} on={appoggiatura} />
					<Setting meta={meta.articulations.arpeggio} on={arpeggio}>
						<IdleEffectSettings
							value={arpeggioIdleEffect}
							pinToTop="negative"
							details={t.descriptions.stream.articulations.arpeggio.idleEffect}
						/>
					</Setting>

					<Subheader meta={meta.mapping} />
					<Setting meta={meta.mapping.velocity} />
					<Setting meta={meta.mapping.pitch} />
					<Setting meta={meta.mapping.duration} />
					<Setting meta={meta.mapping.pan} />
					<Setting meta={meta.mapping.progress} />

					<Subheader meta={meta.parameters} />
					<Setting meta={meta.preset} checkInfo={t.stream.preset.builtInPresets[currentPreset[0]]}>
						<Setting meta={meta.preset.builtInPresets} asSubtitle="closerAfter" noDivider="after" />
						<ItemsView view="grid" current={currentPreset}>
							{builtInPresets.map(name => (
								<ItemsView.Item
									id={name}
									key={name}
									image={<PreviewParameterPreset key={name} thumbnail={exampleThumbnail} name={name} previewIdeality={presetPreviewIdeality[0]} />}
									badge={asteriskBuiltInPresets.includes(name) && [undefined, "asterisk"]}
								>
									{t.stream.preset.builtInPresets[name]}
								</ItemsView.Item>
							))}
						</ItemsView>
						<Setting meta={meta.preset.previewIdeality} on={presetPreviewIdeality} />
						<Setting meta={meta.preset.customPresets} asSubtitle noDivider="after" />
						<div>
							<EmptyMessage.Mini icon="preset">{t.descriptions.stream.preset.empty}</EmptyMessage.Mini>
						</div>
						<Expander.ChildWrapper $tilePadding="standard button to item">
							<Button icon="add">{t.stream.preset.add}</Button>
						</Expander.ChildWrapper>
					</Setting>
					<SortableView items={activeParameterScheme} nonFocusableForSortableItems>
						{scheme => (
							<SettingsCard
								title={scheme.name[0]}
								details={listFormat(scheme.parameters[0])}
								type="button"
								icon
								dragHandle
								onClick={() => pushPage("parameters")}
							>
								<ToggleSwitch on={scheme.enabled} />
								<Tooltip placement="block" title={t.descriptions.condition}>
									<Button subtle icon="filter" minWidthUnbounded />
								</Tooltip>
							</SettingsCard>
						)}
					</SortableView>
					<div>
						<Button icon="add">{t.new}</Button>
					</div>
				</EmptyMessage.YtpDisabled>
			</EmptyMessage.Typical>
		</div>
	);
}
// TODO: 随机化正负角度 (±15°)
// 将在 -15° 至 15° 范围之间的非零角度随机浮动
// 0°时禁用
