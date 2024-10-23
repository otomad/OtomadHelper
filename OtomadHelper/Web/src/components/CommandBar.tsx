import type { TransitionProps } from "react-transition-group";

const StyledCommandBar = styled.div`
	// box-shadow: 0 8px 16px ${c("shadows-flyout")};
	padding: 4px;
	background-color: ${c("background-fill-color-acrylic-background-command-bar")};
	border: 1px solid ${c("stroke-color-surface-stroke-flyout")};
	border-radius: 6px;

	> * {
		${styles.effects.text.body};
		display: flex;
		align-items: center;
		block-size: 100%;
	}

	* {
		white-space: nowrap;
	}

	.command-bar-item {
		overflow: hidden;
		transition: all ${eases.easeInOutFluent} 650ms;

		${tgs()} {
			inline-size: 0;
			scale: 0.5;
			opacity: 0;
		}
	}
`;

export default function CommandBar({ children, ...htmlAttrs }: FCP<{}, "div">) {
	return (
		<StyledCommandBar {...htmlAttrs}>
			<TransitionGroup>
				{children}
			</TransitionGroup>
		</StyledCommandBar>
	);
}

function CommandBarItem({ icon, children, canBeDisabled, disabled, onClick, ...buttonAndTransitionAttrs }: FCP<{
	/** Button icon. */
	icon?: DeclaredIcons;
	/** Can be disabled? */
	canBeDisabled?: boolean;
}, "section"> & TransitionProps) {
	const { transitionAttrs, htmlAttrs } = separateTransitionAttrs(buttonAndTransitionAttrs);

	const button = <Button icon={icon} subtle disabled={disabled} onClick={onClick} {...htmlAttrs}>{children}</Button>;

	return (
		<CssTransition {...transitionAttrs}>
			<div className="command-bar-item">
				{!canBeDisabled ? button : (
					<DisabledButtonWrapper key="complete" disabled={disabled} onClick={onClick}>
						{button}
					</DisabledButtonWrapper>
				)}
			</div>
		</CssTransition>
	);
}

function separateTransitionAttrs(buttonAndTransitionAttrs: object) {
	type HTMLElementAttrs = FCP<{}, "section">;
	const transitionAttrKeys = ["in", "mountOnEnter", "unmountOnExit", "onEnter", "onEntering", "onEntered", "onExit", "onExiting", "onExited", "nodeRef"];
	const transitionAttrs: TransitionProps = {}, htmlAttrs: HTMLElementAttrs = {};
	for (const [key, value] of entries(buttonAndTransitionAttrs))
		if (transitionAttrKeys.includes(key)) transitionAttrs[key] = value;
		else htmlAttrs[key] = value;
	return { transitionAttrs, htmlAttrs };
}

CommandBar.Item = CommandBarItem;
