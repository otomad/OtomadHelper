import datamoshImage from "assets/images/datamoshing.jpg";

export default function Mosh() {
	return (
		<div className="container">
			<SettingsPageControl image={datamoshImage} learnMoreLink="">
				<GlitchyText normal={t.descriptions.mosh.normal} glitchy={t.descriptions.mosh.glitchy} />
				{t.descriptions.mosh.additional}
			</SettingsPageControl>
		</div>
	);
}
