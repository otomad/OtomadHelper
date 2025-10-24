import tipsImage from "assets/images/tips/shuzo_of_shupelunker_tactics.avif";

const affixes = [
	{ id: "prefix", symbol: "C5-⋯" },
	{ id: "suffix", symbol: "⋯-C5" },
];

export default function Shupelunker() {
	const { enabled, affix, exclusiveTrack, offset } = useSelectConfig(c => c.shupelunker);
	const { octaves, fillUp, fillDown, default: defaultAll } = useSelectConfig(c => c.shupelunker.unallocated);
	const meta = metas.shupelunker;

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} learnMoreLink="">{t.descriptions.shupelunker}</SettingsPageControl>

			<SettingsCardToggleSwitch title={t.enabled} icon="enabled" on={enabled} resetTransitionOnChanging />
			<EmptyMessage.Typical icon="slice" title="shupelunker_full" enabled={enabled}>
				<EmptyMessage.YtpDisabled fully={t.titles.shupelunker}>
					<Setting
						meta={meta.affix}
						items={affixes}
						value={affix}
						view="tile"
						idField="id"
						nameField={t.shupelunker.affix}
						iconField={item => <span lang="en">{item.symbol}</span>}
					/>
					<Setting meta={meta.unallocated}>
						<Setting on={octaves} meta={meta.unallocated.octaves} />
						<Setting on={fillUp} meta={meta.unallocated.fillUp} />
						<Setting on={fillDown} meta={meta.unallocated.fillDown} />
						<Setting on={defaultAll} meta={meta.unallocated.default} />
					</Setting>
					<Setting meta={meta.exclusiveTrack} on={exclusiveTrack} />
					<Setting meta={meta.offset} actions={<TextBox.Number value={offset} decimalPlaces={0} suffix={t.units.semitone} positiveSign />} />

					<Subheader meta={meta.keyMappingZones} />
					<PreviewPiano />
				</EmptyMessage.YtpDisabled>
			</EmptyMessage.Typical>
		</div>
	);
}
