export default function Home() {
	const [shown, setShown] = useState(false);

	return (
		<div className="container">
			<InfoBar>This page is for testing components currently.</InfoBar>
			<SettingsCardToggleSwitch heading="Shown" icon="enabled" on={[shown, setShown]} />

			<div>
				<p>123123</p>
			</div>
		</div>
	);
}
