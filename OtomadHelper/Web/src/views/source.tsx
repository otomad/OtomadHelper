// const thumbnail = "https://app/thumbnail/D:/Downloads/test.mp4";

export default function Source() {
	return (
		<div className="container">
			<Button>123</Button>
			<SettingsCard heading="Trim" caption="Adjust start or end time of the specified source" type="button" />
			<Expander heading="Generate at" caption="Specify when to start generating from the project" />
		</div>
	);
}
