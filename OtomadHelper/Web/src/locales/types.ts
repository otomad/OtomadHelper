/* eslint-disable @stylistic/indent */
import type { TOptions } from "i18next";
import English from "./English";

export type I18nArgsFunction<TResult extends string = string> = {
	(options: TOptions & Interpolations<TResult>): TResult;
};
type PluralContexts = "zero" | "one" | "two" | "few" | "many" | "other";
type UnknownKeyDeclaration = Readonly<Record<string, string & I18nArgsFunction>>;
type KeyWithOther<TIdentifier> = TIdentifier extends `${infer _}_${PluralContexts}` ? TIdentifier : never;
type KeyWithoutOther<TIdentifier> = TIdentifier extends `${infer _}_${PluralContexts}` ? never : TIdentifier;

type Inter<TIdentifier extends string> =
	{ [interpolation in TIdentifier]?: TIdentifier extends "count" ? number : Any }; // Count for number, other for any
type Interpolations<TString extends string, TParents = {}> =
	TString extends `${string}{{${infer Interpolation extends string}}}${infer Next}` ?
		Interpolation extends `${infer Identifier},${string}` ?
			Interpolations<Next, TParents & Inter<Identifier>> :
			Interpolations<Next, TParents & Inter<Interpolation>> :
		TParents;

type IncludesInterpolation<TString extends string> = TString extends `${string}{{${string}` ? TString & I18nArgsFunction<TString> : TString;
type NestLocaleWithDefaultValue<TLocale> = {
	[key in keyof TLocale]:
		TLocale[key] extends string ? IncludesInterpolation<TLocale[key]> :
		TLocale[key] extends { _: infer TDefault } ?
			TDefault extends string ? TDefault & NestLocaleWithDefaultValue<TLocale[key]> & IncludesInterpolation<TDefault> :
			NestLocaleWithDefaultValue<TLocale[key]> :
		NestLocaleWithDefaultValue<TLocale[key]>;
} & UnknownKeyDeclaration;
type DiscardConstString<TLocale> = {
	[key in KeyWithoutOther<keyof TLocale>]: TLocale[key] extends object ? DiscardConstString<TLocale[key]> : string;
} & {
	[key in KeyWithOther<keyof TLocale>]?: string;
};

export type LocaleWithDefaultValue = NestLocaleWithDefaultValue<typeof English["javascript"]>;
export type LocaleIdentifiers = DiscardConstString<typeof English>;
