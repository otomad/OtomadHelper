import tipsImage from "assets/images/tips/bathroom_mirror.jpg";

export /* @internal */ const pitchNotations = ["scientific", "helmholtz", "solfeggio", "numbered", "gongche"] as const;

export default function Shupelunker() {
	const { enabled, presetTemplate } = selectConfig(c => c.lyrics);
	const { enabled: karaokeEnabled, futureFill, pastFill } = selectConfig(c => c.lyrics.karaoke);
	const { enabled: notationEnabled, type } = selectConfig(c => c.lyrics.pitchNotation);

	mutexSwitches(karaokeEnabled, notationEnabled);

	const { changePage } = useSnapshot(pageStore);

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} learnMoreLink="">{t.descriptions.lyrics}</SettingsPageControl>
			<EmptyMessage.YtpDisabled fully={t.titles.lyrics}>
				<SettingsCardToggleSwitch title={t.enabled} icon="enabled" on={enabled} resetTransitionOnChanging />

				<EmptyMessage.Typical icon="lyrics" title="lyrics" enabled={enabled}>
					<SettingsCard title={t.lyrics.presetTemplate} details={t.descriptions.lyrics.presetTemplate} icon="subtitles">
						<ComboBox current={presetTemplate} options={[]} />
					</SettingsCard>

					<Subheader>{t.lyrics.karaoke}</Subheader>
					<SettingsCardToggleSwitch title={t.lyrics.enableMode({ mode: t.lyrics.karaoke })} details={t.descriptions.lyrics.karaoke} icon="mic" on={karaokeEnabled} />
					<Disabled disabled={!karaokeEnabled[0]}>
						<SettingsCard icon="karaoke_future_fill" title={t.lyrics.karaoke.futureFill} details={t.descriptions.lyrics.karaoke.futureFill}>
							<ColorPicker color={futureFill} />
						</SettingsCard>
						<SettingsCard icon="karaoke_past_fill" title={t.lyrics.karaoke.pastFill} details={t.descriptions.lyrics.karaoke.pastFill}>
							<ColorPicker color={pastFill} />
						</SettingsCard>
					</Disabled>

					<Subheader>{t.lyrics.pitchNotation}</Subheader>
					<SettingsCardToggleSwitch title={t.lyrics.enableMode({ mode: t.lyrics.pitchNotation })} details={t.descriptions.lyrics.pitchNotation} icon="csharp" on={notationEnabled} />
					<Disabled disabled={!notationEnabled[0]}>
						<ExpanderRadio
							title={t.lyrics.pitchNotation.type}
							details={t.descriptions.lyrics.pitchNotation.type}
							icon="music_note"
							items={pitchNotations}
							value={type as StateProperty<string>}
							view="tile"
							idField
							nameField={t.lyrics.pitchNotation}
							iconField={id => "notation_" + id}
						/>
					</Disabled>

					<Subheader>{t.subheaders.seeAlso}</Subheader>
					<div>
						<Button hyperlink onClick={() => changePage(["tools"])}>{t.lyrics.useStaticText}</Button>
					</div>
				</EmptyMessage.Typical>
			</EmptyMessage.YtpDisabled>
		</div>
	);
}
