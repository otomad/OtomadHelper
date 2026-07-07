<script setup lang="ts">
import { useData } from "vitepress";
import icon from "@vp/theme/icons/print.svg?raw";
import { useLangGet } from "./TeamMembers.vue";
import { computed } from "vue";
const get = useLangGet();
const { isDark } = useData();

const label = computed(() => get({ en: "Print/Save as PDF", zh: "打印/保存为PDF" }));

function print() {
	const { classList } = document.documentElement;
	classList.remove("dark");
	window.print();
	if (isDark.value) classList.add("dark");
}
/* function print() {
	const iframe = document.createElement("iframe");
	iframe.onload = () => {
		const doc = iframe.contentWindow.document;
		document.head.querySelectorAll("link[rel~=stylesheet], style").forEach(styleNode => {
			doc.head.appendChild(styleNode.cloneNode(true));
		});
		const bodyClone = document.body.cloneNode(true);
		doc.body.append(...bodyClone.childNodes);
		const closePrint = () => document.body.removeChild(iframe);
		iframe.contentWindow.onbeforeunload = closePrint;
		iframe.contentWindow.onafterprint = closePrint;
		iframe.contentWindow.print();
	};
	iframe.style.display = "none";
	document.body.appendChild(iframe);
} */
</script>

<template>
	<button type="button" class="save-pdf-button" :title="label" v-html="icon" @click="print()" />
</template>

<style scoped>
.save-pdf-button {
	width: 36px;
	height: 36px;
	color: var(--vp-c-text-1);
	display: grid;
	place-items: center;
	border-radius: 8px;
	margin-right: -22px;
	margin-left: 12px;
	z-index: 2;
	transition: color 250ms;

	&:hover {
		color: var(--vp-c-text-2);
	}

	& :deep(svg) {
		width: 20px;
		height: 20px;
	}

	@media (width < 768px) {
		margin-right: -26px;
		margin-left: 0;
	}

	.VPNavBar:not(.has-sidebar) & {
		opacity: 0;
		display: none;
	}
}
</style>
