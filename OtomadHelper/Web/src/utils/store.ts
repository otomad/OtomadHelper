import { CreateSliceOptions, SliceCaseReducers, configureStore, createSlice } from "@reduxjs/toolkit";

export function createStore<State, CaseReducers extends SliceCaseReducers<State>, Selectors extends Record<string, (state: State) => Any>, Name extends string = string>(sliceOptions: CreateSliceOptions<State, CaseReducers, Name> & { selectors?: Selectors }) {
	type StateType = State & {};
	// @ts-ignore
	type StateValueType = State["value"];
	type ActionsType = typeof slice["actions"];
	type SelectorsType = { [selector in keyof Selectors]: ReturnType<Selectors[selector]> };
	type ActionsAndSelectors = SelectorsType & ActionsType;

	const SET_STORE_STATE_REDUCER = "#set";
	(sliceOptions.reducers as Record<string, AnyFunction>)[SET_STORE_STATE_REDUCER] = (state, action) => { state.value = action.payload; };
	const slice = createSlice(sliceOptions);
	const selectors: Record<string, (state: StateType) => unknown> = sliceOptions.selectors ?? {};

	return () => {
		const store = configureStore({ reducer: slice.reducer });
		const actions = slice.actions;

		const proxyHandler: ProxyHandler<object> = {
			get(target, property) {
				if (hasKey(target, property))
					return target[property];
				if (hasKey(actions, property))
					return (payload: unknown) => store.dispatch((actions[property] as Function)(payload));
				if (hasKey(selectors, property)) {
					const selector = selectors[property];
					return selector instanceof Function ? selector(store.getState()) : selector;
				}
			},
		};

		const storeTarget = {
			store,
			slice,
			actions,
			get state() {
				return store.getState();
			},
			subscribe(listener: (state: StateType) => void) {
				store.subscribe(() => listener(this.state));
			},
			useState: function (getValue: boolean = false) {
				const [state, _setState] = useState(store.getState());
				store.subscribe(() => _setState(store.getState()));
				const setState: SetState<StateType> = value => {
					const setStoreState = (value: StateType) => store.dispatch((slice.actions[SET_STORE_STATE_REDUCER] as Function)(value));
					if (value instanceof Function) setStoreState(value(store.getState()));
					else setStoreState(value);
				};
				return Tuple(getValue ? (state as AnyObject).value : state, setState, new Proxy({}, proxyHandler));
			} as {
				(getValue?: false): [StateType, SetState<StateType>, ActionsAndSelectors];
				(getValue: true): [StateValueType, SetState<StateValueType>, ActionsAndSelectors];
			},
		};

		return new Proxy(storeTarget, proxyHandler) as ActionsAndSelectors & typeof storeTarget;
	};
}
