import ColoredLogo from "assets/svg/Otomad Helper Colored.svg";
import MonoLogo from "assets/svg/Otomad Helper Mono.svg";

const StyledSettingsAbout = styled.div`
	display: flex;
	gap: 16px;
	margin-bottom: 10px;

	.logo {
		display: none;
		width: 150px;
		height: auto;

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

	return (
		<>
			<StyledSettingsAbout>
				<div className="logo" />
				<MonoLogo className="logo dark" />
				<ColoredLogo className="logo light" />
				<div className="right">
					<p>{t.descriptions.settings.about}</p>
					<div className="pairs">
						{Array.from(pairs.entries(), ([key, value]) => <span key={key}>{key + t.colon + value}</span>)}
					</div>
					<div>
						<OpenLink href="https://otomadhelper.readthedocs.io/">{t.settings.about.documentation}</OpenLink>
						<OpenLink href="https://github.com/otomad/OtomadHelper">{t.settings.about.repositoryLink}</OpenLink>
					</div>
				</div>
			</StyledSettingsAbout>
			<SettingsCard title={t.settings.about.version} icon="sync">
				<p>v{version}</p>
				<Button>{t.settings.about.checkForUpdates}</Button>
			</SettingsCard>
		</>
	);
}
