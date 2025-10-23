const pages = import.meta.glob<FC>("./**/*.tsx", { base: "/src/views", import: "default", eager: true });

function EmptyPage() {
	return <div className="container"><EmptyMessage icon="settings" title={t.underConstruction} spinAtBegin noSideEffect /></div>;
}

const navItems = ["home", "source", "score", "audio", "visual", "track", "sonar", "lyrics", "shupelunker", "ytp"] as const;
const navToolItems = ["management", "mosh", "tools"] as const;
const bottomNavItems = ["settings"] as const;

export function redirectIcon(name: string): DeclaredIcons & DeclaredLotties {
	const redirects = {
		source: "video_clip_multiple",
		score: "instrument",
		audio: "volume",
		visual: "image",
		track: "layer",
		shupelunker: "slice",
		tools: "apps",
		management: "library",
		// Subpages
		prve: "sparkle",
		staff: "g_clef",
		pixelScaling: "pixelate_zoom",
	} as const satisfies Record<string, DeclaredIcons | DeclaredLotties>;
	return (hasOwn(redirects, name) ? redirects[name] : name) as DeclaredIcons & DeclaredLotties;
}

const isCompleteAvailable = (page: string[]) => !["management", "mosh", "tools", "settings"].includes(page[0]);
const isAutoLayoutTracks = (page: string[]) => page.length >= 2 && page[0] === "track";

const getTitle = (viewName: string, context: "long" | "full" | "short", plural?: number) => {
	const _context = context === "short" ? undefined : context;
	const t = (context?: string) => {
		const key = new VariableName(viewName).camel;
		if (!i18nExists(t => t.titles[key], context)) return;
		return i18n.t(`titles.${key}`, { count: plural, context });
	};
	const contexts = ["long", "full", undefined] as const;
	const ctx = contexts.slice(contexts.indexOfDefault(_context) ?? 2).firstDefined(context => t(context)) ?? t();
	return ctx;
};

export default function ShellPage() {
	const { page, changePage, pagePath, transition, canBack, back, reset, setPageContentId, poppedScroll, commandBarDisabled, pageChangeResolver } = useSnapshot(pageStore);
	const pageTitles = page.map((crumb, i, { length }) => {
		try {
			return {
				name: getTitle(crumb, "full"),
				link: i === length - 1 ? undefined : page.slice(0, i + 1),
			};
		} catch (error) {
			reset();
			throw error;
		}
	});
	const Page = pages[`./${pagePath}.tsx`] ?? EmptyPage;
	const uiScale1 = useUiScale1();
	const zoom = uiScale1 === 1 ? undefined : uiScale1;
	const { appName } = useAboutApp();
	const { enabled: enablePixelScaling } = useSnapshot(configStore.visual.pixelScaling);
	const documentTitle = (() => {
		const lastPage = page.last();
		return (lastPage ? getTitle(lastPage, "long") + " - " : "") + appName;
	})();
	const pageContentId = useId();
	setPageContentId(pageContentId);
	const searchValue = useState("");

	useEffect(() => {
		document.body.classList.toggle("pixelated", enablePixelScaling);
	}, [enablePixelScaling]);

	const completeDisabled = !isCompleteAvailable(page);
	const autoLayoutTracksMode = isAutoLayoutTracks(page);
	const navBadges = useSnapshot(navBadgeStore);
	const getBadge = (id: string) => hasOwn(navBadges, id) ? navBadges[id] : undefined;

	return (
		<NavigationView
			currentNav={[page, changePage]}
			navItems={[
				...navItems.map(item => ({ text: getTitle(item, "short"), id: item, icon: redirectIcon(item), animatedIcon: redirectIcon(item), badge: getBadge(item) })),
				{ type: "hr" },
				...navToolItems.map(item => ({ text: getTitle(item, "short", 2), id: item, icon: redirectIcon(item), animatedIcon: redirectIcon(item) })),
				...bottomNavItems.map(item => ({ text: getTitle(item, "short", 2), id: item, icon: redirectIcon(item), animatedIcon: redirectIcon(item), bottom: true })),
			]}
			titles={pageTitles}
			transitionName={transition}
			canBack={canBack}
			onBack={back}
			pageContentId={pageContentId}
			poppedScroll={poppedScroll}
			commandBar={(
				<CommandBar disabled={commandBarDisabled}>
					{
						...autoLayoutTracksMode ? [
							<CommandBar.Item key="save" icon="save" caption={t.save} onClick={() => { pageStore.onSave?.(); back(); }} />,
							<CommandBar.Item key="applyToSelectedTracks" caption={t.track.applyToSelectedTracks} icon="arrow_sync_checkmark" />,
						] : [
							<CommandBar.Item
								key="complete"
								icon="checkmark"
								caption={t.complete}
								disabled={completeDisabled}
								canBeDisabled
								onClick={() => completeDisabled && alert("Cannot complete!")}
							/>,
						]
					}
				</CommandBar>
			)}
			searchValue={searchValue}
			onSearch={props => <HandleSearchResults {...props} />}
			onEnter={() => pageChangeResolver?.resolve()}
			style={{ zoom, "--zoom": zoom }} // TODO: Use webview2 native zoom function.
		>
			<title>{documentTitle}</title>
			<Page />
		</NavigationView>
	);
}
