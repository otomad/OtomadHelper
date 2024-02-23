import Logo from "assets/svg/Otomad Helper.svg";

const VERSION = "v7.9";

const StyledSettingsAbout = styled.div`
	display: flex;
	gap: 16px;
	margin-bottom: 10px;

	.logo {
		fill: currentColor;
		width: 150px;
		height: auto;
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
	if (t.__translator__.toString()) pairs.set(t.settings.about.translator, t.__translator__);

	return (
		<>
			<StyledSettingsAbout>
				<Logo className="logo" />
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
			<SettingsCard heading={t.settings.about.version} icon="sync">
				<p>{VERSION}</p>
				<Button>{t.settings.about.checkForUpdates}</Button>
			</SettingsCard>
		</>
	);
}
