import cursor from "assets/cursors/treble_clef.svg?cursor";
import tipsImage from "assets/images/tips/classical_music_mashup.avif";

export default function Staff() {
	const {
		enabled,
	} = useSubConfig(c => c.visual.staff);

	return (
		<div className="container">
			<SettingsPageControl image={tipsImage} cursor={cursor} learnMoreLink="">{t.descriptions.staff}</SettingsPageControl>
			<SettingsCardToggleSwitch title={t.enabled} icon="lightbulb" on={enabled} resetTransitionOnChanging />

			<EmptyMessage.Typical icon="g_clef" title="staff" enabled={enabled}>
				TODO
			</EmptyMessage.Typical>
		</div>
	);
}
