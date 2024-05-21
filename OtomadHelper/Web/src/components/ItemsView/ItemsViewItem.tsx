import { styledExpanderItemText } from "components/Expander/ExpanderItem";

export const GRID_VIEW_ITEM_HEIGHT = 112;

const StyledItemsViewItem = styled.button<{
	/** View mode: list, tile, grid. */
	$view: ItemView;
}>`
	${styles.mixins.forwardFocusRing()};

	${({ $view }) => $view === "grid" ? css`
		:where(.image-wrapper) {
			width: 100%;
			height: ${GRID_VIEW_ITEM_HEIGHT}px;
		}

		.image-wrapper {
			position: relative;

			.checkbox-label {
				position: absolute;
				inset-block-start: 6px;
				inset-inline-end: 6px;
			}
		}

		> .base {
			position: relative;
			overflow: clip;
			border-radius: 4px;
		}

		.text-part {
			display: flex;
			gap: 10px;
			align-items: center;
			margin: 5px 0;
			text-align: start;
		}

		.selection {
			position: absolute;
			inset: 0;
			border-radius: inherit;
			pointer-events: none;
		}

		&:not(.selected) .selection {
			transition: ${fallbackTransitions}, box-shadow ${eases.easeInSmooth} 250ms;
		}

		&:hover .selection {
			background-color: ${c("fill-color-subtle-secondary")};
			box-shadow: 0 0 0 1px ${c("stroke-color-control-stroke-on-accent-tertiary")} inset;
		}

		&:active .selection {
			background-color: ${c("fill-color-subtle-tertiary")};
		}

		&.selected .selection {
			box-shadow:
				0 0 0 2px ${c("accent-color")} inset,
				0 0 0 3px ${c("fill-color-control-solid-default")} inset;
		}

		&.selected:hover .selection {
			box-shadow:
				0 0 0 2px ${c("accent-color", 90)} inset,
				0 0 0 3px ${c("fill-color-control-solid-default")} inset;
		}

		&.selected:active .selection {
			box-shadow:
				0 0 0 2px ${c("accent-color", 80)} inset,
				0 0 0 3px ${c("fill-color-control-solid-default")} inset;
		}
	` : css`
		padding: 2px 4px;

		> .base {
			position: relative;
			display: flex;
			gap: 16px;
			align-items: center;
			min-height: 48px;
			padding: 8px 12px;
			overflow: clip;
			border-radius: 3px;

			> * {
				transition: none;
			}

			&::before {
				${styles.mixins.oval()};
				content: "";
				position: absolute;
				inset-inline-start: 0;
				block-size: ${100 / 3}%;
				inline-size: 3px;
				background-color: ${c("accent-color")};

				@starting-style {
					scale: 1 0;
				}
			}
		}

		main.page > .container > .items-view > & {
			padding-inline: 0;

			> .base {
				padding-inline: 16px;
			}
		}

		&:hover > .base {
			background-color: ${c("fill-color-subtle-secondary")};
		}

		&:active > .base {
			background-color: ${c("fill-color-subtle-tertiary")};

			&::before {
				scale: 1 0.625;
			}

			> * {
				opacity: ${c("pressed-text-opacity")}
			}
		}

		&:not(.selected) > .base::before {
			scale: 1 0;
		}

		&:has(.checkbox-label) > .base::before {
			display: none;
		}

		${styledExpanderItemText};

		.image-wrapper {
			${styles.mixins.flexCenter()};
		}
	`}

	.text > * {
		${styles.effects.text.body};
	}

	&.selected .text .title {
		${styles.effects.text.bodyStrong};
	}

	.checkbox-temp-wrapper {
		${tgs()} {
			opacity: 0;

			&,
			& ~ * {
				translate: ${-(18 + 16)}px;
			}
		}
		
		&:is(.enter-active, .exit-active) {
			&,
			& ~ * {
				transition: translate ${eases.easeOutMax} 250ms, opacity ${eases.easeOutMax} 250ms;
			}
		}
	}
`;

const DefaultImage = styled.img`
	${styles.mixins.square("100%")};
	object-fit: cover;
`;

export /* internal */ default function ItemsViewItem({ image, icon, id: _id, selected = false, details, _view: view, _multiple: multiple, children, className, ...htmlAttrs }: FCP<{
	/** Image. */
	image?: string | ReactNode;
	/** Icon. */
	icon?: DeclaredIcons;
	/** Identifier. */
	id: ObjectKey;
	/** Selected? */
	selected?: boolean;
	/** Detailed description. */
	details?: ReactNode;
	/** @private View mode: list, tile, grid. */
	_view?: ItemView;
	/** @private Multiple selection mode? */
	_multiple?: boolean;
}, "button">) {
	const textPart = (
		<div className="text">
			<p className="title">{children}</p>
			<p className="details">{details}</p>
		</div>
	);
	// const checkbox = multiple && <Checkbox value={[selected]} plain />;
	const checkbox = (
		<CssTransition in={multiple} unmountOnExit>
			<div className="checkbox-temp-wrapper">{/* TODO: Wait for React 19 ref as a prop, and then remove this div. */}
				<Checkbox value={[selected]} plain />
			</div>
		</CssTransition>
	);

	return (
		<StyledItemsViewItem $view={view!} className={[className, view, { selected }]} tabIndex={0} {...htmlAttrs}>
			<div className="base">
				{view === "grid" ? (
					<>
						<div className="image-wrapper">
							{typeof image === "string" ? <DefaultImage src={image} /> : image}
							{checkbox}
						</div>
						<div className="selection" />
					</>
				) : (
					<>
						{checkbox}
						{(image || icon) && (
							<div className="image-wrapper">
								{typeof image === "string" ? <img src={image} /> : icon ? <Icon name={icon} /> : undefined}
							</div>
						)}
						{textPart}
					</>
				)}
			</div>
			{view === "grid" && (
				<div className="text-part">
					{icon && <Icon name={icon} />}
					{textPart}
				</div>
			)}
		</StyledItemsViewItem>
	);
}
