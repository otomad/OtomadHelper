import tipsImage from "assets/images/tips/shuzo_of_shupelunker_tactics.jpg";

const affixes = [
	{ id: "prefix", symbol: "C5-⋯" },
	{ id: "suffix", symbol: "⋯-C5" },
];

export default function Shupelunker() {
	const { affix, offset } = selectConfig(c => c.shupelunker);
	const { octaves, higherNeighbors, lowerNeighbors } = selectConfig(c => c.shupelunker.unallocated);

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} learnMoreLink="">{t.descriptions.shupelunker}</SettingsPageControl>
			<ExpanderRadio
				title={t.shupelunker.affix}
				icon="music_note_strikethrough"
				items={affixes}
				value={affix as StateProperty<string>}
				view="tile"
				idField="id"
				nameField={t.shupelunker.affix}
				iconField={item => <span>{item.symbol}</span>}
			/>
			<Expander title={t.shupelunker.unallocated} icon="unallocated">
				<ToggleSwitch on={octaves}>{t.shupelunker.unallocated.octaves}</ToggleSwitch>
				<ToggleSwitch on={higherNeighbors}>{t.shupelunker.unallocated.higherNeighbors}</ToggleSwitch>
				<ToggleSwitch on={lowerNeighbors}>{t.shupelunker.unallocated.lowerNeighbors}</ToggleSwitch>
			</Expander>
			<SettingsCard title={t.offset} icon="table_resize">
				<TextBox.Number value={offset} decimalPlaces={0} suffix={t.units.semitones} positiveSign />
			</SettingsCard>
		</div>
	);
}
