const StyledTabItem = styled.div`
	border-radius: 3px;
	margin: 3px 5px 4px;
	display: flex;
	align-items: center;
	padding: 9px 16px 11px;
	gap: 16px;
	width: 100%;
	cursor: pointer;

	&:hover,
	&.active {
		background-color: ${c("white", 6)};
	}

	&:active {
		background-color: ${c("white", 4)};
	}

	&:active > * {
		opacity: 0.79;
	}

	.icon {
		margin-bottom: -1px;
	}
`;

const TabItem: FC<{
	icon: string;
	id: string;
	active?: boolean;
}, HTMLDivElement> = ({ icon, children, active, ...htmlAttrs }) => {
	return (
		<StyledTabItem tabIndex={0} {...htmlAttrs} className={classNames({ active })}>
			<Icon name={icon} />
			<span>{children}</span>
		</StyledTabItem>
	);
};

export default TabItem;
