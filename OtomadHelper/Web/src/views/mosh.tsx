import glitchCursor from "assets/cursors/glitch_cursor.ani";
import tipsImage from "assets/images/tips/datamoshing.avif";

export default function Mosh() {
	const tipsEl = useDomRef<"div">();
	useAniCursor(tipsEl, glitchCursor);
	const { goto } = useSnapshot(pageStore);
	const tFull = tAlias({ context: "full" });

	return (
		<div className="container">
			<div>
				<SettingsPageControl image={tipsImage} learnMoreLink="" ref={tipsEl}>
					<GlitchyText normal={t.descriptions.mosh.normal} glitchy={t.descriptions.mosh.glitchy} />
					<br />
					{t.descriptions.mosh.additional}
				</SettingsPageControl>
			</div>
			<InfoBar title={t.infoBar.warning} status="warning" button={<Button>{t.mosh.install}</Button>}>
				{t.descriptions.mosh.notInstalled}
			</InfoBar>
			<SettingsCard
				title={tFull.titles.datamosh}
				details={t.descriptions.mosh.datamosh}
				type="button"
				icon="shuffle"
				disabled
			/>
			<SettingsCard
				title={tFull.titles.datamix}
				details={t.descriptions.mosh.datamix}
				type="button"
				icon="datamix"
				disabled
			/>
			<SettingsCard
				title={tFull.titles.layer}
				details={t.descriptions.mosh.layer}
				type="button"
				icon="layer"
				selectInfo={t(1).selectInfo.videoEvent}
				selectValid={1}
			/>
			<SettingsCard
				title={tFull.titles.render}
				details={t.descriptions.mosh.render}
				type="button"
				icon="movie"
			/>
			<SettingsCard
				title={tFull.titles.scramble}
				details={t.descriptions.mosh.scramble}
				type="button"
				icon="cut"
				selectInfo={t(1).selectInfo.trackEvent}
				selectValid={1}
			/>
			<SettingsCard
				title={tFull.titles.automator}
				details={t.descriptions.mosh.automator}
				type="button"
				icon="automator"
				selectInfo={t(1).selectInfo.videoEvent}
				selectValid={1}
			/>
			<SettingsCard
				title={tFull.titles.stutter}
				details={t.descriptions.mosh.stutter}
				type="button"
				icon="stutter"
				selectInfo={t(1).selectInfo.trackEvent}
				selectValid={1}
			/>
			<SettingsCard
				title={tFull.titles.shake}
				details={t.descriptions.mosh.shake}
				type="button"
				icon="vibrate"
				selectInfo={t(1).selectInfo.videoEvent}
				selectValid={1}
			/>

			<Subheader>{t.subheaders.seeAlso}</Subheader>
			<div>
				<Button hyperlink onClick={() => goto(metas.settings.config.clipsFolder)}>{t.mosh.specifyClipsFolder}</Button>
			</div>
		</div>
	);
}
