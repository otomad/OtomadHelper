import { readFile } from "fs/promises";
import path, { posix, resolve as resolve_ } from "path";
import { JSDOM } from "jsdom";
import type { HtmlTagDescriptor } from "vite";
import "../../utils/object";
import { compileTypeScript, minifyHtml, minifyJavaScript, wrapIife } from "./utils";

export default ({ scripts, minifyHtml: minify = true }: {
	scripts: (string | PriorScript)[];
	/** Minify HTML when building? */
	minifyHtml?: boolean;
}): VitePlugin => {
	let config: VitePluginConfig;
	const resolve = (...paths: string[]) => resolve_(config.root, ...paths);
	let isDev: boolean;
	let scripts_: (PriorScript & { source: string })[];
	const filters: Filter[] = [];
	const bundles = new Map<string, string>();

	return {
		name: "vite-plugin-inject-script",
		enforce: "pre",

		async configResolved(resolvedConfig) {
			config = resolvedConfig;
			isDev = config.command === "serve";

			scripts_ = await Promise.all(scripts.map(async script => {
				if (typeof script === "string") script = { src: script };
				script.src = script.src.trim();
				script.type ??= "script";
				script.injectTo ??= "head-append";
				if (script.filterHtml) filters.push(script.filterHtml);

				let source = await readFile(resolve(script.src), "utf-8");
				if (script.modify) source = script.modify(source);
				if (script.type === "iife") source = wrapIife(source);
				if (script.src.match(/\.[cm]?tsx?/i)) source = compileTypeScript(source);
				if (!isDev) source = await minifyJavaScript(source);

				return Object.assign(script, { source });
			}));
		},

		generateBundle() {
			for (const { src, source, inline } of scripts_) {
				if (inline) continue;
				const name = path.parse(src).name + ".js";
				const referenceId = this.emitFile({
					type: "asset",
					name,
					source,
				});
				bundles.set(src, this.getFileName(referenceId));
			}
		},

		configureServer(server) {
			for (const { src, source, inline } of scripts_) {
				if (inline) continue;
				let route = posix.join("/@inject-script", src);
				route = posix.format({ ...path.parse(route), base: "", ext: ".js" });
				server.middlewares.use(route, (_, res) => {
					res.setHeader("Content-Type", "text/javascript");
					res.end(source);
				});
				bundles.set(src, route);
			}
		},

		transformIndexHtml: {
			order: "post",
			async handler(html, { filename, path }) {
				if (!filterHtmlFilename(filename, filters as never)) return;

				const tags = scripts_.map(script => {
					const { src, type, injectTo, modify: _modify, filterHtml, inline, blocking, source, ...attrs } = script;
					if (!filterHtmlFilename(filename, filterHtml)) return undefined!;

					if (blocking) attrs.blocking = blocking.join(" ");
					if (!inline) attrs.src = bundles.get(src);
					attrs.type = type === "classic" || type === "script" || type === "iife" ? undefined : type;

					const result: Tag = { tag: "script", injectTo: injectTo!, attrs, children: inline ? source : undefined };
					return result;
				}).filter(Boolean);
				const group = Object.groupBy(tags, ({ injectTo }) => injectTo);

				const dom = new JSDOM(html);
				const { document } = dom.window;
				const entry = dom.window.document.head.querySelector('script[type="module"][crossorigin][src$="index.js"]');
				const createElement = (tag: Tag) => {
					const script = document.createElement(tag.tag) as HTMLScriptElement;
					if (tag.children) script.textContent = tag.children;
					for (const [attr, value] of Object.entries(tag.attrs ?? {}))
						if (value != null && value !== false)
							script.setAttribute(attr, value === true ? "" : value);
					return script;
				};
				group["head-prepend"]?.toReversed().forEach(tag => document.head.prepend(createElement(tag)));
				group["body-prepend"]?.toReversed().forEach(tag => document.body.prepend(createElement(tag)));
				group["body-append"]?.forEach(tag => document.body.append(createElement(tag)));
				group["head-append"]?.forEach(tag => {
					const script = createElement(tag);
					if (!entry) document.head.append(script);
					else document.head.insertBefore(script, entry);
				});

				let newHtml = dom.serialize();
				if (minify) newHtml = await minifyHtml(newHtml);
				return newHtml;
			},
		},
	};
};

type Filter = (RegExp | string)[] | RegExp | string | ((filename: string) => boolean);
function filterHtmlFilename(filename: string, filter?: Filter): boolean {
	if (typeof filter === "function") return filter(filename);
	if (filter instanceof RegExp) return filter.test(filename);
	if (typeof filter === "string") return filename === filter;
	if (Array.isArray(filter) && filter.length > 0) return filter.some(oneFilter => filterHtmlFilename(filename, oneFilter));
	return true;
}

interface PriorScript {
	/** Path to the script (js/ts/jsx/tsx). */
	src: string;
	/**
	 * - `"classic"` | `"script"` - Classic script.
	 * - `"module"` - JavaScript module.
	 * - `"iife"` - Wrap the script content with IIFE, and mark as "use strict".
	 * - Others - Directly pass to the type attribute.
	 */
	type?: "classic" | "script" | "module" | "iife" | string & {};
	/** Select where to insert the script. Defaults to "head-append". */
	injectTo?: "head-prepend" | "head-append" | "body-prepend" | "body-append";
	/** Modify the source script content. */
	modify?(html: string): string;
	/** Filter the HTML files to be injected with the script. */
	filterHtml?: Filter;
	/** Directly set the script innerText? */
	inline?: boolean;
	/** The script will be fetched in parallel to parsing and evaluated as soon as it is available. */
	async?: boolean;
	/** Certain operations should be blocked on the fetching of the script. */
	blocking?: ("render")[];
	/** Allow error logging for sites which use a separate domain for static media. */
	crossOrigin?: "anonymous" | "use-credentials";
	/** Indicate the script is meant to be executed after the document has been parsed, but before firing `DOMContentLoaded` event. */
	defer?: boolean;
	/** Provide a hint of the relative priority to use when fetching an external script. */
	fetchPriority?: "high" | "low" | "auto";
	/** Contain inline metadata that a user agent can use to verify that a fetched resource has been delivered without unexpected manipulation. */
	integrity?: string;
	/** Serve fallback scripts to older browsers that do not support modular JavaScript code. */
	noModule?: boolean;
	/** A cryptographic nonce (number used once) to allow scripts in a script-src Content-Security-Policy. */
	nonce?: boolean;
	/** Indicate which referrer to send when fetching the script, or resources fetched by the script. */
	referrerPolicy?: "no-referrer" | "no-referrer-when-downgrade" | "origin" | "origin-when-cross-origin" | "same-origin" | "strict-origin" | "strict-origin-when-cross-origin" | "unsafe-url";
	/** Any other custom attributes. */
	[x: string]: Any;
}

type Tag = Override<HtmlTagDescriptor, { injectTo: NonNull<PriorScript["injectTo"]>; children?: string }>;
