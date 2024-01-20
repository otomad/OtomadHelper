interface IPage {
	page: string[];
	prevPage: string[];
	getPagePath(): string;
	changePage: SetState<string[]>;
	pushPage(...pages: string[]): void;
	canBack(): boolean;
	back(): void;
	getTransition(): "forward" | "backward" | "jump" | "";
}

export const usePageStore = createStore<IPage>()(
	persist((set, get) => {
		const page = ["source"];

		function changePage(pages: string[]): void;
		function changePage(setStateAction: (prevPage: string[]) => string[]): void;
		function changePage(arg: unknown) {
			let pages: string[] = typeof arg === "function" ? arg(get().page) : arg;
			pages = pages.flatMap(page => page.split("/"));
			setPageInternal(pages);
		}

		function setPageInternal(nextPage: string[]) {
			const page = get().page;
			if (arrayEquals(page, nextPage)) return;
			set(() => ({
				prevPage: page,
				page: nextPage,
			}));
		}

		return {
			page,
			prevPage: page,
			getPagePath: () => get().page.join("/"),
			changePage: changePage as SetState<string[]>,
			pushPage: (...pages) => setPageInternal([...get().page, ...pages]),
			canBack: () => get().page.length > 1,
			back() {
				if (get().canBack()) {
					const page = get().page;
					page.pop();
					setPageInternal(page);
				}
			},
			getTransition() {
				const { prevPage, page } = get();
				for (let i = 0; i < Math.max(prevPage.length, page.length); i++) {
					const prevCrumb = prevPage[i], crumb = page[i];
					if (!prevCrumb) return "forward";
					if (!crumb) return "backward";
					if (prevCrumb !== crumb) return "jump";
				}
				return "";
			},
		};
	}, { name: "page" }),
);
