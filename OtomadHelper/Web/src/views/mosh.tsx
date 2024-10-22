import glitchCursor from "assets/cursors/glitch_cursor.ani";
import tipsImage from "assets/images/tips/datamoshing.jpg";

export default function Mosh() {
	const tipsEl = useDomRef<"div">();
	useAniCursor(tipsEl, glitchCursor);
	const { changePage } = useSnapshot(pageStore);

	return (
		<div className="container">
			<div>
				<SettingsPageControl image={tipsImage} learnMoreLink="" clearFloat ref={tipsEl}>
					<GlitchyText normal={t.descriptions.mosh.normal} glitchy={t.descriptions.mosh.glitchy} />
					<br />
					{t.descriptions.mosh.additional}
				</SettingsPageControl>
			</div>
			<InfoBar title={t.infoBar.warning} status="warning" button={<Button>{t.mosh.install}</Button>}>
				{t.descriptions.mosh.notInstalled}
			</InfoBar>
			<SettingsCard
				title={t.mosh.datamosh}
				details={t.descriptions.mosh.datamosh}
				type="button"
				icon="shuffle"
				disabled
			/>
			<SettingsCard
				title={t.mosh.datamix}
				details={t.descriptions.mosh.datamix}
				type="button"
				icon="datamix"
				disabled
			/>
			<SettingsCard
				title={t.mosh.layer}
				details={t.descriptions.mosh.layer}
				type="button"
				icon="layer"
				selectInfo={t(1).selectInfo.videoEvent}
				selectValid={1}
			/>
			<SettingsCard
				title={t.mosh.render}
				details={t.descriptions.mosh.render}
				type="button"
				icon="movie"
			/>
			<SettingsCard
				title={t.mosh.scramble}
				details={t.descriptions.mosh.scramble}
				type="button"
				icon="cut"
				selectInfo={t(1).selectInfo.trackEvent}
				selectValid={1}
			/>
			<SettingsCard
				title={t.mosh.automator}
				details={t.descriptions.mosh.automator}
				type="button"
				icon="automator"
				selectInfo={t(1).selectInfo.videoEvent}
				selectValid={1}
			/>
			<SettingsCard
				title={t.mosh.stutter}
				details={t.descriptions.mosh.stutter}
				type="button"
				icon="stutter"
				selectInfo={t(1).selectInfo.trackEvent}
				selectValid={1}
			/>
			<SettingsCard
				title={t.mosh.shake}
				details={t.descriptions.mosh.shake}
				type="button"
				icon="vibrate"
				selectInfo={t(1).selectInfo.videoEvent}
				selectValid={1}
			/>

			<Subheader>{t.subheaders.seeAlso}</Subheader>
			<div>
				<Button hyperlink onClick={() => changePage(["settings"])}>{t.mosh.specifyClipsFolder}</Button>
			</div>
		</div>
	);
}
