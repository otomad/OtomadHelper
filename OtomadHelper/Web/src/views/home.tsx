export default function Home() {
	const [shown, setShown] = useState(false);
	const [text, setText] = useState(0);

	return (
		<div className="container">
			<InfoBar status="accent">This page is for testing components currently.</InfoBar>
			<SettingsCardToggleSwitch title="Shown" icon="enabled" on={[shown, setShown]} />

			<div>
				<p>123123</p>
				<Tooltip title="hehe">
					<TextBox value={[text, setText]} type="number" placeholder="fuck world!" suffix="px" disabled={shown} />
				</Tooltip>
			</div>
		</div>
	);
}