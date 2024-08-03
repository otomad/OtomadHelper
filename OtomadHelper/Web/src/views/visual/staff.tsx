import cursor from "assets/cursors/treble_clef.svg?cursor";
import tipsImage from "assets/images/tips/classical_music_mashup.jpg";

export default function Staff() {
	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} cursor={cursor} learnMoreLink="">{t.descriptions.staff}</SettingsPageControl>
		</div>
	);
}
