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
	--active-color: var(--vp-c-brand-3);
	--gap: 0.001px;
	--line-thickness: 2px;
	--thumb-size: 20px;

	height: var(--thumb-size); /* needed for Firefox*/
	--_c: color-mix(in srgb, var(--active-color), #000 var(--p, 0%));
	-webkit-appearance: none;
	-moz-appearance: none;
	appearance: none;
	background: none;
	cursor: pointer;
	overflow: hidden;
	border-radius: calc(infinity * 1px);
}
input:focus-visible,
input:hover {
	--p: 25%;
}
input:active,
input:focus-visible {
	--_b: var(--thumb-size);
}
/* chromium */
input[type="range" i]::-webkit-slider-thumb {
	height: var(--thumb-size);
	aspect-ratio: 1;
	border-radius: 50%;
	box-shadow: 0 0 0 var(--_b, var(--line-thickness)) inset var(--_c);
	border-image: linear-gradient(90deg, var(--_c) 50%, var(--vp-c-border) 0) 1/0 100vw/0 calc(100vw + var(--gap));
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
	box-shadow: 0 0 0 var(--_b, var(--line-thickness)) inset var(--_c);
	border-image: linear-gradient(90deg, var(--_c) 50%, var(--vp-c-border) 0) 1/0 100vw/0 calc(100vw + var(--gap));
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
@supports not (color: color-mix(in srgb, red, red)) {
	input {
		--_c: var(--active-color);
	}
}
</style>
