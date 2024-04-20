const useImages = createStore<{
	images: Map<string, HTMLImageElement>;
	get(src: string): HTMLImageElement;
	add(src: string): void;
	has(src: string): boolean;
}>()(
	zustandImmer((_set, get) => ({
		images: new Map<string, HTMLImageElement>(),
		get(src: string) {
			get().add(src);
			return get().images.get(src)!;
		},
		add(src: string) {
			if (get().has(src)) return;
			const image = new Image();
			image.src = src;
			resetImageAttributes(image);
			get().images.set(src, image);
		},
		has(src: string) {
			return get().images.has(src);
		},
	})),
);

{
	const images = Object.values(import.meta.glob<string>("/src/assets/images/**/*", { import: "default", eager: true }));
	const { add } = useImages.getState();
	images.forEach(image => add(image));
}

function resetImageAttributes(image: HTMLImageElement, withAttrs: FCP<{}, "img"> = {}) {
	const { attributes } = image;
	for (const { name } of attributes) {
		if (name === "src") continue;
		attributes.removeNamedItem(name);
	}
	assign(image, {
		alt: "",
		draggable: false,
		...withAttrs as object,
	});
}

export default forwardRef(function Img({ src, duplicate, ...htmlAttrs }: FCP<{
	/** Image source href. */
	src: string;
	/** If the page needs to include the image twice in different locations, please specify an alias here for a better experience. */
	duplicate?: string;
}, "img">, ref: ForwardedRef<"img">) {
	const contentsEl = useDomRef<"div">();
	const imageEl = useDomRef<"img">();
	const images = useImages();
	const source = duplicate ? src + "?" + duplicate : src;

	useImperativeHandle(ref, () => imageEl.current!, []);

	useEffect(() => {
		const contents = contentsEl.current;
		if (!contents) return;
		let image = images.get(source);
		imageEl.current = image;
		if ([...contents.childNodes].includes(image)) return;
		contents.childNodes.forEach(node => node.remove());
		if (image.parentNode) image = image.cloneNode() as HTMLImageElement;
		resetImageAttributes(image, htmlAttrs);
		contents.append(image);

		return () => {
			image.remove();
		};
	}, [src, duplicate]);

	return <Contents ref={contentsEl} className="img" />;
});
