export default function Internal() {
	return (
		<div className="container">
			<InfoBar status="warning" title={t.infoBar.warning}>{t.descriptions.settings.internal.info}</InfoBar>
		</div>
	);
}
