import tipsImage from "assets/images/tips/shapes.png";

export default function Sonar() {
	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} learnMoreLink="">{t.descriptions.sonar}</SettingsPageControl>
		</div>
	);
}
