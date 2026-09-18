<script setup lang="ts">
	import { useI18n } from "@vp/use-i18n";
	import { TextMorph } from "torph/vue";
	import { inBrowser } from "vitepress";
	import { Switch } from "vitepress-plugin-outline-depth/components";
	import { VPTeamPage, VPTeamPageTitle, VPFeatures } from "vitepress/theme-without-fonts";
	import { reactive, ref, computed, watch, useId, type MaybeRef, type ToRef } from "vue";
	const t = useI18n();
	const id = useId();
	const uwu = ref(inBrowser ? localStorage.getItem("uwu") === "true" : false);
	const u = (normal: string, uwuKind: string) => computed(() => (!uwu.value ? normal : uwuKind));

	watch(uwu, uwu => localStorage.setItem("uwu", String(uwu)));

	const img = (src: MaybeRef<string>) => ({
		src,
		width: "100%",
	});

	type ToMaybeRefs<T> = {
		[K in keyof T]: T[K] | ToRef<T[K]> | ToMaybeRefs<T[K]>;
	};
	type Features = ToMaybeRefs<InstanceType<typeof VPFeatures>["$props"]["features"][number]>[];

	const projects = reactive(
		(
			[
				{
					title: "Otomad Helper",
					details: t({ en: "Create YTPMVs in Vegas Pro", zh: "在Vegas Pro中生成音MAD" }),
					// link: "https://otomadhelper.readthedocs.io/",
					icon: img(u("/img/projects/otomad_helper.avif", "/img/projects/otomad_helper_uwu.svg")),
					linkText: t({ en: "Current Project", zh: "当前项目" }),
				},
				{
					title: "om midi",
					details: t({ en: "Create YTPMVs in After Effects", zh: "在After Effects中生成音MAD" }),
					link: t({ en: "https://ommidi.readthedocs.io/", zh: "https://ommidi.readthedocs.io/zh-cn/" }),
					icon: img(u("/img/projects/om_midi.avif", "/img/projects/om_midi_uwu.svg")),
					linkText: t({ en: "Visit", zh: "访问" }),
				},
				{
					title: "VegTips",
					details: t({ en: "Some Practical Tips for Vegas Pro", zh: "Vegas Pro的一些实用小技巧" }),
					link: t({ en: "https://vegtips.readthedocs.io/", zh: "https://vegtips.readthedocs.io/zh/" }),
					icon: img(t({ en: "/img/projects/vegtips.avif", zh: "/img/projects/vegtips_zh-CN.avif" })),
					linkText: t({ en: "Visit", zh: "访问" }),
				},
			] satisfies Features
		).map(project => ({ ...project, target: "_blank" as const })),
	);

	const title = t({ en: "Projects", zh: "项目" });
</script>

<template>
	<VPTeamPage>
		<VPTeamPageTitle>
			<template #title>
				{{ title }}
				<img src="/img/projects/otomad_plus.svg" alt="OTOMAD+" :draggable="false" />
			</template>
		</VPTeamPageTitle>
		<VPFeatures :features="projects" class="projects" />
		<p class="uwu-wrapper">
			<label :for="`${id}-uwu`" :class="{ on: uwu }">
				<TextMorph :text="!uwu ? 'uwu?' : 'no uwu plz'" />
			</label>
			<Switch :id="`${id}-uwu`" v-model="uwu" />
		</p>
	</VPTeamPage>
</template>

<style scoped>
	.projects {
		user-select: none;

		& :deep(.VPLink) {
			overflow: clip;
		}

		& :deep(.VPImage) {
			display: block;
			margin: -24px !important;
			margin-block-end: -48px !important;
			max-width: unset;
			width: calc(100% + 24px * 2);
			border-end-start-radius: 0;
			border-end-end-radius: 0;
		}

		& :deep(.title) {
			border: none;
			font-size: 24px;
		}

		& :deep(a.VPLink:not(:hover) .title) {
			color: var(--vp-c-text-1);
		}

		& :deep(.details) {
			padding-block-start: 0;
			margin-block: 0;
		}

		& :deep(a.VPLink:hover .details) {
			color: color-mix(in srgb, var(--vp-c-brand-1) 20%, var(--vp-c-text-2) 80%);
		}

		& :deep(.link-text-value) {
			margin-block: 6px -2px;
		}

		& :deep(.VPLink:not(a)) {
			background-color: rgb(from var(--vp-c-brand-3) r g b / 20%);
		}

		& :deep(.VPLink:not(a) .title) {
			font-weight: 750;
		}

		& :deep(.VPLink:not(a) .link-text-value) {
			color: var(--vp-c-text-1);
		}

		& :deep(.VPLink:not(a) .link-text-icon) {
			display: none;
		}

		& :deep(a.VPLink .link-text) {
			transition: translate, color, opacity;
			transition-duration: 250ms;
		}

		& :deep(a.VPLink:hover .link-text) {
			translate: 8px;
		}

		& :deep(a.VPLink:active .link-text) {
			translate: 2px;
		}

		& :deep(a.VPLink .vpi-arrow-right) {
			transition: opacity;
			transition-duration: 250ms;
		}

		& :deep(a.VPLink:not(:hover, :active) .vpi-arrow-right) {
			opacity: 0.5;
		}
	}

	.uwu-wrapper {
		margin-block: 2.5rem -1rem;
		display: grid;
		align-items: center;
		grid-template-columns: calc(50% + 1em) calc(50% - 1em);

		label {
			inline-size: fit-content;
			font-family: "Segoe UI Variable", "Segoe UI", sans-serif;
			font-weight: 600;
			padding-inline-end: 1em;
			justify-self: end;
			transition: color 250ms;
			user-select: none;
			cursor: pointer;

			&.on {
				color: var(--vp-c-brand-1);
			}
		}
	}

	.VPTeamPageTitle :deep(.title) {
		display: flex;
		justify-content: center;
		align-items: center;

		@media (width >= 640px) {
			img {
				height: 0.75lh;
				padding-inline-start: 0.9em;
				margin-inline-start: 0.9em;
				border-inline-start: 1px solid;
			}
		}

		@media (width < 640px) {
			flex-direction: column;

			img {
				height: 1.125lh;
				padding-block-start: 0.5em;
				margin-block-start: 0.5em;
				border-block-start: 1px solid;
			}
		}

		img {
			border-color: var(--vp-c-border);
			-webkit-user-drag: none;
		}
	}
</style>
