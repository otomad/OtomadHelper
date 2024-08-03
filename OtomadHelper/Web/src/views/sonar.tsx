import cursor from "assets/cursors/sonar.svg?cursor";
import tipsImage from "assets/images/tips/shapes.png";

export default function Sonar() {
	const {
		enabled, separateDrums, differenceCompositeMode, shadow, shadowColor,
	} = selectConfig(c => c.sonar);
	const graphs = selectConfigArray(c => c.sonar.graphs);

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} cursor={cursor} learnMoreLink="">{t.descriptions.sonar}</SettingsPageControl>

			<EmptyMessage.YtpDisabled fully={t.titles.sonar}>
				<SettingsCardToggleSwitch title={t.enabled} icon="enabled" on={enabled} details={t.descriptions.sonar.enabled} resetTransitionOnChanging />

				<EmptyMessage.Typical icon="sonar" title="sonar" enabled={enabled}>
					<SettingsCardToggleSwitch icon="arrow_split" title={t.sonar.separateDrums} details={t.descriptions.sonar.separateDrums} on={separateDrums} />
					<SettingsCardToggleSwitch icon="invert_color" title={t.sonar.differenceCompositeMode} details={t.descriptions.sonar.differenceCompositeMode} on={differenceCompositeMode} />
					<SettingsCardToggleSwitch icon="shadow" title={t.sonar.shadow} details={t.descriptions.sonar.shadow} on={shadow} $color={shadowColor[0]}>
						<ColorPicker color={shadowColor} />
					</SettingsCardToggleSwitch>

					<Subheader>{t.sonar.graphs}</Subheader>
					{graphs.map((graph, i) => (
						<SettingsCard
							title={graph.drumSound[0]}
							type="button"
							key={i}
							icon={<PreviewGraph name={graph.shape[0]} color={graph.color[0]} />}
						>
							<ToggleSwitch on={graph.enabled} />
						</SettingsCard>
					))}
					<StackPanel $align="space-between">
						<Button icon="add">{t.new}</Button>
						<Button icon="arrow_reset" accent="critical">{t.reset}</Button>
					</StackPanel>
				</EmptyMessage.Typical>
			</EmptyMessage.YtpDisabled>
		</div>
	);
}
