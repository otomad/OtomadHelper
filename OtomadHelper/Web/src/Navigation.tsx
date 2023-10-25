import { pageStore } from "stores/page";

const pages = import.meta.glob<FC>("pages/*.tsx", { import: "default", eager: true });

function EmptyPage() {
	return (
		<h1>未找到页面！</h1>
	);
}

const StyledPage = styled.main`
	height: 100%;

	&.exit {
		opacity: 0;
		translate: 0 -2rem;
	}

	&.enter {
		opacity: 0;
		translate: 0 7rem;
	}

	&.exit-active {
		transition: all ${eases.easeOutSmooth} 100ms;
	}

	&.enter-done {
		transition: all ${eases.easeOutSmooth} 600ms;
	}
`;

export default function Navigation() {
	const [currentNav, setCurrentNav] = pageStore.useState(true);
	const { texts: pageTitles, path: pagePath } = pageStore;
	const navItems = ["source", "midi", "audio", "visual", "sonar", "track"];
	const bottomNavItems = ["tools", "settings"];
	const Page = pages[`/src/pages/${pagePath}.tsx`] ?? EmptyPage;
	const pageNodeRef = useRef<HTMLElement>(null);

	return (
		<NavigationView
			currentNav={[currentNav, setCurrentNav]}
			navItems={[
				...navItems.map(item => ({ text: t[item], id: item })),
				...bottomNavItems.map(item => ({ text: t[item], id: item, bottom: true })),
			]}
			titles={pageTitles}
		>
			<SwitchTransition>
				<Transition nodeRef={pageNodeRef} key={pagePath} timeout={60}>
					<StyledPage ref={pageNodeRef}>
						<Page />
					</StyledPage>
				</Transition>
			</SwitchTransition>
		</NavigationView>
	);
}
