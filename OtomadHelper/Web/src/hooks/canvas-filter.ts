/**
 * Returns the index of a pixel in the image data given its x and y coordinates.
 *
 * @param width - The width of the image data.
 * @param x - The x coordinate of the pixel.
 * @param y - The y coordinate of the pixel.
 * @returns The index of the pixel in the image data.
 */
function getPixelIndex(width: number, x: number, y: number) {
	return ((y | 0) * width + (x | 0)) * 4;
}

/**
 * Clones an ImageData object.
 *
 * @param imageData - The ImageData object to be cloned.
 * @returns A new ImageData object with the same data, width, height, and color space as the original.
 */
function cloneImageData(imageData: ImageData) {
	return new ImageData(new Uint8ClampedArray(imageData.data), imageData.width, imageData.height, { colorSpace: imageData.colorSpace });
}

type FilterType = keyof typeof filters;
type FilterBlobs = Record<FilterType, string>;

const filters = {
	/**
	 * Apply a **Vibrato** effect to the specified image data.
	 * @param imageData - The ImageData object of the image in the canvas.
	 * @returns A new ImageData object with the effect applied.
	 */
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
	/**
	 * Apply a **Spherize** effect to the specified image data.
	 * @param imageData - The ImageData object of the image in the canvas.
	 * @returns A new ImageData object with the effect applied.
	 */
	spherize(imageData: ImageData) {
		const { width, height, data: originalPixels } = imageData;
		const newImageData = cloneImageData(imageData);
		const { data: pixels } = newImageData;
		const centerX = width / 2, centerY = height / 2, radius = Math.min(centerX, centerY);
		const toCenter = (x: number, y: number) => Math.hypot(y - centerY, x - centerX);
		const getRadian = (x: number, y: number) => Math.atan2(y - centerY, x - centerX);
		for (let y = 0; y < height; y++)
			for (let x = 0; x < width; x++) {
				const distance = toCenter(x, y), radian = getRadian(x, y);
				if (distance > radius) continue;
				const newDistance = Math.asin(distance / radius) * 2 * radius / Math.PI;
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
	/**
	 * Apply a **Radial Blur** effect to the specified image data.
	 * @param imageData - The ImageData object of the image in the canvas.
	 * @returns A new ImageData object with the effect applied.
	 */
	radialBlur(imageData: ImageData) {
		const { width, height, data: pixels } = imageData;
		const newImageData = cloneImageData(imageData);
		const { data: newPixels } = newImageData;
		const iteration = 5, angle = 10;
		const radian = angle / 180 * Math.PI;
		const midX = width / 2, midY = height / 2;
		for (let i = 0; i < height; i++)
			for (let j = 0; j < width; j++) {
				let xoffsetCCW: number, yoffsetCCW: number, xoffsetCW: number, yoffsetCW: number;
				xoffsetCCW = xoffsetCW = j - midX;
				yoffsetCCW = yoffsetCW = i - midY;

				let rSum: number, gSum: number, bSum: number, aSum: number;
				rSum = gSum = bSum = aSum = 0;
				let count = 0;

				count++;
				const index = getPixelIndex(width, j, i);
				rSum += pixels[index];
				gSum += pixels[index + 1];
				bSum += pixels[index + 2];
				aSum += pixels[index + 3];

				for (let k = 0; k < iteration; k++) {
					// Counterclockwise (forward) offset
					let xoffset = xoffsetCCW;
					let yoffset = yoffsetCCW;

					// Calculate coordinates through interpolation
					xoffsetCCW = xoffset - radian * yoffset / iteration - radian ** 2 * xoffset / iteration ** 2;
					yoffsetCCW = yoffset - radian * xoffset / iteration - radian ** 2 * yoffsetCCW / iteration ** 2;

					let i0 = yoffsetCCW + midY;
					let j0 = xoffsetCCW + midX;

					if (i0 >= 0 && i0 < height && j0 >= 0 && j0 < width) {
						count++;
						const index = getPixelIndex(width, j0, i0);
						rSum += pixels[index];
						gSum += pixels[index + 1];
						bSum += pixels[index + 2];
						aSum += pixels[index + 3];
					}

					// Clockwise offset
					xoffset = xoffsetCW;
					yoffset = yoffsetCW;

					xoffsetCW = xoffset + radian * yoffset / iteration - radian ** 2 * xoffset / iteration ** 2;
					yoffsetCW = yoffset - radian * xoffset / iteration ** 2 - radian ** 2 * yoffset / iteration ** 2;

					i0 = yoffsetCW + midY;
					j0 = xoffsetCW + midX;

					if (i0 >= 0 && i0 < height && j0 >= 0 && j0 < width) {
						count++;
						const index = getPixelIndex(width, j0, i0);
						rSum += pixels[index];
						gSum += pixels[index + 1];
						bSum += pixels[index + 2];
						aSum += pixels[index + 3];
					}
				}

				newPixels[index] = rSum / count + 0.5;
				newPixels[index + 1] = gSum / count + 0.5;
				newPixels[index + 2] = bSum / count + 0.5;
				newPixels[index + 3] = aSum / count + 0.5;
			}
		return newImageData;
	},
};

interface ISaved {
	imagePath: string;
	filters: FilterBlobs;
	setImagePath(imagePath: string): void;
	setFilter(name: FilterType, blob: string): void;
}

const useSaved = createStore<ISaved>()(
	zustandImmer(set => ({
		imagePath: "",
		filters: {} as FilterBlobs,
		setImagePath: imagePath => set(() => ({ imagePath })),
		setFilter: (name, blob) => set(saved => void (saved.filters[name] = blob)),
	})),
);

/**
 * Returns a function that applies a set of image filters to an image.
 *
 * @param imagePath - The path of the image to be filtered.
 * @returns A map of filter names to their corresponding URLs.
 */
export function useCanvasFilter(imagePath: string) {
	const saved = useSaved();

	const getCanvasFilter = useCallback(async (imagePath: string) => {
		if (imagePath === useSaved.getState().imagePath) return saved.filters;
		else {
			saved.setImagePath(imagePath);
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
		for (const [name, filter] of entries(filters)) {
			const newImageData = filter(imageData);
			context.putImageData(newImageData, 0, 0);
			const blob = await new Promise<string>(resolve => canvas.toBlob(blob => resolve(URL.createObjectURL(blob!))));
			saved.setFilter(name, blob);
		}

		return useSaved.getState().filters;
	}, [imagePath]);

	useEffect(() => {
		getCanvasFilter(imagePath);
	}, [imagePath]);

	return saved.filters;
}
