import { BasicColorPalette, autoColorPalettes } from "helpers/basic-color-palette";
import { useInContextLocalization } from "helpers/jipt-activator";
import links from "helpers/links";

/** Expand the expanders in settings initially? (Do not set it to true in production!) */
const DEV_EXPANDED = true;

export /* @internal */ const systemBackdrops = [
	{ name: "acrylic", enum: "TransientWindow" },
	{ name: "mica", enum: "MainWindow" },
	{ name: "micaAlt", enum: "TabbedWindow" },
	{ name: "solid", enum: "None" },
] as const;

const StyledColorPalette = styled(Expander.ChildWrapper).attrs({
	role: "radiogroup",
})`
	display: flex;
	flex-wrap: wrap;
	gap: 8px;
	padding-block: 0 ${expanderItemPadding[0] * 2}px;
	padding-inline: ${expanderItemPadding[1] + 3}px;
	border-block-start: none !important;
`;

/* const BackgroundImageItemStyle = createGlobalStyle`
	.background-image-item:not(.sortable-overlay *, .dragging, .dropping) {
		@starting-style {
			scale: 0;
		}
	}
`; */

const TooltipPartial = Tooltip.with({ placement: "y" });

export default function Settings() {
	const [currentLanguage, setLanguage] = useLanguage();
	const languages = useLanguageTags();
	const inContextLocalization = useInContextLocalization();
	const systemContrast = useMediaQuery.contrast();
	const reduceTransparency = useMediaQuery.reduceTransparency();
	const schemes = ["light", "dark", "auto"] as const;
	const { scheme: [scheme, setScheme], amoledDark: [amoledDark, setAmoledDark], contrast: [contrast, setContrast] } = useStoreState(colorModeStore);
	const { black: actualAmoledDark, contrast: actualContrast } = useActualColorScheme();
	const {
		fontSize, hideUseTips, autoSwitchSourceFrom, autoCollapsePrveClasses,
		backgroundImageOpacity, backgroundImageTint, backgroundImageBlur, systemBackdrop, accentColor, backgroundColor,
	} = useSelectConfig(c => c.settings);
	const backgroundImages = useBackgroundImages();

	// Dev mode
	const { devMode, rtl } = useStoreState(devStore);

	async function addBackgroundImage() {
		const files = await openFile({ accept: "image/*", multiple: true });
		for (const file of files)
			await backgroundImages.add(file);
	}

	const isAutoColor = (color: string) => autoColorPalettes.includes(color);
	const isCustomColorSelected = (color: string) => !autoColorPalettes.includes(color) && !BasicColorPalette.values.includes(color);
	const getComputedPaletteColor = (kind: "accent" | "background") => () => {
		const color = kind === "background" ? backgroundColor[0] : accentColor[0];
		return isAutoColor(color) ? getComputedStyle(document.documentElement).getPropertyValue(`--${kind}-color-${color}`) : color;
	};
	const autoColorToIcon = (color: ValueOf<typeof autoColorPalettes>) => (color.in("wallpaper") ? color : `logo/${color}`) satisfies DeclaredIcons;

	globals.move = (oldIndex: number, newIndex: number) => backgroundImages.reorder(backgroundImages.items[oldIndex + 1].key, newIndex); // DELETE: DEBUG ONLY.

	return (
		<div className="container">
			<SettingsAbout />
			<ExpanderRadio
				title={<>{t.settings.language}{t.settings.language.toString() !== "Language" && <span lang="en"> / Language</span>}</>}
				icon="globe"
				items={languages}
				expanded={DEV_EXPANDED}
				view="grid"
				value={[currentLanguage, setLanguage]}
				idField
				nameField={language => getLocaleName(language, currentLanguage)}
				checkInfoCondition={t.metadata.name}
				imageField={language => <PreviewLanguage language={language} />}
				detailsField={language => {
					const [hasTranslator, formattedTranslator] = listFormatTranslators(language, currentLanguage);
					return hasTranslator ? formattedTranslator : undefined;
				}}
				itemsViewItemAttrs={{ withBorder: true }}
				itemsViewAttrs={{ style: { paddingBlockEnd: 0 } }}
				readOnly={inContextLocalization[0]}
				before={inContextLocalization[0] && <InfoBar status="warning">{t.descriptions.settings.language.enableInContextLocalization}</InfoBar>}
			>
				<Expander.Item title={t.descriptions.settings.translation} noDivider>
					<Button hyperlink minWidthUnbounded extruded href={links.crowdin.contributeTranslation[currentLanguage]}>{t.settings.about.translation}</Button>
				</Expander.Item>
				<ToggleSwitch
					icon="logo/crowdin"
					on={inContextLocalization}
					details={inContextLocalization[0] ? t.descriptions.settings.language.translating : t.descriptions.settings.language.improveTranslation}
				>
					{inContextLocalization[0] ? t.settings.language.translating : t.settings.language.improveTranslation}
				</ToggleSwitch>
			</ExpanderRadio>

			<Subheader>{t.settings.appearance}</Subheader>
			<Expander
				title={t.settings.appearance.colorScheme}
				icon="paint_brush"
				checkInfo={withObject(t.settings.appearance.colorScheme, t => actualContrast ? t.contrast : scheme === "dark" && amoledDark ? t.black : t[scheme])}
				expanded={DEV_EXPANDED}
			>
				{!systemContrast ? (
					<>
						<ItemsView
							view="grid"
							current={[scheme, setScheme]}
						>
							{schemes.map(scheme =>
								<ItemsView.Item id={scheme} key={scheme} withBorder image={<PreviewColorScheme colorScheme={scheme} />}>{t.settings.appearance.colorScheme[scheme]}</ItemsView.Item>)}
						</ItemsView>
						<ItemsView
							view="grid"
							current={null}
							multiple
						>
							<ItemsView.Item
								id="black"
								key="black"
								selected={[amoledDark, setAmoledDark]}
								image={<PreviewColorScheme colorScheme="black" />}
								details={t.descriptions.settings.appearance.colorScheme.black}
								style={{ opacity: scheme === "light" ? 0.5 : undefined }}
								baseAttrs={{ "data-scheme": classNames("dark black") }}
								withBorder
								disableCheckmarkTransition
							>
								{t.settings.appearance.colorScheme.black}
							</ItemsView.Item>
							<ItemsView.Item
								id="contrast"
								key="contrast"
								selected={[contrast, setContrast]}
								image={<PreviewColorScheme colorScheme="contrast" />}
								withBorder
								disableCheckmarkTransition
							>
								{t.settings.appearance.colorScheme.contrast}
							</ItemsView.Item>
						</ItemsView>
					</>
				) : (
					<>
						<InfoBar status="warning">{t.descriptions.settings.appearance.invalid.systemContrastCannot({ option: t.settings.appearance.colorScheme })}</InfoBar>
						<ItemsView
							view="grid"
							current={null}
							multiple
						>
							<ItemsView.Item id="contrast" key="contrast" selected="checked" image={<PreviewColorScheme colorScheme="contrast" />}>{t.settings.appearance.colorScheme.contrast}</ItemsView.Item>
						</ItemsView>
					</>
				)}
			</Expander>
			<Expander title={t.settings.appearance.palette} icon="color" expanded={DEV_EXPANDED}>
				{actualContrast ? <InfoBar status="warning">{t.descriptions.settings.appearance.invalid[systemContrast ? "systemContrastCannot" : "contrast"]({ option: t.settings.appearance.palette })}</InfoBar> : (
					<>
						<Expander.Item title={t.settings.appearance.palette.accent} icon="color_fill" asSubtitle />
						<StyledColorPalette>
							{autoColorPalettes.map(color => (
								<TooltipPartial key={color} title={t.settings.appearance.palette[color]}>
									<ColorButton
										color={color}
										value={accentColor}
										icon={autoColorToIcon(color)}
										colorAlt={isAutoColor(color) ? `var(--accent-color-${color})` : undefined}
										hidden={color === "wallpaper" && !backgroundImages.currentDominantColor}
										selected={color === "windows" && accentColor[0] === "wallpaper" && !backgroundImages.currentDominantColor}
										autoStartViewTransition
										coloredSelectedOutline
									/>
								</TooltipPartial>
							))}
							{BasicColorPalette.values.map(color => <ColorButton key={color} color={color} value={accentColor} autoStartViewTransition coloredSelectedOutline />)}
							<TooltipPartial title={t.custom}>
								<ColorPicker
									color={accentColor}
									computedColor={getComputedPaletteColor("accent")}
									selected={isCustomColorSelected(accentColor[0])}
									showIconWhenHovering={false}
									showSpectrumWhenUnselected
									autoStartViewTransition
									coloredSelectedOutline
								/>
							</TooltipPartial>
						</StyledColorPalette>
						{(actualContrast || actualAmoledDark) && <InfoBar status="warning">{t.descriptions.settings.appearance.invalid.blackScheme({ option: t.settings.appearance.palette.background })}</InfoBar>}
						<Attrs style={{ opacity: actualContrast || actualAmoledDark ? 0.5 : undefined }}>
							<Expander.Item title={t.settings.appearance.palette.background} icon="color_background" asSubtitle />
							<StyledColorPalette>
								{autoColorPalettes.map(color => (
									<TooltipPartial key={color} title={t.settings.appearance.palette[color]}>
										<ColorButton
											key={color}
											color={color}
											value={backgroundColor}
											icon={autoColorToIcon(color)}
											colorAlt={isAutoColor(color) ? `var(--background-color-${color})` : undefined}
											hidden={color === "wallpaper" && !backgroundImages.currentDominantColor}
											selected={color === "windows" && backgroundColor[0] === "wallpaper" && !backgroundImages.currentDominantColor}
											autoStartViewTransition
										/>
									</TooltipPartial>
								))}
								{BasicColorPalette.values.map(color => <ColorButton key={color} color={color} value={backgroundColor} autoStartViewTransition />)}
								<TooltipPartial title={t.custom}>
									<ColorPicker
										color={backgroundColor}
										computedColor={getComputedPaletteColor("background")}
										selected={isCustomColorSelected(backgroundColor[0])}
										showIconWhenHovering={false}
										showSpectrumWhenUnselected
									/>
								</TooltipPartial>
							</StyledColorPalette>
						</Attrs>
					</>
				)}
			</Expander>
			<ExpanderRadio
				title={t.settings.appearance.transparency}
				icon="glass"
				expanded={DEV_EXPANDED}
				view="grid"
				itemWidth="square"
				items={systemBackdrops}
				value={systemBackdrop}
				idField="name"
				nameField={t.settings.appearance.transparency}
				imageField={({ name }) => <PreviewBackdrop type={name} />}
				before={
					reduceTransparency && <InfoBar status="warning">{t.descriptions.settings.appearance.invalid.reducedTransparency({ option: t.settings.appearance.transparency })}</InfoBar> ||
					systemContrast && <InfoBar status="warning">{t.descriptions.settings.appearance.invalid.systemContrastMayNot({ option: t.settings.appearance.transparency })}</InfoBar>
				}
			/>
			<Expander
				title={t.settings.appearance.backgroundImage}
				icon="wallpaper"
				expanded={DEV_EXPANDED}
				checkInfo={backgroundImages.shown ? t.on : t.off}
			>
				<Expander.ChildWrapper>
					<Button icon="open_file" onClick={addBackgroundImage}>{t.browse}</Button>
				</Expander.ChildWrapper>
				{/* <BackgroundImageItemStyle /> */}{/* Styled component is annoying. */}
				<SortableView
					items={[backgroundImages.items.map(({ key, ...o }) => ({ id: key, pin: key === -1 ? "top" : undefined, ...o }))]}
					fullyDraggable
					view="grid"
					minDistance
					onReorder={async (from, to) => await backgroundImages.reorder(backgroundImages.items[from].key, to - 1)}
					unfocusableForSortableItems
					disableKeyboardSensor
				>
					{(_1, _2, { id, url, displayIndex, color }) => (
						<ItemsView.Item
							className="background-image-item"
							id={id}
							key={id}
							image={id === -1 ? <IconTile name="prohibited" size={48} /> : url}
							selected={[backgroundImages.backgroundImage[0] === id, (v: boolean) => v && backgroundImages.backgroundImage[1](id)]}
							selectionColor={color}
							onContextMenu={id === -1 ? undefined : createContextMenu([
								{ label: t.menu.moveForward, enabled: displayIndex > 0, onClick: () => backgroundImages.reorder(id, displayIndex - 1) },
								{ label: t.menu.moveBackward, enabled: displayIndex < backgroundImages.items.length - 2, onClick: () => backgroundImages.reorder(id, displayIndex + 1) },
								{ kind: "separator" },
								{ label: t.menu.delete, onClick: () => backgroundImages.delete(id), confirmDeleteMessage: t.confirm.delete.backgroundImage },
							])}
						/>
					)}
				</SortableView>
				{backgroundImages.shown && (
					<>
						<Expander.Item title={t.settings.appearance.backgroundImage.opacity} icon="fade">
							<Slider
								value={backgroundImageOpacity}
								min={0}
								max={1}
								step={0.01}
								defaultValue={0.2}
								displayValue={i => (i * 100 | 0) + t.units.percent}
							/>
						</Expander.Item>
						<Expander.Item title={t.settings.appearance.backgroundImage.tint} icon="shape_intersect">
							<Slider
								value={backgroundImageTint}
								min={0}
								max={1}
								step={0.01}
								defaultValue={0}
								displayValue={i => (i * 100 | 0) + t.units.percent}
							/>
						</Expander.Item>
						<Expander.Item title={t.settings.appearance.backgroundImage.blur} icon="blur">
							<Slider
								value={backgroundImageBlur}
								min={0}
								max={64}
								step={0.01}
								defaultValue={0}
								displayValue={i => i + t.units.pixel}
							/>
						</Expander.Item>
					</>
				)}
			</Expander>
			<Expander
				title={t.settings.appearance.fontSize}
				icon="text_font_size"
				checkInfo={fontSize[0].toFixedNumber(1) + t.units.point}
				alwaysShowCheckInfo
				expanded={DEV_EXPANDED}
			>
				<Expander.ChildWrapper>
					<Slider
						value={fontSize}
						min={7}
						max={28}
						defaultValue={14}
						step={0.1}
						staticSmoothInterval={0}
					/>
				</Expander.ChildWrapper>
			</Expander>

			<Subheader>{t.settings.preference}</Subheader>
			<SettingsCardToggleSwitch title={t.settings.preference.autoSwitchSourceFrom} details={t.descriptions.settings.preference.autoSwitchSourceFrom} icon="arrow_swap" on={autoSwitchSourceFrom} />
			<SettingsCardToggleSwitch title={t.settings.preference.autoCollapsePrveClasses} details={t.descriptions.settings.preference.autoCollapsePrveClasses} icon="chevron_down_up" on={autoCollapsePrveClasses} />

			<Subheader>{t.subheaders.config}</Subheader>
			<SettingsCardToggleSwitch title={t.settings.config.hideUsageTips} icon="chat_help_off" on={hideUseTips} />

			<Subheader>{t.settings.dev}</Subheader>
			<SettingsCardToggleSwitch title={t.settings.dev.devMode} icon="devtools" on={devMode} />
			<SettingsCardToggleSwitch title={t.settings.dev.rtl} icon="text_paragraph_direction_left" on={rtl} />
		</div>
	);
}
