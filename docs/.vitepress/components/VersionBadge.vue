<script setup lang="ts">
	import { VPBadge } from "vitepress/theme-without-fonts";
	import { onMounted, ref } from "vue";
	const version = ref("");
	const shieldsIoLink = "https://img.shields.io/github/v/release/otomad/OtomadHelper";

	onMounted(async () => {
		try {
			const response = await fetch(shieldsIoLink);
			const rawText = await response.text();
			const parser = new DOMParser();
			const svgXml = parser.parseFromString(rawText, "application/xml");
			version.value = [...svgXml.querySelectorAll("text")].find(text =>
				text.textContent.startsWith("v"),
			)!.textContent;
		} catch (error) {
			console.error(error);
		}
	});
</script>

<template>
	<VPBadge type="tip" class="version-badge">{{ version }}</VPBadge>
</template>

<style scoped>
	.VPBadge {
		min-block-size: 24px;
		inline-size: min-content;
		transition: scale, opacity;
		transition-duration: 250ms;

		&:empty {
			visibility: hidden;
			transition: none;
			scale: 0.8;
			opacity: 0;
		}

		@starting-style {
			scale: 0.8;
			opacity: 0;
		}
	}
</style>
