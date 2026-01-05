/* eslint-disable import/order */
import LogoIconFluent from "assets/svg/Otomad Helper/fluent.svg?react";
import LogoIconMono from "assets/svg/Otomad Helper/mono.svg?react";
import LogoIconLiquidGlass from "assets/svg/Otomad Helper/liquid_glass.svg?react";
import LogoIconAero from "assets/svg/Otomad Helper/aero.svg?react";
import LogoTitle from "assets/svg/Otomad Helper/title.svg?react";
import LogoSubtitle from "assets/svg/Otomad Helper/subtitle.svg?react";

const floatLeft = keyframes`
	from {
		translate: 24px;
		opacity: 0;
	}
`;

const StyledSettingsAboutLogo = styled.div`
	display: inline-flex;
	flex-direction: column;
	direction: ltr;
	text-align: center;
	pointer-events: if(
		${ifColorScheme.dark} or ${ifColorScheme.contrast}: none;
		else: auto;
	);
	zoom: 1.325;

	.row-1 {
		display: flex;
		align-items: center;
		margin-top: -8px;
		margin-left: -8px;
		filter: if(
			${ifColorScheme.contrast} or ${ifColorScheme.reduceTransparency}: none;
			else: drop-shadow(0 3px 3px #21262c40);
		);
	}

	.icon-wrapper > * {
		&.light {
			display: if(
				${ifColorScheme.dark} or ${ifColorScheme.contrast}: none;
				else: block;
			);
			cursor: pointer;
		}

		&.dark {
			display: if(
				${ifColorScheme.dark} or ${ifColorScheme.contrast}: block;
				else: none;
			);
		}

		&:active {
			scale: 0.95;
		}
	}

	.page-content.jump main.page:is(.enter, .enter-done) & {
		.icon-wrapper {
			animation: ${keyframes`
				from {
					scale: 1.5;
					rotate: -45deg;
					opacity: 0;
				}
			`} 500ms ${eases.easeOutMax} backwards;
		}

		.title,
		.subtitle {
			animation: ${floatLeft} 500ms ${eases.easeOutMax} backwards;
		}
	}

	.subtitle {
		align-self: end;
		animation-delay: 250ms !important;
	}

	.title {
		margin: -2.5px;
		animation-delay: 125ms !important;
	}

	${ifColorScheme.at.light} {
		[data-icon-style="liquid glass"] ~ .title {
			margin-left: 9px;
		}
	}

	svg {
		display: block;
	}
`;

const StyledSettingsAboutLogoWrapper = styled.div`
	display: block;
	inline-size: 100%;
	margin-block-end: 0 !important;
	padding-block: 16px 8px;
	text-align: center;
	content-visibility: auto;
	animation: none;
`;

const OTOMAD_HELPER_LOGO = "Otomad Helper Logo"; // Abandoning localization.

export /* @internal */ default function SettingsAboutLogo() {
	const iconStyles = ["fluent", "aero", "liquid glass"] as const;
	const [iconStyle, setIconStyle] = useState<ValueOf<typeof iconStyles>>("fluent");
	const nextIconStyle = () => setIconStyle(iconStyle => iconStyles.nextItem(iconStyle));

	return (
		<StyledSettingsAboutLogoWrapper>
			<StyledSettingsAboutLogo role="img" aria-label={OTOMAD_HELPER_LOGO}>
				<div className="row-1">
					<div className="icon-wrapper" data-icon-style={iconStyle} onClick={nextIconStyle}>
						<Attrs className="light">
							{
								iconStyle === "liquid glass" ? <LogoIconLiquidGlass /> :
								iconStyle === "aero" ? <LogoIconAero /> :
								<LogoIconFluent />
							}
						</Attrs>
						<LogoIconMono className="dark" />
					</div>
					<LogoTitle className="title" />
				</div>
				<LogoSubtitle className="subtitle" />
			</StyledSettingsAboutLogo>
		</StyledSettingsAboutLogoWrapper>
	);
}
