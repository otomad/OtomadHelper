<script setup lang="ts">
import { VPTeamPage, VPTeamPageTitle, VPTeamMembers, VPTeamPageSection } from "vitepress/theme";

const props = withDefaults(defineProps<{
	lang: string;
}>(), {
	lang: "en",
});

function get(object) {
	let key = props.lang;
	if (key === "zh-CN") key = "zhs";
	return key in object ? object[key] : object.en;
}

const coreMembers = [
	{
		avatar: "https://www.github.com/otomad.png",
		name: get({ en: "otomad", zhs: "兰音" }),
		title: get({ en: "Author", zhs: "作者" }),
		links: [
			{ icon: "github", link: "https://github.com/otomad" },
			{ icon: "youtube", link: "https://youtube.com/@cmosekil" },
			{ icon: "bilibili", link: "https://space.bilibili.com/38207429" },
		],
	},
	{
		avatar: "https://www.github.com/Chaosinism.png",
		name: "Chaosinism",
		title: get({ en: "Original Author", zhs: "原作者" }),
		links: [
			{ icon: "github", link: "https://github.com/Chaosinism" },
		],
	},
	{
		avatar: "https://www.github.com/Evauation.png",
		name: "Evauation",
		title: get({ en: "Collaborator", zhs: "协助者" }),
		links: [
			{ icon: "github", link: "https://github.com/Evauation" },
		],
	},
	{
		avatar: "https://www.github.com/zzzzzz9125.png",
		name: "zzzzzz9125",
		title: get({ en: "Collaborator", zhs: "协助者" }),
		links: [
			{ icon: "github", link: "https://github.com/zzzzzz9125" },
		],
	},
	{
		avatar: "https://www.github.com/wcz123694578.png",
		name: get({ en: "wcz", zhs: "吴从周" }),
		title: get({ en: "Collaborator", zhs: "协助者" }),
		links: [
			{ icon: "github", link: "https://github.com/wcz123694578" },
		],
	},
	{
		avatar: "https://www.github.com/cyahega.png",
		name: "Cyahega",
		title: get({ en: "Vietnamese Translator", zhs: "越南语翻译" }),
		links: [
			{ icon: "github", link: "https://github.com/cyahega" },
		],
	},
];

const title = get({ en: "Contributors", zhs: "贡献者" });
</script>

<template>
	<VPTeamPage>
		<VPTeamPageTitle>
			<template #title>{{ title }}</template>
		</VPTeamPageTitle>
		<VPTeamMembers size="small" :members="coreMembers" />
	</VPTeamPage>
</template>
