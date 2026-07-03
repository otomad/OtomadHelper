<script setup lang="ts">
import { VPSocialLink } from "vitepress/theme-without-fonts";

const props = defineProps<{
	icon: string;
}>();
</script>

<template>
	<object>
		<VPSocialLink :icon="props.icon" link="" :me="false" class="social-icon-only" />
	</object>
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
		width: 1.25em !important;
		height: 1.25em !important;
		margin-inline-end: 0.25em;
	}
}

object {
	display: inline;
}
</style>
