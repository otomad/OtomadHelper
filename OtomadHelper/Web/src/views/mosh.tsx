import glitchCursor from "assets/cursors/glitch_cursor.ani";
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
			<SettingsCard heading={t.mosh.datamosh} caption={t.descriptions.mosh.datamosh} type="button" icon="shuffle" disabled />
			<SettingsCard heading={t.mosh.datamix} caption={t.descriptions.mosh.datamix} type="button" icon="datamix" disabled />
			<SettingsCard heading={t.mosh.layer} caption={t.descriptions.mosh.layer} type="button" icon="track" selectInfo={t(1).selectInfo.videoEvent} />
			<SettingsCard heading={t.mosh.render} caption={t.descriptions.mosh.render} type="button" icon="movie" />
			<SettingsCard heading={t.mosh.scramble} caption={t.descriptions.mosh.scramble} type="button" icon="cut" selectInfo={t(1).selectInfo.trackEvent} />
			<SettingsCard heading={t.mosh.automator} caption={t.descriptions.mosh.automator} type="button" icon="automator" selectInfo={t(1).selectInfo.videoEvent} />
			<SettingsCard heading={t.mosh.stutter} caption={t.descriptions.mosh.stutter} type="button" icon="stutter" selectInfo={t(1).selectInfo.trackEvent} />
			<SettingsCard heading={t.mosh.shake} caption={t.descriptions.mosh.shake} type="button" icon="vibrate" selectInfo={t(1).selectInfo.videoEvent} />
			<div>
				<Button hyperlink>指定数据抹失片段目录</Button>
			</div>
		</div>
	);
}
