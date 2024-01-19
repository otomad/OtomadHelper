export /* internal */ const stretches = ["noStretching", "flexingAndExtending", "extendingOnly", "flexingOnly"] as const;
export /* internal */ const legatos = ["portato", "upToOneBeat", "upToOneBar", "unlimited"] as const;

export default function Visual() {
	const [enabled, setEnabled] = selectConfig(c => c.visual.enabled);
	const [ytpEnabled, setYtpEnabled] = selectConfig(c => c.ytp.enabled);
	const stretch = selectConfig(c => c.visual.stretch);
	const loop = selectConfig(c => c.visual.loop);
	const staticVisual = selectConfig(c => c.visual.staticVisual);
	const noLengthening = selectConfig(c => c.visual.noLengthening);
	const legato = selectConfig(c => c.visual.legato);
	const multitrackForChords = selectConfig(c => c.visual.multitrackForChords);
	const createGroups = selectConfig(c => c.createGroups);
	const glissando = selectConfig(c => c.visual.glissando);
	const mappingVelocity = selectConfig(c => c.visual.mappingVelocity);
	const transformOfx = selectConfig(c => c.visual.transformOfx);
	const enableStaffVisualizer = selectConfig(c => c.visual.enableStaffVisualizer);

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
