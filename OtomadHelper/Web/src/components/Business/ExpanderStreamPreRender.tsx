import { preRenders } from "views/visual";

export default function ExpanderStreamPreRender({ stream }: {
	/** Audio or visual? */
	stream: StreamKind;
}) {
	const { changePage } = useSnapshot(pageStore);
	const { preRender: preRenderVisual } = useSelectConfig(c => c.visual);
	const { preRender: preRenderAudio, preRenderAcidTag } = useSelectConfig(c => c.audio);
	const isAudio = stream === "audio";

	return (
		<Setting
			meta={metas.audio.preRender}
			items={preRenders}
			value={isAudio ? preRenderAudio : preRenderVisual}
			view="tile"
			idField="id"
			iconField="icon"
			nameField={t.stream.preRender}
			detailsField={t({ context: stream }).descriptions.stream.preRender}
		>
			{isAudio && <Setting meta={metas.audio.preRender.acidTag} on={preRenderAcidTag} lock={preRenderAudio[0] === "media" ? null : false} />}
			<Expander.ChildWrapper $tilePadding={isAudio ? "button to item" : "tile view"}>
				<Button hyperlink onClick={() => changePage(["settings"])}>{t.stream.preRender.specifyClipsFolder}</Button>
			</Expander.ChildWrapper>
		</Setting>
	);
}
