<script setup lang="ts">
const props = defineProps<{
	normal: string;
	glitchy: string;
}>();
</script>

<template>
	<span class="glitchy-text">
		<span class="glitchy">{{ glitchy }}</span>
		<span class="normal">{{ normal }}</span>
	</span>
</template>

<style scoped>
.glitchy-text:not(:hover) .normal,
.glitchy-text:hover .glitchy {
	display: none;
}
</style>
