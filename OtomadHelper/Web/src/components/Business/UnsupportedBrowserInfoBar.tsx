import { doesBrowserSupportACertainFeature, useBrowserName } from "helpers/browserslist";

const HEIGHT = 48;

const StyledUnsupportedBrowserInfoBar = styled(InfoBar).attrs({
	status: "error",
})`
	block-size: ${HEIGHT}px;
	border: none;
	border-radius: revert;
`;

const isBrowserSupported = doesBrowserSupportACertainFeature();
export default function UnsupportedBrowserInfoBar() {
	const t = useT();
	const [browserName, updateLink] = useBrowserName();
	return !isBrowserSupported && (
		<StyledUnsupportedBrowserInfoBar
			title={<MarqueeIfOverflow>{t.descriptions.unsupportedBrowser({ browser: browserName })}</MarqueeIfOverflow>}
			length="short"
			button={updateLink && <Button href={updateLink}>{t.unsupportedBrowserClickToUpdate}</Button>}
		/>
	);
}

UnsupportedBrowserInfoBar.height = HEIGHT;
