import type { TOptions } from "i18next";
import English from "./English";

export type I18nArgsFunction<R extends string = string> = {
	(options: TOptions): R;
};
type UnknownKeyDeclaration = Readonly<Record<string, string & I18nArgsFunction>>;

type IncludesInterpolation<S extends string> = S extends `${string}{{${string}` ? S & I18nArgsFunction<S> : S;
type NestLocaleWithDefaultValue<L> = {
	[key in keyof L]:
		L[key] extends string ? IncludesInterpolation<L[key]> :
		NestLocaleWithDefaultValue<L[key]>;
} & UnknownKeyDeclaration;
type DiscardConstString<L> = {
	[key in keyof L]: L[key] extends object ? DiscardConstString<L[key]> : string;
};

export type LocaleWithDefaultValue = NestLocaleWithDefaultValue<typeof English["translation"]>;
export type LocaleIdentifiers = DiscardConstString<typeof English>;
