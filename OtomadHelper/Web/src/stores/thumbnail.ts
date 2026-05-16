import exampleThumbnail from "assets/images/ヨハネの氷.avif";
import IndexedDBStore from "classes/IndexedDBStore";

const thumbnailAtom = atom(exampleThumbnail);

const DATABASE_VERSION = 1;

interface ThumbnailRow {
	imageData: Blob | null;
}

interface ThumbnailKeyRow {
	key: number;
}

export function useThumbnail() {
	const [thumbnail, _setThumbnail] = useAtom(thumbnailAtom);
	type Store = IndexedDBStore<ThumbnailRow, ThumbnailKeyRow>;
	const store = useRef<Store>(undefined);

	function setThumbnailInitially(value: string | Blob) {
		_setThumbnail(prevThumbnail => {
			if (value instanceof Blob)
				value = URL.createObjectURL(value);
			URL.revokeObjectURL(prevThumbnail);
			return value;
		});
	}

	function setThumbnail(value: string | Blob) {
		setThumbnailInitially(value);
		if (store.current?.isDatabaseOpen)
			store.current.set({ imageData: value instanceof Blob ? value : null, key: 0 });
	}

	const resetThumbnail = () => setThumbnail(exampleThumbnail);
	const isDefaultThumbnail = thumbnail === exampleThumbnail;

	async function changeThumbnail() {
		const file = await openFile({ types: [{ accept: { "image/*": [] } }] });
		if (file) setThumbnail(file);
	}
	if (import.meta.env.DEV)
		globals.changeThumbnail = changeThumbnail;

	useAsyncMountEffect(async () => {
		store.current = new IndexedDBStore<ThumbnailRow, ThumbnailKeyRow>("SingleImageDB", DATABASE_VERSION, "thumbnail", {
			keyPath: "key",
			key: { unique: true },
			imageData: null,
		});
		await store.current.open();
		if (store.current.isDatabaseOpen) {
			const storedThumbnail = (await store.current.get(0))?.imageData ?? null;
			if (storedThumbnail !== null) setThumbnailInitially(storedThumbnail);
		}
	});

	return { thumbnail, setThumbnail, changeThumbnail, resetThumbnail, isDefaultThumbnail };
}
