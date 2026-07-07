import { useData } from "vitepress";
import { computed } from "vue";

export default function useI18n() {
	const { lang } = useData();

	return function t(object: Record<string, string>) {
		return computed(() => {
			let key = lang.value;
			if (key === "zh-CN") key = "zh";
			return key in object ? object[key] : object.en;
		});
	};
}
