const StyledDisabledButtonWrapper = styled.div`
	position: relative;

	&.disabled {
		cursor: not-allowed;
	}

	.badge {
		position: absolute;
		inset-block-start: -2px;
		inset-inline-end: -2px;
		cursor: help;
	}
`;

export default function DisabledButtonWrapper({ disabled, children, ref, ...htmlAttrs }: FCP<{}, "div">) {
	return (
		<StyledDisabledButtonWrapper ref={ref} className={{ disabled }} {...htmlAttrs}>
			{children}
			<Badge hidden={!disabled} status="warning" _requestAnimationFrame />
		</StyledDisabledButtonWrapper>
	);
}
