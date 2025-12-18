import crypto from "crypto";
// import esbuild from "esbuild";
import htmlMinifierTerser from "html-minifier-terser";
import type { TransformAttributeOptions, TransformOptions } from "lightningcss";
import { transform as transformCSS, transformStyleAttribute } from "lightningcss";
import * as oxcMinify from "oxc-minify";
// import * as terser from "terser";
import ts from "typescript";
// import * as vite from "vite";

/**
 * Compile TypeScript source code to JavaScript code.
 * @param source - Source code.
 * @param target - Target environment.
 * @returns Compiled JavaScript code.
 */
export function compileTypeScript(source: string, target: keyof typeof ts.ScriptTarget = "ESNext") {
	return ts.transpileModule(source, {
		compilerOptions: {
			module: ts.ModuleKind.ESNext,
			target: ts.ScriptTarget[target],
		},
	}).outputText;
}

/**
 * Minify JavaScript source code.
 * @param code - Source code.
 * @returns Minified code.
 */
export async function minifyJavaScript(code: string) {
	// return (await terser.minify(code, { keep_classnames: true })).code!;

	/* return (await esbuild.transform(code, {
		minify: true,
		charset: "utf8",
	})).code; */

	return (await oxcMinify.minify("index.js", code)).code;
}

/**
 * Wrap JavaScript source code with an IIFE self-executing function.
 * @param source - Source code.
 * @param useStrict - Prepend "use strict" directive?
 * @returns Converted code.
 */
export function wrapIife(source: string, useStrict: boolean = true) {
	return `(function () {\n${useStrict ? '"use strict";\n' : ""}${source}\n})();`;
}

/**
 * Create a hash.
 * @param data - String or binary.
 * @param algorithm - SHA-256, MD5, or something else.
 * @param encoding - Base64, Hex, or something else.
 * @returns Hash of data.
 */
export function createHash(data: string | crypto.BinaryLike, algorithm: "sha256" | "md5", encoding: crypto.BinaryToTextEncoding = "base64url") {
	return crypto.createHash(algorithm).update(data).digest(encoding);
}

/**
 * Minify CSS source code.
 * @param source - Source code.
 * @param type - Which part of CSS?
 * - `undefined` - Full CSS stylesheet code.
 * - `inline` - Inline style attribute code.
 * - `media` - Inline media query string.
 * @returns Minified code.
 */
export function minifyCSS(source: string, type: "inline" | "media" | undefined) {
	const options: TransformOptions<{}> & TransformAttributeOptions = {
		minify: true,
		code: Buffer.from(source),
		filename: "index.html",
	};

	if (type === "inline") return transformStyleAttribute(options).code.toString();
	else if (type === undefined) return transformCSS(options).code.toString();
	else if (type === "media") return transformCSS({ ...options, code: Buffer.from(`@media ${source}{*{top:0}}`) }).code.toString().match(/(?<=@media).*?(?={)/)?.[0].trim() ?? source;
	else return source;
}

/**
 * Minify HTML source code.
 * @param source - Source code.
 * @returns Minified code.
 */
export async function minifyHtml(source: string) {
	return await htmlMinifierTerser.minify(source, {
		collapseBooleanAttributes: true,
		collapseWhitespace: true,
		decodeEntities: true,
		keepClosingSlash: false,
		removeComments: true,
		removeRedundantAttributes: true,
		removeScriptTypeAttributes: true,
		removeStyleLinkTypeAttributes: true,
		removeEmptyAttributes: true,
		useShortDoctype: true,
		processConditionalComments: true,
		minifyCSS,
		minifyJS: true,
		minifyURLs: true,
	});
}
