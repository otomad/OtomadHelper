import { stretches, truncates } from "./visual";
const truncatesInAudio = truncates.filter(item => item.availableInAudio);

export /* @internal */ const tuningMethods = [
	{ id: "none", icon: "prohibited" },
	{ id: "unset", icon: "subtract" },
	{ id: "pitchShift", icon: "plugin" },
	{ id: "elastic", icon: "add_subtract" },
	{ id: "classic", icon: "hourglass" },
	{ id: "oscillator", icon: "waveforms/sawtooth" },
] as const;

export /* @internal */ const exactTuningMethods = [
	{ id: "none", icon: "prohibited", originalName: undefined },
	{ id: "elastic", icon: "add_subtract", originalName: "Élastique" },
	{ id: "classic", icon: "hourglass", originalName: "Classic" },
	{ id: "acid", icon: "logo/acid", originalName: "ACID" },
] as const;

export /* @internal */ const exceeds = [
	{ id: "plugin", icon: "plugin_multiple" },
	{ id: "octave", icon: "octave" },
	{ id: "octaveExp", icon: "octave_beaker" },
	{ id: "dock", icon: "abs_leq" },
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

const builtInPresets = ["normal", "fadeOut"];

// #region Styles
const PrelistenActions = styled(StackPanel)`
	position: relative;
	display: flex;
	flex-wrap: nowrap;
	align-items: stretch;

	* {
		white-space: nowrap;
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
	margin-block-start: 3px;
	transition-behavior: allow-discrete;

	li {
		display: flex;
		gap: 6px;
		align-items: end;

		.icon {
			font-size: 14px;
		}
	}

	~ .settings-card-select-info {
		margin-block-start: 3px;
		margin-inline-start: 1.5px;

		> .badge {
			margin-inline-end: 7px;
		}

		&:not(.shown) {
			opacity: 0;
			visibility: hidden;
		}
	}
`;
// #endregion

export default function Audio() {
	const {
		enabled, preferredTrack: preferredTrackIndex,
		stretch, loop, normalize, truncate, multitrackForChords, stack, timeUnremapping, autoPan, autoPanCurve,
		/* tuningMethod, */ tuningMethodAcid, /* tuningMethodScaleless, */
		stretchAttributeElastic, stretchAttributeClassic, stretchAttributePitchShift, altTuningMethod,
		resample, preserveFormant, vocalFry, currentPreset,
		/* basePitch, */ basePitchBased, cent, glissando,
	} = useSubConfig(c => c.audio);
	const {
		noTuning, tuningMethod, tuningMethodScaleless, basePitch,
	} = useSelectConfig(c => c.audio);
	const { engine, waveform, duration: beepDuration, volume: beepVolume, adjustAudioToBasePitch } = useSubConfig(c => c.audio.prelistenAttributes);
	const { createGroups } = useSubConfig(c => c);
	const activeParameterScheme = useSelectConfigArray(c => c.audio.activeParameterScheme);
	const meta = metas.audio;
	const [stopPrelistening, setStopPrelistening] = useState<() => void>();
	const tuningMethodScalelessUnlocked = !noTuning[0] && tuningMethod[0].in("unset", "elastic", "classic"), tuningMethodScalelessEnabled = tuningMethodScaleless[0] && tuningMethodScalelessUnlocked;
	const altTuningMethodDisabled = !tuningMethod[0].in("elastic", "classic", "unset");
	const { thumbnail } = useThumbnail();

	const { pushPage } = useSnapshot(pageStore);

	function prelistenBasePitch() {
		if (stopPrelistening) {
			stopPrelistening();
			return;
		}
		if (engine.value === "WebAudio") {
			const { stop, promise } = beep(waveform.value,
				(adjustAudioToBasePitch.value ? new Pitch("C", 5) : new Pitch(basePitch[0])).frequency, beepDuration.value, beepVolume.value);
			setStopPrelistening(() => stop);
			promise.then(() => setStopPrelistening(undefined));
		} else return;
	}

	return (
		<div className="container">
			<SettingsPageControlMedia stream="audio" fileName="ヨハネの氷.mp4" enabled={enabled} thumbnail={thumbnail} />

			<EmptyMessage.Typical icon="volume" title="audio" enabled={enabled}>
				<Setting
					meta={meta.preferredTrack}
					actions={(
						<StackPanel>
							<ComboBox current={preferredTrackIndex} ids={[...tracks.keys()]} options={tracks} />
							<QuickSelectCurrentTrack />
						</StackPanel>
					)}
				/>
				<Setting meta={meta.createGroups} on={createGroups} />
				<ExpanderStreamPlaybackRate stream="audio" />
				{/* When using segmented control, excessive explanation of its functions may occupy a large amount of interface space and potentially affect the user experience. */}
				<Setting
					meta={meta.normalize}
					items={normalizeTimes}
					value={normalize}
					view="list"
					idField="id"
					iconField="icon"
					nameField={({ id }) => id === "false" ? t.off : t.stream.normalize[id]}
					checkInfoCondition={id => id === "false" ? t.off : t.stream.normalize[id!]}
					detailsField={({ id }) => id === "false" ? undefined : t.descriptions.stream.normalize[id]}
				/>
				<Setting
					meta={meta.loop}
					selectInfo={subKeys(loop, loop => loop === null && t.descriptions.stream.loop.unset)}
					actions={<TriStateSwitch current={loop} indetText={t.unset} indetIcon="subtract" />}
				/>
				<ExpanderStreamPrerender stream="audio" />
				<EmptyMessage.YtpDisabled>
					<Setting
						meta={meta.stretch}
						items={stretches}
						value={stretch}
						view="tile"
						ieOff
						idField="id"
						iconField="icon"
						nameField={t.stream.stretch}
						detailsField={t.descriptions.stream.stretch}
						disabled={tuningMethod[0] === "none"}
						selectInfo={tuningMethod[0] === "none" && t.descriptions.stream.stretch.disabledByTuningInfo}
						selectValid="warning"
					/>
					<Setting
						meta={meta.truncate}
						items={truncatesInAudio}
						value={truncate}
						view="tile"
						ieOff
						idField="id"
						iconField="icon"
						nameField={t.stream.truncate}
						detailsField={t.descriptions.stream.truncate}
					>
						<TruncateAndLegatoConflictInfoBar />
					</Setting>
					<ExpanderStreamPrologue stream="audio" />
					<ExpanderLegato stream="audio">
						<TruncateAndLegatoConflictInfoBar />
					</ExpanderLegato>
					<Setting meta={meta.multitrackForChords} on={multitrackForChords} />
					<Setting meta={meta.stack} on={stack} />
					<Setting meta={meta.timeUnremapping} on={timeUnremapping} />
					<Setting meta={meta.autoPan} on={autoPan}><Expander.Item.Curve curve={autoPanCurve} /></Setting>

					<Subheader meta={meta.tuning} />
					<Setting
						meta={meta.tuning.tuningMethod}
						items={tuningMethods}
						value={tuningMethod}
						view="tile"
						itemsViewItemAttrs={{ alignItems: "start" }}
						idField="id"
						iconField="icon"
						nameField={({ id }) => id === "unset" ? t.unset : t.stream.tuning.tuningMethod[id]}
						checkInfoCondition={id => id === "unset" ? t.unset : t.stream.tuning.tuningMethod[id!]}
						detailsField={({ id }) => {
							const evaluable = id.in("pitchShift", "elastic", "classic", "oscillator"), isAudioFx = id === "pitchShift", isOscillator = id === "oscillator", sameAsUnset = id.in("pitchShift", "oscillator");
							const check = (bool: boolean) => (bool ? "checkmark" : "dismiss") satisfies DeclaredIcons;
							return (
								<>
									<p>{t.descriptions.stream.tuning.tuningMethod[id]}</p>
									{evaluable && (
										<TuningMethodEvaluation>
											<li><Icon name={check(!isAudioFx && !isOscillator)} />{t.descriptions.stream.tuning.tuningMethod.evaluates.fast}</li>
											<li><Icon name={check(!isAudioFx)} />{t.descriptions.stream.tuning.tuningMethod.evaluates.changeRate}</li>
											<li><Icon name={check(isAudioFx || isOscillator)} />{t.descriptions.stream.tuning.tuningMethod.evaluates.exceedTheRange}</li>
										</TuningMethodEvaluation>
									)}
									{sameAsUnset && <SettingsCard.SelectInfo valid="info" className={{ shown: noTuning[0] }}>{t.descriptions.stream.tuning.tuningMethod.sameAsUnsetInfo}</SettingsCard.SelectInfo>}
								</>
							);
						}}
						before={<Setting meta={meta.tuning.tuningMethod.noTuning} on={noTuning} />}
					>
						<Setting meta={meta.tuning.tuningMethod.acid} on={tuningMethodAcid} lock={noTuning[0] || tuningMethod[0].in("none", "oscillator") ? false : null} />
						<Setting meta={meta.tuning.tuningMethod.scaleless} on={tuningMethodScaleless} lock={tuningMethodScalelessUnlocked ? null : false} />
					</Setting>
					<Setting<Any, Any>
						meta={meta.tuning.stretchAttributes}
						view="tile"
						idField
						{
							...tuningMethod[0] === "elastic" ? {
								value: stretchAttributeElastic,
								items: tuningElasticModes,
								nameField: (id: string) => t.stream.tuning.stretchAttributes.elastic[id],
								checkInfoCondition: (id: string) => t.stream.tuning.stretchAttributes.elastic[id],
								details: t.descriptions.stream.tuning.stretchAttributes({ context: "elastic" }),
							} : tuningMethod[0].in("classic", "pitchShift") ? {
								value: tuningMethod[0] === "pitchShift" ? stretchAttributePitchShift : stretchAttributeClassic,
								items: tuningClassicModes,
								nameField: (id: string) => <TuningClassicModeListItem id={id} />,
								checkInfoCondition: (id: string) => t.stream.tuning.stretchAttributes.classic[id],
								details: t.descriptions.stream.tuning.stretchAttributes({ context: "classic" }),
							} : {
								disabled: true,
								value: [],
								items: [],
								details: t.descriptions.stream.tuning.stretchAttributes,
							}
						}
						disabled={!tuningMethod[0].in("elastic", "classic", "pitchShift") || tuningMethodScalelessEnabled}
					/>
					<Attrs disabled={noTuning[0] || tuningMethod[0] === "none" || tuningMethodScalelessEnabled || undefined}>
						<Attrs disabled={tuningMethod[0] === "oscillator" || undefined}>
							<Setting
								meta={meta.tuning.altTuningMethod}
								items={exceeds}
								value={altTuningMethod}
								view="list"
								idField="id"
								iconField="icon"
								nameField={({ id }) => t.stream.tuning.altTuningMethod[id]}
								detailsField={item => (
									<TransInterpolation
										i18nKey={t.descriptions.stream.tuning.altTuningMethod[item.id]}
										formulaFor39={<MathFormulaFor39 />}
										formulaFor24="±24"
									/>
								)}
								checkInfoCondition={id => t.stream.tuning.altTuningMethod[id!]}
								disabled={altTuningMethodDisabled}
								checkInfo={altTuningMethodDisabled ? tuningMethod[0] === "pitchShift" ? t.stream.tuning.altTuningMethod.multiple : t.stream.tuning.tuningMethod[tuningMethod[0]] : undefined}
							/>
							<Setting
								meta={meta.tuning.resample}
								on={resample}
								lock={tuningMethodScalelessEnabled ? true : tuningMethod[0] === "oscillator" ? false : null}
							/>
							<Setting
								meta={meta.tuning.preserveFormant}
								on={preserveFormant}
								lock={tuningMethod[0].in("elastic", "unset") ? null : false}
							/>
						</Attrs>
						<Setting
							meta={meta.tuning.vocalFry}
							on={vocalFry}
							lock={tuningMethod[0] === "oscillator" ? null : false}
						/>
						<Setting meta={meta.tuning.basePitch} actions={<PitchPicker spn={basePitch} />}>
							<PianoPicker pitch={basePitch} showReset />
							<Setting
								meta={meta.tuning.basePitch.cent}
								actions={(
									<SliderWithBox
										value={cent}
										min={-100}
										max={100}
										decimalPlaces={0}
										defaultValue={0}
										suffix={cent => t(cent).units.cent}
									/>
								)}
							/>
							<Setting meta={meta.tuning.basePitch.based} on={basePitchBased} />
							<Setting meta={meta.tuning.basePitch.auto} on={[false]} disabled selectInfo={t.underConstruction} />
						</Setting>
						<Setting
							meta={meta.tuning.prelisten}
							actions={(
								<PrelistenActions>
									<Button onClick={prelistenBasePitch}>{t.stream.tuning.prelisten.basePitch}</Button>
									<Button>{t.stream.tuning.prelisten.audio}</Button>
									<Button icon="stop" className={["stop", { shown: stopPrelistening }]} onClick={stopPrelistening}>{t.stream.tuning.prelisten.stop}</Button>
								</PrelistenActions>
							)}
						>
							<Setting
								meta={meta.tuning.prelisten.engine}
								actions={<ComboBox current={engine} ids={beepEngines} options={beepEngines} />}
							/>
							<Setting
								meta={meta.tuning.prelisten.waveform}
								actions={(
									<ComboBox
										current={waveform}
										ids={beepWaveforms}
										options={beepWaveforms.map(waveform => t.stream.tuning.prelisten.waveform[waveform])}
										icons={beepWaveforms.map(waveform => `waveforms/${waveform}` as const)}
									/>
								)}
							/>
							<Setting
								meta={meta.tuning.prelisten.duration}
								actions={<TextBox.Number value={beepDuration} min={0} decimalPlaces={0} spinnerStep={100} suffix={t.units.millisecond} />}
							/>
							<Setting
								meta={meta.tuning.prelisten.volumeForBasePitch}
								actions={(
									<Slider
										value={beepVolume}
										min={0}
										max={1}
										step={0.01}
										keyStep={0.01}
										defaultValue={1}
										displayValue={value => (value * 100 | 0) + t.units.percent}
									/>
								)}
							/>
							<Setting meta={meta.tuning.prelisten.adjustAudioToBasePitch} on={adjustAudioToBasePitch} />
						</Setting>
						<Setting meta={meta.tuning.glissando} on={glissando} />
					</Attrs>

					<Subheader meta={meta.mapping} />
					<Setting meta={meta.mapping.velocity} />
					<Setting meta={meta.mapping.pitch} />
					<Setting meta={meta.mapping.duration} />
					<Setting meta={meta.mapping.pan} />
					<Setting meta={meta.mapping.progress} />

					<Subheader meta={meta.parameters} />
					<Setting meta={meta.preset} checkInfo={subKeys(currentPreset, currentPreset => t.stream.preset.builtInPresets[currentPreset])}>
						<Setting meta={meta.preset.builtInPresets} asSubtitle="closerAfter" noDivider="after" />
						<ItemsView view="tile" current={currentPreset}>
							{builtInPresets.map(name => <ItemsView.Item id={name} key={name}>{t.stream.preset.builtInPresets[name]}</ItemsView.Item>)}
						</ItemsView>
						<Setting meta={meta.preset.customPresets} asSubtitle noDivider="after" />
						<div>
							<EmptyMessage.Mini icon="preset">{t.descriptions.stream.preset.empty}</EmptyMessage.Mini>
						</div>
						<Expander.ChildWrapper $tilePadding="standard button to item">
							<Button icon="add">{t.stream.preset.add}</Button>
						</Expander.ChildWrapper>
					</Setting>
					{activeParameterScheme.map((scheme, i) => (
						<SettingsCard
							title={scheme.name[0]}
							details={listFormat(scheme.parameters[0])}
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
	const { hideUseTips } = useSnapshot(configStore.settings);
	if (hideUseTips) return;
	return <InfoBar status="accent" title={t.descriptions.stream.truncateAndLegatoConflictInAudio} />;
}

subscribeStoreKey(configStore.audio, "truncate", value => value !== "lengthenable" && (configStore.audio.legatoDuration = "portato"));
subscribeStoreKey(configStore.audio, "legatoDuration", value => value !== "portato" && (configStore.audio.truncate = "lengthenable"));
