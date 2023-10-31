/* eslint-disable quote-props */
import react from "@vitejs/plugin-react";
import path from "path";
import autoImport from "unplugin-auto-import/vite";
import { defineConfig } from "vite";
import { createSvgIconsPlugin } from "vite-plugin-svg-icons";
import svgr from "vite-plugin-svgr";
import tsconfigPaths from "vite-tsconfig-paths";

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
						"createContext",
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
						"SwitchTransition",
						"TransitionGroup",
						"Transition",
					],
					"react-i18next": [
						"useTranslation",
					],
					"zustand": [
						["create", "createStore"],
					],
					"zustand/middleware": [
						"persist",
					],
				},
			],
			dirs: [
				"./src/components/**",
				"./src/composables/**",
				"./src/utils/**",
				"./src/hooks/**",
				"./src/contexts/**",
				"./src/stores/**",
				"./src/hoc/**",
			],
			dts: "./src/types/auto-imports.d.ts",
			defaultExportByFilename: false,
		}),
		createSvgIconsPlugin({
			iconDirs: [path.resolve(process.cwd(), "src/assets/icons")],
			symbolId: "icon-[dir]-[name]",
		}),
		svgr({
			include: "**/*.svg",
		}),
		tsconfigPaths(),
	],
	build: {
		rollupOptions: {
			output: {
				entryFileNames: "assets/[name].js",
				chunkFileNames: "assets/[name].js",
				assetFileNames: "assets/[name].[ext]",
			},
		},
	},
});
