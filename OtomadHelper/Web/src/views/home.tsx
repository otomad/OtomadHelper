export default function Home() {
	const [shown, setShown] = useState(false);
	const [text, setText] = useState("foo");
	const pitch = useState("C5");
	const timecode = useState("00:00:03.000");
	const options = ["foo", "bar", "baz", "hello", "world"];

	return (
		<div className="container">
			<InfoBar status="accent" title="Info">This page is for testing components currently. The quick brown fox jumps over a lazy dog.</InfoBar>
			<SettingsCardToggleSwitch title="Shown" icon="enabled" on={[shown, setShown]} />
			<TextBox value={[text, setText]} />

			<div>
				<Segmented current={[text, setText]}>
					<Segmented.Item icon="placeholder" id="foo">foo</Segmented.Item>
					<Segmented.Item icon="placeholder" id="bar">bar</Segmented.Item>
					<Segmented.Item icon="placeholder" id="baz">baz</Segmented.Item>
					<Segmented.Item icon="placeholder" id="hello">hello</Segmented.Item>
					<Segmented.Item icon="placeholder" id="world">world</Segmented.Item>
				</Segmented>
			</div>

			<ComboBox options={options} ids={options} current={[text, setText]} />

			<PitchPicker spn={pitch} style={{ maxWidth: "300px" }} />

			<TimecodeBox timecode={timecode} />
		</div>
	);
}
