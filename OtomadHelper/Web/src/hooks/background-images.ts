import IndexedDBStore from "classes/IndexedDBStore";
import { startCircleViewTransition } from "helpers/color-mode";

interface BackgroundImageRow {
	imageData: Blob;
	filename: string;
}

export interface BackgroundImageRowWithMore extends BackgroundImageRow {
	url: string;
	key: number;
}

const keyToUrl = proxyMap<number, string>();
const itemsAtom = atom<BackgroundImageRowWithMore[]>([]);

export function useBackgroundImages() {
	const store = useRef<IndexedDBStore<BackgroundImageRow>>();
	type Store = NonNull<typeof store.current>;
	const [items, setItems] = useAtom(itemsAtom);
	const { backgroundImage } = useSnapshot(configStore.settings);
	const setBackgroundImage: SetStateNarrow<typeof backgroundImage> = value => {
		const previous = configStore.settings.backgroundImage;
		const current = typeof value === "function" ? value(previous) : value;
		if (current !== previous)
			startCircleViewTransition(current !== -1, () => configStore.settings.backgroundImage = current);
	};
	const currentImage = useMemo(() => items.find(item => item.key === backgroundImage)?.url ?? "", [items, backgroundImage]);

	useAsyncMountEffect(async () => {
		store.current = new IndexedDBStore<BackgroundImageRow>("ImagesDB", 1, "backgroundImages", {
			imageData: null,
			filename: null,
		});
		await store.current.open();
		await updateItems();
	});

	async function updateItems() {
		if (!store.current || !store.current.isDatabaseOpen) return;
		const items = await store.current.map(async (value, key) => {
			key = +key;
			const url: string = await Map.prototype.getOrInit.apply(keyToUrl, [key, async () => await fileToBlob(value.imageData)]);
			return { ...value, url, key };
		});
		setItems([{ imageData: null!, filename: "", url: "", key: -1 }, ...items]);
	}

	async function add(image: File) {
		if (!store.current) return;
		await store.current.add({
			imageData: image,
			filename: image.name,
		});
		await updateItems();
	}

	async function delete_(key: number) {
		if (!store.current || +key < 0) return;
		setBackgroundImage(backgroundImage => backgroundImage === key ? -1 : backgroundImage);
		await nextAnimationTick();
		URL.revokeObjectURL(keyToUrl.get(key) ?? "");
		keyToUrl.delete(key);
		await store.current.delete(+key);
		updateItems();
	}

	return {
		items,
		update: updateItems,
		add,
		map: (...args: Parameters<Store["map"]>) => store.current?.map(...args),
		delete: delete_,
		backgroundImage: [backgroundImage, setBackgroundImage] as const,
		currentImage,
	};
}
