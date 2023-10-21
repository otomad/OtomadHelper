const THICKNESS = 3;
const LENGTH = 16;

const StyledTabBar = styled.div`
	position: relative;

	.indicator {
		${styles.mixins.oval()}
		width: ${THICKNESS}px;
		background-color: ${c("accent-color")};
		position: absolute;
		left: 5px;
	}
`;

const TabBar: FC<{
	current: StateProperty<string>;
}> = ({ current: [current, setCurrent], children }) => {
	const indicator = useRef<HTMLDivElement>(null);

	useEffect(() => {
		if (!indicator.current) return;
		console.log(indicator.current.previousElementSibling!.querySelector(".active"));
	});

	return (
		<StyledTabBar>
			<div className="items">
				{React.Children.map(children, child => {
					if (!React.isValidElement(child)) return child;
					const id = child.props.id as string;
					return React.cloneElement(child as ReactElement, {
						active: current === id,
						onClick: () => setCurrent(id),
					});
				})}
			</div>
			<div ref={indicator} className="indicator"></div>
		</StyledTabBar>
	);
};

export default TabBar;
