<script setup lang="ts">
import { VPTeamPage, VPTeamPageTitle, VPFeatures } from "vitepress/theme-without-fonts";
import { useData } from "vitepress";
import { useLangGet } from "./TeamMembers.vue";
const get = useLangGet();

const img = (src: string) => ({
	src,
	width: "100%",
});

const projects = [
	{
		title: "Otomad Helper",
		details: get({ en: "Create YTPMVs in Vegas Pro", zh: "在Vegas Pro中生成音MAD" }),
		// link: "https://otomadhelper.readthedocs.io/",
		icon: img("/img/projects/otomad_helper.avif"),
		linkText: get({ en: "Current Project", zh: "当前项目" }),
	},
	{
		title: "om midi",
		details: get({ en: "Create YTPMVs in After Effects", zh: "在After Effects中生成音MAD" }),
		link: get({ en: "https://ommidi.readthedocs.io/", zh: "https://ommidi.readthedocs.io/zh-cn/" }),
		icon: img("/img/projects/om_midi.avif"),
		linkText: get({ en: "Visit", zh: "访问" }),
	},
	{
		title: "VegTips",
		details: get({ en: "Some Practical Tips for Vegas Pro", zh: "Vegas Pro的一些实用小技巧" }),
		link: get({ en: "https://vegtips.readthedocs.io/", zh: "https://vegtips.readthedocs.io/zh/" }),
		icon: img(get({ en: "/img/projects/vegtips.avif", zh: "/img/projects/vegtips_zh-CN.avif" })),
		linkText: get({ en: "Visit", zh: "访问" }),
	},
];

const title = get({ en: "Projects", zh: "项目" });
</script>

<template>
	<VPTeamPage>
		<VPTeamPageTitle>
			<template #title>{{ title }}</template>
		</VPTeamPageTitle>
		<VPFeatures :features="projects" class="projects" />
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
		font-weight: 700;
	}

	& :deep(.VPLink:not(a) .link-text-value) {
		color: var(--vp-c-text-1);
	}

	& :deep(.VPLink:not(a) .link-text-icon) {
		display: none;
	}
}
</style>
