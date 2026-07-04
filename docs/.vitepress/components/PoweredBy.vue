<script setup lang="ts">
import { VPTeamPage, VPTeamPageTitle, VPLink } from "vitepress/theme-without-fonts";
import { useData } from "vitepress";
import { useLangGet } from "./TeamMembers.vue";
const { lang } = useData();
const get = useLangGet(lang);
const title = get({ en: "The Documentation is Powered By", zh: "文档搭载于" });
</script>

<template>
	<VPTeamPage>
		<VPTeamPageTitle>
			<template #title>{{ title }}</template>
		</VPTeamPageTitle>
		<div class="container">
			<VPLink href="https://vitepress.dev/"><SocialIcon icon="vitepress" />VitePress</VPLink>
			<VPLink href="https://app.readthedocs.org/projects/otomadhelper/">
				<SocialIcon icon="readthedocs" />Read the Docs
			</VPLink>
			<VPLink href="https://github.com/otomad/OtomadHelper/tree/docs">
				<SocialIcon icon="github" />GitHub Pages
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
