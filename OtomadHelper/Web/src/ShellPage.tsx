const pages = import.meta.glob<FC>("/src/views/**/*.tsx", { import: "default", eager: true });

function EmptyPage() {
	return <div className="container"><EmptyMessage icon="settings" title={t.underConstruction} spinAtBegin noSideEffect /></div>;
}

const navItems = ["home", "source", "score", "audio", "visual", "track", "sonar", "lyrics", "shupelunker", "ytp"];
const navToolItems = ["management", "mosh", "tools"];
const bottomNavItems = ["settings"] as const;

const isCompleteAvailable = (page: string[]) => !["mosh", "tools", "settings"].includes(page[0]);
const isAutoLayoutTracks = (page: string[]) => page.length >= 2 && page[0] === "track";

const getTitle = (viewName: string, full: boolean = false, plural?: number) => {
	const tp = plural !== undefined ? t(plural) : t;
	const str = tp.titles[new VariableName(viewName).camel];
	const ctx = full ? str({ context: "full" }) : str;
	return ctx;
};

export default function ShellPage() {
	const { page, changePage, pagePath, transition, canBack, back, reset, setPageContentId, poppedScroll } = useSnapshot(pageStore);
	const pageTitles = page.map((crumb, i, { length }) => {
		try {
			return {
				name: getTitle(crumb, true),
				link: i === length - 1 ? undefined : page.slice(0, i + 1),
			};
		} catch (error) {
			reset();
			throw error;
		}
	});
	const Page = pages[`/src/views/${pagePath}.tsx`] ?? EmptyPage;
	const { uiScale } = useSnapshot(configStore.settings);
	const zoom = uiScale === 100 ? undefined : uiScale / 100;
	const { appName } = useAboutApp();
	const { enabled: enablePixelScaling } = useSnapshot(configStore.visual.pixelScaling);
	const documentTitle = (() => {
		const lastPage = page.last();
		return (lastPage ? getTitle(lastPage, true) + " - " : "") + appName;
	})();
	const pageContentId = useId();
	setPageContentId(pageContentId);

	useEffect(() => { // TODO: React 19 to use new method.
		document.title = documentTitle;
	}, [documentTitle]);
	useEffect(() => { // TODO: React 19 to use new method.
		document.body.classList.toggle("pixelated", enablePixelScaling);
	}, [enablePixelScaling]);

	const completeDisabled = !isCompleteAvailable(page);
	const autoLayoutTracksMode = isAutoLayoutTracks(page);

	return (
		<NavigationView
			currentNav={[page, changePage]}
			navItems={[
				...navItems.map(item => ({ text: getTitle(item), id: item, icon: item, animatedIcon: item })),
				{ type: "hr" },
				...navToolItems.map(item => ({ text: getTitle(item, false, 2), id: item, icon: item, animatedIcon: item })),
				...bottomNavItems.map(item => ({ text: getTitle(item, false, 2), id: item, icon: item, animatedIcon: item, bottom: true })),
			]}
			titles={pageTitles}
			transitionName={transition}
			canBack={canBack}
			onBack={back}
			pageContentId={pageContentId}
			poppedScroll={poppedScroll}
			commandBar={(
				<CommandBar>
					{
						...(autoLayoutTracksMode ? [
							<CommandBar.Item key="save" icon="save" onClick={back}>{t.save}</CommandBar.Item>,
							<CommandBar.Item key="applyToSelectedTracks" icon="arrow_sync_checkmark">{t.track.applyToSelectedTracks}</CommandBar.Item>,
						] : [
							<CommandBar.Item key="complete" icon="checkmark" disabled={completeDisabled} canBeDisabled onClick={() => completeDisabled && alert("Cannot complete!")}>{t.complete}</CommandBar.Item>,
						])
					}
				</CommandBar>
			)}
			style={{ zoom, "--zoom": zoom }}
		>
			<Page />
		</NavigationView>
	);
}
