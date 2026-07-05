<script lang="ts">
import { ref, watch, useId, onMounted, onUnmounted } from "vue";

const depth = ref(6);
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

const id = useId();
const depthLabel = "目录层级";
const autoExpandLabel = "自动展开";

const outlineRoot = ref<HTMLUListElement>();
const outlineMarker = ref<HTMLDivElement>();
const observer = ref<MutationObserver>();
const pauseObserve = ref(false);

onMounted(() => {
	outlineRoot.value = document.querySelector<HTMLUListElement>(".VPDocOutlineItem.root")!;
	outlineMarker.value = document.querySelector<HTMLDivElement>(".outline-marker")!;
	observer.value = new MutationObserver(([mutation]) => {
		if (pauseObserve.value) return;
		if (mutation.type === "attributes" && mutation.attributeName === "style") updateOutlineMarker();
		outlineMarker.value?.scrollIntoView({ behavior: "smooth", block: "center", container: "nearest" });
	});
	observer.value.observe(outlineMarker.value, { attributes: true });
});

onUnmounted(() => {
	observer.value?.disconnect();
});

watch([depth, autoExpand], () => updateOutlineMarker(), { immediate: true });

function updateOutlineMarker() {
	if (!outlineRoot.value || !outlineMarker.value) return;
	let activeLink = outlineRoot.value.querySelector<HTMLAnchorElement>(".outline-link.active");
	if (!activeLink) return;
	pauseObserve.value = true;
	try {
		while (activeLink && !activeLink.checkVisibility({ visibilityProperty: true })) {
			const ul = activeLink?.parentElement?.parentElement;
			if (!ul.classList.contains("VPDocOutlineItem") || ul.classList.contains("root")) return;
			activeLink = ul.previousElementSibling;
		}
		outlineMarker.value.style.top = activeLink.offsetTop + outlineRoot.value.offsetTop + 7 + "px";
	} finally {
		pauseObserve.value = false;
	}
}
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
	}
	.VPDocOutlineItem.root > li > ul > li > ul > li > ul > li:has(.outline-link.active) > a {
		color: var(--vp-c-text-1);
	}
}
@container style(--outline-depth < 5) {
	.VPDocOutlineItem.root > li > ul > li > ul > li > ul {
		visibility: collapse;
		block-size: 0;
	}
	.VPDocOutlineItem.root > li > ul > li > ul > li:has(.outline-link.active) > a {
		color: var(--vp-c-text-1);
	}
}
@container style(--outline-depth < 4) {
	.VPDocOutlineItem.root > li > ul > li > ul {
		visibility: collapse;
		block-size: 0;
	}
	.VPDocOutlineItem.root > li > ul > li:has(.outline-link.active) > a {
		color: var(--vp-c-text-1);
	}
}
@container style(--outline-depth < 3) {
	.VPDocOutlineItem.root > li > ul {
		visibility: collapse;
		block-size: 0;
	}
	.VPDocOutlineItem.root > li:has(.outline-link.active) > a {
		color: var(--vp-c-text-1);
	}
}
@container style(--outline-auto-expand: true) {
	.VPDocOutlineItem.root .outline-link.active + ul,
	.VPDocOutlineItem.root ul:has(.outline-link.active) {
		visibility: visible;
		block-size: auto;
	}
	.VPDocOutlineItem.root a:not(.active, :hover, #\#) {
		color: var(--vp-c-text-2);
	}
}
</style>
