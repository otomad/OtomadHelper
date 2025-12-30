import tipsImage from "assets/images/tips/bathroom_mirror.avif";

export /* @internal */ const musicalNotationSystems = ["scientific", "helmholtz", "solfege", "numbered", "gongche", "gongshang", "lyulyu", "midiNumber", "frequency"] as const;

export default function Lyrics() {
	const { enabled, presetTemplate } = useSelectConfig(c => c.lyrics);
	const { enabled: karaokeEnabled, futureFill: [futureFill, setFutureFill], pastFill: [pastFill, setPastFill] } = useSelectConfig(c => c.lyrics.karaoke);
	const { enabled: notationEnabled, type } = useSelectConfig(c => c.lyrics.pitchNotation);
	const meta = metas.lyrics;

	mutexSwitches(karaokeEnabled, notationEnabled);

	const { changePage } = useSnapshot(pageStore);

	const previewKaraoke = <PreviewKaraoke reset={!enabled[0]} {...karaokeEnabled[0] ? { futureFill, pastFill } : null} />;
	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} imageOverlay={previewKaraoke} learnMoreLink="">{t.descriptions.lyrics}</SettingsPageControl>

			<SettingsCardToggleSwitch title={t.enabled} icon="lightbulb" on={enabled} resetTransitionOnChanging />
			<EmptyMessage.Typical icon="lyrics" title="lyrics" enabled={enabled}>
				<EmptyMessage.YtpDisabled fully={t.titles.lyrics}>
					<Setting meta={meta.presetTemplate} actions={<ComboBox current={presetTemplate} options={[]} ids={[]} />} />

					<Subheader>{t.lyrics.karaoke.toString()}</Subheader>
					<Setting meta={meta.karaoke} title={t.lyrics.enableMode({ mode: t.lyrics.karaoke })} on={karaokeEnabled} />
					<Attrs disabled={!karaokeEnabled[0]}>
						<Setting
							meta={meta.karaoke.futureFill}
							actions={<ColorPicker color={[futureFill, setFutureFill]} />}
						/>
						<Setting
							meta={meta.karaoke.pastFill}
							actions={<ColorPicker color={[pastFill, setPastFill]} />}
						/>
					</Attrs>

					<Subheader>{t.lyrics.pitchNotation}</Subheader>
					<Setting meta={meta.pitchNotation} title={t.lyrics.enableMode({ mode: t.lyrics.pitchNotation })} on={notationEnabled} />
					<Attrs disabled={!notationEnabled[0]}>
						<Setting
							meta={meta.pitchNotation.system}
							items={musicalNotationSystems}
							value={type}
							view="tile"
							idField
							iconField={id => "notation_" + new VariableName(id).snake}
							nameField={id => <Preserves>{t.lyrics.pitchNotation[id].split("\n")[0]}</Preserves>}
							detailsField={id => t.lyrics.pitchNotation[id].split("\n")[1]}
							checkInfoCondition={id => <Preserves>{t.lyrics.pitchNotation[id!].split("\n")[0]}</Preserves>}
						/>
					</Attrs>

					<Subheader>{t.subheaders.seeAlso}</Subheader>
					<div>
						<Button hyperlink onClick={() => changePage(["tools"])}>{t.lyrics.useStaticText}</Button>
					</div>
				</EmptyMessage.YtpDisabled>
			</EmptyMessage.Typical>
		</div>
	);
}
