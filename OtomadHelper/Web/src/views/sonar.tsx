import tipsImage from "assets/images/tips/shapes.avif";

export default function Sonar() {
	const {
		enabled, splitDrums, differenceCompositeMode, shadow, shadowColor,
	} = useSelectConfig(c => c.sonar);
	const graphs = useSelectConfigArray(c => c.sonar.graphs);
	const meta = metas.sonar;

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} learnMoreLink="">{t.descriptions.sonar}</SettingsPageControl>

			<SettingsCardToggleSwitch title={t.enabled} icon="lightbulb" on={enabled} details={t.descriptions.sonar.enabled} resetTransitionOnChanging />
			<EmptyMessage.Typical icon="sonar" title="sonar" enabled={enabled}>
				<EmptyMessage.YtpDisabled fully={t.titles.sonar}>
					<Setting meta={meta.splitDrums} on={splitDrums} />
					<Setting meta={meta.differenceCompositeMode} on={differenceCompositeMode} />
					<Setting
						meta={meta.shadow}
						on={shadow}
						color={shadowColor[0]}
						actions={<ColorPicker color={shadowColor} />}
					/>

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
				</EmptyMessage.YtpDisabled>
			</EmptyMessage.Typical>
		</div>
	);
}
