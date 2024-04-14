import react from "@vitejs/plugin-react";
import { transform as transformCSS } from "lightningcss";
import path from "path";
import autoImport from "unplugin-auto-import/vite";
import { defineConfig } from "vite";
import { compression } from "vite-plugin-compression2";
import htmlMinifier from "vite-plugin-html-minifier";
import { createSvgIconsPlugin } from "vite-plugin-svg-icons";
import svgr from "vite-plugin-svgr";
import tsconfigPaths from "vite-tsconfig-paths";
import autoImportConfig from "./auto-import.config";
import globalized from "./src/plugins/vite/globalized";

// https://vitejs.dev/config/
export default defineConfig({
	plugins: [
		react({
			babel: {
				plugins: [
					[
						"babel-plugin-styled-components",
						{
							ssr: true,
							displayName: true,
							fileName: false,
							minify: true,
							pure: false,
						},
					],
					"babel-plugin-transform-jsx-classnames",
				],
			},
		}),
		autoImport({
			imports: autoImportConfig,
			dirs: [
				"./src/components/**",
				"./src/composables/**",
				"./src/utils/**",
				"./src/hooks/**",
				"./src/contexts/**",
				"./src/stores/**",
				"./src/hoc/**",
				"./src/classes/**",
			],
			dts: "./src/types/auto-imports.d.ts",
			defaultExportByFilename: false,
		}),
		globalized(),
		createSvgIconsPlugin({
			iconDirs: [path.resolve(process.cwd(), "src/assets/icons")],
			symbolId: "icon-[dir]-[name]",
		}),
		svgr({
			include: "**/*.svg",
		}),
		tsconfigPaths(),
		htmlMinifier({
			minify: {
				collapseWhitespace: true,
				keepClosingSlash: false,
				removeComments: true,
				removeRedundantAttributes: true,
				removeScriptTypeAttributes: true,
				removeStyleLinkTypeAttributes: true,
				removeEmptyAttributes: true,
				useShortDoctype: true,
				minifyCSS: (text: string) => transformCSS({ minify: true, code: Buffer.from(text), filename: "index.html" }).code.toString(),
				minifyJS: true,
				minifyURLs: true,
			},
		}),
		compression({
			skipIfLargerOrEqual: true,
			deleteOriginalAssets: true,
			include: [/\.(x?html?|mht(ml)?|hta|md|css|s[ca]ss|less|styl|[mc]?[jt]sx?|json[5c]?|ya?ml|xa?ml|toml|ini|config|map|(web)?manifest|appcache|[to]tf|ttc|vtt|svg|bmp|ico|cur|ani)$/iu],
		}),
	],
	base: "",
	publicDir: "src/public",
	build: {
		target: "ESNext",
		rollupOptions: {
			output: {
				entryFileNames: "[name].js",
				chunkFileNames: "chunks/[name].js",
				assetFileNames: "assets/[name]-[hash].[ext]",
				/* manualChunks(id) {
					if (id.endsWith(".js")) return "../" + id;
					if (id.includes("node_modules")) return "vendor";
					else if (id.includes("svg-icons-register")) return "svgs";
					else if (id.includes("assets/lotties")) return "lotties";
				}, */
				banner:
`/*!
 * Otomad Helper
 * @homepage https://otomadhelper.readthedocs.io/
 * @repository https://github.com/otomad/OtomadHelper
 * @license GPL 3.0
 * @author Ranne
 */`,
			},
		},
		chunkSizeWarningLimit: 500_000, // 500MB
		minify: "terser",
		terserOptions: {
			keep_classnames: true,
		},
	},
	assetsInclude: [
		"**/*.cur",
		"**/*.ani",
	],
});
