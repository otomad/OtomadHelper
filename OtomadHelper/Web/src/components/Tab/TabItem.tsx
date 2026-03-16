const StyledTabItem = styled.button`
	position: relative;
	display: flex;
	align-items: center;
	min-block-size: 30px;
	overflow-inline: hidden;
	border-radius: 8px / 4.5px; // Original: 3px; by adding with the border width: (3px + 5px) / (3px + 1.5px).

	.tab-bar.vertical & {
		gap: 16px;
		inline-size: stretch;
		padding-block: 9px 11px;
		padding-inline: 16px 12px;
		background-clip: padding-box;
		border: solid transparent;
		border-width: 1.5px 5px;

		&:hover,
		&.selected {
			background-color: ${c("fill-color-subtle-secondary")};

			${ifColorScheme.at.contrast} {
				background-color: ${cc("Highlight")} !important;

				* {
					color: ${cc("HighlightText")} !important;
					forced-color-adjust: none;
				}

				.badge {
					background-color: ${cc("HighlightText")};
					outline: 1px solid ${cc("Highlight")};

					&,
					* {
						color: ${cc("Highlight")} !important;
					}
				}
			}
		}

		&:not(.selected):active,
		&.selected:not(:active):hover {
			background-color: ${c("fill-color-subtle-tertiary")};
		}

		&:active > * {
			opacity: ${c("pressed-text-opacity")};
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
		inline-size: 100%;
		text-align: start;
	}

	.badge-wrapper {
		position: relative;

		.badge {
			position: absolute;
			inset-block-start: 0;
			inset-inline-end: 0;
			translate: 50% -50%;

			&:dir(rtl) {
				translate: -50% -50%;
			}
		}
	}

	.badge-wrapper-adjust-beacon {
		${styles.mixins.gridCenter()};
		min-inline-size: 16px;
	}

	.tab-bar.horizontal & {
		gap: 8px;
		padding: 14px 12px;

		&:hover {
			color: ${c("fill-color-text-secondary")};
		}

		&:active {
			color: ${c("fill-color-text-tertiary")};
		}
	}

	&:active .animated-icon {
		--state: pressed;
	}

	&.selected .animated-icon {
		--selected: true;
	}

	&.selected {
		${styles.text.bodyStrong};
	}

	&::before,
	&::after {
		content: "";
		position: absolute;
		inset: 0;
		display: block;
		border-radius: 3px;
		pointer-events: none;
	}

	&::before {
		anchor-name: var(--anchor-name);
	}

	&::after {
		position: fixed;
		position-anchor: var(--anchor-name);
		inset: anchor(top) anchor(right) anchor(bottom) anchor(left);
	}

	${styles.mixins.forwardFocusRing("&::after")};
`;

const BadgeItem = ({ hidden: layoutHidden, badge: [badge, status, hidden] = [false] as never }: { hidden?: boolean; badge?: BadgeArgs }) =>
	<Badge status={status ?? "accent"} hidden={hidden || layoutHidden} unmountOnExit={false}>{badge}</Badge>;

export /* @internal */ default function TabItem({ icon, animatedIcon, children, selected = false, collapsed, id: _id, focusable = true, badge, ariaCurrentWhenSelected, autoScrollIntoView = true, _vertical: vertical, onClick, ...htmlAttrs }: FCP<{
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
	badge?: BadgeArgs;
	/** Set the `aria-current` attribute only when the tab item has been selected. */
	ariaCurrentWhenSelected?: React.AriaAttributes["aria-current"];
	/** Auto scroll into view while selected? @default true */
	autoScrollIntoView?: boolean;
	/** @private Use the vertical NavigationView style? */
	_vertical?: boolean;
}, GenericElement>) {
	const tabItemEl = useDomRef<"button">();
	const anchorName = "--tab-item-focus-ring" + useId();
	const scrollIntoView = (force = false) => {
		if ((selected || force) && autoScrollIntoView)
			scrollIntoViewAlt(tabItemEl, !vertical);
	};
	useEffect(() => scrollIntoView(), [selected]);
	useKeyboardFocus(tabItemEl, () => scrollIntoView(true));

	return (
		<Tooltip placement="inline-end" offset={5} disabled={!collapsed} title={children} applyAriaLabel={false}>
			<StyledTabItem
				ref={tabItemEl}
				type="button"
				tabIndex={focusable ? 0 : -1}
				role="tab"
				aria-selected={selected}
				aria-current={selected ? ariaCurrentWhenSelected : undefined}
				onClick={e => { onClick?.(e); scrollIntoView(true); }}
				{...htmlAttrs}
				className={{ selected }}
				style={{ "--anchor-name": anchorName }}
			>
				{(icon || animatedIcon) && (
					<div className="badge-wrapper">
						{icon && !animatedIcon && <Icon name={icon} />}
						{animatedIcon && <AnimatedIcon name={animatedIcon} showFallbackIcon />}
						<BadgeItem hidden={!(vertical && collapsed)} badge={badge} />
					</div>
				)}
				<div className="badge-wrapper fill">
					<div className="text">{children}</div>
					{!vertical && <BadgeItem badge={badge} />}
				</div>
				{vertical && (
					<div className="badge-wrapper-adjust-beacon">
						<BadgeItem badge={badge} />
					</div>
				)}
			</StyledTabItem>
		</Tooltip>
	);
}
