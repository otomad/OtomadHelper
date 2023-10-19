/* eslint-disable quote-props */
import react from "@vitejs/plugin-react-swc";
import autoImport from "unplugin-auto-import/vite";
import { defineConfig } from "vite";
// import babel from "vite-plugin-babel";

// https://vitejs.dev/config/
export default defineConfig({
	plugins: [
		// babel(),
		react(),
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
				},
			],
			dirs: [
				"./src/components/**",
				"./src/composables/**",
				"./src/utils/**",
			],
			dts: "./src/types/auto-imports.d.ts",
		}),
	],
});
