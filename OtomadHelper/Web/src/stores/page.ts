export const pageStore = createStore({
	name: "page",
	initialState: {
		value: ["source"],
	},
	reducers: {
		setPage: (state, action: PayloadAction<string[] | string>) => {
			let page = action.payload;
			if (typeof page === "string") page = page.split("/");
			state.value = page;
		},
	},
	selectors: {
		texts: state => {
			return state.value.map(page => t[page]);
		},
		path: state => {
			return state.value.join("/");
		},
	},
})();
