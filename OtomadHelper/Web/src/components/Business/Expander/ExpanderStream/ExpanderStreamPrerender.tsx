import { prerenders } from "views/visual";

export default function ExpanderStreamPrerender({ stream }: {
	/** Audio or visual? */
	stream: StreamKind;
}) {
	const { goto } = useSnapshot(pageStore);
	const { prerender: prerenderVisual } = selectConfig(c => c.visual);
	const { prerender: prerenderAudio, prerenderAcidTag } = selectConfig(c => c.audio);
	const isAudio = stream === "audio";

	return (
		<Setting
			meta={metas.audio.prerender}
			items={prerenders}
			value={isAudio ? prerenderAudio : prerenderVisual}
			view="tile"
			parenOff
			idField="id"
			iconField="icon"
			nameField={t.stream.prerender}
			detailsField={t({ context: stream }).descriptions.stream.prerender}
		>
			{isAudio && <Setting meta={metas.audio.prerender.acidTag} on={prerenderAcidTag} lock={prerenderAudio[0] === "media" ? null : false} />}
			<Expander.ChildWrapper $tilePadding={isAudio ? "subtle button to item" : "tile view"}>
				<Button hyperlink onClick={() => goto(metas.settings.config.clipsFolder)}>{t.stream.prerender.specifyClipsFolder}</Button>
			</Expander.ChildWrapper>
		</Setting>
	);
}
