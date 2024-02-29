const pages = import.meta.glob<FC>("/src/views/**/*.tsx", { import: "default", eager: true });

function EmptyPage() {
	return <EmptyMessage icon="settings" heading={t.underConstruction} />;
}

const navItems = ["home", "source", "score", "audio", "visual", "track", "sonar", "lyrics", "shupelunker", "ytp"];
const navToolItems = ["mosh", "tools"];
const bottomNavItems = ["settings"] as const;

const isCompleteAvailable = (page: string[]) => !["mosh", "tools", "settings"].includes(page[0]);
const isAutoLayoutTracks = (page: string[]) => page.length >= 2 && page[0] === "track";

export default function ShellPage() {
	const { page, changePage, getPagePath, transition, canBack, back, reset } = usePageStore();
	const pageTitles = page.map((crumb, i, { length }) => {
		try {
			return {
				name: t.titles[crumb]({ context: "full" }),
				link: i === length - 1 ? undefined : page.slice(0, i + 1),
			};
		} catch (error) {
			reset();
			throw error;
		}
	});
	const Page = pages[`/src/views/${getPagePath()}.tsx`] ?? EmptyPage;
	const [uiScale] = selectConfig(c => c.settings.uiScale);
	const documentTitle = (() => {
		const lastPage = page.at(-1);
		return (lastPage ? t.titles[lastPage]({ context: "full" }) + " - " : "") + import.meta.env.VITE_APP_NAME;
	})();
	const completeDisabled = !isCompleteAvailable(page);
	const autoLayoutTracksMode = isAutoLayoutTracks(page);

	useEffect(() => {
		document.title = documentTitle;
	}, [documentTitle]);

	return (
		<NavigationView
			currentNav={[page, changePage]}
			navItems={[
				...navItems.map(item => ({ text: t.titles[item], id: item, animatedIcon: item })),
				{ type: "hr" },
				...navToolItems.map(item => ({ text: t(2).titles[item], id: item, animatedIcon: item })),
				...bottomNavItems.map(item => ({ text: t(2).titles[item], id: item, animatedIcon: item, bottom: true })),
			]}
			titles={pageTitles}
			transitionName={transition}
			canBack={canBack()}
			onBack={back}
			commandBar={(
				<CommandBar onClick={() => completeDisabled && alert("Cannot complete!")}>
					{
						autoLayoutTracksMode ? (
							<>
								<Button icon="save" subtle onClick={back}>{t.save}</Button>
								<Button icon="checkmark" subtle>{t.track.applyToSelectedTracks}</Button>
							</>
						) :
							<Button icon="checkmark" subtle disabled={completeDisabled}>{t.complete}</Button>
					}
				</CommandBar>
			)}
			style={{ zoom: uiScale === 100 ? undefined : uiScale / 100 }}
		>
			<Page />
		</NavigationView>
	);
}
