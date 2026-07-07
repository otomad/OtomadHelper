<script setup lang="ts">
import { defineModel, computed } from "vue";
const value = defineModel<number>({ default: 0 });
const valueString = computed({
	get: () => value.value.toString(),
	set: v => (value.value = Number(v)),
});
</script>

<template>
	<!-- See: https://juejin.cn/post/7269786623813615668 -->
	<input type="range" v-model="valueString" />
</template>

<style lang="css" scoped>
input {
	--gap: 0.001px;
	--line-thickness: 2px;
	--thumb-size: 20px;

	height: var(--thumb-size); /* needed for Firefox */
	--active-color: var(--vp-c-brand-3);
	-webkit-appearance: none;
	-moz-appearance: none;
	appearance: none;
	background: none;
	cursor: pointer;
	overflow: hidden;
	border-radius: calc(infinity * 1px);
	margin: 1px;
}
input:focus-visible,
input:hover {
	--active-color: var(--vp-c-brand-1);
}
input:active,
input:focus-visible {
	--spread-radius: var(--thumb-size);
}
/* Chromium */
input[type="range" i]::-webkit-slider-thumb {
	height: var(--thumb-size);
	aspect-ratio: 1;
	border-radius: 50%;
	box-shadow: 0 0 0 var(--spread-radius, var(--line-thickness)) inset var(--active-color);
	border-image: linear-gradient(90deg, var(--active-color) 50%, var(--vp-c-border) 0) 1/0 100vw/0
		calc(100vw + var(--gap));
	clip-path: polygon(
		0 calc(50% + var(--line-thickness) / 2),
		-100vw calc(50% + var(--line-thickness) / 2),
		-100vw calc(50% - var(--line-thickness) / 2),
		0 calc(50% - var(--line-thickness) / 2),
		0 0,
		100% 0,
		100% calc(50% - var(--line-thickness) / 2),
		100vw calc(50% - var(--line-thickness) / 2),
		100vw calc(50% + var(--line-thickness) / 2),
		100% calc(50% + var(--line-thickness) / 2),
		100% 100%,
		0 100%
	);
	appearance: none;
	transition: 0.3s;
}
/* Firefox */
input[type="range"]::-moz-range-thumb {
	height: var(--thumb-size);
	width: var(--thumb-size);
	background: none;
	border-radius: 50%;
	box-shadow: 0 0 0 var(--spread-radius, var(--line-thickness)) inset var(--active-color);
	border-image: linear-gradient(90deg, var(--active-color) 50%, var(--vp-c-border) 0) 1/0 100vw/0
		calc(100vw + var(--gap));
	clip-path: polygon(
		0 calc(50% + var(--line-thickness) / 2),
		-100vw calc(50% + var(--line-thickness) / 2),
		-100vw calc(50% - var(--line-thickness) / 2),
		0 calc(50% - var(--line-thickness) / 2),
		0 0,
		100% 0,
		100% calc(50% - var(--line-thickness) / 2),
		100vw calc(50% - var(--line-thickness) / 2),
		100vw calc(50% + var(--line-thickness) / 2),
		100% calc(50% + var(--line-thickness) / 2),
		100% 100%,
		0 100%
	);
	appearance: none;
	transition: 0.3s;
}
</style>
