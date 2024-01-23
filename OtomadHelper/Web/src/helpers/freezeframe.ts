export const freezeframes = {} as Record<string, string>;

const images = import.meta.glob<string>("../assets/images/**/*.gif", { import: "default", eager: true });
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
	freezeframes[key.replace("../assets/images/", "")] = dataUrl;
}
