export default function Settings() {
	const { i18n } = useTranslation();
	const languages = Object.keys(i18n.options.resources ?? {});
	const schemes = ["light", "dark", "auto"] as const;
	const { scheme, setScheme } = useColorMode();
	const [checked, setChecked] = useState(false);
	const [ind, setInd] = useState(false);
	const checkboxes = "foo,bar,baz".split(",");
	const [checks, setChecks] = useState<string[]>([]);

	return (
		<div className="container">
			<Expander heading="Language" expanded>
				{languages.map(language => (
					<RadioButton key={language} id={language} value={[i18n.language, i18n.changeLanguage]}>
						{language}
					</RadioButton>
				))}
			</Expander>
			<Expander heading="Color scheme" expanded>
				{schemes.map(thisScheme => (
					<RadioButton key={thisScheme} id={thisScheme} value={[scheme, setScheme]}>
						{thisScheme}
					</RadioButton>
				))}
			</Expander>
			<Expander heading="Test" expanded>
				<Checkbox value={[checked, setChecked]} indeterminate={[ind, setInd]}>复选框</Checkbox>
				<Checkbox value={[ind, setInd]}>不定状态</Checkbox>
				{checkboxes.map(check =>
					<Checkbox key={check} id={check} value={[checks, setChecks]}>{check}</Checkbox>)}
			</Expander>
		</div>
	);
}
