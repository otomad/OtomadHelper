/* eslint-disable quote-props */
import react from "@vitejs/plugin-react";
import path from "path";
import autoImport from "unplugin-auto-import/vite";
import { defineConfig } from "vite";
import { createSvgIconsPlugin } from "vite-plugin-svg-icons";
import svgr from "vite-plugin-svgr";

// https://vitejs.dev/config/
export default defineConfig({
	plugins: [
		react({
			babel: {
				plugins: [
					[
						"babel-plugin-styled-components",
						{
							ssr: false,
							displayName: true,
							fileName: false,
							minify: true,
							pure: false,
						},
					],
				],
			},
		}),
		autoImport({
			imports: [
				"react",
				{
					"react": [
						["default", "React"],
					],
					"react-dom/client": [
						["*", "ReactDOM"],
					],
					"styled-components": [
						"styled",
						"keyframes",
						"css",
						"createGlobalStyle",
						"isStyledComponent",
					],
					"classnames": [
						["default", "classNames"],
					],
					"react-transition-group": [
						["CSSTransition", "Transition"],
						"SwitchTransition",
						"TransitionGroup",
					],
					"react-i18next": [
						"useTranslation",
					],
				},
			],
			dirs: [
				"./src/components/**",
				"./src/composables/**",
				"./src/utils/**",
			],
			dts: "./src/types/auto-imports.d.ts",
		}),
		createSvgIconsPlugin({
			iconDirs: [path.resolve(process.cwd(), "src/assets/icons")],
			symbolId: "icon-[dir]-[name]",
		}),
		svgr({
			include: "**/*.svg",
		}),
	],
	resolve: {
		alias: {
			"@": path.resolve(__dirname, "./src"),
			"assets": path.resolve(__dirname, "./src/assets"),
			"components": path.resolve(__dirname, "./src/components"),
			"pages": path.resolve(__dirname, "./src/pages"),
			"styles": path.resolve(__dirname, "./src/styles"),
			"types": path.resolve(__dirname, "./src/types"),
			"utils": path.resolve(__dirname, "./src/utils"),
			"locales": path.resolve(__dirname, "./src/locales"),
			"stores": path.resolve(__dirname, "./src/stores"),
		},
	},
});
