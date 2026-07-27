import { nextAnimationTick } from "../delay";

// 自动展开与定位的核心函数
const handleHashOpenAndScroll = async (delayTimes = 0) => {
	const { hashId, textFragment } = new URLEx();
	if (!hashId || hashId === "?") return;

	while (delayTimes--) await nextAnimationTick();

	// 找到对应 id 的元素
	const targetId = decodeURIComponent(hashId);
	const heading = document.getElementById(targetId);

	if (heading && heading.matches("details > summary > :is(h1, h2, h3, h4, h5, h6)")) {
		const details = heading.closest("details")!;
		details.open = true;
		if (textFragment) return;
		const scroll = async () => {
			const SCROLL_PADDING_TOP_CLASS = "scroll-padding-top";
			document.documentElement.classList.add(SCROLL_PADDING_TOP_CLASS);
			await details.scrollIntoView({ block: "start" });
			await details.scrollIntoView({ block: "start" });
			document.documentElement.classList.remove(SCROLL_PADDING_TOP_CLASS);
		};
		await scroll();
		if (document.activeViewTransition) {
			await document.activeViewTransition?.finished;
			await scroll();
		}
	}
};

export default handleHashOpenAndScroll;

const FRAGMENT_DIRECTIVE = ":~:text=";
export class URLEx extends URL {
	static #loaded = false;
	private static get href() {
		try {
			if (URLEx.#loaded) throw "";
			// See: https://stackoverflow.com/a/67075798/19553213
			return performance.getEntriesByType("navigation")[0].name;
		} catch (error) {
			return location.href;
		} finally {
			URLEx.#loaded = true;
		}
	}
	constructor(url?: string | URL, base?: string | URL | undefined) {
		if (!url) url = URLEx.href;
		super(url, base);
	}
	get href() {
		return decodeURIComponent(super.href);
	}
	get search() {
		return decodeURIComponent(super.search);
	}
	get hash() {
		return decodeURIComponent(super.hash).split(FRAGMENT_DIRECTIVE)[0] || "";
	}
	get textFragment() {
		return decodeURIComponent(super.hash).split(FRAGMENT_DIRECTIVE)[1] || "";
	}
	get hashId() {
		return this.hash.slice(1);
	}
}
