interface IPage {
	page: string[];
	prevPage: string[];
	transition: "forward" | "backward" | "jump" | "";
	getPagePath(): string;
	changePage: SetState<string[]>;
	pushPage(...pages: string[]): void;
	canBack(): boolean;
	back(): void;
	resetTransition(): void;
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

		function getTransition(page: string[], nextPage: string[]) {
			for (let i = 0; i < Math.max(page.length, nextPage.length); i++) {
				const crumb = page[i], nextCrumb = nextPage[i];
				if (!crumb) return "forward";
				if (!nextCrumb) return "backward";
				if (crumb !== nextCrumb) return "jump";
			}
			return "";
		}

		function setPageInternal(nextPage: string[]) {
			const { page } = get();
			if (arrayEquals(page, nextPage)) return;
			set(() => ({
				prevPage: page,
				page: nextPage,
				transition: getTransition(page, nextPage),
			}));
		}

		return {
			page,
			prevPage: page,
			transition: "forward",
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
			resetTransition: () => set(() => ({ transition: "jump" })),
		};
	}, {
		name: "page",
		partialize: state => ({ page: state.page }),
	}),
);
