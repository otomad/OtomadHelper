import glitchCursor from "assets/cursurs/glitch_cursor.ani";
import tipsImage from "assets/images/tips/datamoshing.jpg";

export default function Mosh() {
	const tipsEl = useDomRef<HTMLDivElement>();
	useAniCursor(tipsEl, glitchCursor);

	return (
		<div className="container">
			<Contents>
				<SettingsPageControl image={tipsImage} learnMoreLink="" clearFloat ref={tipsEl}>
					<GlitchyText normal={t.descriptions.mosh.normal} glitchy={t.descriptions.mosh.glitchy} />
					<br />
					{t.descriptions.mosh.additional}
				</SettingsPageControl>
			</Contents>
		</div>
	);
}
