const images = import.meta.glob<string>("/src/assets/images/**/*", { import: "default", eager: true });
for (const imagePath of Object.values(images))
	try {
		const image = new Image();
		image.src = imagePath;
	} catch { }
