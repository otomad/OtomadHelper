<script setup lang="ts">
	import { VPIcon } from "vitepress/theme-without-fonts";

	const props = defineProps<{
		icon: string;
	}>();
</script>

<template>
	<VPIcon class="VPIcon" :icon="`simple-icons:${icon}`" />
</template>

<style scoped>
	.VPIcon {
		vertical-align: text-bottom;
		width: 1.25em !important;
		height: 1.25em !important;
		margin-inline-end: 0.25em;
	}
</style>
