import tipsImage from "assets/images/tips/classical_music_mashup.jpg";

export default function Staff() {
	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} learnMoreLink="">{t.descriptions.staff}</SettingsPageControl>
		</div>
	);
}
