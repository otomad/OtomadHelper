import type { TOptions } from "i18next";
import translation from "locales/config";
import type { LocaleWithDefaultValue } from "locales/types";

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
					key,
					raw,
				};
			};
			const getMissingKey = (key: string) => {
				const displayValue = `<${key}>`;
				return displayValue;
			};
			const translate = (keys: string[], options?: TOptions) => {
				const key = getParentsPrefix(...keys);
				return translation.t(key, options);
			};
			const getWithArgsFunction = (...prefixes: string[]) => (options: TOptions) =>
				translate(prefixes, options);
			const getWithArgsProxy = (...parents: string[]) => {
				const keys = [rootName, ...parents];
				const info = getDeclarationInfo(...keys);
				if (info.missing)
					return getMissingKey(info.key);
				else if (!info.includesInterpolation && !info.isCategory)
					return translate(keys);
				else return new Proxy(getWithArgsFunction(...keys), {
					get(_target, currentName): unknown {
						if (currentName === Symbol.toPrimitive || currentName === "toString")
							return () => translate(keys);
						if (typeof currentName === "string")
							return getWithArgsProxy(...parents, currentName);
					},
				});
			};
			return getWithArgsProxy();
		},
	}) as LocaleDictionary;
type LocaleDictionary = LocaleWithDefaultValue;
/** 获取本地化字符串对象。 */
export const t = getProxy({}) as LocaleDictionary;
Object.freeze(t);
