const StyledTabItemWrapper = styled.div`
	.tab-bar.vertical & {
		padding: 1.5px 5px;
	}
`;

const StyledTabItem = styled.button`
	position: relative;
	display: flex;
	align-items: center;
	width: -webkit-fill-available;
	min-height: 30px;
	overflow-x: hidden;
	border-radius: 3px;

	.tab-bar.vertical & {
		gap: 16px;
		padding: 9px 16px 11px;

		&:hover,
		&.selected {
			background-color: ${c("fill-color-subtle-secondary")};
		}

		&:not(.selected):active,
		&.selected:not(:active):hover {
			background-color: ${c("fill-color-subtle-tertiary")};
		}

		&:active > * {
			opacity: ${c("pressed-text-opacity")}
		}
	}

	.icon,
	.animated-icon {
		display: flex;
		margin-bottom: -1px;
	}

	.animated-icon {
		&,
		* {
			color: ${c("fill-color-text-primary-solid")};
		}
	}

	.text {
		line-height: 20px;
		white-space: nowrap;
	}

	.fill {
		width: 100%;
		text-align: start;
	}

	.badge-wrapper {
		position: relative;

		.badge {
			position: absolute;
			top: 0;
			right: 0;
			translate: 50% -50%;

			&:dir(rtl) {
				right: auto;
				left: 0;
				translate: -50% -50%;
			}
		}
	}

	.tab-bar.horizontal & {
		gap: 8px;
		padding: 14px 12px;

		&:hover {
			opacity: 0.79;
		}

		&:active {
			opacity: 0.54;
		}

		${ifColorScheme.light} & {
			&:hover {
				opacity: 0.61;
			}

			&:active {
				opacity: 0.45;
			}
		}
	}

	&:active .animated-icon {
		--state: pressed;
	}

	&.selected .animated-icon {
		--selected: true;
	}

	&.selected {
		${styles.effects.text.bodyStrong};
	}
`;

export /* @internal */ default function TabItem({ icon, animatedIcon, children, selected = false, collapsed, id: _id, focusable = true, badge, _vertical: vertical, ...htmlAttrs }: FCP<{
	/** Icon. */
	icon?: DeclaredIcons;
	/** Animated icon. */
	animatedIcon?: DeclaredLotties;
	/** Identifier. */
	id: string;
	/** Selected? */
	selected?: boolean;
	/** Hide the text label and only show the icon? */
	collapsed?: boolean;
	/** Can be focused? */
	focusable?: boolean;
	/** Badge. */
	badge?: string | number;
	/** @private Use the vertical NavigationView style? */
	_vertical?: boolean;
}, "section">) {
	const tabItemEl = useDomRef<"button">();

	const onEnter = async () => {
		const el = tabItemEl.current;
		if (!el) return;
		await animateSize(el, () => el.classList.toggle("collapsed", false));
		el.dispatchEvent(new Event("transitionend"));
	};

	const onExit = async () => {
		const el = tabItemEl.current;
		if (!el) return;
		await animateSize(el, () => el.classList.toggle("collapsed", true));
		el.dispatchEvent(new Event("transitionend"));
	};

	const scrollIntoView = () => {
		if (selected)
			tabItemEl.current?.scrollIntoViewIfNeeded();
	};

	useEffect(() => scrollIntoView(), [selected]);

	const BadgeItem = useCallback(({ hidden }: { hidden?: boolean }) =>
		<Badge status="accent" hidden={badge === undefined || hidden}>{badge}</Badge>, [badge]);

	return (
		<Tooltip placement="right" offset={5} disabled={!collapsed} title={children}>
			<Transition
				nodeRef={tabItemEl}
				in={!collapsed}
				onEnter={onEnter}
				onExit={onExit}
			>
				<StyledTabItemWrapper {...htmlAttrs}>
					<StyledTabItem
						type="button"
						ref={tabItemEl}
						tabIndex={focusable ? 0 : -1}
						{...htmlAttrs}
						className={{ selected }}
					>
						{(icon || animatedIcon) && (
							<div className="badge-wrapper">
								{icon && !animatedIcon && <Icon name={icon} />}
								{animatedIcon && <AnimatedIcon name={animatedIcon} />}
								<BadgeItem hidden={!(vertical && collapsed)} />
							</div>
						)}
						<div className="badge-wrapper fill">
							<div className="text">{children}</div>
							{!vertical && <BadgeItem />}
						</div>
						{vertical && <BadgeItem />}
					</StyledTabItem>
				</StyledTabItemWrapper>
			</Transition>
		</Tooltip>
	);
}
