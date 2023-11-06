export default function Settings() {
	const { i18n } = useTranslation();
	const languages = Object.keys(i18n.options.resources ?? {});
	const schemes = ["light", "dark", "auto"] as const;
	const { scheme, setScheme } = useColorMode();
	const [checked, setChecked] = useState<CheckState>("unchecked");
	const checkStates = ["unchecked", "checked", "indeterminate"] as const satisfies readonly CheckState[];
	const [checkStatesI, setCheckStatesI] = useState(0);
	const checkboxes = "foo,bar,baz".split(",");
	const [checks, setChecks] = useState<string[]>([]);
	const [on, setOn] = useState(false);
	const [disabled, setDisabled] = useState(false);

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
			<Expander heading="Test">
				<Checkbox value={[checked, setChecked]}>复选框</Checkbox>
				<Checkbox value={[checked === "indeterminate", (ind: boolean) => setChecked(ind ? "indeterminate" : "unchecked")]}>不定状态</Checkbox>
				<Checkbox value={[checkStates[checkStatesI], () => setCheckStatesI(i => (i + 1) % 3)]}>切换</Checkbox>
				{checkboxes.map(check =>
					<Checkbox key={check} id={check} value={[checks, setChecks]}>{check}</Checkbox>)}
			</Expander>
			<Expander heading="Test" expanded>
				<ToggleSwitch on={[on, setOn]} disabled={disabled} />
				<ToggleSwitch on={[disabled, setDisabled]} />
			</Expander>
		</div>
	);
}
