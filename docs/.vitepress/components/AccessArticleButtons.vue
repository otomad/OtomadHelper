<script setup lang="ts">
// See: https://github.com/okineadev/vitepress-plugin-llms/blob/main/src/vitepress-components/CopyOrDownloadAsMarkdownButtons.vue
import iconCheck from "@vp/theme/icons/check.svg?raw";
import iconChevron from "@vp/theme/icons/chevron.svg?raw";
import iconCopy from "@vp/theme/icons/copy.svg?raw";
import iconDownload from "@vp/theme/icons/download.svg?raw";
import iconExternal from "@vp/theme/icons/external.svg?raw";
import iconMarkdown from "@vp/theme/icons/markdown.svg?raw";
import iconPrint from "@vp/theme/icons/print.svg?raw";
import iconRss from "@vp/theme/icons/rss.svg?raw";
import iconShare from "@vp/theme/icons/share.svg?raw";
import iconPlay from "@vp/theme/icons/play.svg?raw";
import iconPause from "@vp/theme/icons/pause.svg?raw";
import iconStop from "@vp/theme/icons/stop.svg?raw";

const aiProviders = {};

// const isOpen = ref(false);
// const dropdownContainer = ref<HTMLElement>();
// const isRendered = ref(false);
// const dropdownMenu = ref<HTMLElement>();

// // const { aiProviders, copied, copyAsMarkdown, downloadMarkdown, downloaded, openInAI, viewAsMarkdown } =
// // 	useCopyOrDownloadAsMarkdownButtons();

// const aiProviderIcons: Record<string, string> = {
// 	ChatGPT: iconChatGPT,
// 	Claude: iconClaude,
// };

// function closeDropdown(): void {
// 	if (!isOpen.value) {
// 		isRendered.value = false;
// 		return;
// 	}

// 	isOpen.value = false;

// 	const el = dropdownMenu.value;
// 	if (!el) {
// 		isRendered.value = false;
// 		return;
// 	}

// 	const onEnd = (): void => {
// 		isRendered.value = false;
// 		el.removeEventListener("transitionend", onEnd);
// 	};

// 	el.addEventListener("transitionend", onEnd);
// }

// function toggleDropdown(): void {
// 	if (isOpen.value) {
// 		closeDropdown();
// 	} else {
// 		isRendered.value = true;
// 		requestAnimationFrame(() => {
// 			isOpen.value = true;
// 		});
// 	}
// }

// function resolveProviderIcon(provider: MarkdownAiProvider): string {
// 	return aiProviderIcons[provider.name] ?? iconExternal;
// }

// async function handleCopyAsMarkdown(): Promise<void> {
// 	await copyAsMarkdown();
// 	closeDropdown();
// }

// function handleViewAsMarkdown(): void {
// 	viewAsMarkdown();
// 	closeDropdown();
// }

// function handleOpenInAI(provider: MarkdownAiProvider): void {
// 	openInAI(provider);
// 	closeDropdown();
// }

// function handleClickOutside(event: MouseEvent): void {
// 	if (dropdownContainer.value && !dropdownContainer.value.contains(event.target as Node)) {
// 		closeDropdown();
// 	}
// }

// onMounted(() => document.addEventListener("click", handleClickOutside));
// onUnmounted(() => document.removeEventListener("click", handleClickOutside));
</script>

<script lang="ts">
import { h } from "vue";

function InnerButton(_props, { attrs: { icon: _icon, name, ...attrs } }) {
	const [iconName, icon] = Object.entries(_icon)[0];
	const id = "--access-article-button-" + iconName.replace(/^icon/i, "").toLowerCase();
	return h("button", { ...attrs, class: "button", title: name, style: { "--anchor-name": id } }, [
		h("span", { class: "icon", innerHTML: icon }),
	]);
}
</script>

<template>
	<div class="markdown-copy-buttons">
		<div class="markdown-copy-buttons-pretend-content">
			<div class="markdown-copy-buttons-inner">
				<InnerButton name="Print/Save as PDF" :icon="{ iconPrint }" />

				<!-- Markdown button -->
				<div class="button-group dropdown-trigger">
					<!-- <button class="copy-page">
						<span v-html="copied ? iconCheck : iconCopy" class="icon"></span>
						<span class="label">
							{{ copied ? "Copied" : "Copy page" }}
						</span>
					</button> -->
					<InnerButton name="View as Markdown" :icon="{ iconMarkdown }" />

					<span class="divider"></span>

					<!-- Chevron area -->
					<button class="chevron-wrapper" popovertarget="markdown-copy-menu">
						<span v-html="iconChevron" class="icon chevron"></span>
					</button>
				</div>

				<InnerButton name="RSS Feed" :icon="{ iconRss }" />

				<InnerButton name="Share" :icon="{ iconShare }" />

				<InnerButton name="Read Aloud" :icon="{ iconPlay }" />

				<div class="button-group">
					<InnerButton name="Pause Reading" :icon="{ iconPause }" />
					<span class="divider"></span>
					<InnerButton name="Stop Reading" :icon="{ iconStop }" />
				</div>
			</div>
		</div>

		<!-- Markdown Dropdown -->
		<div class="dropdown-menu" popover="auto" id="markdown-copy-menu">
			<button class="dropdown-item">
				<span v-html="iconCopy" class="icon"></span>
				Copy page as Markdown
				<!-- <span v-html="iconExternal" class="icon external"></span> -->
			</button>
			<button class="dropdown-item">
				<span v-html="iconDownload" class="icon"></span>
				Download Markdown
				<!-- <span v-html="iconExternal" class="icon external"></span> -->
			</button>
			<button class="dropdown-item">
				<span class="icon"><SocialIcon icon="openai" /></span>
				Open in ChatGPT
				<span v-html="iconExternal" class="icon external"></span>
			</button>
			<button class="dropdown-item">
				<span class="icon"><SocialIcon icon="claude" /></span>
				Open in Claude
				<span v-html="iconExternal" class="icon external"></span>
			</button>

			<!-- <button
						v-for="provider in aiProviders"
						:key="provider.name"
						class="dropdown-item"
						@click="handleOpenInAI(provider)"
					>
						<span v-html="resolveProviderIcon(provider)" class="icon"></span>
						Open in {{ provider.name }}
						<span v-html="iconExternal" class="icon external"></span>
					</button> -->
		</div>
	</div>
</template>

<style scoped>
.markdown-copy-buttons {
	display: flex;
	margin-block: -28px 8px;

	@media print {
		display: none;
	}

	@media (width < 768px) {
		margin-block: -21px 1px;
	}

	/* 模仿 aside 的占位符。 */
	&::after {
		content: "";
		display: block;
		width: 256px;
		flex-shrink: 0;

		@media (width < 1280px) {
			display: none;
		}
	}
}

.markdown-copy-buttons-pretend-content {
	width: 100%;

	@media (width >= 960px) {
		padding-inline: 32px;
	}
}

.markdown-copy-buttons-inner {
	max-width: 688px;
	margin: 16px auto;
	display: flex;
	gap: 8px;
	position: relative;
	flex-wrap: wrap;
}

.button-group {
	display: flex;
	align-items: stretch;
	background: transparent;
	border: 1px solid var(--vp-c-divider);
	border-radius: 6px;
	color: var(--vp-c-text-1);
	font-size: 14px;
	padding: 0;
	overflow: hidden;
}

.dropdown-trigger {
	anchor-name: --markdown-copy-dropdown-trigger;
}

.button {
	display: flex;
	align-items: center;
	gap: 8px;
	padding: 9px 12px;
	background: transparent;
	border: 1px solid var(--vp-c-divider);
	border-radius: 6px;
	color: var(--vp-c-text-1);
	cursor: pointer;
	white-space: nowrap;
	anchor-name: var(--anchor-name);

	@media (width >= 768px) {
		padding: 11px 14px;
	}

	.button-group & {
		border: none;
		border-radius: 0;
	}

	* {
		flex-shrink: 0;
	}
}

:global(.access-article-button-tooltip) {
	position: fixed;
	position-area: block-end;
	position-try-fallbacks: flip-block;
	display: block;
	padding: 4px 8px;
	margin-block: 8px;
	background-color: var(--vp-c-bg-soft);
	pointer-events: none;
	border-radius: 4px;
	font-size: 14px;
	z-index: 101;
}

.label {
	white-space: nowrap;
}

.divider {
	width: 1px;
	height: 25px;
	align-self: center;
	background: var(--vp-c-divider);
	opacity: 0.6;

	button:hover + &,
	&:has(+ button:hover) {
		opacity: 0;
	}
}

.chevron-wrapper {
	display: flex;
	align-items: center;
	justify-content: center;
	padding: 0 10px;
	cursor: pointer;
	background: transparent;
	border: none;
}

.dropdown-menu {
	position: fixed;
	top: 0;
	left: 0;
	min-width: 240px;
	background: var(--vp-c-bg-elv);
	border: 1px solid var(--vp-c-divider);
	border-radius: 8px;
	overflow: hidden;
	z-index: 100;
	box-shadow: 0 8px 16px rgba(0, 0, 0, 0.15);
	opacity: 1;
	pointer-events: auto;
	position-anchor: --markdown-copy-dropdown-trigger;
	position-area: end span-end;
	padding: 0;
	margin-block: 4px;

	@starting-style {
		transform: translateY(-6px) scale(0.96);
		opacity: 0;
	}

	&:not(:popover-open) {
		transform: translateY(-6px) scale(0.96);
		opacity: 0;
	}
}

.dropdown-item {
	position: relative;
	width: 100%;
	display: flex;
	align-items: center;
	gap: 10px;
	padding: 10px 16px;
	background: transparent;
	border: none;
	color: var(--vp-c-text-1);
	font-size: 14px;
	cursor: pointer;
	text-align: left;

	.icon.external {
		margin-left: auto !important;
		opacity: 0.6;
	}
}

.icon,
.button :deep(.icon) {
	&,
	& :deep(object),
	& :deep(.social-icon-only span) {
		width: 18px !important;
		height: 18px !important;
		margin: 0 !important;
		display: block;
	}
}

.chevron {
	transform: rotate(90deg);

	.markdown-copy-buttons:has(.dropdown-menu:popover-open) & {
		transform: rotate(270deg);
	}
}

.dropdown-item:hover .icon.external {
	opacity: 1;
	transform: translateX(2px);
}

@media (prefers-reduced-motion: no-preference) {
	.dropdown-menu {
		transition:
			opacity cubic-bezier(0.4, 0, 0.2, 1),
			transform cubic-bezier(0.4, 0, 0.2, 1),
			display;
		transition-duration: 0.18s;
		transition-behavior: allow-discrete;
		transform-origin: top;
	}

	/* Hover zones */
	.chevron-wrapper:hover,
	.button:hover {
		background: var(--vp-c-bg-soft);
	}

	.button {
		transition:
			all 0.25s cubic-bezier(0.4, 0, 0.2, 1),
			padding 0s;
	}

	.button-group,
	.chevron-wrapper,
	.dropdown-item,
	.dropdown-item .icon.external,
	.download-btn {
		transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
	}

	.button-group:hover,
	.button:not(.button-group *):hover {
		border-color: var(--vp-c-brand-1);
		transform: translateY(-1px);
		box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
	}

	.dropdown-item {
		&::before {
			content: "";
			position: absolute;
			left: 0;
			top: 0;
			width: 0;
			height: 100%;
			background: var(--vp-c-brand-1);
			transition: width 0.2s cubic-bezier(0.4, 0, 0.2, 1);
		}

		&:hover {
			padding-left: 20px;

			&::before {
				width: 3px;
			}
		}

		&:active {
			background-color: var(--vp-c-bg-alt);
		}
	}

	.chevron {
		transition: transform 0.25s cubic-bezier(0.4, 0, 0.2, 1);
	}

	.divider {
		transition: opacity 250ms;
	}
}
</style>
