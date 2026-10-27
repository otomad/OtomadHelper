<script setup lang="ts">
	import { useI18n } from "@vp/use-i18n";
	import { onBeforeUnmount, onMounted, reactive, ref, type ComputedRef } from "vue";

	const t = useI18n();

	type Image = { light: ComputedRef<string>; dark: ComputedRef<string> };

	/**
	 * A single screenshot that looks the same in both themes.
	 */
	const single = (en: string, zh: string = en): Image => {
		const src = t({ en, zh });
		return { light: src, dark: src };
	};

	/**
	 * A screenshot with separate light & dark variants.
	 */
	const themed = (lightEn: string, lightZh: string, darkEn: string, darkZh: string): Image => ({
		light: t({ en: lightEn, zh: lightZh }),
		dark: t({ en: darkEn, zh: darkZh }),
	});

	const lead = reactive({
		title: t({
			en: "From MIDI to masterpiece, automatically",
			zh: "从 MIDI 到成品，全自动",
		}),
		sub: t({
			en: "Otomad Helper turns a score into a fully-arranged Vegas Pro timeline — audio, visuals, effects and everything in between, generated for you.",
			zh: "音MAD助手把一份乐谱变成铺满 Vegas Pro 时间轴的成品——音频、画面、效果，以及其间的所有细节，统统替你生成。",
		}),
	});

	const stats = reactive([
		{ value: "1", label: t({ en: "Click to generate", zh: "一键生成" }) },
		{ value: "14+", label: t({ en: "Dedicated tabs", zh: "专属页签" }) },
		{ value: "60+", label: t({ en: "Effects & moshes", zh: "特效与抹失" }) },
		{ value: "∞", label: t({ en: "Possibilities", zh: "无限可能" }) },
	]);

	const sections = reactive([
		{
			tag: t({ en: "Score", zh: "乐曲" }),
			title: t({ en: "MIDI in, tracks out", zh: "乐谱进，轨道出" }),
			desc: t({
				en: "Feed Otomad Helper any MIDI sequence and it lays out every note onto the timeline — trimmed, synced and tuned, ready to render.",
				zh: "给音MAD助手任意一份 MIDI，它就把每个音符铺到时间轴上——修剪、对拍、调音，一步到位，随时可以渲染。",
			}),
			points: [
				t({ en: "Import MIDI from any DAW, FL Studio included", zh: "从任意 DAW（含 FL Studio）导入 MIDI" }),
				t({ en: "Adaptive BPM, time signatures & variable tempo", zh: "自适应 BPM、拍号与变速" }),
				t({ en: "Pick exactly which tracks and notes to generate", zh: "精确选择要生成的音轨与音符" }),
			],
			image: themed(
				"/img/overview_light.png",
				"/img/overview_light_zh-CN.png",
				"/img/overview_dark.png",
				"/img/overview_dark_zh-CN.png",
			),
		},
		{
			tag: t({ en: "Audio", zh: "音频" }),
			title: t({ en: "Studio-grade tuning", zh: "录音室级调音" }),
			desc: t({
				en: "Pitch, stretch, formant and pan — sculpt every sample with surgical precision until it sounds exactly right.",
				zh: "音高、伸缩、共振峰与声像——以手术刀般的精度雕琢每一条采样，直到它听起来恰到好处。",
			}),
			points: [
				t({ en: "Elastic pitch, formant lock & granular oscillator", zh: "弹性变调、共振峰锁定与粒子振荡器" }),
				t({ en: "Map note velocity to gain, volume & pan", zh: "把音符力度映射为增益、音量与声像" }),
				t({ en: "Auto-pan with envelope automation", zh: "用包络自动化实现自动声像" }),
			],
			image: single("/img/v4/audio/tuning.png", "/img/v4/audio/tuning_zh-CN.png"),
		},
		{
			tag: t({ en: "Visual", zh: "画面" }),
			title: t({ en: "Motion graphics on autopilot", zh: "动态图形自动挡" }),
			desc: t({
				en: "Flip, rotate, scale, mirror, hue — a whole choreography of visuals driven by your notes, not your mouse.",
				zh: "翻转、旋转、缩放、镜像、色相——由音符而非鼠标驱动的一整套画面编舞。",
			}),
			points: [
				t({ en: "13+ effect classes with dynamic keyframing", zh: "13+ 类效果与动态关键帧" }),
				t({ en: "Color grading, glow & transform controls", zh: "调色、发光与变换控制" }),
				t({ en: "Sonar: beat-synced visuals for drums", zh: "声呐：为鼓组打造的节拍同步视觉" }),
			],
			image: single("/img/v4/visual/effects.png", "/img/v4/visual/effects_zh-CN.png"),
		},
		{
			tag: t({ en: "Source", zh: "素材" }),
			title: t({ en: "An arsenal of sources", zh: "素材的军火库" }),
			desc: t({
				en: "Mix and match any number of clips — let the Mystery Box surprise you, or orchestrate them like an ensemble.",
				zh: "混搭任意数量的剪辑——让“素材盲盒”给你惊喜，或像指挥乐团一样编排它们。",
			}),
			points: [
				t({ en: "Mystery Box & Source Syncopator", zh: "素材盲盒与踩点" }),
				t({ en: "Source Orchestra: one clip per MIDI track", zh: "素材乐团：每条音轨一个剪辑" }),
				t({ en: "Switch per beat, per marker or per round", zh: "按拍、按标记、按轮次切换" }),
			],
			image: single("/img/v4/source/comb.png", "/img/v4/source/comb_zh-CN.png"),
		},
		{
			tag: t({ en: "Tools", zh: "工具" }),
			title: t({ en: "A tool for every chore", zh: "面面俱到的工具" }),
			desc: t({
				en: "Grids, 3D boxes, gradients, subtitles, legato — the tedious parts of editing, reduced to a click.",
				zh: "网格、3D 方盒、渐变、字幕、填补间隙——剪辑中的繁琐工序，一键搞定。",
			}),
			points: [
				t({ en: "Auto layout: grid, 3D box & gradient tracks", zh: "自动布局：网格、3D 方盒与渐变轨道" }),
				t({ en: "Batch subtitle generation & quick-select", zh: "批量字幕生成与快速选择" }),
				t({ en: "Replace events, custom fades & MIDI export", zh: "替换剪辑、自定淡化与 MIDI 导出" }),
			],
			image: single("/img/v4/tools/auto_layout_tracks.png", "/img/v4/tools/auto_layout_tracks_zh-CN.png"),
		},
		{
			tag: t({ en: "Moshes & YTP", zh: "抹失与 YTP" }),
			title: t({ en: "Glitch, stutter & poop", zh: "故障、结巴与鬼畜" }),
			desc: t({
				en: "Datamosh, scramble, stutter, shake — plus the full classic YTP arsenal of chorus, vibrato and color carnage.",
				zh: "数据抹失、随机扰乱、结巴、镜头摇晃——外加合唱、颤音与色彩暴走的完整 YTP 军火库。",
			}),
			points: [
				t({ en: "One-click datamosh & datamix", zh: "一键数据抹失与抹拭" }),
				t({ en: "Scramble, stutter, shake & automator", zh: "扰乱、结巴、摇晃与自动乱调" }),
				t({ en: "18+ classic YTP effects", zh: "18+ 经典 YTP 效果" }),
			],
			image: single("/img/v4/moshes/datamosh.png", "/img/v4/moshes/datamosh_zh-CN.png"),
		},
	]);

	const cta = reactive({
		title: t({ en: "Ready to make something chaotic?", zh: "准备好大闹一场了吗？" }),
		text: t({
			en: "Download Otomad Helper and turn a MIDI file into a full YTPMV in minutes.",
			zh: "下载音MAD助手，几分钟内把一份 MIDI 变成完整的音MAD。",
		}),
		download: t({ en: "Download now", zh: "立即下载" }),
		docs: t({ en: "Read the docs", zh: "阅读文档" }),
	});

	const root = ref<HTMLElement | null>(null);
	let observer: IntersectionObserver | undefined;

	onMounted(() => {
		const el = root.value;
		if (!el) return;
		const items = el.querySelectorAll<HTMLElement>(".reveal");
		if (!("IntersectionObserver" in window) || window.matchMedia("(prefers-reduced-motion: reduce)").matches) {
			items.forEach(item => item.classList.add("visible"));
			return;
		}
		observer = new IntersectionObserver(
			entries => {
				for (const entry of entries) {
					if (entry.isIntersecting) {
						entry.target.classList.add("visible");
						observer!.unobserve(entry.target);
					}
				}
			},
			{ threshold: 0.15, rootMargin: "0px 0px -40px 0px" },
		);
		items.forEach(item => observer!.observe(item));
	});

	onBeforeUnmount(() => observer?.disconnect());
</script>

<template>
	<section ref="root" class="home-showcase">
		<header class="intro reveal">
			<h2 class="lead">{{ lead.title }}</h2>
			<p class="lead-sub">{{ lead.sub }}</p>
		</header>

		<ul class="stats">
			<li v-for="(stat, i) in stats" :key="i" class="stat reveal" :style="{ '--delay': `${i * 80}ms` }">
				<span class="stat-value">{{ stat.value }}</span>
				<span class="stat-label">{{ stat.label }}</span>
			</li>
		</ul>

		<div class="sections">
			<article v-for="(section, i) in sections" :key="i" class="row reveal" :class="{ reversed: i % 2 === 1 }">
				<div class="media">
					<img class="light shadow-less" :src="section.image.light" :alt="section.title" loading="lazy" />
					<img class="dark shadow-less" :src="section.image.dark" :alt="section.title" loading="lazy" />
				</div>
				<div class="body">
					<span class="tag">{{ section.tag }}</span>
					<h3 class="title">{{ section.title }}</h3>
					<p class="desc">{{ section.desc }}</p>
					<ul class="points">
						<li v-for="(point, j) in section.points" :key="j">{{ point }}</li>
					</ul>
				</div>
			</article>
		</div>

		<div class="cta reveal">
			<h2>{{ cta.title }}</h2>
			<p>{{ cta.text }}</p>
			<div class="actions">
				<a
					class="btn primary"
					href="https://github.com/otomad/OtomadHelper/releases/latest"
					target="_blank"
					rel="noopener"
				>
					{{ cta.download }}
				</a>
				<a class="btn secondary" href="./introduction">{{ cta.docs }}</a>
			</div>
		</div>
	</section>
</template>

<style scoped>
	.home-showcase {
		max-width: 1152px;
		margin-inline: auto;
		padding-block: 3rem 0;
	}

	/* Intro */
	.home-showcase .intro {
		text-align: center;
		max-width: 44rem;
		margin-inline: auto;
	}

	.home-showcase .lead {
		margin: 0;
		background: linear-gradient(120deg, #fb7299, #2fbdec);
		-webkit-background-clip: text;
		background-clip: text;
		color: transparent;
		font-size: 2rem;
		line-height: 1.2;
		letter-spacing: -0.02em;
	}

	.home-showcase .lead-sub {
		margin-block-start: 1rem;
		margin-block-end: 0;
		color: var(--vp-c-text-2);
		font-size: 1.05rem;
		line-height: 1.7;
	}

	/* Stats */
	.home-showcase .stats {
		list-style: none;
		margin: 3rem 0 0;
		padding: 0;
		display: grid;
		grid-template-columns: repeat(2, 1fr);
		gap: 1rem;

		.stat {
			margin: 0;
		}
	}

	@media (min-width: 640px) {
		.home-showcase .stats {
			grid-template-columns: repeat(4, 1fr);
		}
	}

	.home-showcase .stat {
		display: flex;
		flex-direction: column;
		align-items: center;
		gap: 0.25rem;
		padding: 1.5rem 1rem;
		border: 1px solid var(--vp-c-divider);
		border-radius: 12px;
		background: var(--vp-c-bg-soft);
	}

	.home-showcase .stat-value {
		font-size: 2.25rem;
		font-weight: 700;
		line-height: 1;
		background: linear-gradient(120deg, #fb7299, #2fbdec);
		-webkit-background-clip: text;
		background-clip: text;
		color: transparent;
	}

	.home-showcase .stat-label {
		color: var(--vp-c-text-2);
		font-size: 0.9rem;
	}

	/* Sections */
	.home-showcase .sections {
		margin-block-start: 4rem;
		display: grid;
		gap: 4rem;
	}

	.home-showcase .row {
		display: grid;
		grid-template-columns: 1fr;
		gap: 1.5rem;
		align-items: center;
	}

	@media (min-width: 800px) {
		.home-showcase .row {
			grid-template-columns: 1fr 1fr;
			gap: 3rem;
		}

		.home-showcase .row.reversed .media {
			order: 2;
		}

		.home-showcase .row.reversed .body {
			order: 1;
		}
	}

	.home-showcase .media {
		position: relative;
		border-radius: 12px;
		overflow: hidden;
		border: 1px solid var(--vp-c-divider);
		background: var(--vp-c-bg-soft);
		box-shadow: var(--vp-shadow-2);
	}

	.home-showcase .media img {
		display: block;
		width: 100%;
		height: auto;
		max-height: none;
		border-radius: 0;
	}

	.home-showcase .tag {
		display: inline-block;
		padding: 0.25rem 0.75rem;
		border-radius: 999px;
		font-size: 0.78rem;
		font-weight: 600;
		letter-spacing: 0.02em;
		color: var(--vp-c-brand-1);
		background: var(--vp-c-brand-soft);
	}

	.home-showcase .title {
		margin: 0.75rem 0 0;
		border-top: none;
		padding-top: 0;
		font-size: 1.6rem;
		line-height: 1.3;
		letter-spacing: -0.01em;
	}

	.home-showcase .desc {
		margin: 0.75rem 0 0;
		color: var(--vp-c-text-2);
		line-height: 1.7;
	}

	.home-showcase .points {
		list-style: none;
		margin: 1rem 0 0;
		padding: 0;
		display: grid;
		gap: 0.5rem;
	}

	.home-showcase .points li {
		position: relative;
		margin: 0;
		padding-inline-start: 1.4em;
		color: var(--vp-c-text-1);
	}

	.home-showcase .points li::before {
		content: "";
		position: absolute;
		inset-inline-start: 0;
		top: 0.55em;
		width: 0.5em;
		height: 0.5em;
		border-radius: 50%;
		background: linear-gradient(120deg, #fb7299, #2fbdec);
	}

	/* Call to action */
	.home-showcase .cta {
		margin-block-start: 5rem;
		text-align: center;
		padding: 3rem 1.5rem;
		border-radius: 16px;
		border: 1px solid var(--vp-c-divider);
		background: linear-gradient(180deg, var(--vp-c-bg-soft), transparent);
	}

	.home-showcase .cta h2 {
		margin: 0;
		border-top: none;
		padding-top: 0;
		font-size: 1.75rem;
		letter-spacing: -0.01em;
	}

	.home-showcase .cta p {
		margin: 0.75rem auto 0;
		max-width: 40rem;
		color: var(--vp-c-text-2);
		line-height: 1.7;
	}

	.home-showcase .actions {
		margin-block-start: 1.5rem;
		display: flex;
		flex-wrap: wrap;
		justify-content: center;
		gap: 0.75rem;
	}

	.home-showcase .btn {
		display: inline-flex;
		align-items: center;
		justify-content: center;
		padding: 0 1.5rem;
		height: 44px;
		border-radius: 8px;
		font-weight: 600;
		font-size: 0.95rem;
		text-decoration: none;
		border: 1px solid transparent;
		transition:
			transform 0.2s ease,
			background-color 0.2s ease,
			border-color 0.2s ease,
			color 0.2s ease;
	}

	.home-showcase .btn:hover {
		transform: translateY(-2px);
	}

	.home-showcase .btn:active {
		transform: translateY(0);
	}

	.home-showcase .btn.primary {
		background: var(--vp-c-brand-3);
		color: var(--vp-c-white);
	}

	.home-showcase .btn.primary:hover {
		background: var(--vp-c-brand-2);
	}

	.home-showcase .btn.secondary {
		background: var(--vp-c-bg-soft);
		border-color: var(--vp-c-divider);
		color: var(--vp-c-text-1);
	}

	.home-showcase .btn.secondary:hover {
		border-color: var(--vp-c-brand-1);
		color: var(--vp-c-brand-1);
	}

	/* Reveal-on-scroll */
	.home-showcase .reveal {
		opacity: 0;
		transform: translateY(28px);
		transition:
			opacity 0.7s cubic-bezier(0.22, 1, 0.36, 1),
			transform 0.7s cubic-bezier(0.22, 1, 0.36, 1);
		transition-delay: var(--delay, 0ms);
	}

	.home-showcase .reveal.visible {
		opacity: 1;
		transform: none;
	}

	@media (prefers-reduced-motion: reduce) {
		.home-showcase .reveal {
			opacity: 1;
			transform: none;
			transition: none;
		}
	}
</style>
