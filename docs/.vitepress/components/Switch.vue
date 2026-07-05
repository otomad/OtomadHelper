<script setup lang="ts">
// See: https://github.com/vuejs/vitepress/blob/main/src/client/theme-default/components/VPSwitch.vue
import { defineModel } from "vue";
const on = defineModel<boolean>({ default: false });
</script>

<template>
	<button class="VPSwitch" type="button" role="switch" @click="on = !on">
		<span class="check" />
		<input type="checkbox" v-model="on" />
	</button>
</template>

<style scoped>
.VPSwitch {
	position: relative;
	border-radius: 11px;
	display: block;
	width: 40px;
	height: 22px;
	flex-shrink: 0;
	border: 1px solid var(--vp-input-border-color);
	background-color: var(--vp-input-switch-bg-color);
	transition:
		border-color 0.25s,
		background-color 0.25s !important;

	&:hover {
		border-color: var(--vp-c-brand-1);
	}
}

.check {
	position: absolute;
	top: 1px;
	/*rtl:ignore*/
	left: 1px;
	width: 18px;
	height: 18px;
	border-radius: 50%;
	background-color: var(--vp-c-neutral-inverse);
	box-shadow: var(--vp-shadow-1);
	transition: all 0.25s !important;
}

input {
	display: none;
}

.VPSwitch:has(input:checked) {
	background-color: var(--vp-c-brand-1);
	border-color: var(--vp-c-brand-1);

	&:hover {
		border-color: var(--vp-c-brand-3);
	}

	.check {
		translate: 18px;
	}
}
</style>
