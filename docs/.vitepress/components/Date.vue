<script setup lang="ts">
import { computed } from "vue";
import { useData } from "vitepress";

const props = defineProps<{
	value: string;
}>();

const { lang } = useData();

const standardDateString = computed(() => {
	const [year, month, day] = props.value.split(/[\/\-\.]/).map(num => num.padStart(2, "0"));
	return `${year}-${month}-${day}`;
});

const formattedDate = computed(() => {
	const date = new Date(standardDateString.value);
	console.log("​ ​ lang​", lang);
	return new Intl.DateTimeFormat(lang.value, {
		year: "numeric",
		month: "2-digit",
		day: "2-digit",
		timeZone: "UTC",
	}).format(date);
});
</script>

<template>
	<time :datetime="standardDateString">{{ formattedDate }}</time>
</template>

<style scoped>
time {
	font-variant-numeric: tabular-nums;
}
</style>
