// This component will show in the DEV mode only!

const StyledContextMenu = styled.menu`
	position: fixed;
	z-index: ${styles.z.contextMenu};
	margin: 0;
	padding: 2px;
	background-color: ${c("background-fill-color-acrylic-background-command-bar")};
	border: 1px solid ${c("stroke-color-surface-stroke-flyout")};
	border-radius: 8px;
	box-shadow: 0 8px 16px ${c("shadows-flyout")};
	backdrop-filter: blur(60px);

	li {
		display: block;
		padding: 9px 16px;

		&:hover,
		&:active {
			border-radius: 4px;
		}

		&:hover {
			background-color: ${c("fill-color-subtle-secondary")};
		}

		&:active {
			background-color: ${c("fill-color-subtle-tertiary")};

			.content {
				opacity: ${c("pressed-text-opacity")};
			}
		}

		&[disabled] {
			background-color: transparent !important;

			.content {
				opacity: ${c("disabled-text-opacity")};
			}
		}
	}

	hr {
		margin: 0;
		margin-block: 3px;
		border: none;
		border-bottom: 1px solid ${c("stroke-color-divider-stroke-default")};
	}

	@starting-style {
		opacity: 0;
	}
`;

export default function DevContextMenu() {
	if (window.isWebView) return;

	const [location, setLocation] = useState<TwoD>([NaN, NaN]);
	const [menu, setMenu] = useState<ContextMenuOutput>();
	const isMenuShown = useMemo(() => menu != null && menu.items.length > 0, [menu]);
	const menuEl = useDomRef<"menu">();
	const ifFinite = (value: number) => Number.isFinite(value) ? value + "px" : undefined;
	function clearMenu() {
		setLocation([NaN, NaN]);
		setMenu(undefined);
	}

	useListen("dev:showContextMenu", (e, menu) => {
		setLocation([e.clientX, e.clientY]);
		setMenu(menu);
	});

	useEventListener(window, "mousedown", e => isMenuShown && !isInPath(e, menuEl) && clearMenu(), undefined, [isMenuShown, clearMenu]);
	useEventListener(window, "keydown", e => isMenuShown && e.code === "Escape" && clearMenu(), undefined, [isMenuShown, clearMenu]);
	useEventListener(window, "blur", () => isMenuShown && clearMenu(), undefined, [isMenuShown, clearMenu]);

	return (
		<Portal>
			<StyledContextMenu ref={menuEl} role={isMenuShown ? "menu" : undefined} style={{ left: ifFinite(location[0]), top: ifFinite(location[1]) }} hidden={!isMenuShown} aria-hidden={!isMenuShown}>
				{menu && menu.items.map((item, i) =>
					item.kind === "command" ? (
						<li key={i} role="menuitem" disabled={item.enabled === false} onClick={() => { clearMenu(); item.command?.(); }}>
							<span className="content">
								{processAccessKey(item.label)}
							</span>
						</li>
					) : item.kind === "separator" ? <hr key={i} role="separator" /> : undefined,
				)}
			</StyledContextMenu>
		</Portal>
	);
}

function processAccessKey(label: string) {
	const nodes: ReactNode[] = [];
	let currentWord = "";
	const submitCurrentWord = () => currentWord && nodes.push(currentWord);
	for (let i = 0; i < label.length; i++) {
		const char = label[i];
		if (char !== "&") {
			currentWord += char;
			continue;
		}
		const nextChar = label[i + 1];
		if (nextChar !== undefined && nextChar !== "&") {
			submitCurrentWord();
			currentWord = "";
			nodes.push(<u key={i}>{nextChar}</u>);
			i++;
		}
	}
	submitCurrentWord();
	return nodes;
}
