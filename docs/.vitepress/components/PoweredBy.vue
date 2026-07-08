<script setup lang="ts">
import { VPTeamPage, VPTeamPageTitle, VPLink } from "vitepress/theme-without-fonts";
import { useData } from "vitepress";
import useI18n from "@vp/use-i18n";
import { reactive } from "vue";
const t = useI18n();
const title = t({ en: "The Documentation is Powered By", zh: "文档搭载于" });
const vitePressLink = t({ en: "https://vitepress.dev/", zh: "https://vitepress.dev/zh/" });
const links = reactive([
	{ name: "VitePress", icon: "vitepress", href: vitePressLink },
	{ name: "Read the Docs", icon: "readthedocs", href: "https://app.readthedocs.org/projects/otomadhelper/" },
	{ name: "GitHub Pages", icon: "github", href: "https://github.com/otomad/OtomadHelper/tree/docs" },
]);
</script>

<template>
	<VPTeamPage>
		<VPTeamPageTitle>
			<template #title>{{ title }}</template>
		</VPTeamPageTitle>
		<div class="container">
			<VPLink v-for="link in links" class="vp-external-link-icon" :href="link.href">
				<SocialIcon :icon="link.icon" />{{ link.name }}
			</VPLink>
		</div>
	</VPTeamPage>
</template>

<style scoped>
.container {
	margin: 0 auto;
	max-width: 1152px;
	display: flex;
	flex-wrap: wrap;
	gap: 8px 16px;
	justify-content: center;
}
</style>
