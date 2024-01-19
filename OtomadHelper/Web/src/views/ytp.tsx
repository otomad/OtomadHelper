import ytpImage from "assets/images/yoooo_a_boom.jpg";

export default function Ytp() {
	const [enabled, setEnabled] = selectConfig(c => c.ytp.enabled);

	return (
		<div className="container">
			<SettingsPageControl image={ytpImage}>
				<p>
					YTP is for creating YTPs using various effects known in the YTP Genre. YTP supports multisource.
					<br /><br />
					<a>Learn more about YTP</a>
				</p>
			</SettingsPageControl>
			<SettingsCardToggleSwitch heading={t.enabled} selectInfo="1 media source has been selected" icon="enabled" on={[enabled, setEnabled]} />

			{!enabled ? (
				<EmptyMessage
					key="disabled"
					icon="ytp"
					heading={t.empty.disabled.heading({ name: t.titles.ytp })}
					caption={t.empty.disabled.caption({ name: t.titles.ytp })}
				>
					<Button onClick={() => setEnabled(true)}>{t.enable}</Button>
				</EmptyMessage>
			) : (
				<>

				</>
			)}
		</div>
	);
}
