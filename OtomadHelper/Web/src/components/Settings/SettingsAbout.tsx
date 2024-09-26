import ColoredLogo from "assets/svg/Otomad Helper Colored.svg?react";
import MonoLogo from "assets/svg/Otomad Helper Mono.svg?react";

const CONTRIBUTE_TRANSLATION_LINK = "https://crowdin.com/project/otomadhelper";

const StyledSettingsAbout = styled.div`
	display: flex;
	gap: 16px;
	margin-bottom: 10px;

	.logo {
		display: none;
		width: 150px;
		height: auto;
		content-visibility: auto;

		${ifColorScheme.light} &.light {
			display: block;
		}

		${ifColorScheme.dark} &.dark {
			display: block;
			fill: currentColor;
		}
	}

	.right {
		display: flex;
		flex-direction: column;
		gap: 5px;

		> div {
			display: flex;
			flex-wrap: wrap;
			column-gap: 10px;

			> * {
				display: inline-block;
			}
		}

		.pairs {
			color: ${c("fill-color-text-secondary")};
		}
	}
`;

export default function SettingsAbout() {
	const pairs = new Map<string, string>([
		[t.settings.about.author, t.settings.about.__author__],
		[t.settings.about.originalAuthor, t.settings.about.__originalAuthor__],
	]);
	if (t.metadata.__translator__.toString()) pairs.set(t.settings.about.translator, t.metadata.__translator__);
	const { version } = useAboutApp();
	const [showTranslators, setShowTranslators] = useState(false);

	return (
		<>
			<StyledSettingsAbout>
				<MonoLogo className="logo dark" />
				<ColoredLogo className="logo light" />
				<div className="right">
					<p>{t.descriptions.settings.about}</p>
					<div className="pairs">
						{Array.from(pairs.entries(), ([key, value]) => <span key={key}>{key + t.colon + value}</span>)}
					</div>
					<div>
						<Link href="https://otomadhelper.readthedocs.io/">{t.settings.about.documentation}</Link>
						<Link href="https://github.com/otomad/OtomadHelper/releases">{t.settings.about.changeLog}</Link>
						<Link href="https://github.com/otomad/OtomadHelper">{t.settings.about.repositoryLink}</Link>
						<Link href="https://github.com/otomad/OtomadHelper/issues">{t.settings.about.feedback}</Link>
						<Link href="https://www.gnu.org/licenses/gpl-3.0.html">{t.settings.about.license}</Link>
						<Link onClick={() => setShowTranslators(true)}>{t.settings.about.translators}</Link>
						<Link href={CONTRIBUTE_TRANSLATION_LINK}>{t.settings.about.translation}</Link>
					</div>
					<Translators shown={[showTranslators, setShowTranslators]} />
				</div>
			</StyledSettingsAbout>
			<SettingsCard title={t.settings.about.version} icon="sync">
				<p>v{version}</p>
				<Button onClick={() => checkForUpdates(version)}>{t.settings.about.checkForUpdates}</Button>
			</SettingsCard>
		</>
	);
}

async function checkForUpdates(currentVersion: string) {
	const byApi = fetch("https://api.github.com/repos/otomad/OtomadHelper/releases/latest")
		.then(res => res.json())
		.then(data => data.tag_name as string);
	const byRaw = fetch("https://raw.githubusercontent.com/otomad/OtomadHelper/webview2/version.txt")
		.then(res => res.text())
		.then(data => data.trim());
	const tagName = await Promise.race([byApi, byRaw]);
	const compare = new Version(tagName).compareTo(currentVersion);
	console.log(tagName, compare);
}

const StyledTranslatorsTable = styled.div`
	display: grid;
	grid-template-columns: repeat(2, 1fr);
	column-gap: 10px;

	p:nth-child(2n + 1) {
		text-align: end;
	}

	p.current {
		${styles.effects.text.bodyStrong};
		color: ${c("accent-color")};
	}

	+ p {
		margin-block-start: 1em;
	}
`;

function Translators({ shown: [shown, setShown] }: FCP<{
	/** Show the content dialog? */
	shown: StateProperty<boolean>;
}>) {
	const currentLanguage = useCurrentLanguage();
	const languages = useLanguageTags();
	const translatorNames = languages.mapObject(lang => [lang, t({ lng: lang }).metadata.__translator__]);

	const availableLanguageNames = {
		original: languages.mapObject(lang => [lang, t({ lng: lang }).metadata.name]),
		english: languages.mapObject(lang => [lang, t({ lng: "en" }).settings.language[lang]]),
		current: languages.mapObject(lang => [lang, t.settings.language[lang]]),
	};
	const languageDisplayNames = keys(availableLanguageNames);
	const [displayName, setDisplayName] = useState<typeof languageDisplayNames[number]>("original");
	const nextDisplayName = useCallback(() => setDisplayName(name => {
		const getNext = (current: typeof name) => languageDisplayNames[(languageDisplayNames.indexOf(current) + 1) % languageDisplayNames.length];
		let next = getNext(name);
		if (currentLanguage === "en" && next === "english") next = getNext(next);
		return next;
	}), [currentLanguage]);
	const languageNames = availableLanguageNames[displayName];

	useInterval(nextDisplayName, 2000);

	return (
		<ContentDialog
			shown={[shown, setShown]}
			title={t.settings.about.translators}
			buttons={close => (
				<>
					<Button onClick={() => window.open(CONTRIBUTE_TRANSLATION_LINK)}>{t.settings.about.translation}</Button>
					<Button autoFocus accent onClick={close}>{t.ok}</Button>
				</>
			)}
		>
			<StyledTranslatorsTable>
				{languages.map(lang => {
					const langAttr = displayName === "english" ? "en" : displayName === "original" ? lang : undefined;
					const isCurrentLang = { current: lang === currentLanguage };
					const translator = translatorNames[lang].toString();
					return (
						<Fragment key={lang}>
							<p lang={langAttr} className={isCurrentLang}>{languageNames[lang]}</p>
							<p lang={translator ? lang : "en"} className={isCurrentLang}>{translator || "—"}</p>
						</Fragment>
					);
				})}
			</StyledTranslatorsTable>
			<p>{t.descriptions.settings.translation}</p>
		</ContentDialog>
	);
}
