export default function QuickSelectCurrentTrack() {
	return (
		<Tooltip title={t.source.preferredTrack.quickSelect} placement="block">
			<Button icon="cursor_hover" minWidthUnbounded />
		</Tooltip>
	);
}
