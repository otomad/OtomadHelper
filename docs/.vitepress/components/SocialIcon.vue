<script setup lang="ts">
import { VPSocialLink } from "vitepress/theme-without-fonts";

const props = defineProps<{
	icon: string;
}>();
</script>

<template>
	<VPSocialLink :icon="props.icon" link="" :me="false" class="social-icon-only" />
</template>

<style scoped>
.social-icon-only {
	interactivity: inert;
	user-select: none;
	display: contents;
	color: inherit;

	& > :deep(span) {
		display: inline-block;
		fill: inherit;
		vertical-align: text-bottom;
	}
}
</style>
