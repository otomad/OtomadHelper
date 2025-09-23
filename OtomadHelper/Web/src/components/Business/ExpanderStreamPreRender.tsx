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
		<ExpanderRadio
			title={t.stream.preRender}
			details={t.descriptions.stream.preRender}
			icon="movie"
			items={preRenders}
			value={isAudio ? preRenderAudio : preRenderVisual}
			view="tile"
			idField="id"
			iconField="icon"
			nameField={t.stream.preRender}
			detailsField={t({ context: stream }).descriptions.stream.preRender}
		>
			{isAudio && <ToggleSwitch on={preRenderAcidTag} lock={preRenderAudio[0] === "media" ? null : false} icon="logo/acid" details={t.descriptions.stream.preRender.acidTag}>{t.stream.preRender.acidTag}</ToggleSwitch>}
			<Expander.ChildWrapper $tilePadding={isAudio ? "button to item" : "tile view"}>
				<Button hyperlink onClick={() => changePage(["settings"])}>{t.stream.preRender.specifyClipsFolder}</Button>
			</Expander.ChildWrapper>
		</ExpanderRadio>
	);
}
