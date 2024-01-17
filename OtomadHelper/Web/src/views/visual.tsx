const stretches = ["noStretching", "flexingAndExtending", "extendingOnly", "flexingOnly"] as const;
const legatos = ["portato", "upToOneBeat", "upToOneBar", "unlimited"] as const;

export default function Source() {
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
			<ExpanderRadio
				heading={t.audioVisual.stretch}
				caption={t.descriptions.audioVisual.stretch}
				items={stretches}
				value={stretch}
				idField
				nameField={t.audioVisual.stretch}
			/>
			<SettingsCardToggleSwitch heading={t.audioVisual.loop} caption={t.descriptions.audioVisual.loop} on={loop} />
			<SettingsCardToggleSwitch heading={t.audioVisual.staticVisual} caption={t.descriptions.audioVisual.staticVisual} on={staticVisual} />
			<SettingsCardToggleSwitch heading={t.audioVisual.noLengthening} caption={t.descriptions.audioVisual.noLengthening.visual} on={noLengthening} />
			<ExpanderRadio
				heading={t.audioVisual.legato}
				caption={t.descriptions.audioVisual.legato}
				items={legatos}
				value={legato}
				idField
				nameField={t.audioVisual.legato}
			/>
			<SettingsCardToggleSwitch heading={t.audioVisual.multitrackForChords} caption={t.descriptions.audioVisual.multitrackForChords} on={multitrackForChords} />
			<SettingsCardToggleSwitch heading={t.audioVisual.createGroups} caption={t.descriptions.audioVisual.createGroups} on={createGroups} />
			<Expander
				heading={t.audioVisual.glissando}
				caption={t.descriptions.audioVisual.glissando}
				actions={
					<ToggleSwitch on={glissando} />
				}
			/>
			<SettingsCard heading={t.audioVisual.mappingVelocity} caption={t.descriptions.audioVisual.mappingVelocity} type="button">
				<ToggleSwitch on={mappingVelocity} />
			</SettingsCard>
			<SettingsCardToggleSwitch heading={t.audioVisual.transformOfx} caption={t.descriptions.audioVisual.transformOfx} on={transformOfx} />

			<Subheader>{t.audioVisual.effects}</Subheader>
			<SettingsCard heading={t.audioVisual.effects.prve} caption={t.descriptions.audioVisual.effects.prve} type="button" />
			<SettingsCard heading={t.audioVisual.effects.staff} caption={t.descriptions.audioVisual.effects.staff} type="button">
				<ToggleSwitch on={enableStaffVisualizer} />
			</SettingsCard>

			<Subheader>{t.subheader.parameters}</Subheader>
		</div>
	);
}
