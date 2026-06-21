// https://vitepress.dev/guide/custom-theme
import { h } from "vue";
import type { Theme } from "vitepress";
import DefaultTheme from "vitepress/theme-without-fonts";
import MyLayout from "./Layout.vue";
import "./style.css";

export default {
	extends: DefaultTheme,
	Layout: MyLayout,
	enhanceApp({ app, router, siteData }) {
		// ...
	},
} satisfies Theme;
