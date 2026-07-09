import { type SiteConfig, type DefaultTheme } from "vitepress";
import { readFile, writeFile } from "fs/promises";
import { Feed } from "feed";
import hostname from "./hostname";
import { join } from "path/posix";
import { join as localJoin } from "path";
import { parseHTML, NodeFilter } from "linkedom";

export async function createRssFeeds(config: SiteConfig) {
	const locales = Object.entries(config.site.locales).map(([subdirectory, locale]) => {
		const isRoot = subdirectory === "root";
		return {
			subdirectory: isRoot ? "" : subdirectory,
			isRoot,
			lang: locale.lang!,
			title: locale.title || config.site.title,
			description: locale.description || config.site.description,
			path: isRoot ? hostname : join(hostname, subdirectory, "/"),
			copyright: (locale.themeConfig as DefaultTheme.Config).footer!.copyright!,
		};
	});
	const nonRootLanguages = locales.filter(locale => !locale.isRoot).map(locale => locale.subdirectory);

	for (const locale of locales) {
		const feed = new Feed({
			title: locale.title,
			description: locale.description,
			id: locale.path,
			link: locale.path,
			language: locale.lang,
			favicon: join(hostname, "favicon_1.ico"),
			copyright: locale.copyright,
			feedLinks: { rss2: join(locale.path, "feed.xml") },
		});

		const posts = await Promise.all(
			config.pages
				.filter(pagePath => {
					if (!locale.isRoot) return pagePath.startsWith(locale.subdirectory + "/");
					else return !nonRootLanguages.some(lang => pagePath.startsWith(lang + "/"));
				})
				.map(async pagePath => {
					const htmlPagePath = pagePath.replace(/\.md$/i, ".html");
					const { content, title } = await extractHtmlContent(localJoin(config.outDir, htmlPagePath));
					return {
						url: join(hostname, htmlPagePath),
						lang: locale.lang,
						title,
						content: content || undefined,
					};
				}),
		);

		for (const post of posts) {
			feed.addItem({
				title: post.title,
				id: post.url,
				link: post.url,
				content: post.content,
				// 我不关心文章发布日期。
				date: undefined!,
			});
		}

		await writeFile(localJoin(config.outDir, locale.subdirectory, "feed.xml"), feed.rss2(), "utf-8");
	}
}

interface ExtractHtmlContentResult {
	content: string;
	title: string;
}
async function extractHtmlContent(htmlFilePath: string): Promise<ExtractHtmlContentResult> {
	try {
		const htmlContent = await readFile(htmlFilePath, "utf-8");
		const { document } = parseHTML(htmlContent);

		// VitePress 的默认主题正文都包裹在 .vp-doc 核心类名下
		const docContainer = document.querySelector(".vp-doc > div");
		if (!docContainer) throw null;

		// 删除标题旁边的 # 锚点链接，和其它无用的东西
		for (const uselessElement of docContainer.querySelectorAll(
			".header-anchor, object, input, button, datalist, textarea",
		))
			uselessElement.remove();

		// 循环删除不需要的属性，只保留必要的内容属性（如 href, src, alt）
		for (const element of docContainer.querySelectorAll("*"))
			for (const { name: attrName } of element.attributes)
				if (
					attrName.startsWith("data-") ||
					attrName === "tabindex" ||
					attrName === "class" ||
					attrName === "style" ||
					attrName.startsWith("on")
				)
					element.removeAttribute(attrName);

		// 删除注释
		const commentIterator = toIterable(
			document.createNodeIterator(docContainer, NodeFilter.SHOW_COMMENT),
		).toArray() as Comment[];
		commentIterator.forEach(comment => comment.remove());

		// 将图片和超链接的相对路径改为绝对路径
		for (const img of docContainer.querySelectorAll("img")) img.src &&= new URL(img.src, hostname).href;
		for (const a of docContainer.querySelectorAll("a")) a.href &&= new URL(a.href, hostname).href;

		// 获取文章标题
		const title = docContainer.querySelector("h1")?.innerText.trim() || "";

		// 返回清洗干净后的正文 HTML 字符串
		const content = docContainer.innerHTML.trim();
		return { content, title };
	} catch {
		return { content: "", title: "" };
	}
}

// 匪夷所思之 `NodeIterator` 未实现迭代器，不支持 `Symbol.iterator` 接口。
function* toIterable(nodeIterator: NodeIterator) {
	let node;
	while ((node = nodeIterator.nextNode())) yield node;
}
