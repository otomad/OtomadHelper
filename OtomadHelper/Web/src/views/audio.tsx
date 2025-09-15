import exampleThumbnail from "assets/images/ヨハネの氷.avif";
import { legatos, stretches, truncates } from "./visual";
const truncatesInAudio = truncates.filter(item => item.availableInAudio);

export /* @internal */ const tuningMethods = [
	{ id: "none", icon: "prohibited" },
	{ id: "unset", icon: "line_horizontal" },
	{ id: "pitchShift", icon: "plugin" },
	{ id: "elastic", icon: "plus_minus" },
	{ id: "classic", icon: "hourglass" },
	{ id: "oscillator", icon: "waveforms/triangle" },
] as const;

export /* @internal */ const exactTuningMethods = [
	{ id: "none", icon: "prohibited" },
	{ id: "elastic", icon: "plus_minus" },
	{ id: "classic", icon: "hourglass" },
	{ id: "acid", icon: "logo/acid" },
] as const;

export /* @internal */ const exceeds = [
	{ id: "plugin", icon: "plugin_multiple" },
	{ id: "octave", icon: "octave" },
	{ id: "octaveExp", icon: "octave_beaker" },
	{ id: "wrap", icon: "abs_leq" },
	{ id: "silent", icon: "speaker_mute" },
] as const;

export /* @internal */ const normalizeTimes = [
	{ id: "false", icon: "dismiss" },
	{ id: "once", icon: "checkmark_1" },
	{ id: "always", icon: "checkmark_multiple" },
] as const;

export /* @internal */ const beepEngines = ["WebAudio"] as const;
const beepWaveforms = ["sinusoid", "triangle", "square", "sawtooth"] as const satisfies OscillatorCommonType[];

export /* @internal */ const tuningElasticModes = ["pro", "efficient", "soloist_monophonic", "soloist_speech"] as const;
export /* @internal */ const tuningClassicModes = forMap(19, i => "a" + String(i).padStart(2, "0"), 1) as readonly (keyof LocaleIdentifiers["javascript"]["stream"]["tuning"]["stretchAttributes"]["classic"])[];

/** @deprecated */
const tracks = [t.source.preferredTrack.newTrack, "1: Lead"];

const buildInPresets = ["normal", "fadeOut"];

const TooltipPartial = Tooltip.with({ placement: "y" });

// #region Styles
const PrelistenActions = styled(StackPanel)`
	position: relative;
	display: flex;
	align-items: stretch;

	* {
		transition-behavior: allow-discrete;
	}

	:has(~ .stop.shown) {
		opacity: 0;
		pointer-events: none;
		// Do not use \`visibility: hidden;\`, it has a bad behavior.
	}

	.stop {
		position: absolute;
		inset: 0;

		&:not(.shown) {
			opacity: 0;
			pointer-events: none;
		}
	}
`;

const TuningMethodEvaluation = styled.ul`
	li {
		display: flex;
		gap: 6px;
		align-items: flex-end;

		.icon {
			font-size: 14px;
		}
	}
`;
// #endregion

export default function Audio() {
	const {
		enabled, preferredTrack: preferredTrackIndex,
		stretch, loop, normalize, truncate, legato, multitrackForChords, stack, timeUnremapping, autoPan, autoPanCurve,
		tuningMethod, tuningMethodAcid, tuningMethodScaleless,
		stretchAttributeElastic, stretchAttributeClassic, stretchAttributePitchShift, alternativeForExceedTheRange, resample, preserveFormant, currentPreset,
		basePitch, basePitchBased, cent, glissando,
	} = useSelectConfig(c => c.audio);
	const { engine, waveform, duration: beepDuration, volume: beepVolume, adjustAudioToBasePitch } = useSelectConfig(c => c.audio.prelistenAttributes);
	const { createGroups } = useSelectConfig(c => c);
	const activeParameterScheme = useSelectConfigArray(c => c.audio.activeParameterScheme);
	const [stopPrelistening, setStopPrelistening] = useState<() => void>();
	const tuningMethodScalelessUnlocked = tuningMethod[0].in("unset", "elastic", "classic"), tuningMethodScalelessEnabled = tuningMethodScaleless[0] && tuningMethodScalelessUnlocked;
	const alternativeForExceedTheRangeDisabled = !tuningMethod[0].in("elastic", "classic", "unset");

	const { pushPage } = useSnapshot(pageStore);

	function prelistenBasePitch() {
		if (stopPrelistening) {
			stopPrelistening();
			return;
		}
		if (engine[0] === "WebAudio") {
			const { stop, promise } = beep(waveform[0],
				(adjustAudioToBasePitch[0] ? new Pitch("C", 5) : new Pitch(basePitch[0])).frequency, beepDuration[0], beepVolume[0]);
			setStopPrelistening(() => stop);
			promise.then(() => setStopPrelistening(undefined));
		} else return;
	}

	return (
		<div className="container">
			<SettingsPageControlMedia stream="audio" fileName="ヨハネの氷.mp4" enabled={enabled} thumbnail={exampleThumbnail} />

			<EmptyMessage.Typical icon="volume" title="audio" enabled={enabled}>
				<SettingsCard title={t.source.preferredTrack} details={t.descriptions.source.preferredTrack} icon="preferred_track">
					<StackPanel>
						<ComboBox current={preferredTrackIndex} ids={[...tracks.keys()]} options={tracks} />
						<QuicklySelectCurrentTrack />
					</StackPanel>
				</SettingsCard>
				<SettingsCardToggleSwitch title={t.stream.createGroups} details={t.descriptions.stream.createGroups} icon="group" on={createGroups} />
				<ExpanderStreamPlaybackRate stream="audio" />
				{/* When using segmented control, excessive explanation of its functions may occupy a large amount of interface space and potentially affect the user experience. */}
				{/* <SettingsCard title={t.stream.normalize} details={t.descriptions.stream.normalize} icon="normalize" selectInfo={normalize[0] !== "false" && t.descriptions.stream.normalize[normalize[0]]}>
					<Segmented current={normalize}>
						{normalizeTimes.map(({ id, icon }) => <Segmented.Item id={id} key={id} icon={icon}>{id === "false" ? t.off : t.stream.normalize[id]}</Segmented.Item>)}
					</Segmented>
				</SettingsCard> */}
				<ExpanderRadio
					title={t.stream.normalize}
					details={t.descriptions.stream.normalize}
					icon="normalize"
					items={normalizeTimes}
					value={normalize}
					view="list"
					idField="id"
					iconField="icon"
					nameField={({ id }) => id === "false" ? t.off : t.stream.normalize[id]}
					checkInfoCondition={id => id === "false" ? t.off : t.stream.normalize[id!]}
					detailsField={({ id }) => id === "false" ? undefined : t.descriptions.stream.normalize[id]}
				/>
				<SettingsCard
					title={t.stream.loop}
					details={t.descriptions.stream.loop}
					selectInfo={loop[0] === "auto" && t.descriptions.stream.loop.unset}
					icon="loop"
				>
					<ThreeStageSwitch current={loop} indetText={t.unset} indetIcon="line_horizontal" />
				</SettingsCard>
				<ExpanderStreamPreRender stream="audio" />
				<EmptyMessage.YtpDisabled>
					<ExpanderRadio
						title={t.stream.stretch}
						details={t.descriptions.stream.stretch}
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
						items={truncatesInAudio}
						value={truncate}
						view="tile"
						idField="id"
						iconField="icon"
						nameField={t.stream.truncate}
						detailsField={t.descriptions.stream.truncate}
					>
						<TruncateAndLegatoConflictInfoBar />
					</ExpanderRadio>
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
					>
						<TruncateAndLegatoConflictInfoBar />
					</ExpanderRadio>
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
					<SettingsCardToggleSwitch
						title={t.stream.autoPan}
						details={t.descriptions.stream.autoPan}
						icon="stereo"
						on={autoPan}
					>
						<Expander.Item.Curve curve={autoPanCurve} />
					</SettingsCardToggleSwitch>

					<Subheader>{t.stream.tuning}</Subheader>
					<ExpanderRadio
						title={t.stream.tuning.tuningMethod}
						details={t.descriptions.stream.tuning.tuningMethod}
						icon="tuning"
						items={tuningMethods}
						value={tuningMethod}
						view="tile"
						itemsViewItemAttrs={{ topAlignIcon: true }}
						idField="id"
						iconField="icon"
						nameField={({ id }) => id === "unset" ? t.unset : t.stream.tuning.tuningMethod[id]}
						checkInfoCondition={id => id === "unset" ? t.unset : t.stream.tuning.tuningMethod[id!]}
						detailsField={({ id }) => {
							const evaluable = id.in("pitchShift", "elastic", "classic"), isAudioFx = id === "pitchShift";
							const check = (bool: boolean) => (bool ? "checkmark" : "dismiss") satisfies DeclaredIcons;
							return (
								<>
									<p>{t.descriptions.stream.tuning.tuningMethod[id]}</p>
									{evaluable && (
										<TuningMethodEvaluation>
											<li><Icon name={check(!isAudioFx)} />{t.descriptions.stream.tuning.tuningMethod.evaluates.fast}</li>
											<li><Icon name={check(!isAudioFx)} />{t.descriptions.stream.tuning.tuningMethod.evaluates.changeRate}</li>
											<li><Icon name={check(isAudioFx)} />{t.descriptions.stream.tuning.tuningMethod.evaluates.exceedTheRange}</li>
										</TuningMethodEvaluation>
									)}
								</>
							);
						}}
					>
						<ToggleSwitch on={tuningMethodAcid} lock={tuningMethod[0] !== "none" ? null : false} icon="logo/acid" details={t.descriptions.stream.tuning.tuningMethod.acid}>{t.stream.tuning.tuningMethod.acid}</ToggleSwitch>
						<ToggleSwitch on={tuningMethodScaleless} lock={tuningMethodScalelessUnlocked ? null : false} icon="scaleless" details={t.descriptions.stream.tuning.tuningMethod.scaleless}>{t.stream.tuning.tuningMethod.scaleless}</ToggleSwitch>
					</ExpanderRadio>
					<Attrs disabled={tuningMethod[0] === "none" || tuningMethodScalelessEnabled || undefined}>
						<ExpanderRadio<Any, Any>
							title={t.stream.tuning.stretchAttributes}
							icon="tuning_wrench"
							view="tile"
							idField
							{
								...tuningMethod[0] === "elastic" ? {
									value: stretchAttributeElastic,
									items: tuningElasticModes,
									nameField: t.stream.tuning.stretchAttributes.elastic,
									details: t({ context: "elastic" }).descriptions.stream.tuning.stretchAttributes,
								} : tuningMethod[0].in("classic", "pitchShift") ? {
									value: tuningMethod[0] === "pitchShift" ? stretchAttributePitchShift : stretchAttributeClassic,
									items: tuningClassicModes,
									nameField: (id: string) => <TuningClassicModeListItem id={id} />,
									checkInfoCondition: (id: string) => t.stream.tuning.stretchAttributes.classic[id],
									details: t({ context: "classic" }).descriptions.stream.tuning.stretchAttributes,
								} : {
									disabled: true,
									value: [],
									items: [],
									details: t.descriptions.stream.tuning.stretchAttributes,
								}
							}
						/>
						<Attrs disabled={tuningMethod[0] === "oscillator" || undefined}>
							<ExpanderRadio
								title={t.stream.tuning.alternativeForExceedTheRange}
								details={t.descriptions.stream.tuning.alternativeForExceedTheRange}
								icon="tuning_warning"
								items={exceeds}
								value={alternativeForExceedTheRange}
								view="list"
								idField="id"
								iconField="icon"
								nameField={({ id }) => t.stream.tuning.alternativeForExceedTheRange[id]}
								detailsField={item => (
									<TransInterpolation
										i18nKey={t => t.descriptions.stream.tuning.alternativeForExceedTheRange[item.id]}
										formulaFor39={<MathFormulaFor39 />}
										formulaFor24="±24"
									/>
								)}
								checkInfoCondition={id => t.stream.tuning.alternativeForExceedTheRange[id!]}
								disabled={alternativeForExceedTheRangeDisabled}
								checkInfo={alternativeForExceedTheRangeDisabled ? tuningMethod[0] === "pitchShift" ? t.stream.tuning.alternativeForExceedTheRange.multiple : t.stream.tuning.tuningMethod[tuningMethod[0]] : undefined}
							/>
							<SettingsCardToggleSwitch
								title={t.stream.tuning.resample}
								details={t.descriptions.stream.tuning.resample}
								icon="link_multiple"
								lock={tuningMethodScalelessEnabled ? true : tuningMethod[0] === "oscillator" ? false : null}
								on={resample}
							/>
							<SettingsCardToggleSwitch
								title={t.stream.tuning.preserveFormant}
								details={t.descriptions.stream.tuning.preserveFormant}
								icon="speech"
								on={preserveFormant}
								lock={tuningMethod[0].in("elastic", "unset") ? null : false}
							/>
						</Attrs>
						<Expander
							title={t.stream.tuning.basePitch}
							details={t.descriptions.stream.tuning.basePitch}
							icon="music_note"
							actions={<PitchPicker spn={basePitch} />}
						>
							<Expander.Item title={t.stream.tuning.basePitch.cent} details={t.descriptions.stream.tuning.basePitch.cent} icon="fine_tune">
								<SliderWithBox
									value={cent}
									min={-100}
									max={100}
									decimalPlaces={0}
									defaultValue={0}
									suffix={t(cent[0]).units.cent}
								/>
							</Expander.Item>
							<ToggleSwitch icon="relative" on={basePitchBased} details={t.descriptions.stream.tuning.basePitch.based}>{t.stream.tuning.basePitch.based}</ToggleSwitch>
							<ToggleSwitch icon="tuning_sparkle" on={[false]} disabled selectInfo={t.underConstruction}>{t.stream.tuning.basePitch.auto}</ToggleSwitch>
						</Expander>
						<Expander
							title={t.stream.tuning.prelisten}
							details={t.descriptions.stream.tuning.prelisten}
							icon="headphone"
							actions={(
								<PrelistenActions>
									<Button onClick={prelistenBasePitch}>{t.stream.tuning.prelisten.basePitch}</Button>
									<Button>{t.stream.tuning.prelisten.audio}</Button>
									<Button icon="stop" className={["stop", { shown: stopPrelistening }]} onClick={stopPrelistening}>{t.stream.tuning.prelisten.stop}</Button>
								</PrelistenActions>
							)}
						>
							<Expander.Item icon="table_column_top_bottom" title={t.stream.tuning.prelisten.engine}>
								<ComboBox current={engine} ids={beepEngines} options={beepEngines} />
							</Expander.Item>
							<Expander.Item icon="sound_wave" title={t.stream.tuning.prelisten.waveform}>
								<ComboBox
									current={waveform}
									ids={beepWaveforms}
									options={beepWaveforms.map(waveform => t.stream.tuning.prelisten.waveform[waveform])}
									icons={beepWaveforms.map(waveform => `waveforms/${waveform}` as const)}
								/>
							</Expander.Item>
							<Expander.Item icon="timer" title={t.duration}>
								<TextBox.Number value={beepDuration} min={0} decimalPlaces={0} spinnerStep={100} suffix={t.units.millisecond} />
							</Expander.Item>
							<Expander.Item icon="volume" title={t.stream.tuning.prelisten.volumeForBasePitch}>
								<Slider
									value={beepVolume}
									min={0}
									max={1}
									step={0.01}
									defaultValue={1}
									displayValue={value => (value * 100 | 0) + t.units.percent}
								/>
							</Expander.Item>
							<ToggleSwitch icon="remix_add" on={adjustAudioToBasePitch} details={t.descriptions.stream.tuning.prelisten.adjustAudioToBasePitch}>{t.stream.tuning.prelisten.adjustAudioToBasePitch}</ToggleSwitch>
						</Expander>
						<SettingsCardToggleSwitch
							on={glissando}
							title={t.stream.articulations.glissando}
							details={t.descriptions.stream.articulations.glissando}
							icon="slide_note"
						/>
					</Attrs>

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
					{activeParameterScheme.map((scheme, i) => (
						<SettingsCard
							title={scheme.name[0]}
							details={listFormat(scheme.parameters[0], "conjunction", "narrow")}
							type="button"
							key={i}
							icon
							onClick={() => pushPage("parameters")}
						>
							<ToggleSwitch on={scheme.enabled} />
						</SettingsCard>
					))}
					<div>
						<Button icon="add">{t.new}</Button>
					</div>
				</EmptyMessage.YtpDisabled>
			</EmptyMessage.Typical>
		</div>
	);
}

function TruncateAndLegatoConflictInfoBar() {
	return <InfoBar status="accent" title={t.descriptions.stream.truncateAndLegatoConflictInAudio} />;
}

subscribeStoreKey(configStore.audio, "truncate", value => value !== "lengthenable" && (configStore.audio.legato = "portato"));
subscribeStoreKey(configStore.audio, "legato", value => value !== "portato" && (configStore.audio.truncate = "lengthenable"));
