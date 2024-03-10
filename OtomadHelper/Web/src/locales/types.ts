/* eslint-disable @stylistic/indent */
import type { TOptions } from "i18next";
import English from "./English";
import SChinese from "./SChinese";

export type I18nArgsFunction<R extends string = string> = {
	(options: TOptions): R;
};
type PluralContexts = "other" | "zero";
type UnknownKeyDeclaration = Readonly<Record<string, string & I18nArgsFunction>>;
type KeyWithOther<T> = T extends `${infer _}_${PluralContexts}` ? T : never;
type KeyWithoutOther<T> = T extends `${infer _}_${PluralContexts}` ? never : T;

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
	[key in KeyWithoutOther<keyof L>]: L[key] extends object ? DiscardConstString<L[key]> : string;
} & {
	[key in KeyWithOther<keyof L>]?: string;
};

export type LocaleWithDefaultValue = NestLocaleWithDefaultValue<typeof SChinese["javascript"]>;
export type LocaleIdentifiers = DiscardConstString<typeof English>;
