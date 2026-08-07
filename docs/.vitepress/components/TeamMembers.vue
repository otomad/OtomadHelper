<script setup lang="ts">
	import { useI18n } from "@vp/use-i18n";
	import { VPTeamPage, VPTeamPageTitle, VPTeamMembers } from "vitepress/theme-without-fonts";
	const t = useI18n();

	const coreMembers = [
		{
			avatar: "https://www.github.com/otomad.png",
			name: t({ en: "otomad", zh: "兰音" }),
			title: t({ en: "Author", zh: "作者" }),
			links: [
				{ icon: "github", link: "https://github.com/otomad" },
				{ icon: "youtube", link: "https://youtube.com/@cmosekil" },
				{ icon: "bilibili", link: "https://space.bilibili.com/38207429" },
				{ icon: "niconico", link: "https://nicovideo.jp/user/132958059" },
				{ icon: "x", link: "https://x.com/3Qvery" },
			],
		},
		{
			avatar: "https://www.github.com/Chaosinism.png",
			name: "Chaosinism",
			title: t({ en: "Original Author", zh: "原作者" }),
			links: [
				{ icon: "github", link: "https://github.com/chsh2/" }, // https://github.com/Chaosinism
				{ icon: "youtube", link: "https://youtube.com/@chaosinism9491" },
				{ icon: "bilibili", link: "https://space.bilibili.com/89358" },
				{ icon: "niconico", link: "https://nicovideo.jp/user/31807351" },
				{ icon: "x", link: "https://x.com/chaosinism" },
				{ icon: "bluesky", link: "https://bsky.app/profile/chsh2.bsky.social" },
				{ icon: "mastodon", link: "https://mastodon.social/@chsh2" },
			],
		},
		{
			avatar: "https://www.github.com/Evauation.png",
			name: "Evauation",
			title: t({ en: "Documentation Writer", zh: "文档撰写者" }),
			links: [
				{ icon: "github", link: "https://github.com/Evauation" },
				{ icon: "youtube", link: "https://youtube.com/@Evauation" },
				{ icon: "x", link: "https://x.com/Evauation" },
				{ icon: "twitch", link: "https://twitch.tv/evauation" },
				{ icon: "reddit", link: "https://reddit.com/u/Evauation" },
			],
		},
		{
			avatar: "https://www.github.com/zzzzzz9125.png",
			name: "zzzzzz9125",
			title: t({ en: "Collaborator", zh: "协助者" }),
			links: [
				{ icon: "github", link: "https://github.com/zzzzzz9125" },
				{ icon: "youtube", link: "https://youtube.com/@zzzzzz9125" },
				{ icon: "bilibili", link: "https://space.bilibili.com/12355745" },
				{ icon: "x", link: "https://x.com/zzzzzz9125" },
			],
		},
		{
			avatar: "https://www.github.com/wcz123694578.png",
			name: t({ en: "wcz", zh: "吴从周" }),
			title: t({ en: "Collaborator", zh: "协助者" }),
			links: [
				{ icon: "github", link: "https://github.com/wcz123694578" },
				{ icon: "bilibili", link: "https://space.bilibili.com/34747251" },
			],
		},
		{
			avatar: "https://www.github.com/cyahega.png",
			name: "Cyahega",
			title: t({ en: "Vietnamese Translator", zh: "越南语翻译" }),
			links: [
				{ icon: "github", link: "https://github.com/cyahega" },
				{ icon: "youtube", link: "https://youtube.com/@Cyahegaotomad" },
				{ icon: "bilibili", link: "https://space.bilibili.com/2008986724" },
				{ icon: "niconico", link: "https://nicovideo.jp/user/98626014" },
				{ icon: "x", link: "https://x.com/cyahega" },
				{ icon: "facebook", link: "https://facebook.com/cyahegakawaii" },
			],
		},
		{
			avatar: "https://github.com/AdeGimank123.png",
			name: "AdeGimank123",
			title: t({ en: "Indonesian Translator", zh: "印尼语翻译" }),
			links: [{ icon: "github", link: "https://github.com/AdeGimank123" }],
		},
		{
			avatar: "https://github.com/JuJunG.png",
			name: "JuJunG",
			title: t({ en: "Indonesian Translator", zh: "印尼语翻译" }),
			links: [
				{ icon: "youtube", link: "https://youtube.com/@jujungamers" },
				{ icon: "niconico", link: "https://nicovideo.jp/user/126633391" },
				{ icon: "x", link: "https://x.com/JujunGamers_" },
			],
		},
	];

	const title = t({ en: "Contributors", zh: "贡献者" });
</script>

<template>
	<VPTeamPage>
		<VPTeamPageTitle>
			<template #title>{{ title }}</template>
		</VPTeamPageTitle>
		<VPTeamMembers size="small" :members="coreMembers" />
	</VPTeamPage>
</template>
