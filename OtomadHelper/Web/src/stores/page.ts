export interface PageScroll {
	readonly page: string;
	readonly elementIndex: number;
	readonly offsetY: number;
}

type PageScrollList = (PageScroll | undefined)[];

type PageTransitionName = "forward" | "backward" | "jump" | "";

interface IPage {
	page: string[];
	prevPage: string[];
	transition: PageTransitionName;
	scrolls: PageScrollList;
	poppedScroll?: PageScroll;
	getPagePath(): string;
	changePage: SetState<string[]>;
	pushPage(...pages: string[]): void;
	canBack(): boolean;
	back(): void;
	resetTransition(): void;
	reset(): void;
	isAlerted404: boolean;
	pageContentId?: string;
	setPageContentId(value: string): void;
}

const NAME = "page";

export const usePageStore = createStore<IPage>()(
	persist((set, get) => {
		const page = ["source"];
		const scrolls: PageScrollList = [];

		function changePage(pages: string[]): void;
		function changePage(setStateAction: (prevPage: string[]) => string[]): void;
		function changePage(arg: unknown) {
			let pages: string[] = typeof arg === "function" ? arg(get().page) : arg;
			pages = pages.flatMap(page => page.split("/"));
			setPageInternal(pages);
		}

		function getTransition(page: string[], nextPage: string[]) {
			for (let i = 0; i < Math.max(page.length, nextPage.length); i++) {
				const crumb = page[i], nextCrumb = nextPage[i];
				if (!crumb) return "forward";
				if (!nextCrumb) return "backward";
				if (crumb !== nextCrumb) return "jump";
			}
			return "";
		}

		function getScrolls(transition: PageTransitionName, nextPage: string[]) {
			const { scrolls: prevScrolls, poppedScroll: prevPoppedScroll, page: prevPage, pageContentId } = get();
			const prevLastPage = prevPage.last();
			let scrolls: PageScrollList, poppedScroll: PageScroll | undefined;
			const findScroll = (page: string) => prevScrolls.findLast(scroll => scroll?.page === page);
			switch (transition) {
				case "jump":
					scrolls = [];
					break;
				case "":
				default:
					poppedScroll = prevPoppedScroll;
					scrolls = prevScrolls;
					break;
				case "backward": {
					const nextPageCopy = nextPage.slice();
					const lastPage = nextPageCopy.pop()!;
					poppedScroll = findScroll(lastPage);
					scrolls = nextPageCopy.map(page => findScroll(page));
					break;
				}
				case "forward":
					scrolls = nextPage.map(page => {
						if (page !== prevLastPage) return findScroll(page);

						if (!pageContentId) return;
						const pageContentEl = document.getElementById(pageContentId);
						if (!pageContentEl) return;
						const begin = pageContentEl.getBoundingClientRect().top;
						const containerEl = pageContentEl?.querySelector<HTMLElement>(":scope > main > .container");
						if (!containerEl) return;
						let i = -1;
						for (let child of containerEl.children) {
							i++;
							while (isElementContents(child))
								child = child.firstElementChild!;
							asserts<HTMLElement>(child);
							if (isElementHidden(child)) continue;
							let { top, bottom } = child.getBoundingClientRect();
							top -= begin;
							bottom -= begin;
							if (bottom <= 0) continue;
							return {
								page,
								elementIndex: i,
								offsetY: -top,
							};
						}
					});
					break;
			}
			return { scrolls, poppedScroll };
		}

		function setPageInternal(nextPage: string[]) {
			document.getElementById(STOP_TRANSITION_ID)?.remove();
			const { page } = get();
			if (page.equals(nextPage)) return;
			const transition = getTransition(page, nextPage);
			// document.startViewTransition(() =>
			set(() => ({
				prevPage: page,
				page: nextPage,
				transition,
				...getScrolls(transition, nextPage),
			}));
			// );
		}

		return {
			page,
			prevPage: page,
			transition: "forward",
			scrolls,
			poppedScroll: undefined,
			getPagePath: () => get().page.join("/"),
			changePage: changePage as SetState<string[]>,
			pushPage: (...pages) => setPageInternal([...get().page, ...pages]),
			canBack: () => get().page.length > 1,
			back() {
				if (get().canBack()) {
					const page = get().page.slice();
					page.pop();
					setPageInternal(page);
				}
			},
			resetTransition: () => set({ transition: "jump" }),
			reset() {
				if (!get().isAlerted404)
					alert("404 Not Found!");
				get().isAlerted404 = true;
				localStorage.removeItem(NAME);
				location.reload();
			},
			isAlerted404: false,
			pageContentId: undefined,
			setPageContentId: pageContentId => get().pageContentId !== pageContentId && set({ pageContentId }),
		};
	}, {
		name: NAME,
		partialize: state => ({ page: state.page }),
	}),
);
