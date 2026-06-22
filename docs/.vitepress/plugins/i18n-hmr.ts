import type { Plugin } from "vitepress";
import path from "path/posix";

export default (): Plugin => {
	let targetLangs: string[] = [];
	let root = process.cwd();
	return {
		name: "vitepress-i18n-hmr",
		config(config) {
			root = config.root!;
			const siteConfig = (config as any).vitepress;
			targetLangs = Object.keys(siteConfig.userConfig.locales!).filter(lang => lang !== "root");
		},
		handleHotUpdate(ctx) {
			const relativePath = path.relative(root, ctx.file);
			if (relativePath.endsWith(".md") && !targetLangs.some(lang => relativePath.startsWith(lang + "/"))) {
				// 找到对应的中文虚拟路径，触发更新
				for (const lang of targetLangs) {
					const virtualFile = path.join(root, lang, relativePath);
					console.log(virtualFile);
					ctx.server.ws.send({
						type: "custom",
						event: "vitepress:page-updated",
						data: { path: virtualFile },
					});
				}
			}
		},
	};
};
