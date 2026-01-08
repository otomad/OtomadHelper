import exampleThumbnail from "assets/images/ヨハネの氷.avif";

const AlternatelyEffects = Enum({
	hFlip: { label: t.prve.effects.hFlip },
	vFlip: { label: t.prve.effects.vFlip },
	hMirror: { label: t.prve.effects.hMirror },
	vMirror: { label: t.prve.effects.vMirror },
	monochrome: { label: t.ytp.effects.monochrome },
	hueInvert: { label: t.prve.effects.hueInvert },
	luminInvert: { label: t.prve.effects.luminInvert },
});

const GraduallyEffects = Enum({
	hue: { label: t.stream.parameters.hue },
	saturation: { label: t.stream.parameters.saturation },
	contrast: { label: t.stream.parameters.contrast },
	threshold: { label: t.stream.parameters.threshold },
	brightness: { label: t.stream.parameters.brightness },
	opacity: { label: t.settings.appearance.backgroundImage.opacity },
});

const StyledMirrorGradientTrackFlyoutEditor = styled.div`
	.items-view {
		display: flex;
		justify-content: start;
		overflow-inline: auto;

		.items-view-item {
			flex-shrink: 0;
			inline-size: 100px;

			.base {
				${styles.mixins.square("100px")};

				.preview-prve img {
					animation: none !important;
				}
			}
		}
	}
`;

export default function MirrorGradientTrackFlyoutEditor() {
	return (
		<StyledMirrorGradientTrackFlyoutEditor>
			<HorizontalScroll as={Fragment}>
				<ItemsView view="grid" current={null}>
					<Subheader vertical>{t.track.gradient.groups.alternately}</Subheader>
					{AlternatelyEffects.map(({ key, label }) => (
						<ItemsView.Item
							id={key}
							key={key}
							image={<PreviewPrve thumbnail={exampleThumbnail} effect={key === "monochrome" ? "chromatic" : key} />}
						>
							{label}
						</ItemsView.Item>
					))}
					<Subheader vertical>{t.track.gradient.groups.gradually}</Subheader>
					{GraduallyEffects.map(({ key, label }) => (
						<ItemsView.Item
							id={key}
							key={key}
							image={<PreviewGraduallyGradient thumbnail={exampleThumbnail} effect={key} />}
						>
							{label}
						</ItemsView.Item>
					))}
				</ItemsView>
			</HorizontalScroll>
		</StyledMirrorGradientTrackFlyoutEditor>
	);
}
