import { ImageFitTypes as ImageFitType } from "components/BackgroundImage";
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

const SampleTextFontSize = styled.div`
	${styles.effects.text.body};
	margin-block-start: 1lh;
	font-variant-numeric: tabular-nums;
	text-align: center;

	.info {
		${styles.effects.text.caption};
	}

	* {
		user-select: text;
	}
`;

/* const BackgroundImageItemStyle = createGlobalStyle`
	.background-image-item:not(.sortable-overlay *, .dragging, .dropping) {
		@starting-style {
			scale: 0;
		}
	}
`; */

const TooltipBlock = Tooltip.with({ placement: "block" });

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
		fontSize, hideUseTips, autoSwitchSourceFrom, autoCollapsePrveClasses, previewWithSource,
		backgroundImageOpacity, backgroundImageTint, backgroundImageBlur, backgroundImageFit, backgroundImagePosition,
		systemBackdrop, accentColor, backgroundColor,
	} = useSelectConfig(c => c.settings);
	const backgroundImages = useBackgroundImages();
	const { pushPage } = useSnapshot(pageStore);
	const meta = metas.settings;
	const backgroundImagePositionDisabled = backgroundImageFit[0] === "stretch";
	const backgroundColorInvalid = actualContrast || actualAmoledDark;

	// Dev mode
	const { devMode, rtl } = useStoreState(devStore);

	async function addBackgroundImage() {
		const files = await openFile({ types: [{ accept: { "image/*": [] } }], multiple: true });
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
			<Setting
				meta={meta.language}
				title={<>{t.settings.language}{t.settings.language.toString() !== "Language" && <span lang="en"> / Language</span>}</>}
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
			</Setting>

			<Subheader meta={meta.appearance} />
			<Setting
				meta={meta.appearance.colorScheme}
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
								<ItemsView.Item id={scheme} key={scheme} image={<PreviewColorScheme colorScheme={scheme} />}>{t.settings.appearance.colorScheme[scheme]}</ItemsView.Item>)}
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
								disableCheckmarkTransition
							>
								{t.settings.appearance.colorScheme.black}
							</ItemsView.Item>
							<ItemsView.Item
								id="contrast"
								key="contrast"
								selected={[contrast, setContrast]}
								image={<PreviewColorScheme colorScheme="contrast" />}
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
							<ItemsView.Item
								id="contrast"
								key="contrast"
								selected="checked"
								image={<PreviewColorScheme colorScheme="contrast" />}
							>
								{t.settings.appearance.colorScheme.contrast}
							</ItemsView.Item>
						</ItemsView>
					</>
				)}
			</Setting>
			<Setting meta={meta.appearance.palette} expanded={DEV_EXPANDED}>
				{actualContrast ? <InfoBar status="warning">{t.descriptions.settings.appearance.invalid[systemContrast ? "systemContrastCannot" : "contrast"]({ option: t.settings.appearance.palette })}</InfoBar> : (
					<>
						<Setting meta={meta.appearance.palette.accent} asSubtitle />
						<StyledColorPalette>
							{autoColorPalettes.map(color => (
								<TooltipBlock key={color} title={t.settings.appearance.palette[color]}>
									<ColorButton
										color={color}
										value={accentColor}
										icon={autoColorToIcon(color)}
										colorAlt={isAutoColor(color) ? `var(--accent-color-${color})` : undefined}
										hidden={color === "wallpaper" && !backgroundImages.currentDominantColor}
										selected={color === "windows" && accentColor[0] === "wallpaper" && !backgroundImages.currentDominantColor}
										autoStartViewTransition
										selectedOutlineColor="colored"
									/>
								</TooltipBlock>
							))}
							{BasicColorPalette.map(({ value: color, key: name }) => (
								<TooltipBlock key={color} title={t.settings.appearance.palette[name]}>
									<ColorButton color={color} value={accentColor} autoStartViewTransition selectedOutlineColor="colored" />
								</TooltipBlock>
							))}
							<TooltipBlock title={t.custom}>
								<ColorPicker
									color={accentColor}
									computedColor={getComputedPaletteColor("accent")}
									selected={isCustomColorSelected(accentColor[0])}
									showIconWhenHovering={false}
									showSpectrumWhenUnselected
									autoStartViewTransition
									selectedOutlineColor="colored"
								/>
							</TooltipBlock>
						</StyledColorPalette>
						{backgroundColorInvalid && <InfoBar status="warning">{t.descriptions.settings.appearance.invalid.blackScheme({ option: t.settings.appearance.palette.background })}</InfoBar>}
						<Attrs style={{ opacity: backgroundColorInvalid ? 0.5 : undefined }}>
							<Setting meta={meta.appearance.palette.background} asSubtitle />
							<StyledColorPalette>
								{autoColorPalettes.map(color => (
									<TooltipBlock key={color} title={t.settings.appearance.palette[color]}>
										<ColorButton
											key={color}
											color={color}
											value={backgroundColor}
											icon={autoColorToIcon(color)}
											colorAlt={isAutoColor(color) ? `var(--background-color-${color})` : undefined}
											hidden={color === "wallpaper" && !backgroundImages.currentDominantColor}
											selected={color === "windows" && backgroundColor[0] === "wallpaper" && !backgroundImages.currentDominantColor}
											autoStartViewTransition={!backgroundColorInvalid}
										/>
									</TooltipBlock>
								))}
								{BasicColorPalette.items.map(({ value: color, key: name }) => (
									<TooltipBlock key={color} title={t.settings.appearance.palette[name]}>
										<ColorButton key={color} color={color} value={backgroundColor} autoStartViewTransition={!backgroundColorInvalid} />
									</TooltipBlock>
								))}
								<TooltipBlock title={t.custom}>
									<ColorPicker
										color={backgroundColor}
										computedColor={getComputedPaletteColor("background")}
										selected={isCustomColorSelected(backgroundColor[0])}
										showIconWhenHovering={false}
										showSpectrumWhenUnselected
									/>
								</TooltipBlock>
							</StyledColorPalette>
						</Attrs>
					</>
				)}
			</Setting>
			<Setting
				meta={meta.appearance.transparency}
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
			<Setting
				meta={meta.appearance.backgroundImage}
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
					nonFocusableForSortableItems
					disableKeyboardSensor
				>
					{(_1, _2, { id, url, displayIndex, color }) => (
						<ItemsView.Item
							className="background-image-item"
							id={id}
							key={id}
							image={id === -1 ? <IconTile name="prohibited" size={48} /> : <BackgroundImageImg src={url} autoAlt fit={backgroundImageFit[0]} position={backgroundImagePosition[0]} />}
							selected={[backgroundImages.backgroundImage[0] === id, (v: boolean) => v && backgroundImages.backgroundImage[1](id)]}
							selectionColor={color}
							withBorder
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
						<Expander.Item title={t.fit} icon="aspect_ratio">
							<ComboBox current={backgroundImageFit} ids={ImageFitType.keys} options={ImageFitType.labels} icons={(ImageFitType.meta as AnyObject).icon} />
						</Expander.Item>
						<Expander.Item title={t.settings.appearance.backgroundImage.position} icon="location_target" disabled={backgroundImagePositionDisabled}>
							<PositionControl value={backgroundImagePosition} disabled={backgroundImagePositionDisabled} />
						</Expander.Item>
					</>
				)}
			</Setting>
			<Setting
				meta={meta.appearance.fontSize}
				checkInfo={fontSize[0] + t.units.point}
				expanded={DEV_EXPANDED}
			>
				<Expander.ChildWrapper $single>
					<Slider
						value={fontSize}
						min={7}
						max={28}
						defaultValue={14}
						step={0.1}
						staticSmoothInterval={0}
					/>
					<SampleTextFontSize>
						<p className="sample">{t.descriptions.settings.appearance.fontSize.sampleText}</p>
						<p className="info"><Preserves soft>{t.descriptions.settings.appearance.fontSize.info({ current: fontSize[0], default: 14 })}</Preserves></p>
					</SampleTextFontSize>
				</Expander.ChildWrapper>
			</Setting>

			<Subheader meta={meta.preference} />
			<Setting meta={meta.internal} onClick={() => pushPage("internal")} />
			<Setting meta={meta.preference.autoSwitchSourceFrom} on={autoSwitchSourceFrom} />
			<Setting meta={meta.preference.autoCollapsePrveClasses} on={autoCollapsePrveClasses} />
			<Setting meta={meta.preference.previewWithSource} on={previewWithSource} />

			<Subheader meta={meta.config} />
			<Setting meta={meta.config.userConfig}>
				<Setting
					meta={meta.config.userConfig.backupAndRestore}
					actions={(
						<StackPanel>
							<Button icon="arrow_download">{t.export}</Button>
							<Button icon="arrow_upload">{t.import}</Button>
						</StackPanel>
					)}
				/>
				<Setting meta={meta.config.userConfig.fileLocation} selectInfo="C:\" actions={<Button icon="location_target">{t.locate}</Button>} />
				<Setting meta={meta.config.userConfig.dangerZone} actions={<Button icon="arrow_reset" accent="critical">{t.reset}</Button>} />
			</Setting>
			<Setting meta={meta.config.clipsFolder}>
				<Expander.ChildWrapper $single>
					<TextBox value={[]} style={{ inlineSize: "100%" }} />
					<StackPanel style={{ marginBlockStart: "0.5lh" }}>
						<Button icon="folder_arrow_up_right">{t.browse}</Button>
						<Button icon="location_target">{t.locate}</Button>
					</StackPanel>
				</Expander.ChildWrapper>
			</Setting>
			<Setting meta={meta.config.hideUsageTips} on={hideUseTips} />

			<Subheader meta={meta.dev} />
			<Setting meta={meta.dev.devMode} on={devMode} />
			<Setting meta={meta.dev.rtl} on={rtl} />
		</div>
	);
}
