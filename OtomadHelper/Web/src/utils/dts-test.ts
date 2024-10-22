/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unnecessary-type-parameters */

/* @see https://github.com/vuejs/core/blob/main/packages-private/dts-test/utils.d.ts */

// This directory contains a number of d.ts assertions
// use \@ts-expect-error where errors are expected.

// export function describe(_name: string, _fn: () => void): void { }
// export function test(_name: string, _fn: () => any): void { }

export function expectType<T>(_value: T): void { }
export function expectAssignable<T, T2 extends T = T>(_value: T2): void { }

export type IsUnion<T, U extends T = T> = (
	T extends any ? (U extends T ? false : true) : never
) extends false
	? false
	: true;

export type IsAny<T> = 0 extends 1 & T ? true : false;

export type Prettify<T> = { [K in keyof T]: T[K] } & {};
