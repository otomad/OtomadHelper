export default function IeOff() {
	const t = useT();
	return <span role="img" aria-label={t({ context: "aria" }).ieOff}>{t.ieOff}</span>;
}
