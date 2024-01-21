import exampleThumbnail from "assets/images/ヨハネの氷.jpg";

const prves = [
	{ class: "flip", icon: "placeholder", effects: ["hFlip", "vFlip", "ccwFlip", "cwFlip"] },
	{ class: "rotation", icon: "placeholder", effects: ["ccwRotate", "cwRotate", "turned"] },
	{ class: "scale", icon: "placeholder", effects: ["zoomOutIn"] },
	{ class: "mirror", icon: "placeholder", effects: ["hMirror", "vMirror", "ccwMirror", "cwMirror"] },
	{ class: "invert", icon: "placeholder", effects: ["negative", "luminInvert"] },
	{ class: "hue", icon: "placeholder", effects: ["hueInvert", ...forMapFromTo(3, 8, i => "stepChangeHue" + i)] },
	{ class: "monochrome", icon: "placeholder", effects: ["monochrome"] },
	{ class: "time", icon: "placeholder", effects: [] },
	{ class: "time2", icon: "placeholder", effects: [] },
	{ class: "ec", icon: "placeholder", effects: [] },
	{ class: "swing", icon: "placeholder", effects: [] },
	{ class: "blur", icon: "placeholder", effects: [] },
	{ class: "wipe", icon: "placeholder", effects: [] },
];

const StyledContainer = styled.div`
	.grid-view-item .image-wrapper {
		width: 200px;
		height: 112px;
	}
`;

export default function Prve() {
	const selectPrve = useState("normal");

	return (
		<StyledContainer>
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
		</StyledContainer>
	);
}
