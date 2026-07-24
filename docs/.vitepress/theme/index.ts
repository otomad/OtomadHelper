/// <reference types="vitepress/client" />
/// <reference types="./shims-vue.d.ts" />

// https://vitepress.dev/guide/custom-theme
import { inBrowser, type Theme, type Router } from "vitepress";
import DefaultTheme, { VPButton } from "vitepress/theme-without-fonts";
import MyLayout from "./Layout.vue";
import handleHashOpenAndScroll from "./hash-open-and-scroll";
import "./fonts.css";
import "./style.css";
import "./view-transitions.css";
import "markdown-it-container-details-heading/vitepress-theme.css";
import VitePressMermaid from "../plugins/markdown-it/vitepress-mermaid/index.vue";

const globalComponents = import.meta.glob<{}>("./*.vue", { base: "../components", import: "default", eager: true });

export default {
	extends: DefaultTheme,
	Layout: MyLayout,
	enhanceApp({ app, router, siteData }) {
		// Register custom global components
		for (const [tagName, component] of Object.entries(globalComponents))
			app.component(tagName.slice(2, -4), component); // `tagName` is "./MyComponent.vue".
		app.component("Button", VPButton);
		app.component("vitepress-mermaid", VitePressMermaid);

		if (!inBrowser) return;

		// Route change handlers
		const beforeRouteChangeHandlers = new RouteChangeHandlers<"before">(),
			afterRouteChangeHandlers = new RouteChangeHandlers<"after">(),
			afterPageLoadHandlers = new RouteChangeHandlers<"after">();
		router.onBeforeRouteChange = beforeRouteChangeHandlers.invoke;
		router.onAfterRouteChange = afterRouteChangeHandlers.invoke;
		router.onAfterPageLoad = afterPageLoadHandlers.invoke;

		// View Transition API
		let resolver: PromiseWithResolvers<void> | undefined;
		const locales = Object.keys(siteData.value.locales).filter(lang => lang !== "root");
		beforeRouteChangeHandlers.add(toWithSearchAndHash => {
			if (!inBrowser) return;
			const from = location.pathname,
				to = toWithSearchAndHash.replace(/[?#].*/, "");
			if (!enableTransitions() || from === to) return;
			const localeChanged = isLocaleChanged(from, to, locales);
			resolver = Promise.withResolvers<void>();
			if (localeChanged) document.documentElement.classList.add("locale-changing");
			const viewTransition = document.startViewTransition(async () => await resolver!.promise).finished;
			viewTransition.finally(() => {
				document.documentElement.classList.remove("locale-changing");
			});
		});
		afterRouteChangeHandlers.add(() => {
			if (!enableTransitions() || !resolver) return;
			resolver.resolve();
		});

		// Details hash changed
		afterRouteChangeHandlers.add(() => handleHashOpenAndScroll());
	},
} satisfies Theme;

type RouteChangeHandler<TType extends "before" | "after"> = NonNullable<Router[`on${Capitalize<TType>}RouteChange`]>;
class RouteChangeHandlers<TType extends "before" | "after"> {
	handlers: RouteChangeHandler<TType>[] = [];
	add(handler: RouteChangeHandler<TType>, clientOnly: boolean = true) {
		this.handlers.push((async (...args) => {
			if (clientOnly && !inBrowser) return;
			return await handler(...args);
		}) as RouteChangeHandler<TType>);
	}
	invoke = (async to => {
		return await this.handlers.reduce<Promise<boolean>>(async (accumResultPromise, handler) => {
			const accumResult = await accumResultPromise;
			const currentResult = await handler(to);
			return !(accumResult === false || currentResult === false);
		}, Promise.resolve(true));
	}) satisfies RouteChangeHandler<"before"> as unknown as RouteChangeHandler<TType>;
}

const enableTransitions = () =>
	inBrowser && "startViewTransition" in document && !window.matchMedia("(prefers-reduced-motion: reduce)").matches;

function isLocaleChanged(from: string, to: string, locales: string[]) {
	const [fromLocale, toLocale] = [from, to].map(route => locales.find(locale => route.startsWith("/" + locale)));
	return fromLocale !== toLocale;
}
