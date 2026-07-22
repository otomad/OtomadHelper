/// <reference types="vitepress/client" />

// https://vitepress.dev/guide/custom-theme
import { inBrowser, type Theme, type Router } from "vitepress";
import { nextTick } from "vue";
import DefaultTheme, { VPButton } from "vitepress/theme-without-fonts";
import MyLayout from "./Layout.vue";
import "./fonts.css";
import "./style.css";
import "./view-transitions.css";
import "markdown-it-container-details-heading/vitepress-theme.css";

const globalComponents = import.meta.glob<{}>("./*.vue", { base: "../components", import: "default", eager: true });

export default {
	extends: DefaultTheme,
	Layout: MyLayout,
	enhanceApp({ app, router, siteData }) {
		// Register custom global components
		for (const [tagName, component] of Object.entries(globalComponents))
			app.component(tagName.slice(2, -4), component); // `tagName` is "./MyComponent.vue".
		app.component("Button", VPButton);

		if (!inBrowser) return;

		// Route change handlers
		const beforeRouteChangeHandlers = new RouteChangeHandlers<"before">(),
			afterRouteChangeHandlers = new RouteChangeHandlers<"after">();
		router.onBeforeRouteChange = beforeRouteChangeHandlers.invoke;
		router.onAfterRouteChange = afterRouteChangeHandlers.invoke;

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
		afterRouteChangeHandlers.add(handleHashOpenAndScroll);
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

// 自动展开与定位的核心函数
const handleHashOpenAndScroll = async () => {
	const hash = location.hash.slice(1);
	if (!hash) return;

	// 找到对应 id 的元素
	const targetId = decodeURIComponent(hash);
	const heading = document.getElementById(targetId);

	if (heading && heading.matches("details > summary > :is(h1, h2, h3, h4, h5, h6)")) {
		const details = heading.closest("details")!;
		details.open = true;
		const scroll = async () => {
			const SCROLL_PADDING_TOP_CLASS = "scroll-padding-top";
			document.documentElement.classList.add(SCROLL_PADDING_TOP_CLASS);
			await details.scrollIntoView({ block: "start" });
			await details.scrollIntoView({ block: "start" });
			document.documentElement.classList.remove(SCROLL_PADDING_TOP_CLASS);
		};
		await scroll();
		if (document.activeViewTransition) {
			await document.activeViewTransition?.finished;
			await scroll();
		}
	}
};
