/*
 * @see https://github.com/theKashey/react-event-injector
 */

type FilterOnEvents<TDomAttributes> = Partial<OmitNevers<{
	[event in keyof TDomAttributes]-?:
		event extends `on${string}` ?
		event extends `${string}Capture` ? never : TDomAttributes[event] :
		never;
}>>;

type AllEvents = FilterOnEvents<React.DOMAttributes<HTMLElement> & {
	// For events which are not shown in the DOM attributes.
	onAnimationCancel: AnimationEventHandler<HTMLElement>;
	onScrollEnd: BaseEventHandler<HTMLElement>;
}>;

type Options = boolean | AddEventListenerOptions | EventListenerOptions;

const eventAttrToType = (eventName: string) => eventName.toLowerCase().replace(/^on/, "");

const EventInjector = forwardRef(function EventInjector<TElement extends ReactElement>({ children, options, ...events }: FCP<{
	children: TElement;
	/**
	 * Event listener options.
	 *
	 * @default
	 * ```typescript
	 * { passive: false }
	 * ```
	 */
	options?: Options;
} & AllEvents>, ref: ForwardedRef<"section">) {
	const childEl = useDomRef<"section">();
	useImperativeHandleRef(ref, childEl);

	useEffect(() => {
		const child = childEl.current;
		if (!child) return;
		for (const [name, listener] of Object.entries(events))
			child.addEventListener(eventAttrToType(name), listener as never, options);
		return () => {
			for (const [name, listener] of Object.entries(events))
				child.removeEventListener(eventAttrToType(name), listener as never, options);
		};
	}, [children, options]);

	return cloneRef(children, childEl);
});

export default functionModule(EventInjector, {
	/**
	 * ```typescript
	 * { passive: true }
	 * ```
	 */
	Passive: ({ options = { passive: true }, ...props }: PropsOf<typeof EventInjector>) => <EventInjector {...props} options={options} />,
	/**
	 * ```typescript
	 * { passive: false }
	 * ```
	 */
	Active: ({ options = { passive: false }, ...props }: PropsOf<typeof EventInjector>) => <EventInjector {...props} options={options} />,
});
