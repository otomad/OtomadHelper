const useImages = createStore({
	images: proxyMap<string, HTMLImageElement>(),
	get(src: string) {
		useImages.add(src);
		return useImages.images.get(src)!;
	},
	add(src: string) {
		if (useImages.has(src)) return;
		const image = new Image();
		image.src = src;
		resetImageAttributes(image);
		useImages.images.set(src, valtioRef(image));
	},
	has(src: string) {
		return useImages.images.has(src);
	},
});

// {
// 	const images = Object.values(import.meta.glob<string>("/src/assets/images/**/*", { import: "default", eager: true }));
// 	const { add } = useImages;
// 	images.forEach(image => add(image));
// }

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
	if ("alt" in withAttrs && withAttrs.alt === undefined)
		image.removeAttribute("alt");
}

export default function Img({ src, duplicate, ref, ...htmlAttrs }: FCP<{
	/** Image source href. */
	src: string;
	/** If the page needs to include the image twice in different locations, please specify an alias here for a better experience. */
	duplicate?: string;
}, "img">) {
	const contentsEl = useDomRef<"div">();
	const imageEl = useDomRef<"img">();
	const images = useSnapshot(useImages);
	const source = duplicate ? src + "?" + duplicate : src;

	useImperativeHandleRef(ref, imageEl);

	useEffect(() => {
		const contents = contentsEl.current;
		if (!contents) return;
		let image = images.get(source);
		imageEl.current = image;
		if ([...contents.childNodes].includes(image as ChildNode)) return;
		contents.childNodes.forEach(node => node.remove());
		if (image.parentNode) image = image.cloneNode() as HTMLImageElement;
		resetImageAttributes(image, htmlAttrs);
		contents.append(image);

		return () => {
			image.remove();
		};
	}, [src, duplicate, contentsEl, htmlAttrs, imageEl, images, source]);

	return <Contents ref={contentsEl} className="img" />;
}
