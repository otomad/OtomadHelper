const ExpanderContext = createContext<{
	/**
	 * Where is the current component located in Expander / SettingCard?
	 * Or `undefined` if it is not in an Expander / SettingCard.
	 */
	place?: "children" | "action";
}>({
	place: undefined,
});

export /** @internal */ default ExpanderContext;
