import { styledExpanderItemBase, styledExpanderItemContent } from "components/Expander/ExpanderItem";

const THUMB_SIZE = 18;
const THUMB_PRESSED_WIDTH = 22;

const StyledToggleSwitchLabel = styled.label`
	display: flex;
	align-items: center;
	justify-content: space-between;
	
	.right {
		display: flex;
		align-items: center;
		gap: 12px;
		
		.text {
			width: unset !important;
		}
	}
	
	.expander-child-items & {
		${styledExpanderItemBase};
		${styledExpanderItemContent};
	}

	.stroke {
		${styles.mixins.oval()};
		width: 40px;
		height: 20px;
		border: 1px solid ${c("stroke-color-control-strong-stroke-default")};
		overflow: hidden;
	}

	.base {
		${styles.mixins.square("100%")};
		background-color: ${c("fill-color-control-alt-secondary")};
		position: relative;
	}
	
	.thumb {
		${styles.mixins.square(`${THUMB_SIZE}px`)};
		${styles.mixins.oval()};
		background-color: ${c("fill-color-text-secondary")};
		position: absolute;
		left: 0;
		scale: calc(12 / ${THUMB_SIZE});
	}

	&:hover .base {
		background-color: ${c("fill-color-control-alt-tertiary")};
		
		.thumb {
			scale: calc(14 / ${THUMB_SIZE});
		}
	}

	&:active .base {
		background-color: ${c("fill-color-control-alt-quarternary")};
		
		.thumb {
			scale: calc(14 / ${THUMB_SIZE});
			width: ${THUMB_PRESSED_WIDTH}px;
		}
	}

	&[disabled] {
		.stroke {
			border-color: ${c("stroke-color-control-strong-stroke-disabled")};
		}

		.base {
			background-color: ${c("fill-color-control-alt-disabled")};
		}
		
		.thumb {
			background-color: ${c("fill-color-text-disabled")};
		}
		
		.text {
			color: ${c("fill-color-text-disabled")};
		}
	}

	&.active {
		.stroke {
			border-color: ${c("accent-color")};
		}

		.base {
			background-color: ${c("accent-color")};
		}
		
		.thumb {
			background-color: ${c("fill-color-text-on-accent-primary")};
			outline: 1px solid ${c("stroke-color-control-stroke-secondary")};
			left: calc(100% - ${THUMB_SIZE}px);
		}

		&:hover {
			opacity: 0.9;
		}

		&:active {
			opacity: 0.8;
			
			.thumb {
				left: calc(100% - ${THUMB_PRESSED_WIDTH}px);
			}
		}

		&[disabled] {
			.stroke {
				border-color: ${c("stroke-color-control-strong-stroke-disabled")};
			}

			.base {
				background-color: ${c("stroke-color-control-strong-stroke-disabled")};
			}
			
			.thumb {
				background-color: ${c("fill-color-text-on-accent-disabled")};
			}
		}
	}
`;

export default function ToggleSwitch({ on: [on, setOn], disabled, children }: FCP<{
	/** 打开？ */
	on: StateProperty<boolean>;
	/** 禁用？ */
	disabled?: boolean;
}>) {
	const textLabel = on ? "On" : "Off";
	
	const handleCheck = () => {
		if (disabled) return;
		setOn?.(!on);
	};

	return (
		<StyledToggleSwitchLabel className={{ active: on }} disabled={disabled} onClick={handleCheck}>
			<div className="text">{children}</div>
			<div className="right">
				<span className="text">{textLabel}</span>
				<div className="stroke">
					<div className="base">
						<div className="thumb" />
					</div>
				</div>
			</div>
		</StyledToggleSwitchLabel>
	);
}
