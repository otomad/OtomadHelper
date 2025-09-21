type SettingMetaType = "container" | "button" | "expander" | "switch";

interface SettingMeta {
	id: string;
	title: string;
	details: string;
	icon: DeclaredIcons;
	type?: SettingMetaType;
	items?: Omit<SettingMeta, "items">[];
}
