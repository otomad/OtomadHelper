<script setup lang="ts">
	import { useI18n } from "@vp/use-i18n";
	import { VPTeamPage, VPTeamPageTitle, VPLink, VPBadge } from "vitepress/theme-without-fonts";
	import { reactive, version as vueVersion } from "vue";
	declare const VITEPRESS_VER: string;

	const t = useI18n();
	const title = t({ en: "The Documentation is Powered By", zh: "文档搭载于" });
	const vitePressLink = t({ en: "https://vitepress.dev/", zh: "https://vitepress.dev/zh/" });
	const links = reactive([
		{ name: "VitePress", icon: "vitepress", href: vitePressLink },
		{ name: "Read the Docs", icon: "readthedocs", href: "https://app.readthedocs.org/projects/otomadhelper/" },
		{ name: "GitHub Pages", icon: "github", href: "https://github.com/otomad/OtomadHelper/tree/docs" },
	]);
	const versions = reactive([
		{ name: "VitePress", icon: "vitepress", version: VITEPRESS_VER, color: "#5C73E7" },
		{ name: "Vue", icon: "vue", version: vueVersion, color: "#4FC08D" },
	]);
</script>

<template>
	<VPTeamPage>
		<VPTeamPageTitle>
			<template #title>{{ title }}</template>
		</VPTeamPageTitle>
		<div class="container links">
			<VPLink
				v-for="{ name, icon, href } in links"
				class="vp-external-link-icon"
				:key="name"
				:href="href"
				target="_blank"
			>
				<SocialIcon :icon="icon" />
				{{ name }}
			</VPLink>
		</div>
		<div class="container versions">
			<VPBadge
				v-for="{ name, icon, version, color } in versions"
				:key="name"
				type="tip"
				:style="{ '--color': color }"
			>
				<SocialIcon :icon="icon" />
				{{ name }}
				<span class="ver">v{{ version }}</span>
			</VPBadge>
		</div>
	</VPTeamPage>
</template>

<style scoped>
	.container {
		margin: 0 auto;
		max-width: 1152px;
		display: flex;
		flex-wrap: wrap;
		justify-content: center;
		padding-inline: 24px;

		&.links {
			gap: 10px 16px;
		}

		&.versions {
			margin-block-start: 32px;
			gap: 6px;
		}
	}

	.VPBadge {
		background-color: var(--color);
		border-color: transparent;
		color: white;
		cursor: default;

		@supports (color: contrast-color(red)) {
			color: contrast-color(var(--color));
		}

		.VPIcon {
			margin-inline-end: 0px;
		}

		.ver {
			font-weight: 400;
		}
	}
</style>
