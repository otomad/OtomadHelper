export default function Parameters() {
	return (
		<div className="container">
			<Expander title={t.condition} details={t.descriptions.condition} icon="filter" />
			<div>
				<Button>{t.stream.parameters.copyFromAnotherParameterScheme}</Button>
			</div>
			<Subheader>{t.subheaders.parameters}</Subheader>
		</div>
	);
}
