import type { YtpEffectName } from "views/ytp";

const availableEffects = ["changeHue", "rotateHue", "monochrome", "negative", "repeatRapidly", "randomTuning", "upsize", "mirror", "highContrast", "oversaturation", "emphasizeThrice", "spectrum", "thermal", "emboss", "bump", "edge"] satisfies YtpEffectName[];

export default function PreviewModeYtp({ thumbnail }: FCP<{
	/** Thumbnail. */
	thumbnail: string;
}>) {
	const [name, setName] = useState("");
	const timeoutId = useRef<Timeout>(undefined);
	const dispose = () => clearTimeout(timeoutId.current);

	function nextEffect() {
		dispose();
		setName(availableEffects.randomOne());
		const nextTimeout = randBetween(1000, 2000);
		timeoutId.current = setTimeout(nextEffect, nextTimeout);
	}

	useEffect(() => {
		nextEffect();
		return dispose;
	}, [thumbnail]);

	return <PreviewYtp thumbnail={thumbnail} name={name} key={name} />;
}
