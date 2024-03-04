const images = import.meta.glob<string>("/src/assets/images/**/*", { import: "default" });
for (const fetchImage of Object.values(images))
	fetchImage().then(imagePath => {
		try {
			const image = new Image();
			image.src = imagePath;
		} catch { }
	});
