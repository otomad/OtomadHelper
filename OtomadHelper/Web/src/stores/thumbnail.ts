import exampleThumbnail from "assets/images/ヨハネの氷.avif";

const thumbnailAtom = atom(exampleThumbnail);

export function useThumbnail() {
	const [thumbnail, _setThumbnail] = useAtom(thumbnailAtom);
	const setThumbnail = setStateInterceptor(_setThumbnail, undefined, (_, prevThumbnail) => URL.revokeObjectURL(prevThumbnail));
	const resetThumbnail = () => setThumbnail(exampleThumbnail);
	const isDefaultThumbnail = thumbnail === exampleThumbnail;

	async function changeThumbnail() {
		const file = await openFile({ types: [{ accept: { "image/*": [] } }] });
		if (file) {
			const url = URL.createObjectURL(file);
			_setThumbnail(url);
		}
	}
	if (import.meta.env.DEV)
		globals.changeThumbnail = changeThumbnail;

	return { thumbnail, setThumbnail, changeThumbnail, resetThumbnail, isDefaultThumbnail };
}
