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
	reset(): void;
	isAlerted404: boolean;
}

const NAME = "page";

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
			document.getElementById(STOP_TRANSITION_ID)?.remove();
			const { page } = get();
			if (page.equals(nextPage)) return;
			// document.startViewTransition(() =>
			set(() => ({
				prevPage: page,
				page: nextPage,
				transition: getTransition(page, nextPage),
			}));
			// );
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
			reset() {
				if (!get().isAlerted404)
					alert("404 Not Found!");
				get().isAlerted404 = true;
				localStorage.removeItem(NAME);
				location.reload();
			},
			isAlerted404: false,
		};
	}, {
		name: NAME,
		partialize: state => ({ page: state.page }),
	}),
);
