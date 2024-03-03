const StyledDisabledButtonWrapper = styled.div`
	position: relative;

	&.disabled {
		cursor: not-allowed;
	}

	.badge {
		position: absolute;
		top: -2px;
		right: -2px;
		cursor: pointer;
	}
`;

export default forwardRef(function DisabledButtonWrapper({ disabled, children, ...htmlAttrs }: FCP<{}, "div">, ref: ForwardedRef<"div">) {
	return (
		<StyledDisabledButtonWrapper ref={ref} className={{ disabled }} {...htmlAttrs}>
			{children}
			<Badge hidden={!disabled} status="warning" />
		</StyledDisabledButtonWrapper>
	);
});
