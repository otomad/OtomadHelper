// This component will show in the DEV mode only!

import type { MenuOutput } from "utils/context-menu";

const StyledContextMenu = styled.menu`
	position: fixed;
	margin: 0;
	padding: 2px;
	background-color: ${c("background-fill-color-acrylic-background-default")};
	border: 1px solid ${c("stroke-color-surface-stroke-flyout")};
	border-radius: 8px;
	box-shadow: 0 8px 16px ${c("shadows-flyout")};
	backdrop-filter: blur(60px);

	li {
		display: block;
		padding: 9px 16px;
	}

	li:hover {
		background-color: ${c("fill-color-subtle-secondary")};
		border-radius: 4px;
	}
`;

export default function DevContextMenu() {
	if (window.isWebView) return;

	const [location, setLocation] = useState<TwoD>([NaN, NaN]);
	const [menu, setMenu] = useState<MenuOutput>();
	const isMenuShown = useMemo(() => menu != null && menu.items.length !== 0, [menu]);
	const menuEl = useDomRef<"menu">();
	const ifFinite = (value: number) => Number.isFinite(value) ? value + "px" : undefined;
	function clearMenu() {
		setLocation([NaN, NaN]);
		setMenu(undefined);
	}

	useListen("dev:showContextMenu", ([e, menu]) => {
		setLocation([e.clientX, e.clientY]);
		setMenu(menu);
	});

	useEventListener(window, "mousedown", e => isMenuShown && !isInPath(e, menuEl) && clearMenu(), undefined, [isMenuShown, clearMenu]);

	return (
		<Portal>
			<StyledContextMenu ref={menuEl} style={{ left: ifFinite(location[0]), top: ifFinite(location[1]) }}>
				{menu && menu.items.map((item, i) =>
					item.kind === "command" ? <li key={i} disabled={item.enabled === false} onClick={() => { clearMenu(); item.command?.(); }}>{item.label}</li> :
					item.kind === "separator" ? <hr /> : undefined,
				)}
			</StyledContextMenu>
		</Portal>
	);
}
