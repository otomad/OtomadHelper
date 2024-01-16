export default function Source() {
	const [stretch, setStretch] = useState(false);

	return (
		<div className="container">
			<SettingsCardToggleSwitch heading={t.audioVisual.stretch} caption={t.descriptions.audioVisual.stretch} on={[stretch, setStretch]} />
		</div>
	);
}
