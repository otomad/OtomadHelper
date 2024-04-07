export default function Home() {
	const [shown, setShown] = useState(false);
	const [text, setText] = useState(0);
	const slider = useState(50);

	return (
		<div className="container">
			<InfoBar status="accent" title="Info">This page is for testing components currently. The quick brown fox jumps over a lazy dog.</InfoBar>
			<SettingsCardToggleSwitch title="Shown" icon="enabled" on={[shown, setShown]} />

			<div>
				<p>123123</p>
				<Tooltip title="hehe">
					<TextBox value={[text, setText]} type="number" placeholder="fuck world!" suffix="px" disabled={shown} />
				</Tooltip>
				<Slider value={slider} />
			</div>
		</div>
	);
}
