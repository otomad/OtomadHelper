const StyledTabItem = styled.button`
	border-radius: 3px;
	margin: 3px 5px 4px;
	display: flex;
	align-items: center;
	padding: 9px 16px 11px;
	gap: 16px;
	cursor: pointer;
	width: -webkit-fill-available;
	position: relative;

	&:hover,
	&.active {
		background-color: ${c("white", 6)};
	}

	&:not(.active):active,
	&.active:not(:active):hover {
		background-color: ${c("white", 4)};
	}

	&:active > * {
		opacity: 0.79;
	}

	.icon {
		margin-bottom: -1px;
	}

	// 允许点击元素外边距。
	&::before {
		content: "";
		position: absolute;
		inset: -1.5px 0;
	}
`;

const TabItem: FC<{
	icon: string;
	id: string;
	active?: boolean;
}, HTMLButtonElement> = ({ icon, children, active, id: _id, ...htmlAttrs }) => {
	return (
		<StyledTabItem tabIndex={0} {...htmlAttrs} className={classNames({ active })}>
			<Icon name={icon} />
			<span>{children}</span>
		</StyledTabItem>
	);
};

export default TabItem;
