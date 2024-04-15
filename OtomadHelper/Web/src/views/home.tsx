export default function Home() {
	const [shown, setShown] = useState(false);
	const [text, setText] = useState("foo");

	return (
		<div className="container">
			<InfoBar status="accent" title="Info">This page is for testing components currently. The quick brown fox jumps over a lazy dog.</InfoBar>
			<SettingsCardToggleSwitch title="Shown" icon="enabled" on={[shown, setShown]} />

			<div>
				<Segmented current={[text, setText]}>
					<Segmented.Item icon="placeholder" id="foo">foo</Segmented.Item>
					<Segmented.Item icon="placeholder" id="bar">bar</Segmented.Item>
					<Segmented.Item icon="placeholder" id="baz">baz</Segmented.Item>
					<Segmented.Item icon="placeholder" id="hello">hello</Segmented.Item>
					<Segmented.Item icon="placeholder" id="world">world</Segmented.Item>
				</Segmented>
			</div>
		</div>
	);
}
