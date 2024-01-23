import type { TOptions } from "i18next";
import translation from "locales/config";
import type { LocaleWithDefaultValue } from "locales/types";
const I18N_ITEM_SYMBOL = Symbol.for("i18n_item");

export function isI18nItem(newChild: Any): newChild is Record<string, string> {
	return !!newChild?.[I18N_ITEM_SYMBOL];
}

const getProxy = (target: object) =>
	new Proxy(target, {
		get(target, rootName) {
			if (typeof rootName === "symbol") return;
			if (typeof target === "function") target = {};
			const getParentsPrefix = (...prefixes: string[]) => prefixes.length ? prefixes.join(".") : "";
			const getDeclarationInfo = (...keys: string[]) => {
				const key = getParentsPrefix(...keys);
				const raw = translation.getResource("en", "translation", key) as string | object;
				return {
					isCategory: typeof raw === "object",
					includesInterpolation: typeof raw === "string" && raw.includes("{{"),
					missing: raw === undefined,
					missingDefault: typeof raw === "object" && !("_" in raw),
					key,
					raw,
				};
			};
			const getMissingKey = (key: string) => {
				const displayValue = `<${key}>`;
				return displayValue;
			};
			const translate = (keys: string[], options?: TOptions) => {
				const { isCategory, missingDefault } = getDeclarationInfo(...keys);
				if (missingDefault) return getMissingKey(getParentsPrefix(...keys));
				const key = !isCategory ? getParentsPrefix(...keys) : getParentsPrefix(...keys, "_");
				return translation.t(key, { ...target, ...options });
			};
			const getWithArgsFunction = (...prefixes: string[]) => {
				const func = (options: TOptions) => translate(prefixes, options);
				func[I18N_ITEM_SYMBOL] = true;
				return func;
			};
			const getWithArgsProxy = (...parents: string[]) => {
				const keys = [rootName, ...parents];
				const info = getDeclarationInfo(...keys);
				if (info.missing)
					return getMissingKey(info.key);
				// else if (!info.includesInterpolation && !info.isCategory)
				// 	return translate(keys);
				else return new Proxy(getWithArgsFunction(...keys), {
					get(target, currentName): unknown {
						if (currentName === Symbol.toPrimitive || currentName === "toString")
							return () => translate(keys);
						if (typeof currentName === "string")
							return getWithArgsProxy(...parents, currentName);
						if (typeof currentName === "symbol")
							return target[currentName as typeof I18N_ITEM_SYMBOL];
					},
				});
			};
			return getWithArgsProxy();
		},
	}) as LocaleDictionary;
type LocaleDictionary = LocaleWithDefaultValue;
const targetFunction = (options?: number | bigint | TOptions) => {
	if (options === undefined) options = {};
	else if (typeof options === "number" || typeof options === "bigint") options = { count: Number(options) };
	return getProxy(options);
};
/** 获取本地化字符串对象。 */
export const t = getProxy(targetFunction) as LocaleDictionary & typeof targetFunction;
Object.freeze(t);