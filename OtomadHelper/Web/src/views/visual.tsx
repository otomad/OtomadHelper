const stretches = ["noStretching", "flexingAndExtending", "extendingOnly", "flexingOnly"] as const;
const legatos = ["portato", "upToOneBeat", "upToOneBar", "unlimited"] as const;

export default function Visual() {
	// const config = useConfig();
	// const { enabled } = config.visual;
	// const setEnabled = (v: boolean) => useConfig.setState(c => void (c.visual.enabled = v));
	const [enabled, setEnabled] = useStoreSelector(useConfig, config => config.visual.enabled);

	// const [enabled, setEnabled] = useState(true);
	const [ytpEnabled, setYtpEnabled] = useState(false);
	const stretch = useState("noStretching");
	const loop = useState(false);
	const staticVisual = useState(false);
	const noLengthening = useState(false);
	const legato = useState("upToOneBeat");
	const multitrackForChords = useState(false);
	const createGroups = useState(true);
	const glissando = useState(false);
	const mappingVelocity = useState(false);
	const transformOfx = useState(false);
	const enableStaffVisualizer = useState(false);

	return (
		<div className="container">
			<MediaPreviewCard stream="visual" fileName="我的视频.mp4" enabled={[enabled, setEnabled]} />

			{!enabled ? (
				<EmptyMessage
					key="disabled"
					icon="visual"
					heading={t.empty.visualDisabled.heading}
					caption={t.empty.visualDisabled.caption}
				>
					<Button onClick={() => setEnabled(true)}>{t.enable}</Button>
				</EmptyMessage>
			) : ytpEnabled ? (
				<EmptyMessage
					key="ytpEnabled"
					icon="ytp"
					heading={t.empty.ytpEnabled.heading}
					caption={t.empty.ytpEnabled.caption}
				>
					<Button onClick={() => setYtpEnabled(false)}>{t.empty.ytpEnabled.ok}</Button>
				</EmptyMessage>
			) : (
				<>
					<ExpanderRadio
						heading={t.audioVisual.stretch}
						caption={t.descriptions.audioVisual.stretch}
						icon="stretch"
						items={stretches}
						value={stretch}
						idField
						nameField={t.audioVisual.stretch}
					/>
					<SettingsCardToggleSwitch heading={t.audioVisual.loop} caption={t.descriptions.audioVisual.loop} icon="loop" on={loop} />
					<SettingsCardToggleSwitch heading={t.audioVisual.staticVisual} caption={t.descriptions.audioVisual.staticVisual} icon="visual" on={staticVisual} />
					<SettingsCardToggleSwitch heading={t.audioVisual.noLengthening} caption={t.descriptions.audioVisual.noLengthening.visual} icon="no_lengthening" on={noLengthening} />
					<ExpanderRadio
						heading={t.audioVisual.legato}
						caption={t.descriptions.audioVisual.legato}
						icon="legato"
						items={legatos}
						value={legato}
						idField
						nameField={t.audioVisual.legato}
					/>
					<SettingsCardToggleSwitch heading={t.audioVisual.multitrackForChords} caption={t.descriptions.audioVisual.multitrackForChords} icon="chords" on={multitrackForChords} />
					<SettingsCardToggleSwitch heading={t.audioVisual.createGroups} caption={t.descriptions.audioVisual.createGroups} icon="group_object" on={createGroups} />
					<Expander
						heading={t.audioVisual.glissando}
						caption={t.descriptions.audioVisual.glissando}
						icon="swirl"
						actions={
							<ToggleSwitch on={glissando} />
						}
					/>
					<SettingsCard heading={t.audioVisual.mappingVelocity} caption={t.descriptions.audioVisual.mappingVelocity} icon="signal" type="button">
						<ToggleSwitch on={mappingVelocity} />
					</SettingsCard>
					<SettingsCardToggleSwitch heading={t.audioVisual.transformOfx} caption={t.descriptions.audioVisual.transformOfx} icon="transform_ofx" on={transformOfx} />

					<Subheader>{t.audioVisual.effects}</Subheader>
					<SettingsCard heading={t.audioVisual.effects.prve} caption={t.descriptions.audioVisual.effects.prve} type="button" icon="sparkle" />
					<SettingsCard heading={t.audioVisual.effects.staff} caption={t.descriptions.audioVisual.effects.staff} type="button" icon="g_clef">
						<ToggleSwitch on={enableStaffVisualizer} />
					</SettingsCard>

					<Subheader>{t.subheader.parameters}</Subheader>
				</>
			)}
		</div>
	);
}
