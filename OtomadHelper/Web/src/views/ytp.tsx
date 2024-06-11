import cursor from "assets/cursors/poo.cur";
import tipsImage from "assets/images/tips/yoooo_a_boom.jpg";
import exampleThumbnail from "assets/images/ヨハネの氷.jpg";

const effects = ["chorus", "delay", "changePitch", "reverse", "changeSpeed", "vibrato", "changeHue", "rotateHue", "monochrome", "negative", "repeatRapidly", "randomTuning", "upsize", "spherize", "mirror", "highContrast", "oversaturation", "emphasizeThrice"];

export default function Ytp() {
	const { enabled: [enabled, setEnabled] } = selectConfig(c => c.ytp);
	const [selectEffects, setSelectEffects] = useState<string[]>([]);
	const selectEffectCount = selectEffects.length;
	const selectAll = useSelectAll([selectEffects, setSelectEffects], effects);

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} cursor={cursor} learnMoreLink="">{t.descriptions.ytp}</SettingsPageControl>
			<SettingsCardToggleSwitch title={t.enabled} selectInfo={t(1).selectInfo.source} icon="enabled" on={[enabled, setEnabled]} resetTransitionOnChanging />

			{!enabled ? (
				<EmptyMessage
					key="disabled"
					icon="ytp"
					title={t.empty.disabled.title({ name: t.titles.ytp })}
					details={t.empty.disabled.details({ name: t.titles.ytp })}
					iconOff
				>
					<Button onClick={() => setEnabled(true)} accent>{t.enable}</Button>
				</EmptyMessage>
			) : (
				<>
					<Subheader>{t.subheaders.parameters}</Subheader>
					<SettingsCard title={t.ytp.constraint} details={t.descriptions.ytp.constraint} icon="constraint" />
					<SettingsCard title={t.ytp.clips} details={t.descriptions.ytp.clips} icon="number" />
					<Subheader>{t.subheaders.effects}</Subheader>
					<Expander
						title={t.ytp.effects}
						details={t.descriptions.ytp.effects}
						icon="sparkle"
						actions={<Badge hidden={!selectEffectCount}>{selectEffectCount}</Badge>}
					>
						<Checkbox value={selectAll}>{t.selectAll}</Checkbox>
						<ItemsView view="grid" current={[selectEffects, setSelectEffects]} multiple>
							{effects.map(name => (
								<ItemsView.Item
									key={name}
									id={name}
									image={<PreviewYtp thumbnail={exampleThumbnail} name={name} />}
								>
									{name}
								</ItemsView.Item>
							))}
						</ItemsView>
					</Expander>
				</>
			)}
		</div>
	);
}
