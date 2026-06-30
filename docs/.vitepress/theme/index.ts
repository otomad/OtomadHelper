/// <reference types="vitepress/client" />

// https://vitepress.dev/guide/custom-theme
import { h } from "vue";
import { inBrowser, type Theme } from "vitepress";
import DefaultTheme from "vitepress/theme-without-fonts";
import MyLayout from "./Layout.vue";
import "./style.css";
import "./view-transitions.css";

const globalComponents = import.meta.glob<{}>("./*.vue", { base: "../components", import: "default", eager: true });

export default {
	extends: DefaultTheme,
	Layout: MyLayout,
	enhanceApp({ app, router, siteData }) {
		// Register custom global components
		for (const [tagName, component] of Object.entries(globalComponents))
			app.component(tagName.slice(2, -4), component); // `tagName` is "./MyComponent.vue".

		if (!inBrowser) return;

		// View Transition API
		let resolver: PromiseWithResolvers<void> | undefined;
		const locales = Object.keys(siteData.value.locales).filter(lang => lang !== "root");
		router.onBeforeRouteChange = toWithSearchAndHash => {
			if (!globalThis.location) return;
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
		};
		router.onAfterRouteChange = () => {
			// Handle View Transition API
			(() => {
				if (!enableTransitions() || !resolver) return;
				resolver.resolve();
			})();

			// Handle details hash changed
			(() => {
				handleHashOpenAndScroll();
			})();
		};
	},
} satisfies Theme;

const enableTransitions = () =>
	inBrowser && "startViewTransition" in document && !window.matchMedia("(prefers-reduced-motion: reduce)").matches;

function isLocaleChanged(from: string, to: string, locales: string[]) {
	const [fromLocale, toLocale] = [from, to].map(route => locales.find(locale => route.startsWith("/" + locale)));
	return fromLocale !== toLocale;
}

// 自动展开与定位的核心函数
const handleHashOpenAndScroll = () => {
	const hash = location.hash.slice(1);
	if (!hash) return;

	// 找到对应 id 的元素
	const targetId = decodeURIComponent(hash);
	const heading = document.getElementById(targetId);

	if (heading && heading.matches("details > summary > :is(h1, h2, h3, h4, h5, h6)")) {
		heading.closest("details")!.open = true;
	}
};
