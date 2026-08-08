import { nextAnimationTick } from "../delay";

// 自动展开与定位的核心函数
const handleHashOpenAndScroll = async (delayTimes = 0) => {
	const hash = location.hash.slice(1);
	if (!hash || hash === "?") return;

	while (delayTimes--) await nextAnimationTick();

	// 找到对应 id 的元素
	const targetId = decodeURIComponent(hash);
	const heading = document.getElementById(targetId);

	if (heading) {
		let scroll: () => Promise<unknown>;
		const scrollToBlockStart = (element: Element) => element.scrollIntoView({ block: "start" });
		if (heading.matches("details > summary > :is(h1, h2, h3, h4, h5, h6)")) {
			const details = heading.closest("details")!;
			details.open = true;
			scroll = async () => {
				const SCROLL_PADDING_TOP_CLASS = "scroll-padding-top";
				document.documentElement.classList.add(SCROLL_PADDING_TOP_CLASS);
				await scrollToBlockStart(details);
				await scrollToBlockStart(details);
				document.documentElement.classList.remove(SCROLL_PADDING_TOP_CLASS);
			};
		} else {
			scroll = () => scrollToBlockStart(heading);
		}
		await scroll();
		if (document.activeViewTransition) {
			await document.activeViewTransition?.finished;
			await scroll();
		}
	}
};

export default handleHashOpenAndScroll;
