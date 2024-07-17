import exampleThumbnail from "assets/images/ヨハネの氷.png";
import { legatos, noLengthenings, stretches } from "./visual";
const noLengtheningsInAudio = noLengthenings.filter(item => item.availableInAudio);

export /* @internal */ const tuningMethods = [
	{ id: "noTuning", icon: "prohibited" },
	{ id: "pitchShift", icon: "plugin" },
	{ id: "elastic", icon: "plus_minus" },
	{ id: "classic", icon: "hourglass" },
	{ id: "scaleless", icon: "scaleless" },
] as const;

export /* @internal */ const exceeds = [
	{ id: "plugin", icon: "plugin_multiple" },
	{ id: "octave", icon: "octave" },
	{ id: "dock", icon: "abs_leq" },
	{ id: "silent", icon: "speaker_mute" },
] as const;
const getExceedsName = (id: string | undefined, tuningMethod: string) => !id ? "" : t.stream.tuning.alternativeForExceedsTheRange[
	id === "plugin" && tuningMethod === "pitchShift" ? "multiple" : id
];

const beepEngines = ["ConsoleBeep", "PowerShell", "NAudio"];
const beepWaveforms = ["sinusoid", "triangle", "square", "sawtooth"];

const tracks = [t.source.preferredTrack.newTrack, "1: Lead"];

const TooltipPartial = Tooltip.with({ placement: "y" });

export default function Audio() {
	const {
		enabled, preferredTrack: [preferredTrackIndex, setPreferredTrackIndex],
		stretch, loop, normalize, noLengthening, legato, multitrackForChords,
		tuningMethod, stretchAttribute, alternativeForExceedsTheRange, resample, preserveFormant, basePitch,
	} = selectConfig(c => c.audio);
	const { engine, waveform, duration: beepDuration, adjustAudioToBasePitch } = selectConfig(c => c.audio.prelistenAttributes);
	const { createGroups } = selectConfig(c => c);

	const { pushPage } = useSnapshot(pageStore);

	const preferredTrack = useMemo(() => {
		return [tracks[preferredTrackIndex], (item: string) => setPreferredTrackIndex(tracks.indexOf(item))] as StateProperty<string>;
	}, [preferredTrackIndex]);

	return (
		<div className="container">
			<SettingsPageControlMedia stream="audio" fileName="ヨハネの氷.mp4" enabled={enabled} thumbnail={exampleThumbnail} />

			<EmptyMessage.Typical icon="audio" name={t.titles.audio} enabled={enabled}>
				<SettingsCard title={t.source.preferredTrack} details={t.descriptions.source.preferredTrack} icon="preferred_track">
					<ComboBox current={preferredTrack} options={tracks} />
				</SettingsCard>
				<SettingsCardToggleSwitch title={t.stream.createGroups} details={t.descriptions.stream.createGroups} icon="group_object" on={createGroups} />
				<EmptyMessage.YtpDisabled>
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
					<SettingsCardToggleSwitch title={t.stream.loop} details={t.descriptions.stream.loop} icon="loop" on={loop} />
					<SettingsCardToggleSwitch title={t.stream.normalize} details={t.descriptions.stream.normalize} icon="normalize" on={normalize} />
					<ExpanderRadio
						title={t.stream.noLengthening}
						details={t.descriptions.stream.noLengthening}
						icon="no_lengthening"
						items={noLengtheningsInAudio}
						value={noLengthening as StateProperty<string>}
						view="tile"
						idField="id"
						nameField={t.stream.noLengthening}
						iconField="icon"
					/>
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

					<Subheader>{t.stream.tuning}</Subheader>
					<ExpanderRadio
						title={t.stream.tuning.tuningMethod}
						icon="tuning"
						items={tuningMethods}
						value={tuningMethod as StateProperty<string>}
						view="tile"
						idField="id"
						iconField="icon"
						nameField={t.stream.tuning.tuningMethod}
					/>
					<Disabled disabled={tuningMethod[0] === "noTuning"}>
						<ExpanderRadio
							title={t.stream.tuning.stretchAttributes}
							details={t.descriptions.stream.tuning.stretchAttributes}
							icon="tuning_wrench"
							items={[]}
							value={stretchAttribute as StateProperty<string>}
							view="tile"
							idField="id"
							nameField={t.stream.tuning.stretchAttributes}
						/>
						<ExpanderRadio
							title={t.stream.tuning.alternativeForExceedsTheRange}
							details={t.descriptions.stream.tuning.alternativeForExceedsTheRange}
							icon="exceeds"
							items={exceeds}
							value={alternativeForExceedsTheRange as StateProperty<string>}
							view="tile"
							idField="id"
							iconField="icon"
							nameField={item => getExceedsName(item.id, tuningMethod[0])}
							checkInfoCondition={id => getExceedsName(id, tuningMethod[0])}
						/>
						<SettingsCardToggleSwitch title={t.stream.tuning.resample} details={t.descriptions.stream.tuning.resample} icon="lock" on={resample} />
						<SettingsCardToggleSwitch title={t.stream.tuning.preserveFormant} details={t.descriptions.stream.tuning.preserveFormant} icon="speech" on={preserveFormant} />
						<SettingsCard title={t.stream.tuning.basePitch} details={t.descriptions.stream.tuning.basePitch} icon="music_note">
							<PitchPicker pitch={basePitch} />
						</SettingsCard>
						<Expander
							title={t.stream.tuning.prelisten}
							details={t.descriptions.stream.tuning.prelisten}
							icon="headphone"
							actions={(
								<StackPanel $gap={6}>
									<Button>{t.stream.tuning.prelisten.basePitch}</Button>
									<Button>{t.stream.tuning.prelisten.audio}</Button>
								</StackPanel>
							)}
						>
							<Expander.Item title={t.stream.tuning.prelisten.engine}>
								<ComboBox options={beepEngines} current={engine} />
							</Expander.Item>
							<Expander.Item title={t.stream.tuning.prelisten.waveform}>
								<ComboBox options={beepWaveforms.map(waveform => t.stream.tuning.prelisten.waveform[waveform])} current={waveform} />
							</Expander.Item>
							<Expander.Item title={t.stream.tuning.prelisten.duration}>
								<TextBox.Number value={beepDuration} min={0} decimalPlaces={0} suffix={t.units.milliseconds} />
							</Expander.Item>
							<ToggleSwitch on={adjustAudioToBasePitch} details={t.descriptions.stream.tuning.prelisten.adjustAudioToBasePitch}>{t.stream.tuning.prelisten.adjustAudioToBasePitch}</ToggleSwitch>
						</Expander>
					</Disabled>

					<Subheader>{t.subheaders.effects}</Subheader>
					<SettingsCard
						title={t.titles.customEffect}
						details={t.descriptions.stream.effects.customEffect}
						type="button"
						icon="custom_effect"
						trailingIcon="open"
					/>

					<Subheader>{t.subheaders.parameters}</Subheader>
					<SettingsCard
						title={t.stream.mapping}
						details={t.descriptions.stream.mapping}
						icon="mapping"
						type="button"
					/>
				</EmptyMessage.YtpDisabled>
			</EmptyMessage.Typical>
		</div>
	);
}
