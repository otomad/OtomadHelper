<script setup lang="ts">
import { computed } from "vue";

const props = defineProps<{
	items: string;
}>();

const items = computed(() => props.items.split(/\s+>\s+/));
</script>

<template>
	<span class="menu-path">
		<template v-for="(item, i) in items" :key="item" class="menu-item">
			<span class="menu-item">{{ item }}</span>
			<span v-if="i !== items.length - 1" class="menu-arrow"><span class="sr-only">→</span></span>
		</template>
	</span>
</template>

<style scoped>
.menu-arrow {
	.sr-only {
		font-size: 0;
	}

	&::before {
		content: "";
		width: var(--vp-code-font-size);
		height: var(--vp-code-font-size);
		display: inline-block;
		background-color: var(--vp-c-text-3);
		mask-image: url(@vp/theme/icons/keys/right.svg?inline);
		mask-size: 100%;
		margin-inline: 0.25em;
		margin-block-end: -1px;
	}
}
</style>
