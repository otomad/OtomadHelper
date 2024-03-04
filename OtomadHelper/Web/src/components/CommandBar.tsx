const StyledCommandBar = styled.div`
	// box-shadow: 0 8px 16px ${c("shadows-flyout")};
	padding: 4px;
	background-color: ${c("background-fill-color-acrylic-background-default")};
	border: 1px solid ${c("stroke-color-surface-stroke-flyout")};
	border-radius: 6px;

	> * {
		${styles.effects.text.body};
		display: flex;
		height: 100%;
		/* overflow-x: hidden; */
	}

	* {
		white-space: nowrap;
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
	/** 按钮图标。 */
	icon?: string;
	/** 可被禁用的？ */
	canBeDisabled?: boolean;
}, "section"> & TransitionProps) {
	const ref = useDomRef<HTMLDivElement>();
	const [onEnter, onExit, endListener] = simpleAnimateSize(ref, "width", 750, eases.easeInOutMax, undefined, { keepClippingAtEnd: true });
	const { transitionAttrs, htmlAttrs } = separateTransitionAttrs(buttonAndTransitionAttrs);

	const button = <Button icon={icon} subtle disabled={disabled} onClick={onClick} {...htmlAttrs}>{children}</Button>;

	return (
		<Transition
			{...transitionAttrs}
			nodeRef={ref}
			addEndListener={endListener}
			onEnter={onEnter}
			onExit={onExit}
			unmountOnExit
		>
			<div ref={ref}>
				{!canBeDisabled ? button : (
					<DisabledButtonWrapper key="complete" disabled={disabled} onClick={onClick}>
						{button}
					</DisabledButtonWrapper>
				)}
			</div>
		</Transition>
	);
}

function separateTransitionAttrs(buttonAndTransitionAttrs: object) {
	type HTMLElementAttrs = FCP<{}, "section">;
	const transitionAttrKeys = ["in", "mountOnEnter", "unmountOnExit", "onEnter", "onEntering", "onEntered", "onExit", "onExiting", "onExited", "nodeRef"];
	const transitionAttrs: TransitionProps = {}, htmlAttrs: HTMLElementAttrs = {};
	for (const [key, value] of entries(buttonAndTransitionAttrs))
		if (transitionAttrKeys.includes(key)) transitionAttrs[key] = value;
		else htmlAttrs[key] = value;
	return { transitionAttrs: transitionAttrs as object, htmlAttrs };
}

CommandBar.Item = CommandBarItem;
