export /* @internal */ const pitchNotations = [
	{ id: "scientific", symbol: "C4" },
	{ id: "helmholtz", symbol: "cʹ" },
	{ id: "solfeggio", symbol: "Do" },
	{ id: "numbered", symbol: "1̇" },
	{ id: "gongche", symbol: "上" },
] as const;

export default function Shupelunker() {
	const { enabled, presetTemplate } = selectConfig(c => c.lyrics);
	const { enabled: karaokeEnabled, backgroundColor, foregroundColor } = selectConfig(c => c.lyrics.karaoke);
	const { enabled: notationEnabled, type } = selectConfig(c => c.lyrics.pitchNotation);

	mutexSwitches(karaokeEnabled, notationEnabled);

	const { changePage } = useSnapshot(pageStore);

	return (
		<div className="container">
			<EmptyMessage.YtpDisabled fully={t.titles.lyrics}>
				<SettingsCardToggleSwitch title={t.enabled} icon="enabled" on={enabled} resetTransitionOnChanging />

				<EmptyMessage.Typical icon="lyrics" title="lyrics" enabled={enabled}>
					<SettingsCard title={t.lyrics.presetTemplate} details={t.lyrics.presetTemplate} icon="placeholder">
						<ComboBox current={presetTemplate} options={[]} />
					</SettingsCard>

					<Subheader>{t.lyrics.karaoke}</Subheader>
					<SettingsCardToggleSwitch title={t.lyrics.enableMode({ mode: t.lyrics.karaoke })} icon="enabled" on={karaokeEnabled} />
					<Disabled disabled={!karaokeEnabled[0]}>
						<SettingsCard icon="placeholder" title={t.lyrics.karaoke.backgroundColor} details={t.lyrics.karaoke.backgroundColor}>
							<ColorPicker color={backgroundColor} />
						</SettingsCard>
						<SettingsCard icon="placeholder" title={t.lyrics.karaoke.foregroundColor} details={t.lyrics.karaoke.foregroundColor}>
							<ColorPicker color={foregroundColor} />
						</SettingsCard>
					</Disabled>

					<Subheader>{t.lyrics.pitchNotation}</Subheader>
					<SettingsCardToggleSwitch title={t.lyrics.enableMode({ mode: t.lyrics.pitchNotation })} icon="enabled" on={notationEnabled} />
					<Disabled disabled={!notationEnabled[0]}>
						<ExpanderRadio
							title={t.lyrics.pitchNotation.type}
							details={t.lyrics.pitchNotation.type}
							icon="placeholder"
							items={pitchNotations}
							value={type as StateProperty<string>}
							view="tile"
							idField="id"
							nameField={t.lyrics.pitchNotation}
							iconField={item => <span>{item.symbol}</span>}
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
