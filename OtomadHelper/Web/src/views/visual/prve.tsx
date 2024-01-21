import exampleThumbnail from "assets/images/ヨハネの氷.jpg";

const prves = [
	{ class: "flip", icon: "placeholder", effects: ["hFlip", "vFlip", "ccwFlip", "cwFlip"] },
	{ class: "rotation", icon: "placeholder", effects: [] },
	{ class: "scale", icon: "placeholder", effects: [] },
	{ class: "mirror", icon: "placeholder", effects: [] },
	{ class: "invert", icon: "placeholder", effects: [] },
	{ class: "hue", icon: "placeholder", effects: [] },
	{ class: "monochrome", icon: "placeholder", effects: [] },
	{ class: "time", icon: "placeholder", effects: [] },
	{ class: "time2", icon: "placeholder", effects: [] },
	{ class: "ec", icon: "placeholder", effects: [] },
	{ class: "swing", icon: "placeholder", effects: [] },
	{ class: "blur", icon: "placeholder", effects: [] },
	{ class: "wipe", icon: "placeholder", effects: [] },
];

export default function Prve() {
	const selectPrve = useState("");

	return (
		<div className="container">
			<Expander heading={t.condition} caption={t.descriptions.condition} icon="filter" />
			<div>
				<Button>从其它参数复制至此</Button>
			</div>
			<Subheader>{t.subheader.parameters}</Subheader>
			{/* 以上内容是在参数中的，不是在此，实际中应去掉上述部分，暂时放到这以防忘记。 */}

			{prves.map(({ class: klass, icon, effects }) => (
				<ExpanderRadio
					key={klass}
					heading={t.prve.classes[klass]}
					icon={icon}
					items={["normal", ...effects]}
					value={selectPrve as StateProperty<string>}
					view="grid"
					idField
					nameField
					imageField={name =>
						<PreviewPrve key={name} thumbnail={exampleThumbnail} name={name} />}
				/>
			))}
		</div>
	);
}
