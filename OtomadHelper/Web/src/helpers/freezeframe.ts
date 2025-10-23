import prvePingpong from "assets/images/effects/prve_pingpong.gif";
import prveWhirl from "assets/images/effects/prve_whirl.webp";

export const freezeframes = {} as Record<string, string>;

const images = {
	// ...import.meta.glob<string>("/src/assets/images/**/*.gif", { import: "default", eager: true }),
	// ...import.meta.glob<string>("/src/assets/images/**/*.apng", { import: "default", eager: true }),
	// ...import.meta.glob<string>("/src/assets/images/**/*.webp", { import: "default", eager: true }),
	"effects/prve_pingpong.gif": prvePingpong,
	"effects/prve_whirl.webp": prveWhirl,
};
const canvas = document.createElement("canvas");
const context = canvas.getContext("2d")!;
for (const [key, imageSrc] of Object.entries(images)) {
	const image = new Image();
	await new Promise(resolve => {
		image.onload = resolve;
		image.src = imageSrc;
	});
	canvas.width = image.width;
	canvas.height = image.height;
	context.drawImage(image, 0, 0);
	const dataUrl = canvas.toDataURL();
	freezeframes[key] = dataUrl;
}
