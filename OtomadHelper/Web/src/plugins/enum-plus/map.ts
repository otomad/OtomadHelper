import type { PluginFunc } from "enum-plus";

const mapPlugin: PluginFunc = (_options, Enum) => {
	Enum.extends({
		map() {
			this.items.map(({ raw, ...others }) => ({ ...raw as object, ...others }));
		},
	});
};

declare module "enum-plus/extension" {
	export interface InheritableEnumItems<T, K, V> {
		map(): (T["raw"] & { key: K; value: V; title: string })[];
	}
}

export default mapPlugin;
