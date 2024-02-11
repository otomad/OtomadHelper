import glitchCursor from "assets/cursurs/glitch_cursor.ani";
import tipsImage from "assets/images/tips/datamoshing.jpg";

export default function Mosh() {
	const tipsEl = useRef<HTMLDivElement | null>(null);
	useAniCursor(tipsEl, glitchCursor);

	return (
		<div className="container">
			<Contents>
				<SettingsPageControl image={tipsImage} learnMoreLink="" style={{ cursor: `url(${glitchCursor}) 8 8, default` }} ref={tipsEl}>
					<GlitchyText normal={t.descriptions.mosh.normal} glitchy={t.descriptions.mosh.glitchy} />
					<br />
					{t.descriptions.mosh.additional}
				</SettingsPageControl>
			</Contents>
		</div>
	);
}
