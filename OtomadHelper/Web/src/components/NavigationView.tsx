const StyledNavButton = styled(Button)`
	width: 48px;
	height: 40px;
	margin: 3px 5px;
	background-color: transparent;
	border: none !important;
	min-width: unset;
	font-size: 16px;

	.icon {
		display: block;
	}
`;

const NavButton: FC<{}, HTMLButtonElement> = ({ ...htmlAttrs }) => {
	return (
		<StyledNavButton icon="global_nav_button" {...htmlAttrs} />
	);
};

const StyledNavigationView = styled.div`
	${styles.mixins.square("100%")};
	display: flex;

	.left {
		width: 320px;
	}
`;

const NavigationView: FC = ({ children }) => {
	const [currentNav, setCurrentNav] = useState("a");

	return (
		<StyledNavigationView>
			<div className="left">
				<NavButton />
				<TabBar current={[currentNav, setCurrentNav]}>
					<TabItem id="a" icon="placeholder">Text</TabItem>
					<TabItem id="b" icon="placeholder">Text</TabItem>
					<TabItem id="c" icon="placeholder">Text</TabItem>
				</TabBar>
			</div>
			<div className="right">
				{children}
			</div>
		</StyledNavigationView>
	);
};

export default NavigationView;
