<script setup lang="tsx">
	// See: https://github.com/okineadev/vitepress-plugin-llms/blob/main/src/vitepress-components/CopyOrDownloadAsMarkdownButtons.vue
	import iconCheck from "@vp/theme/icons/check.svg?raw";
	import iconChevron from "@vp/theme/icons/chevron.svg?raw";
	import iconCopy from "@vp/theme/icons/copy.svg?raw";
	import iconDownload from "@vp/theme/icons/download.svg?raw";
	import iconEllipsis from "@vp/theme/icons/ellipsis.svg?raw";
	import iconExternal from "@vp/theme/icons/external.svg?raw";
	import iconMarkdown from "@vp/theme/icons/markdown.svg?raw";
	import iconPause from "@vp/theme/icons/pause.svg?raw";
	import iconPlay from "@vp/theme/icons/play.svg?raw";
	import iconPrint from "@vp/theme/icons/print.svg?raw";
	import iconRss from "@vp/theme/icons/rss.svg?raw";
	import iconShare from "@vp/theme/icons/share.svg?raw";
	import iconStop from "@vp/theme/icons/stop.svg?raw";

	import { getRssFeedLink } from "@vp/plugins/rss-feed_get-link";
	import { useI18n } from "@vp/use-i18n";
	import { useData } from "vitepress";
	import { reactive, onMounted, ref, useTemplateRef } from "vue";
	const t = useI18n();
	const data = useData();

	const labels = reactive({
		print: t({ en: "Print/Save as PDF", zh: "打印/保存为PDF" }),
		viewMd: t({ en: "View as Markdown", zh: "查看Markdown" }),
		copyMd: t({ en: "Copy page as Markdown", zh: "复制全文为Markdown" }),
		copying: t({ en: "Copying…", zh: "复制中⋯⋯" }),
		copied: t({ en: "Copied!", zh: "已复制！" }),
		downloadMd: t({ en: "Download Markdown", zh: "下载Markdown" }),
		chatGpt: t({ en: "Open in ChatGPT", zh: "在ChatGPT中打开" }),
		claude: t({ en: "Open in Claude", zh: "在Claude中打开" }),
		rss: t({ en: "RSS Feed", zh: "RSS订阅" }),
		share: t({ en: "Share", zh: "分享" }),
		read: t({ en: "Read Aloud", zh: "大声朗读" }),
		pauseRead: t({ en: "Pause Reading", zh: "暂停朗读" }),
		resumeRead: t({ en: "Resume Reading", zh: "继续朗读" }),
		stopRead: t({ en: "Stop Reading", zh: "停止朗读" }),
	});

	const dropdownMenu = useTemplateRef("dropdown-menu");
	const buttonsInner = useTemplateRef("markdown-copy-buttons-inner");
	const closeDropdownMenu = () => dropdownMenu.value?.hidePopover?.();

	const print = () => window.print();

	onMounted(() => {
		const { classList } = document.documentElement;
		window.onbeforeprint = event => {
			classList.add("locale-changing");
			classList.remove("dark");
		};
		window.onafterprint = async event => {
			if (data.isDark.value) classList.add("dark");
			await new Promise(resolve => requestAnimationFrame(resolve));
			classList.remove("locale-changing");
		};
	});

	function getMarkdownLink() {
		const { href } = location;
		if (href.endsWith(".html")) return href.slice(0, -5) + ".md";
		else if (href.endsWith("/")) return href + "index.md";
		else return;
	}

	const viewAsMarkdown = () => {
		const markdown = getMarkdownLink();
		if (markdown) window.open(markdown);
	};

	const isCopyingMarkdown = ref<false | null | true>(false);
	const copiedMarkdownTimeoutId = ref<NodeJS.Timeout>();
	const copiedMarkdownCloseDropdownTimeoutId = ref<NodeJS.Timeout>();
	const copyAsMarkdown = async () => {
		if (isCopyingMarkdown.value) return;
		clearTimeout(copiedMarkdownTimeoutId.value);
		clearTimeout(copiedMarkdownCloseDropdownTimeoutId.value);
		isCopyingMarkdown.value = false;
		const markdown = getMarkdownLink();
		if (!markdown) return;
		isCopyingMarkdown.value = true;
		const content = await fetch(markdown).then(response => response.text());
		await navigator.clipboard.writeText(content);
		isCopyingMarkdown.value = null;
		copiedMarkdownTimeoutId.value = setTimeout(() => {
			isCopyingMarkdown.value = false;
		}, 2000);
		copiedMarkdownCloseDropdownTimeoutId.value = setTimeout(() => {
			closeDropdownMenu();
		}, 1500);
	};

	const downloadMarkdown = () => {
		closeDropdownMenu();
		const markdown = getMarkdownLink();
		if (!markdown) return;
		const a = document.createElement("a");
		a.download = document.title.replaceAll("|", "-");
		a.href = markdown;
		document.body.append(a);
		a.click();
		a.remove();
	};

	const openInAi = (provider: string) => {
		closeDropdownMenu();
		const markdown = getMarkdownLink();
		const defaultAiProviders = {
			ChatGPT: "https://chatgpt.com/?hints=search&prompt=",
			Claude: "https://claude.ai/new?q=",
		} as const;
		if (!markdown || !(provider in defaultAiProviders)) return;
		const prompt = t({
			en: `Read from ${markdown} so I can ask questions about it.`,
			zh: `请阅读 ${markdown} ，以便我可以提出相关问题。`,
		});
		window.open(
			defaultAiProviders[provider as keyof typeof defaultAiProviders] + encodeURIComponent(prompt.value),
			"_blank",
		);
	};

	const rssFeed = () => {
		const lang = data.lang.value;
		const link = getRssFeedLink(lang);
		if (link) window.open(link);
	};

	const share = async () => {
		await navigator.share?.({
			title: document.title,
			url: location.href,
		});
	};

	const isSpeaking = ref<false | null | true>(false);
	const speak = () => {
		const post = document.querySelector(".vp-doc > div") as HTMLDivElement;
		const utterance = new SpeechSynthesisUtterance(post.innerText);
		const voices = speechSynthesis.getVoices();
		const locale = new Intl.Locale(document.documentElement.lang).maximize();
		const getLocale = (lang: string) => new Intl.Locale(lang).maximize();
		const preferredVoice =
			voices.find(
				({ lang, localService }) => getLocale(lang).toString() === locale.toString() && !localService,
			) ??
			voices.find(({ lang, localService }) => getLocale(lang).language === locale.language && !localService) ??
			voices.find(({ lang, localService }) => getLocale(lang).toString() === locale.toString()) ??
			voices.find(({ lang, localService }) => getLocale(lang).language === locale.language);
		if (preferredVoice) utterance.voice = preferredVoice;
		// 本来想用随机语音，但容易随机到会失败的语音，建议还是用首选语音了。
		// let preferredVoices = voices.filter(({ lang, localService }) => getLocale(lang).toString() === locale.toString() && !localService);
		// if (!preferredVoices.length) preferredVoices = voices.filter(({ lang, localService }) => getLocale(lang).language === locale.language && !localService);
		// if (!preferredVoices.length) preferredVoices = voices.filter(({ lang }) => getLocale(lang).toString() === locale.toString());
		// if (!preferredVoices.length) preferredVoices = voices.filter(({ lang }) => getLocale(lang).language === locale.language);
		// if (preferredVoices.length)
		// 	utterance.voice = preferredVoices[Math.random() * preferredVoices.length | 0];
		utterance.onend = () => stopSpeak();
		utterance.onerror = e => console.error(e);
		speechSynthesis.speak(utterance);
		buttonsInner.value?.startViewTransition(() => (isSpeaking.value = true));
	};
	const pauseSpeak = () => {
		if (isSpeaking.value) speechSynthesis.pause();
		else speechSynthesis.resume();
		isSpeaking.value = isSpeaking.value ? null : true;
	};
	const stopSpeak = () => {
		speechSynthesis.cancel();
		buttonsInner.value?.startViewTransition(() => (isSpeaking.value = false));
	};
	document.startViewTransition;
</script>

<script lang="tsx">
	import { defineComponent, useId } from "vue";
	import { inBrowser } from "vitepress";

	// Polyfill start view transition.
	if (inBrowser) Node.prototype.startViewTransition ??= fn => { fn?.(); return {}; };

	const InnerButton = defineComponent({
		props: {
			icon: String,
			name: String,
		},
		setup(props) {
			const id = useId();
			return () => (
				<button type="button" class="button" aria-label={props.name} interestfor={id}>
					<span class="icon" innerHTML={props.icon}></span>
					<div role="tooltip" class="tooltip" popover="hint" id={id}>{props.name}</div>
				</button>
			);
		}
	});
</script>

<template>
	<div class="markdown-copy-buttons">
		<div class="markdown-copy-buttons-pretend-content">
			<div class="markdown-copy-buttons-inner" ref="markdown-copy-buttons-inner">
				<InnerButton :name="labels.print" :icon="iconPrint" @click="print()" />

				<!-- Markdown button -->
				<div class="button-group dropdown-trigger">
					<InnerButton :name="labels.viewMd" :icon="iconMarkdown" @click="viewAsMarkdown()" />

					<span class="divider"></span>

					<!-- Chevron area -->
					<button class="chevron-wrapper" popovertarget="markdown-copy-menu">
						<span v-html="iconChevron" class="icon chevron"></span>
					</button>
				</div>

				<InnerButton :name="labels.rss" :icon="iconRss" @click="rssFeed()" />

				<InnerButton :name="labels.share" :icon="iconShare" @click="share()" />

				<InnerButton
					v-if="isSpeaking === false"
					:name="labels.read"
					:icon="iconPlay"
					class="speak-button"
					@click="speak()"
				/>

				<div v-else class="button-group speak-button">
					<InnerButton
						:name="isSpeaking ? labels.pauseRead : labels.resumeRead"
						:icon="isSpeaking ? iconPause : iconPlay"
						@click="pauseSpeak()"
					/>
					<span class="divider"></span>
					<InnerButton :name="labels.stopRead" :icon="iconStop" @click="stopSpeak()" />
				</div>
			</div>
		</div>

		<!-- Markdown Dropdown -->
		<div class="dropdown-menu" popover="auto" id="markdown-copy-menu" ref="dropdown-menu">
			<button class="dropdown-item" @click="copyAsMarkdown()" :disabled="!!isCopyingMarkdown">
				<span
					v-html="isCopyingMarkdown ? iconEllipsis : isCopyingMarkdown === null ? iconCheck : iconCopy"
					class="icon"
				></span>
				{{ isCopyingMarkdown ? labels.copying : isCopyingMarkdown === null ? labels.copied : labels.copyMd }}
			</button>
			<button class="dropdown-item" @click="downloadMarkdown()">
				<span v-html="iconDownload" class="icon"></span>
				{{ labels.downloadMd }}
			</button>
			<button class="dropdown-item" @click="openInAi('ChatGPT')">
				<span class="icon"><SocialIcon icon="openai" /></span>
				{{ labels.chatGpt }}
				<span v-html="iconExternal" class="icon external"></span>
			</button>
			<button class="dropdown-item" @click="openInAi('Claude')">
				<span class="icon"><SocialIcon icon="claude" /></span>
				{{ labels.claude }}
				<span v-html="iconExternal" class="icon external"></span>
			</button>
		</div>
	</div>
</template>

<style scoped>
	.markdown-copy-buttons {
		display: flex;
		margin-block: -28px 8px;
		view-transition-name: access-article-buttons;

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
		interest-delay: 0s normal;

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

	.button :deep(.tooltip) {
		position: fixed;
		position-area: bottom;
		position-try-fallbacks: flip-block;
		z-index: var(--vp-z-index-layout-top);
		color: var(--vp-c-text-1);
		border: 1px solid var(--vp-c-divider);
		border-radius: 6px;
		background: var(--vp-c-bg-soft);
		margin-block: 5px;
		padding: 3px 8px;
		transition: margin, opacity, display, overlay;
		transition-behavior: allow-discrete;
		transition-duration: 250ms;
		pointer-events: none;
		box-shadow: var(--vp-shadow-3);
		font-size: 14px;

		@starting-style {
			opacity: 0;
			margin: 0;
		}

		&:not(:popover-open) {
			opacity: 0;
			margin: 0;
		}

		@media (any-hover: none) {
			display: none;
		}
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

	.icon:deep,
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
				display,
				overlay;
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

		.button-group:active,
		.button:not(.button-group *):active {
			border-color: var(--vp-c-brand-1);
			transform: none;
			box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);
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

		.speak-button {
			view-transition-name: speak-button;

			:root:active-view-transition & {
				view-transition-name: none;
			}
		}

		:global(::view-transition-old(speak-button)),
		:global(::view-transition-new(speak-button)) {
			width: 100%;
			height: 100%;
		}
	}
</style>
