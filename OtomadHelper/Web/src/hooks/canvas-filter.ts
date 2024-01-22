function getPixelIndex(width: number, x: number, y: number) {
	return ((y | 0) * width + (x | 0)) * 4;
}

function cloneImageData(imageData: ImageData) {
	return new ImageData(new Uint8ClampedArray(imageData.data), imageData.width, imageData.height, { colorSpace: imageData.colorSpace });
}

type FilterType = keyof typeof filters;
type FilterBlobs = Record<FilterType, string>;

const filters = {
	vibrato(imageData: ImageData) {
		const { width, height, data: originalPixels } = imageData;
		const newImageData = cloneImageData(imageData);
		const { data: pixels } = newImageData;
		const rate = width / 72;
		for (let y = 0; y < height; y++)
			for (let x = 0; x < width; x++) {
				const i = getPixelIndex(width, x, y);
				const newY = Math.sin(x / rate) * rate + y;
				let newI = getPixelIndex(width, x, newY);
				while (newI < 0) newI += width * 4;
				while (newI > pixels.length) newI -= width * 4;
				pixels[i] = originalPixels[newI];
				pixels[i + 1] = originalPixels[newI + 1];
				pixels[i + 2] = originalPixels[newI + 2];
				pixels[i + 3] = originalPixels[newI + 3];
			}
		return newImageData;
	},
	spherize(imageData: ImageData) {
		const { width, height, data: originalPixels } = imageData;
		const newImageData = cloneImageData(imageData);
		const { data: pixels } = newImageData;
		const centerX = width / 2, centerY = height / 2, radius = Math.min(centerX, centerY);
		const toCenter = (x: number, y: number) => Math.sqrt((y - centerY) ** 2 + (x - centerX) ** 2);
		const getRadian = (x: number, y: number) => Math.atan2(y - centerY, x - centerX);
		for (let y = 0; y < height; y++)
			for (let x = 0; x < width; x++) {
				const distance = toCenter(x, y), radian = getRadian(x, y);
				if (distance > radius) continue;
				const newDistance = (distance / radius) ** 2.5 * radius;
				const newX = newDistance * Math.cos(radian) + centerX, newY = newDistance * Math.sin(radian) + centerY;
				const i = getPixelIndex(width, x, y);
				const newI = getPixelIndex(width, newX, newY);
				pixels[i] = originalPixels[newI];
				pixels[i + 1] = originalPixels[newI + 1];
				pixels[i + 2] = originalPixels[newI + 2];
				pixels[i + 3] = originalPixels[newI + 3];
			}
		return newImageData;
	},
};

const saved = {
	imagePath: "",
	filters: {} as FilterBlobs,
};

export async function getCanvasFilter(imagePath: string) {
	if (imagePath === saved.imagePath) return saved.filters;
	else {
		saved.imagePath = imagePath;
		for (const url of Object.values(saved.filters))
			URL.revokeObjectURL(url);
	}

	const canvas = document.createElement("canvas");
	const context = canvas.getContext("2d")!;

	const image = new Image();
	await new Promise(resolve => {
		image.onload = resolve;
		image.src = imagePath;
	});
	canvas.width = image.width;
	canvas.height = image.height;
	context.drawImage(image, 0, 0);

	const imageData = context.getImageData(0, 0, canvas.width, canvas.height);
	for (const [name, filter] of Object.entries(filters)) {
		const newImageData = filter(imageData);
		context.putImageData(newImageData, 0, 0);
		const blob = await new Promise<string>(resolve => canvas.toBlob(blob => resolve(URL.createObjectURL(blob!))));
		saved.filters[name as FilterType] = blob;
	}

	return saved.filters;
}

export function useCanvasFilter(imagePath: string) {
	const [canvasFilter, setCanvasFilter] = useState<FilterBlobs>({} as FilterBlobs);

	useEffect(() => {
		getCanvasFilter(imagePath).then(r => { setCanvasFilter(r); });
	}, [imagePath]);

	return canvasFilter;
}
