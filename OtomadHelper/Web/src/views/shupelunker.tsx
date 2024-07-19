import tipsImage from "assets/images/tips/shuzo_of_shupelunker_tactics.jpg";

const affixes = [
	{ id: "prefix", symbol: "C5-⋯" },
	{ id: "suffix", symbol: "⋯-C5" },
];

export default function Shupelunker() {
	const { enabled, affix, offset } = selectConfig(c => c.shupelunker);
	const { octaves, higherNeighbors, lowerNeighbors } = selectConfig(c => c.shupelunker.unallocated);

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} learnMoreLink="">{t.descriptions.shupelunker}</SettingsPageControl>
			<EmptyMessage.YtpDisabled fully={t.titles.shupelunker}>
				<SettingsCardToggleSwitch title={t.enabled} icon="enabled" on={enabled} resetTransitionOnChanging />

				<EmptyMessage.Typical icon="shupelunker" title="shupelunker_full" enabled={enabled}>
					<ExpanderRadio
						title={t.shupelunker.affix}
						details={t.descriptions.shupelunker.affix}
						icon="music_note_strikethrough"
						items={affixes}
						value={affix as StateProperty<string>}
						view="tile"
						idField="id"
						nameField={t.shupelunker.affix}
						iconField={item => <span lang="en">{item.symbol}</span>}
					/>
					<Expander title={t.shupelunker.unallocated} details={t.descriptions.shupelunker.unallocated} icon="unallocated">
						<ToggleSwitch on={octaves} details={t.descriptions.shupelunker.unallocated.octaves}>{t.shupelunker.unallocated.octaves}</ToggleSwitch>
						<ToggleSwitch on={lowerNeighbors} details={t.descriptions.shupelunker.unallocated.lowerNeighbors}>{t.shupelunker.unallocated.lowerNeighbors}</ToggleSwitch>
						<ToggleSwitch on={higherNeighbors} details={t.descriptions.shupelunker.unallocated.higherNeighbors}>{t.shupelunker.unallocated.higherNeighbors}</ToggleSwitch>
					</Expander>
					<SettingsCard title={t.offset} details={t.descriptions.shupelunker.offset} icon="table_resize">
						<TextBox.Number value={offset} decimalPlaces={0} suffix={t.units.semitones} positiveSign />
					</SettingsCard>

					<Subheader>{t.shupelunker.keyMappingZone}</Subheader>
				</EmptyMessage.Typical>
			</EmptyMessage.YtpDisabled>
		</div>
	);
}
