// This component unite SettingsCard, SettingsCardToggleSwitch, Expander, ExpanderRadio, ExpanderItem into one.

import type { SettingMeta } from "helpers/settings-metas";
import { $t, settingsMetas } from "helpers/settings-metas";
export const metas = settingsMetas;
export type SettingMetaInside = { meta: SettingMeta };

interface Props {
	meta: { meta: Partial<SettingMeta> };
}

type InheritFrom<T> = Props & PartialWith<T extends Function ? PropsOf<T> : T, keyof SettingMeta>;

export default function Setting(props: InheritFrom<typeof SettingsCardToggleSwitch>): React.JSX.Element;
export default function Setting<TItem, TKey extends PropertyKey>(props: InheritFrom<typeof ExpanderRadio<TItem, TKey>>): React.JSX.Element;
export default function Setting(props: InheritFrom<Omit<PropsOf<typeof SettingsCard>, "children"> & { actions: PropsOf<typeof SettingsCard>["children"] }>): React.JSX.Element;
export default function Setting(props: InheritFrom<typeof Expander>): React.JSX.Element;
export default function Setting(props: InheritFrom<typeof Expander.Item>): React.JSX.Element;
export default function Setting({ meta: { meta }, ...props }: InheritFrom<typeof SettingsCard | typeof SettingsCardToggleSwitch | typeof Expander | typeof ExpanderRadio | typeof Expander.Item>) {
	const [lastGotoPath, lastGotoPathTimestamp] = useSnapshot(pageStore).lastGotoPath ?? [];
	const { path, link, type } = meta as RequiredWith<SettingMeta, "path">;
	props.title ??= $t(meta.title);
	props.details ??= $t(meta.details);
	props.icon ??= meta.icon;
	props.anchor = CSS.escape(path);
	if (lastGotoPath === path) props.className = classNames(props, "focus-highlight");
	const expanded = !!(lastGotoPath !== path && lastGotoPath?.startsWith(path));
	const _requestExpanded = expanded ? [true, lastGotoPathTimestamp] : undefined;
	const { place } = useContext(Expander.Context);
	const isExpanderChild = place === "children";
	const { changePage } = useSnapshot(pageStore);

	if ("on" in props || type === "switch")
		if (!isExpanderChild)
			return <SettingsCardToggleSwitch {...props as Any} _requestExpanded={_requestExpanded} />;
		else {
			const { title, ..._props } = props;
			return <ToggleSwitch {..._props as Any}>{title}</ToggleSwitch>;
		}
	else if ("items" in props || type === "radiogroup")
		return <ExpanderRadio {...props as Any} />;
	else if (isExpanderChild)
		return <Expander.Item {...props as Any} />;
	else if ("children" in props || type === "expander")
		return <Expander {...props as Any} _requestExpanded={_requestExpanded} />;
	else {
		const { actions, ..._props } = props as PropsOf<typeof Expander>;
		if (type === "container")
			return <SettingsCard {..._props as Any}>{actions}</SettingsCard>;
		else if (link || type === "link")
			return <SettingsCard type="button" onClick={() => link && changePage([link])} {..._props as Any}>{actions}</SettingsCard>;
		else
			return <SettingsCard type="button" {..._props as Any}>{actions}</SettingsCard>;
	}
}

function useMeta(meta?: SettingMetaInside, overriddenProps: {
	title?: ReactNode;
	details?: ReactNode;
	icon?: DeclaredIcons | ReactElement;
} = {}) {
	if (!meta) return overriddenProps;
	return {
		title: overriddenProps.title ?? $t(meta.meta.title)!,
		details: overriddenProps.details ?? $t(meta.meta.details)!,
		icon: overriddenProps.icon ?? meta.meta.icon,
		anchor: meta.meta.path!,
	};
}

Setting.useMeta = useMeta;
