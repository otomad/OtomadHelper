<script lang="ts">
import { ref, computed, watch, useId, onMounted, onUnmounted } from "vue";

const depth = ref(2);
const autoExpand = ref(true);

watch(
	[depth, autoExpand],
	([depth, autoExpand]) => {
		document.body.style.setProperty("--outline-depth", depth);
		document.body.style.setProperty("--outline-auto-expand", autoExpand);
	},
	{ immediate: true },
);
</script>

<script setup lang="ts">
import VPSwitch from "./Switch.vue";
import Slider from "./Slider.vue";
import useI18n from "@vp/use-i18n";

const t = useI18n();
const id = useId();
const depthLabel = t({ en: "Outline depth", zh: "目录层级" });
const autoExpandLabel = t({ en: "Auto expand", zh: "自动展开" });

const outlineMarker = ref<HTMLDivElement>();
const observer = ref<MutationObserver>();
const navHeight = 64;

onMounted(() => {
	outlineMarker.value = document.querySelector<HTMLDivElement>(".outline-marker")!;
	observer.value = new MutationObserver(([mutation]) => {
		if (mutation.type === "attributes" && mutation.attributeName === "style") {
			outlineMarker.value?.scrollIntoView({ behavior: "smooth", block: "center", container: "nearest" });
		}
	});
	observer.value.observe(outlineMarker.value, { attributes: true });
});

onUnmounted(() => {
	observer.value?.disconnect();
});
</script>

<template>
	<div class="outline-depth-toggle">
		<label :for="`${id}-depth`">{{ depthLabel }}</label>
		<Slider :id="`${id}-depth`" min="2" max="6" step="1" v-model="depth" />
		<label :for="`${id}-auto-expand`">{{ autoExpandLabel }}</label>
		<VPSwitch :id="`${id}-auto-expand`" v-model="autoExpand" />
	</div>
</template>

<style scoped>
.outline-depth-toggle {
	display: grid;
	grid-template-columns: auto 1fr;
	gap: 8px 6.4px;
	align-items: center;
	padding: 4px 0 8px 17px;
	margin-bottom: 8px;
}

label {
	white-space: nowrap;
	font-size: 0.875rem;
	color: var(--vp-c-text-2);
}

.VPSwitch {
	justify-self: end;
}
</style>

<style>
.VPDocOutlineItem.root ul {
	transition:
		block-size cubic-bezier(0, 0, 0, 1) 250ms,
		visibility 250ms allow-discrete;
	overflow: clip;

	@starting-style {
		block-size: 0;
	}

	:active-view-transition & {
		transition: none;
	}
}

@container style(--outline-depth < 6) {
	.VPDocOutlineItem.root > li > ul > li > ul > li > ul > li > ul {
		visibility: collapse;
		block-size: 0;
		--collapse: true;
	}
	.VPDocOutlineItem.root > li > ul > li > ul > li > ul > li:has(.outline-link.active) > a {
		color: var(--vp-c-text-1);
		anchor-name: --outline-link-active;
	}
}
@container style(--outline-depth < 5) {
	.VPDocOutlineItem.root > li > ul > li > ul > li > ul {
		visibility: collapse;
		block-size: 0;
		--collapse: true;
	}
	.VPDocOutlineItem.root > li > ul > li > ul > li:has(.outline-link.active) > a {
		color: var(--vp-c-text-1);
		anchor-name: --outline-link-active;
	}
}
@container style(--outline-depth < 4) {
	.VPDocOutlineItem.root > li > ul > li > ul {
		visibility: collapse;
		block-size: 0;
		--collapse: true;
	}
	.VPDocOutlineItem.root > li > ul > li:has(.outline-link.active) > a {
		color: var(--vp-c-text-1);
		anchor-name: --outline-link-active;
	}
}
@container style(--outline-depth < 3) {
	.VPDocOutlineItem.root > li > ul {
		visibility: collapse;
		block-size: 0;
		--collapse: true;
	}
	.VPDocOutlineItem.root > li:has(.outline-link.active) > a {
		color: var(--vp-c-text-1);
		anchor-name: --outline-link-active;
	}
}
@container style(--outline-auto-expand: true) {
	.VPDocOutlineItem.root .outline-link.active + ul,
	.VPDocOutlineItem.root ul:has(.outline-link.active) {
		visibility: visible;
		block-size: auto;
		--collapse: false;
	}
	.VPDocOutlineItem.root a:not(.active, :hover, #\#) {
		color: var(--vp-c-text-2);
		anchor-name: none !important;
	}
}
.outline-link.active {
	anchor-name: --outline-link-active;
}
@container style(--collapse: true) {
	.outline-link {
		anchor-name: none !important;
	}
}

.VPDocAsideOutline > .content {
	.outline-marker:not([style*="opacity: 0"]) {
		position-anchor: --outline-link-active;
		top: calc((anchor(top) + anchor(bottom) - 18px) / 2) !important;
	}
}
</style>
