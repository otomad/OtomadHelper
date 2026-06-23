// https://vitepress.dev/guide/custom-theme
import { h } from "vue";
import type { Theme } from "vitepress";
import DefaultTheme from "vitepress/theme-without-fonts";
import MyLayout from "./Layout.vue";
import "./style.css";
import "./view-transitions.css";

export default {
	extends: DefaultTheme,
	Layout: MyLayout,
	enhanceApp({ app, router, siteData }) {
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
			if (!enableTransitions() || !resolver) return;
			resolver.resolve();
		};
	},
} satisfies Theme;

const enableTransitions = () =>
	globalThis.window &&
	"startViewTransition" in document &&
	!window.matchMedia("(prefers-reduced-motion: reduce)").matches;

function isLocaleChanged(from: string, to: string, locales: string[]) {
	const [fromLocale, toLocale] = [from, to].map(route => locales.find(locale => route.startsWith("/" + locale)));
	return fromLocale !== toLocale;
}
