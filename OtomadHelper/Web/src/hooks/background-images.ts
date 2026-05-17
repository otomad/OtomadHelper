import { arrayMove } from "@dnd-kit/sortable";
import IndexedDBStore from "classes/IndexedDBStore";
import { startCircleViewTransition } from "helpers/color-mode";
import Vibrant from "helpers/vibrant";

const DATABASE_VERSION = 2;

interface BackgroundImageRow {
	imageData: Blob;
	filename: string;
	displayIndex: number;
	color: string;
	fit: Config.ImageFitType;
	position: TwoD;
}

export interface BackgroundImageRowWithMore extends BackgroundImageRow {
	url: string;
	key: number;
}

const DEFAULT_BACKGROUND_IMAGE_ROW: BackgroundImageRowWithMore = {
	imageData: null!,
	filename: "",
	url: "",
	key: -1,
	displayIndex: -1,
	color: "",
	fit: "cover",
	position: [50, 50],
};

const keyToUrl = proxyMap<number, string>();
const itemsAtom = atom<BackgroundImageRowWithMore[]>([]);

export function useBackgroundImages() {
	type Store = IndexedDBStore<BackgroundImageRow>;
	const store = useRef<Store>(undefined);
	const [items, setItems] = useAtom(itemsAtom);
	const { backgroundImage: currentImageKey } = useSnapshot(configStore.settings);
	const setCurrentImageKey = setStateNarrow(
		current => startCircleViewTransition(current !== -1, () => configStore.settings.backgroundImage = current, { cursor: "wait" }),
		() => configStore.settings.backgroundImage,
	);
	const currentItem = useMemo(() => items.find(item => item.key === currentImageKey), [items, currentImageKey]);
	const currentImage = useMemo(() => currentItem?.url ?? "", [currentItem]);
	const currentDominantColor = useMemo(() => currentItem?.color || undefined, [currentItem]);
	const shown = useMemo(() => currentImageKey !== -1, [currentImageKey]);
	const fit = useMemo(() => currentItem?.fit ?? DEFAULT_BACKGROUND_IMAGE_ROW.fit, [currentItem]);
	const position = useMemo(() => currentItem?.position ?? DEFAULT_BACKGROUND_IMAGE_ROW.position, [currentItem]);

	useAsyncMountEffect(async () => {
		store.current = new IndexedDBStore<BackgroundImageRow>("ImagesDB", DATABASE_VERSION, "backgroundImages", {
			imageData: null,
			filename: null,
			displayIndex: null,
			color: null,
			fit: null,
			position: null,
		});
		await store.current.open();
		await updateItems();
	});

	async function updateItems() {
		if (!store.current?.isDatabaseOpen) return;
		const items = await store.current.sortedMap("displayIndex", async (value, key) => {
			key = +key;
			const url: string = await keyToUrl.getOrInsertAsync(key, async () => await fileToBlob(value.imageData));
			return { ...value, url, key };
		});
		setItems([DEFAULT_BACKGROUND_IMAGE_ROW, ...items]);
	}

	async function reorderItems(getIndex: (oldIndex: number) => number | undefined) {
		if (!store.current?.isDatabaseOpen) return;
		const requests = [];
		for await (const cursor of store.current.cursor()) {
			const oldIndex = cursor.value.displayIndex, newIndex = getIndex(oldIndex);
			if (newIndex === undefined || newIndex === oldIndex) continue;
			cursor.value.displayIndex = newIndex;
			const request = cursor.update(cursor.value);
			requests.push(IndexedDBStore.getResult(request));
		}
		return Promise.all(requests);
	}

	async function add(image: File) {
		if (!store.current) return;
		const length = await store.current.length;
		const imgResource = new Image();
		imgResource.src = await fileToData(image);
		const palette = await Vibrant.from(await fileToData(image)).getPalette();
		const color = palette.Vibrant?.hex ?? "";
		await store.current.add({
			imageData: image,
			filename: image.name,
			displayIndex: length,
			color,
			fit: DEFAULT_BACKGROUND_IMAGE_ROW.fit,
			position: DEFAULT_BACKGROUND_IMAGE_ROW.position,
		});
		await updateItems();
	}

	async function delete_(key: number) {
		if (!store.current || +key < 0) return;
		const currentIndex = items.find(row => row.key === key)?.displayIndex ?? NaN;
		setCurrentImageKey(backgroundImage => backgroundImage === key ? -1 : backgroundImage);
		await nextAnimationTick();
		URL.revokeObjectURL(keyToUrl.get(key) ?? "");
		keyToUrl.delete(key);
		await store.current.delete(+key);
		if (Number.isFinite(currentIndex))
			await reorderItems(index => { if (index > currentIndex) return index - 1; });
		await updateItems();
	}

	async function reorder(key: number, newIndex: number) {
		if (!store.current || +key < 0) return;
		setItems(items => arrayMove(items, items.findIndex(item => item.key === key), clamp(newIndex + 1, 1, items.length)));
		const length = await store.current.length;
		if (length === 0) return;
		newIndex = clamp(newIndex, 0, length - 1);
		const oldIndex = items.find(row => row.key === key)?.displayIndex ?? -1;
		if (oldIndex === newIndex || oldIndex === -1) return;
		const min = Math.min(oldIndex, newIndex), max = Math.max(oldIndex, newIndex);
		await reorderItems(index => {
			if (index < min || index > max) return;
			else if (index === oldIndex) return newIndex;
			else return index + (oldIndex <= newIndex ? -1 : 1);
		});
		await updateItems();
	}

	async function setFit(value: typeof fit) {
		if (!store.current || currentImageKey < 0) return;
		setItems(produce(draft => {
			const item = draft.find(item => item.key === currentImageKey);
			if (item) item.fit = value;
		}));
		await store.current.set("fit", value, currentImageKey);
	}

	async function setPosition(value: typeof position, submit = false) {
		if (!store.current || currentImageKey < 0) return;
		setItems(produce(draft => {
			const item = draft.find(item => item.key === currentImageKey);
			if (item) item.position = value;
		}));
		if (!submit) return;
		await store.current.set("position", value, currentImageKey);
	}

	return {
		items,
		update: updateItems,
		add,
		map: (...args: Parameters<Store["map"]>) => store.current?.map(...args),
		delete: delete_,
		reorder,
		currentImageKey: [currentImageKey, setCurrentImageKey] as const,
		currentImage,
		currentDominantColor,
		shown,
		fit: [fit, setFit] as const,
		position: [position, setPosition] as const,
	};
}

// BUG: 如果使用原生右键菜单，则“往后挪”会很卡。但是“往前挪”不卡，使用自定义右键菜单也不卡，原因未知。目前仅在 WebView 2 中触发。
