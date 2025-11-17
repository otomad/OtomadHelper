// This component unite SettingsCard, SettingsCardToggleSwitch, Expander, ExpanderRadio, ExpanderItem, SubExpander into one.

import type { SettingMeta } from "helpers/settings-metas";
import { settingsMetas } from "helpers/settings-metas";
export const metas = settingsMetas;
export type SettingMetaInside = { meta?: SettingMeta };

interface Props {
	meta: { meta: SettingMeta };
}

type InheritFrom<T> = Props & PartialWith<T extends Function ? PropsOf<T> : T, keyof SettingMeta>;

export default function Setting(props: InheritFrom<typeof SettingsCardToggleSwitch>): React.JSX.Element;
export default function Setting<TItem, TKey extends PropertyKey>(props: InheritFrom<typeof ExpanderRadio<TItem, TKey>>): React.JSX.Element;
export default function Setting(props: InheritFrom<Omit<PropsOf<typeof SettingsCard>, "children"> & { actions: PropsOf<typeof SettingsCard>["children"] }>): React.JSX.Element;
export default function Setting(props: InheritFrom<typeof Expander>): React.JSX.Element;
export default function Setting(props: InheritFrom<typeof Expander.Item>): React.JSX.Element;
export default function Setting(props: InheritFrom<typeof Expander.Sub>): React.JSX.Element;
export default function Setting({ meta: { meta }, ...props }: InheritFrom<typeof SettingsCard | typeof SettingsCardToggleSwitch | typeof Expander | typeof ExpanderRadio | typeof Expander.Item | typeof Expander.Sub>) {
	const { lastGotoPath } = useSnapshot(pageStore);
	const { path, cssPath, link, type } = meta;
	// Act backstop unless explicit passing undefined.
	if (!("title" in props)) props.title = meta.translatedTitle;
	if (!("details" in props)) props.details = meta.translatedDetails;
	if (!("icon" in props)) props.icon = meta.icon;
	props.anchor = cssPath;
	if (lastGotoPath?.value === path) props.className = classNames(props, "focus-highlight");
	const expanded = !!(lastGotoPath?.value !== path && lastGotoPath?.value.startsWith(path));
	const _requestExpanded = expanded ? TransientValue.computed(lastGotoPath, () => true) : undefined;
	const { place } = useContext(Expander.Context);
	const isExpanderChild = place === "children";
	const { goto } = useSnapshot(pageStore);

	if (type === "subheader")
		throw new TypeError("The Setting component doesn't support subheader type", { cause: meta });
	if ("on" in props || type === "switch")
		if (!isExpanderChild)
			return <SettingsCardToggleSwitch {...props as Any} _requestExpanded={_requestExpanded} />;
		else {
			const { title, ..._props } = props;
			return <ToggleSwitch {..._props as Any}>{title}</ToggleSwitch>;
		}
	else if ("items" in props || type === "radiogroup")
		if (props.items instanceof Enum)
			return <ExpanderRadio.Enum {...props as Any} _requestExpanded={_requestExpanded} />;
		else
			return <ExpanderRadio {...props as Any} _requestExpanded={_requestExpanded} />;
	else if (isExpanderChild)
		if ("children" in props)
			return <Expander.Sub {...props as Any} _requestExpanded={_requestExpanded} />;
		else {
			const { actions, ..._props } = props as PropsOf<typeof Expander>;
			return <Expander.Item {..._props as Any}>{actions}</Expander.Item>;
		}
	else if ("children" in props || type === "expander")
		return <Expander {...props as Any} _requestExpanded={_requestExpanded} />;
	else {
		const { actions, ..._props } = props as PropsOf<typeof Expander>;
		if (type === "button")
			return <SettingsCard type="button" {..._props as Any}>{actions}</SettingsCard>;
		else if (link || type === "link")
			return <SettingsCard type="button" onClick={() => link && goto(link)} {..._props as Any}>{actions}</SettingsCard>;
		else
			return <SettingsCard {..._props as Any}>{actions}</SettingsCard>;
	}
}

interface UseMetaOverriddenProps {
	title?: ReactNode;
	details?: ReactNode;
	icon?: DeclaredIcons;
	children?: ReactNode;
	anchor?: string;
}
function useMeta({ meta }: SettingMetaInside = {}, overriddenProps: UseMetaOverriddenProps | IArguments = {}): RequiredWith<UseMetaOverriddenProps, "anchor"> {
	if (isArguments(overriddenProps)) overriddenProps = overriddenProps[0] as UseMetaOverriddenProps;
	if (!meta) return overriddenProps as never;
	return {
		title: "title" in overriddenProps ? overriddenProps.title : meta.translatedTitle!,
		details: "details" in overriddenProps ? overriddenProps.details : meta.translatedDetails!,
		icon: "icon" in overriddenProps ? overriddenProps.icon : meta.icon,
		anchor: "anchor" in overriddenProps ? overriddenProps.anchor! : meta.cssPath,
		children: "children" in overriddenProps ? overriddenProps.children : meta.translatedTitle!,
	};
}

Setting.useMeta = useMeta;
