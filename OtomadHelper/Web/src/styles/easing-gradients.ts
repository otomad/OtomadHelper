// See: https://github.com/larsenwork/postcss-easing-gradients

import type { DataType } from "csstype";
import { easingCoordinates } from "easing-coordinates";
type ICoordinate = ReturnType<typeof easingCoordinates>[number];

interface EasingGradientOptions {
	/**
	 * Color stops. A lower number creates a more "low poly" gradient with less code but a higher risk of banding.
	 * @default 15
	 */
	colorStops?: number;
	/**
	 * Alpha decimals. A lower number can result in banding.
	 * @default 5
	 */
	alphaDecimals?: number;
	/**
	 * Color mode. The default color space used for interpolation and is closest to what most browsers use.
	 *
	 * Other options are "lrgb", "rgb", "hsl", "lab" and "lch" as per [chromajs](http://gka.github.io/chroma.js/#chroma-mix) documentation.
	 *
	 * @default "srgb-linear"
	 */
	colorMode?: "srgb" | "hsl" | "lab" | "lch" | "oklab" | "oklch" | "srgb-linear";
}

type ColorStop = [
	color: DataType.NamedColor | string & {},
	progress: string | string[],
	easing?: DataType.EasingFunction,
];

interface ColorStop_Standard {
	color: DataType.NamedColor | string & {};
	progress: string;
}

interface ColorStop_Normalized extends ColorStop_Standard {
	easing: DataType.EasingFunction;
}

export function easingGradient(type: "linear" | "radial" | "conic", towards: string, colorStops: ColorStop[], options: EasingGradientOptions = {}): string {
	options.colorStops ??= 13;

	const resultStops = normalizeGradientColorStops(colorStops).flatMap(({ color, progress, easing }, i, colorStops) => {
		if (easing === "linear" || i === colorStops.length - 1)
			return [{ color, progress }];
		else
			try {
				const colors: [string, string] = [color, colorStops[i + 1].color];
				const progresses: [string, string] = [progress, colorStops[i + 1].progress];
				const coordinates = easingCoordinates(
					easing,
					options.colorStops! - 1,
				);
				const parsedColorStops = getColorStops(
					colors,
					progresses,
					coordinates,
					options.alphaDecimals,
					options.colorMode,
				);
				return parsedColorStops;
			} catch {
				errorMsg({ color, progress, easing });
				return [{ color, progress }];
			}
	});

	return `${type}-gradient(${towards}, ${simplifyColorStops(resultStops)})`;
}

/**
 * Calculate the color stops based on start+stopColor in an array and easingType.
 * @param colors - Two colors in an array.
 * @param progresses - Two stop progress percents in an array.
 * @param coordinates - An array of coordinates (object with x and y keys).
 * @param alphaDecimals - How many decimals should be on the returned color values.
 * @param colorMode - Color space used for color interpolation http://gka.github.io/chroma.js/#chroma-mix.
 * @returns An array of colorStops (a string with color and position).
 */
function getColorStops(
	colors: [string, string],
	progresses: [string, string],
	coordinates: ICoordinate[],
	alphaDecimals: number = 5,
	colorMode: NonNullable<EasingGradientOptions["colorMode"]> = "srgb-linear",
): ColorStop_Standard[] {
	return mapCoordinateToGradientProgress(coordinates, progresses, alphaDecimals).map(({ progress, amount }) => {
		const color = `color-mix(in ${colorMode}, ${colors[0]}, ${colors[1]} ${amount})`;
		// const color = chroma.mix(colors[0], colors[1], amount, colorMode).css("hsl");
		return { color, progress };
	});
}

function normalizeGradientColorStops(colorStops: ColorStop[]): ColorStop_Normalized[] {
	return colorStops.flatMap(([color, progresses, easing = "linear"]) => {
		if (!Array.isArray(progresses)) return { color, progress: progresses, easing };
		else return progresses.map((progress, i) => {
			const isLast = i === progresses.length - 1;
			return { color, progress, easing: isLast ? easing : "linear" };
		});
	});
}

function mapCoordinateToGradientProgress(coordinates: ICoordinate[], [start, end]: [string, string], decimalPlaces: number) {
	const PERCENT_DENOMINATOR = 100;
	const toPercent = (decimal: number) => `${(decimal * PERCENT_DENOMINATOR).toFixedNumber(decimalPlaces - Math.log10(PERCENT_DENOMINATOR))}%` as const;
	const toDecimal = (percent: string) => +percent.slice(0, -1) / PERCENT_DENOMINATOR;
	return coordinates.map(({ x, y }) => {
		const amount = toPercent(y);
		let progress: string;
		if (start.endsWith("%") && end.endsWith("%")) {
			const startValue = toDecimal(start), endValue = toDecimal(end);
			const progressValue = (endValue - startValue) * x + startValue;
			progress = toPercent(progressValue);
		} else
			progress = `calc((${end} - ${start}) * ${x.toFixedNumber(decimalPlaces)} + ${start})`;
		return { progress, amount };
	});
}

function simplifyColorStops(colorStops: ColorStop_Standard[]) {
	const results: { color: string; progresses: string[] }[] = [];
	for (const stop of colorStops)
		if (stop.progress === "0%") stop.progress = "";
		else break;
	for (let i = colorStops.length - 1; i >= 0; i--) {
		const stop = colorStops[i];
		if (stop.progress === "100%") stop.progress = "";
		else break;
	}
	for (const { color, progress } of colorStops) {
		const prevStop = results.at(-1);
		if (prevStop?.color === color && progress && !prevStop.progresses.includes(progress))
			prevStop.progresses.push(progress);
		else
			results.push({ color, progresses: progress ? [progress] : [] });
	}
	return results.map(({ color, progresses }) => [color, ...progresses].join(" ")).join(", ");
}

/**
 * Wrap a string telling the user we couldn't parse it.
 * @param input - A string.
 * @returns The full error message wrapped around the string.
 */
function errorMsg(input: unknown) {
	console.error("Couldn't parse:\n",
		input,
		"\nCheck the syntax to see if it's correct/supported.",
	);
}
