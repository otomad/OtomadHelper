export default function Parameters() {
	return (
		<div className="container">
			<Expander title={t.condition} details={t.descriptions.condition} icon="filter" />
			<div>
				<Button icon="copy_arrow_right">{t.stream.parameters.copyFromAnotherParameterScheme}</Button>
			</div>
			<Subheader>{t.titles.parameters}</Subheader>
		</div>
	);
}
