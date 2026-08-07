<script setup lang="ts">
	import AccessArticleButtons from "@vp/components/AccessArticleButtons.vue";
	import PrintHeaderTitle from "@vp/components/PrintHeaderTitle.vue";
	import VersionBadge from "@vp/components/VersionBadge.vue";
	import { useData } from "vitepress";
	import { createMermaidRenderer } from "vitepress-mermaid-renderer";
	import DefaultTheme from "vitepress/theme-without-fonts";
	import { nextTick, provide, onMounted, watch } from "vue";
	import { useI18nThemeConfig } from "../use-i18n";
	import handleHashOpenAndScroll from "./hash-open-and-scroll";
	import flyoutShadowStyle from "./readthedocs-flyout-shadow.css?inline";

	const { isDark, localeIndex } = useData();

	const enableTransitions = () =>
		"startViewTransition" in document && !window.matchMedia("(prefers-reduced-motion: reduce)").matches;

	provide("toggle-appearance", async ({ clientX: x, clientY: y }: MouseEvent) => {
		if (!enableTransitions()) {
			isDark.value = !isDark.value;
			return;
		}

		const clipPath = [
			`circle(0px at ${x}px ${y}px)`,
			`circle(${Math.hypot(Math.max(x, innerWidth - x), Math.max(y, innerHeight - y))}px at ${x}px ${y}px)`,
		];

		await document.startViewTransition({
			update: async () => {
				isDark.value = !isDark.value;
				await nextTick();
			},
			types: ["instant"],
		}).ready;

		await document.documentElement.animate(
			{ clipPath: isDark.value ? clipPath.reverse() : clipPath },
			{
				duration: 300,
				easing: "ease-in",
				fill: "both",
				pseudoElement: `::view-transition-${isDark.value ? "old" : "new"}(root)`,
			},
		).finished;

		// WARN: 自 Chromium 150 开始该过渡动画会发生异常，但 Chromium 149 却没事。
	});

	onMounted(async () => {
		// Readthedocs flyout custom style.
		if (!location.hostname.includes("readthedocs")) return;
		const READTHEDOCS_FLYOUT = "readthedocs-flyout";
		await new Promise<void>(resolve => {
			if (document.querySelector(READTHEDOCS_FLYOUT)?.shadowRoot) {
				resolve();
				return;
			}
			const observer = new MutationObserver(mutationsList => {
				for (const mutation of mutationsList) {
					if (mutation.type === "childList" && mutation.addedNodes.length > 0) {
						for (const node of mutation.addedNodes)
							if (
								node instanceof HTMLElement &&
								node.tagName.toLowerCase() === READTHEDOCS_FLYOUT &&
								node.shadowRoot
							) {
								resolve();
								observer.disconnect();
								return;
							}
					}
				}
			});
			observer.observe(document.body, { childList: true });
		});
		const flyout = document.querySelector(READTHEDOCS_FLYOUT)!.shadowRoot!;
		const stylesheet = new CSSStyleSheet();
		stylesheet.replaceSync(flyoutShadowStyle);
		flyout.adoptedStyleSheets.push(stylesheet);
		flyout.firstElementChild!.part = "flyout";
	});

	onMounted(() => {
		// 阻止连续点按 <kbd> 元素时浏览器自动选择文本。
		document.addEventListener("mousedown", e => {
			const kbd = (e.target as HTMLElement).closest("kbd");
			// 仅屏蔽双击及连击。
			if (kbd && e.detail > 1) e.preventDefault();
		});
	});

	onMounted(async () => {
		await handleHashOpenAndScroll(10);
	});

	// Mermaid
	function initMermaid() {
		const mermaidRenderer = createMermaidRenderer({
			theme: isDark.value ? "dark" : "default",
			fitToContainer: true,
		});
		mermaidRenderer.setToolbar({
			i18n: {
				localeIndex: localeIndex.value,
				locales: {
					"zh-CN": useI18nThemeConfig("zh").mermaid,
				},
			},
		});
	}

	nextTick(() => initMermaid());
	watch(
		() => [isDark.value, localeIndex.value],
		() => initMermaid(),
	);
</script>

<template>
	<DefaultTheme.Layout>
		<template #home-hero-info-before><VersionBadge /></template>
		<template #nav-bar-title-after><PrintHeaderTitle /></template>
		<template #doc-top><AccessArticleButtons /></template>
	</DefaultTheme.Layout>
</template>

<style>
	:root:active-view-transition-type(instant) {
		* {
			view-transition-name: none !important;
			view-transition-class: none !important;
			transition: none !important;
			animation: none !important;
		}

		&::view-transition-old(root),
		&::view-transition-new(root) {
			animation: none;
			mix-blend-mode: normal;
		}

		&::view-transition-old(root),
		&.dark::view-transition-new(root) {
			z-index: 1;
		}

		&::view-transition-new(root),
		&.dark::view-transition-old(root) {
			z-index: 9999;
		}
	}

	:root.locale-changing * {
		view-transition-name: none !important;
		transition: none !important;
		animation: none !important;
	}

	:root.locale-changing {
		scroll-behavior: auto !important;
	}

	.version-badge {
		margin-block: -8px 8px;
		display: block;

		@media (width < 960px) {
			margin-inline: auto;
		}
	}
</style>
