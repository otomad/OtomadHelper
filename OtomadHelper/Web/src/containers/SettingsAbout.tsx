import links from "helpers/links";
import SettingsAboutLogo from "../components/Business/SettingsAboutLogo";

const StyledSettingsAbout = styled.div`
	${styles.effects.text.body};
	display: contents;

	.about-info {
		${styles.effects.text.caption};
		align-self: center;
		inline-size: 80%;
		color: ${c("fill-color-text-secondary")};
		text-align-last: center;
		text-wrap: pretty;
		hyphens: manual;

		&:not(:lang(zh), :lang(ja), :lang(ko), :lang(th), :lang(lo), :lang(km), :lang(my)) {
			text-align: center;
		}
	}

	.collaborators,
	.links {
		display: flex;
		flex-wrap: wrap;
		// row-gap: 5px; // WARN: BUG in column-rule-outset, it will unexpectedly offset.
		justify-content: center;
		column-rule: 1px solid ${c("fill-color-text-tertiary")};
	}

	.collaborators {
		column-gap: 24px;
		column-rule-outset: -4px;

		.role {
			${styles.effects.text.caption};
			color: ${c("fill-color-text-secondary")};
		}

		.name {
			${styles.effects.text.body};
		}
	}

	.links {
		${styles.effects.text.bodyStrong};
		column-gap: 14px;
		column-rule: 3px dotted ${c("accent-color", 40)};
		column-rule-outset: -8px;
	}

	> * {
		margin-block-end: 0.3lh; // 6px ÷ (1lh := 20px)
	}
`;

export default function SettingsAbout() {
	const t = useT();
	const [currentLanguage] = useLanguage();
	const [hasTranslator, formattedTranslator] = listFormatTranslators(currentLanguage, currentLanguage);
	const collaborators = new Map<string, string>([
		[t.settings.about.author, t.settings.about.__author__],
		[t.settings.about.originalAuthor, t.settings.about.__originalAuthor__],
		[t.settings.about.translator, hasTranslator ? formattedTranslator : ""],
	]);
	const { version } = useAboutApp();
	const [showTranslators, setShowTranslators] = useState(false);
	const { pushPage } = useSnapshot(pageStore);
	const meta = metas.settings;

	return (
		<>
			<StyledSettingsAbout>
				<SettingsAboutLogo />
				<div className="collaborators">
					{Array.from(collaborators.entries(), ([key, value]) => !value ? undefined : (
						<div key={key}>
							<div className="role">{key}</div>
							<div className="name">{value}</div>
						</div>
					))}
				</div>
				<p className="about-info">{t.descriptions.settings.about}</p>
				<div className="links">
					<Link href={links.otomadHelper.documentation[currentLanguage]}>{t.settings.about.documentation}</Link>
					<Link href={links.otomadHelper.repository}>{t.settings.about.repositoryLink}</Link>
					<Link href={links.otomadHelper.changelog}>{t.settings.about.changelog}</Link>
					<Link href={links.otomadHelper.issues}>{t.settings.about.feedback}</Link>
					<Link onClick={() => pushPage("license")}>{t.titles.license}</Link>
					<Link href={links.crowdin.contributeTranslation[currentLanguage]}>{t.settings.about.translation}</Link>
				</div>
				<Translators shown={[showTranslators, setShowTranslators]} />
			</StyledSettingsAbout>
			<Setting
				meta={meta.version}
				actions={(
					<>
						<p>v{version}</p>
						<Button icon="arrow_sync" onClick={() => checkForUpdates(version)}>{t.settings.about.checkForUpdates}</Button>
					</>
				)}
			>
				<AboutInformation />
				<Expander.ChildWrapper $tilePadding="subtle button to item">
					<Attrs hyperlink minWidthUnbounded>
						<Button href={links.gpl3}>{t.titles.license({ context: "full" })}</Button>
						<Button href={links.otomadHelper.credits}>{t.settings.about.credits}</Button>
						<Button onClick={() => setShowTranslators(true)} aria-haspopup="dialog">{t.settings.about.translators}</Button>
					</Attrs>
				</Expander.ChildWrapper>
			</Setting>
			<Setting meta={meta.help}>
				<HelpLinks />
			</Setting>
		</>
	);
}

async function checkForUpdates(currentVersion: string) {
	const byApi = fetch(links.api.versionTagGitHubApi)
		.then(res => res.json())
		.then(data => data.tag_name as string);
	const byRaw = fetch(links.api.versionTagGitHubRaw)
		.then(res => res.text())
		.then(data => data.trim());
	const tagName = await Promise.race([byApi, byRaw]);
	const compare = new Version(tagName).compareTo(currentVersion);
	console.log(tagName, compare);
}

const StyledTableBase = styled.div<{
	$table?: boolean;
}>(ifNotProp("$table", css`
	display: grid;
	grid-template-columns: repeat(2, 1fr);
	column-gap: 12px;
`, css`
	display: table;

	:is(td, th):not(:first-of-type) {
		border-inline-start: 12px solid transparent;
	}
`));

const StyledTranslatorsTable = styled(StyledTableBase)`
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
	const [currentLanguage] = useLanguage();
	const languages = useLanguageTags();
	const t = useT();

	const availableLanguageNames = {
		original: languages.mapObject(lang => [lang, t.metadata.name({ lng: lang })]),
		english: languages.mapObject(lang => [lang, getLocaleName(lang, "en")]),
		current: languages.mapObject(lang => [lang, getLocaleName(lang, currentLanguage)]),
	};
	const languageDisplayNames = keys(availableLanguageNames);
	const [displayName, setDisplayName] = useState<typeof languageDisplayNames[number]>("original");
	const nextDisplayName = useCallback(() => setDisplayName(name => {
		const getNext = (current: typeof name) => languageDisplayNames[(languageDisplayNames.indexOf(current) + 1) % languageDisplayNames.length];
		let next = getNext(name);
		if (isEnglish(currentLanguage) && next === "english") next = getNext(next);
		return next;
	}), [currentLanguage, languageDisplayNames]);
	const languageNames = availableLanguageNames[displayName];

	useInterval(nextDisplayName, 2000);

	return (
		<ContentDialog
			shown={[shown, setShown]}
			title={t.settings.about.translators}
			buttons={close => (
				<>
					<Button onClick={() => window.open(links.crowdin.contributeTranslation[currentLanguage])}>{t.settings.about.translation}</Button>
					<Button autoFocus accent onClick={close}>{t.ok}</Button>
				</>
			)}
		>
			<StyledTranslatorsTable>
				{languages.map(lang => {
					const langAttr = displayName === "english" ? "en" : displayName === "original" ? lang : undefined;
					const isCurrentLang = { current: lang === currentLanguage };
					const [hasTranslator, formattedTranslator] = listFormatTranslators(lang, langAttr ?? currentLanguage);
					return (
						<Fragment key={lang}>
							<p lang={langAttr} className={isCurrentLang}>{languageNames[lang]}</p>
							<p lang={hasTranslator ? lang : "en"} className={isCurrentLang}>{formattedTranslator}</p>
						</Fragment>
					);
				})}
			</StyledTranslatorsTable>
			<p>{t.descriptions.settings.translation}</p>
		</ContentDialog>
	);
}

const StyledAboutInformation = styled(ItemsView)`
	.card {
		block-size: 100%;
		overflow: hidden;
		color: ${c("foreground-color")};
		// word-break: break-word; // Deprecated, behaviors same as below.
		word-break: normal;
		overflow-wrap: anywhere;

		.base {
			display: flex;
			flex-direction: column;
			gap: 0.75rem;
			justify-content: space-between;
		}

		.name {
			${styles.effects.text.bodyStrong};
		}

		.version {
			${styles.effects.text.bodyLarge};
			margin-block-end: -0.25rem;
			color: ${c("fill-color-text-secondary")};
		}

		.icon {
			float: inline-end;
			margin-block-start: 2px;
			margin-inline-start: 4px;
			color: ${c("fill-color-text-secondary")};
			font-size: 16px;
		}
	}
`;

const $v = (name: string, version: string, link?: string) => ({ name, version, link });
function AboutInformation() {
	const { appName, version } = useAboutApp();
	const data = [
		$v(appName, "v" + version, links.otomadHelper.repository),
		$v("React", "v" + React.version, links.react),
	];

	return (
		<StyledAboutInformation
			view="grid"
			current={null}
			className="monospace"
			role="group"
			lang="en"
			translate="no"
		>
			{data.map(({ name, version, link }) => (
				<Card width={200} key={name} as={Link} href={link}>
					<p className="name"><Icon name="open" />{name}</p>
					<p className="version">{version}</p>
				</Card>
			))}
		</StyledAboutInformation>
	);
}

function listFormatTranslators_static(translators: string[] | string, lang: string) {
	if (typeof translators === "string" || isI18nItem(translators)) translators = translators.toString().split("\n").toTrimmed();
	const formatted = listFormat(translators, lang);
	return formatted;
}

export function listFormatTranslators(targetLanguage: string, displayLanguage: string): [hasTranslator: boolean, formattedTranslator: string] {
	const translators = t.metadata.__translator__({ lng: targetLanguage }), hasTranslator = translators.length > 0;
	const formattedTranslator = hasTranslator ? listFormatTranslators_static(translators, displayLanguage) : "—";
	return [hasTranslator, formattedTranslator];
}

function HelpLinks() {
	const [currentLanguage] = useLanguage();
	const tAbout = tAlias.settings.about;
	const helpsV4: Record<Intl.UnicodeBCP47LocaleIdentifier, { name: string; link: string; version?: string }[]> = {
		zh: [
			{ name: tAbout.documentation, version: "0.1", link: links.helpV4.chinese.documentation_chaosinism_v0_1 },
			{ name: tAbout.troubleshooting, version: "0.1", link: links.helpV4.chinese.troubleshooting_chaosinism_v0_1 },
			{ name: tAbout.tutorialVideo, version: "0.1", link: links.helpV4.chinese.tutorialVideo_chaosinism_v0_1 },
			{ name: tAbout.documentationForFeature({ feature: t.titles.staff({ context: "full" }) }), version: "0.1", link: links.helpV4.chinese.documentation_staffVisualizer_chaosinism_v0_1 },
			{ name: tAbout.releaseNotes, version: "4.9.25.0", link: links.helpV4.chinese.releaseNotes_v4_9_25_0 },
			{ name: tAbout.releaseNotes, version: "4.10.17.0", link: links.helpV4.chinese.releaseNotes_v4_10_17_0 },
			{ name: tAbout.tutorialVideo, version: "4.26.14.0", link: links.helpV4.chinese.tutorialVideo_v4_26_14_0 },
		],
		en: [
			{ name: tAbout.documentation, link: links.helpV4.english.documentation_evauation },
			{ name: tAbout.tutorialVideoForFeature({ feature: t.titles.ytp }), link: links.helpV4.english.tutorialVideo_ytpPlus },
			{ name: tAbout.tutorialVideoForFeature({ feature: t.mosh.datamosh }), version: "1.4.0", link: links.helpV4.english.tutorialVideo_datamosh_delthas_v1_4_0 },
			{ name: tAbout.tutorialVideo, version: "4.16.4.0", link: links.helpV4.english.tutorialVideo_greenBean_v4_16_4_0 },
			{ name: tAbout.tutorialVideo, version: "4.16.4.0", link: links.helpV4.english.tutorialVideo_cassidy_v4_16_4_0 },
			{ name: tAbout.tutorialVideo, version: "4.26.14.0", link: links.helpV4.english.tutorialVideo_v4_26_14_0 },
		],
		vi: [
			{ name: tAbout.tutorialVideo, version: "4.26.14.0", link: links.helpV4.vietnamese.tutorialVideo_cyahega_v4_26_14_0 },
		],
	};

	const icons: Record<string, DeclaredIcons> = {
		bilibili: "logo/bilibili",
		youtube: "logo/youtube",
		"docs.google": "logo/google_docs",
	};

	return (
		<>
			<Expander.Sub title={<b>{tAbout.previousVersionDocumentation + " (v4.x)"}</b>} noIndentation>
				{Object.entries(helpsV4).map(([language, links]) => (
					<Fragment key={language}>
						<Expander.Item title={tAbout.documentationInLanguage({ language: getLocaleName(language, currentLanguage), count: links.length })} noDivider />
						<Expander.ChildWrapper $noDivider>
							<StackPanel $gap={[8, 14]}>
								{links.map(({ name, version, link }) => {
									const icon = Object.entries(icons).find(([domain]) => link.includes(domain))?.[1];
									return (
										<Link key={link} href={link}>
											{icon && <Icon name={icon} filled style={{ marginInlineEnd: "0.35em" }} />}
											{name}{version ? ` (v${version})` : undefined}
										</Link>
									);
								})}
							</StackPanel>
						</Expander.ChildWrapper>
					</Fragment>
				))}
			</Expander.Sub>
		</>
	);
}
