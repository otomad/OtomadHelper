<script setup lang="ts">
import { computed } from "vue";

const props = defineProps<{
	path: string;
}>();

const isKeyShortcut = computed(() => /\s\+\s/.test(props.path));
const isMenuPath = computed(() => /\s>\s/.test(props.path));
const items = computed(() => props.path.split(/\s+[+>]\s+/));
</script>

<template>
	<kbd
		class="wrapped"
		:class="isKeyShortcut ? 'key-shortcuts' : isMenuPath ? 'menu-path' : undefined"
		:aria-keyshortcuts="isKeyShortcut ? items.join('+') : undefined"
	>
		<template v-for="(item, i) in items" :key="item">
			<kbd class="item" :aria-keyshortcuts="isKeyShortcut ? item : undefined">{{ item }}</kbd>
			<template v-if="i !== items.length - 1">
				<span v-if="isMenuPath" class="sep menu-arrow">→</span>
				<span v-else class="sep key-shortcut-add">+</span>
			</template>
		</template>
	</kbd>
</template>

<style scoped>
.sep {
	font-family: "Lucide Keyboard Icons";
	display: inline-block;
	font-size: var(--vp-code-font-size);
	line-height: var(--vp-code-line-height);
	margin-inline: 0.25em;
	vertical-align: middle;
	margin-block-start: -2px;

	&.menu-arrow {
		color: var(--vp-c-text-3);
	}

	&.key-shortcut-add {
		color: var(--vp-c-text-2);
	}
}

kbd.menu-path,
.menu-path kbd {
	text-autospace: normal;
}
</style>
