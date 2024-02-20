import tipsImage from "assets/images/tips/yoooo_a_boom.jpg";
import exampleThumbnail from "assets/images/ヨハネの氷.jpg";

const effects = ["chorus", "delay", "changePitch", "reverse", "changeSpeed", "vibrato", "changeHue", "rotateHue", "monochrome", "negative", "repeatRapidly", "randomTuning", "upsize", "spherize", "mirror", "highContrast", "oversaturation", "emphasizeThrice"];

export default function Ytp() {
	const [enabled, setEnabled] = selectConfig(c => c.ytp.enabled);
	const selectEffects = useState("");

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} learnMoreLink="">{t.descriptions.ytp}</SettingsPageControl>
			<SettingsCardToggleSwitch heading={t.enabled} selectInfo="1 media source has been selected" icon="enabled" on={[enabled, setEnabled]} />

			{!enabled ? (
				<EmptyMessage
					key="disabled"
					icon="ytp"
					heading={t.empty.disabled.heading({ name: t.titles.ytp })}
					caption={t.empty.disabled.caption({ name: t.titles.ytp })}
					iconOff
				>
					<Button onClick={() => setEnabled(true)} accent>{t.enable}</Button>
				</EmptyMessage>
			) : (
				<>
					<Subheader>{t.subheaders.parameters}</Subheader>
					<SettingsCard heading={t.ytp.constraint} caption={t.descriptions.ytp.constraint} icon="constraint" />
					<SettingsCard heading={t.ytp.clips} caption={t.descriptions.ytp.clips} icon="number" />
					<Subheader>{t.subheaders.effects}</Subheader>
					<Expander heading={t.ytp.effects} caption={t.descriptions.ytp.effects} icon="sparkle" checkInfo={selectEffects[0]}>
						<GridView current={selectEffects}>
							{effects.map(name => (
								<GridView.Item
									key={name}
									id={name}
									image={<PreviewYtp thumbnail={exampleThumbnail} name={name} />}
								>
									{name}
								</GridView.Item>
							))}
						</GridView>
					</Expander>
				</>
			)}
		</div>
	);
}
