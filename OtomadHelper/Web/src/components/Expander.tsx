import SettingsCard from "./Settings/SettingsCard";

const Expander: FC<PropsOf<typeof SettingsCard> & {
	actions?: ReactNode;
}> = ({ icon, heading, caption, actions }) => {
	const settingsCardProps = { icon, heading, caption, children: actions };
	const [expanded, setExpanded] = useState(false);

	return (
		<SettingsCard {...settingsCardProps} type="expander" trailingIcon={expanded ? "chevron_up" : "chevron_down"} onClick={() => setExpanded(expanded => !expanded)} />
	);
};

export default Expander;
