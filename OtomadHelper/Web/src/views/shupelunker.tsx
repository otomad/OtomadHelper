import tipsImage from "assets/images/tips/shuzo_of_shupelunker_tactics.jpg";

export default function Shupelunker() {
	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} learnMoreLink="">{t.descriptions.shupelunker}</SettingsPageControl>
		</div>
	);
}
