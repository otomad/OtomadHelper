<script setup lang="ts">
import { useData } from "vitepress";
const { page } = useData();
</script>

<template>
	<div class="print-header-title">{{ page.title }}</div>
</template>

<style scoped>
.print-header-title {
	display: none;
	position: fixed;
	width: 100%;
	text-align: center;
	font-size: 14px;
	font-weight: normal;

	@media print {
		display: block;
	}
}
</style>
