import type { TOptions } from "i18next";
import English from "./English";
import SChinese from "./SChinese";

export type I18nArgsFunction<R extends string = string> = {
	(options: TOptions): R;
};
type UnknownKeyDeclaration = Readonly<Record<string, string & I18nArgsFunction>>;
type KeyWithOther<T> = T extends `${infer _}_other` ? T : never;
type KeyWithNoOther<T> = T extends `${infer _}_other` ? never : T;

type IncludesInterpolation<S extends string> = S extends `${string}{{${string}` ? S & I18nArgsFunction<S> : S;
type NestLocaleWithDefaultValue<L> = {
	[key in keyof L]:
		L[key] extends string ? IncludesInterpolation<L[key]> :
		L[key] extends { _: infer D } ?
			D extends string ? D & NestLocaleWithDefaultValue<L[key]> & IncludesInterpolation<D> :
			NestLocaleWithDefaultValue<L[key]> :
		NestLocaleWithDefaultValue<L[key]>;
} & UnknownKeyDeclaration;
type DiscardConstString<L> = {
	[key in KeyWithNoOther<keyof L>]: L[key] extends object ? DiscardConstString<L[key]> : string;
} & {
	[key in KeyWithOther<keyof L>]?: string;
};

export type LocaleWithDefaultValue = NestLocaleWithDefaultValue<typeof SChinese["translation"]>;
export type LocaleIdentifiers = DiscardConstString<typeof English>;