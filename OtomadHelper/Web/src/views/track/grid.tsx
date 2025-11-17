import contextMenuCur from "assets/cursors/context_menu.svg?cursor";
import exampleThumbnail from "assets/images/ヨハネの氷.avif";
import ApprovalsAppIcon from "assets/svg/approvals_app.svg?react";

export /* @internal */ const arrayTypes = ["square", "fixed"] as const;
export /* @internal */ const directionTypes = ["lr-tb", "tb-lr", "rl-tb", "tb-rl"] as const;
export /* @internal */ const fitTypes = ["cover", "contain"] as const;
export /* @internal */ const parityTypes = ["unflipped", "even_columns", "odd_columns", "even_rows", "odd_rows", "even_checker", "odd_checker", "even_dots", "odd_dots", "even_gridlines", "odd_gridlines", "all_flipped", "random"] as const;
const parityTypes_rowsFirst = parityTypes.toMoved(3, 5, 1);
type GridParityType = typeof parityTypes[number];
const MAX_COL_ROW = 100;
const GAP = 6;
const RULER_THICKNESS = 16;
const ASTERISK = "∗";
const PREVIEW_GRID_BORDER_RADIUS = 8;

const /** @deprecated */ count = 26, /** @deprecated */ projectWidth = 1920, /** @deprecated */ projectHeight = 1080;

const getGridUnitTypeName = (unit: WebMessageEvents.GridUnitType, count: number) => {
	const tc = t({ context: "full", count });
	return unit === "auto" ? tc.auto : unit === "pixel" ? tc.units.pixel : tc.units.fraction;
};

export /* @internal */ const getParityText = (parity: GridParityType) => t.track.grid.parity[new VariableName(parity).camel];
export /* @internal */ const getParityIcon = (parity: GridParityType): DeclaredIcons =>
	parity === "unflipped" ? "dismiss_square" : parity === "all_flipped" ? "checkmark_square" : parity === "random" ? "question_square" : `parity/${parity}`;

export /* @internal */ const matchParity = (parity: GridParityType, column: number, row: number, randomSeed?: string): boolean => {
	if (parity === "random") return (randomSeed === undefined ? Math.random() : seedRandom(`${randomSeed},${column},${row}`)()) >= 0.5;
	return {
		unflipped: false,
		all_flipped: true,
		even_columns: !(column % 2),
		odd_columns: !!(column % 2),
		even_rows: !(row % 2),
		odd_rows: !!(row % 2),
		even_checker: !!((column + row) % 2),
		odd_checker: !((column + row) % 2),
		even_dots: !(column % 2) && !(row % 2),
		odd_dots: !!(column % 2) && !!(row % 2),
		even_gridlines: !!(column % 2) || !!(row % 2),
		odd_gridlines: !(column % 2) || !(row % 2),
	}[parity];
};

// #region Style
const PreviewGridContainer = styled.div`
	${styles.mixins.square("100%")};
	${styles.mixins.gridCenter()};
	container: preview-grid-container / size;
	margin-block-start: ${RULER_THICKNESS - GAP}px;
`;

const PreviewGrid = styled.div`
	${styles.effects.flyout};
	--project-width: 1920;
	--project-height: 1080;
	display: grid;
	align-self: center;
	width: min(100cqw, calc(100cqh / var(--project-height) * var(--project-width)));
	height: min(100cqh, calc(100cqw / var(--project-width) * var(--project-height)));
	border-radius: ${PREVIEW_GRID_BORDER_RADIUS}px;
	backdrop-filter: none;
	transition: ${fallbackTransitions}, --grid-template-count ${eases.easeOutMax} 250ms;

	&.column {
		grid-auto-flow: row;
		grid-template-columns: repeat(var(--grid-template-count), 1fr);
	}

	&.row {
		grid-auto-flow: column;
		grid-template-rows: repeat(var(--grid-template-count), 1fr);
	}

	&.square {
		grid-template-rows: repeat(var(--grid-template-count), 1fr);
		grid-template-columns: repeat(var(--grid-template-count), 1fr);
	}

	.padding-wrapper {
		padding: var(--padding, 0);
	}

	[role="img"] {
		${styles.mixins.square("100%")};
		position: relative;
		align-content: end;
		min-height: 0;
		contain: strict;
		object-fit: var(--fit);
		overflow: hidden;
		background-image: url("${exampleThumbnail}");
		background-repeat: no-repeat;
		background-position: center;
		background-size: var(--fit);
		cursor: ${contextMenuCur};

		&.h-flip {
			scale: -1 1;

			&::after {
				right: var(--margin);
				left: unset;
			}
		}

		&.v-flip {
			scale: 1 -1;

			&::after {
				top: var(--margin);
				bottom: unset;
			}
		}

		&.h-flip.v-flip {
			scale: -1 -1;

			&::after {
				top: var(--margin);
				right: var(--margin);
				bottom: unset;
				left: unset;
			}
		}

		&:active {
			animation: ${keyframes`
				from {
					box-shadow: 0 0
				}
			`} duration timing-function delay iteration-count direction fill-mode;
		}

		&::after {
			${styles.mixins.oval()};
			${styles.effects.text.caption};
			--size: 16px;
			--margin: 4px;
			content: attr(data-index);
			position: absolute;
			bottom: var(--margin);
			left: var(--margin);
			display: inline-block;
			block-size: var(--size);
			min-inline-size: var(--size);
			padding: 0 3px;
			font-size: 10px;
			text-align: center;
			background-color: ${c("fill-color-system-solid-neutral-background", 75)};
			scale: inherit;
		}
	}

	&:has([role="img"]:hover) [role="img"]:not(:hover) {
		opacity: 0.75;
	}

	&:has([role="img"].highlight) [role="img"]:not(.highlight) {
		opacity: 0.5;
	}
`;

const xWidth = "1.25ex";
const Determinant = styled.div`
	display: grid;
	grid-template-columns: 1fr auto 1fr;
	gap: ${GAP}px;
	align-self: center;
	margin-block: 4px 16px;

	> * {
		display: grid;
		gap: inherit;
	}

	> :first-child {
		grid-column: 2;
		grid-template-columns: 1fr ${xWidth} 1fr;
	}

	> :last-child {
		grid-column: 3;
		grid-template-columns: ${xWidth} 1fr;
		margin-inline-start: auto;
	}

	label {
		${styles.effects.text.body};
		align-content: center;
	}

	.text-box {
		transition: ${fallbackTransitions}, inline-size 0s;
	}

	@media (width < 750px) {
		grid-template-columns: 1fr ${xWidth} 1fr ${xWidth} 1fr;

		> * {
			display: contents;
		}

		> :first-child {
			${forMapFromTo(1, 3, 1, i => css`> :nth-child(${i - 1 + 4}) { grid-area: 2 / ${i}; }`)}
		}

		.text-box {
			inline-size: unset;
		}
	}

	:has(> &) {
		position: relative;
	}
`;

const floatDownHidden = css`
	translate: 0 24px;
	opacity: 0;
	pointer-events: none;
`;

const floatUpHidden = css`
	${floatDownHidden};
	translate: 0 -16px;
`;

const Mask = styled.div`
	position: fixed;
	inset: 0;
`;

const FlyoutEditor = styled.div`
	${styles.effects.flyout};
	${styles.mixins.gridCenter()};
	position: absolute;
	inset: 0;
	inset-block-end: 8px;
	z-index: 1;
	padding: ${GAP}px;
	overflow: visible;
	transition-behavior: allow-discrete;

	&[hidden] {
		${floatDownHidden};
	}

	@starting-style {
		${floatDownHidden};
	}

	> * {
		gap: ${GAP}px;
	}

	.text-box {
		max-inline-size: 200px;
	}

	.span {
		display: grid;
		grid-template-columns: 1fr auto 1fr;
	}

	.length {
		display: flex;
		gap: 12px;
		align-items: center;

		> :not(.text-box) {
			flex-shrink: 0;
		}
	}
`;

const COMMAND_BAR_WRAPPER_ANCHOR_NAME = "--command-bar-wrapper";
const StyledContainerPreview = styled.div`
	&:has(${FlyoutEditor}:not([hidden])) {
		${Determinant} {
			${floatDownHidden};
		}

		.command-bar-group {
			${floatUpHidden};
		}
	}

	.command-bar-wrapper {
		anchor-name: ${COMMAND_BAR_WRAPPER_ANCHOR_NAME};
		position: relative;

		.reset-btn {
			position: absolute;
			inset-block-start: 4px;
			inset-inline-end: 0;
			z-index: 2;
			transition-behavior: allow-discrete;

			&[hidden] {
				${floatUpHidden};
			}

			@starting-style {
				${floatUpHidden};
			}

			@supports (anchor-name: ${COMMAND_BAR_WRAPPER_ANCHOR_NAME}) {
				position: fixed;
				position-anchor: ${COMMAND_BAR_WRAPPER_ANCHOR_NAME};
				position-area: span-inline-start center;
				inset-block-start: unset;
				inset-inline-end: unset;
			}
		}
	}
`;

const Label = ({ id, htmlFor, ...htmlAttrs }: RequiredWith<FCP<{}, "label">, "id" | "htmlFor">) =>
	<label id={`${id}-${htmlFor}-label`} htmlFor={`${id}-${htmlFor}`} aria-hidden {...htmlAttrs} />;

const Ruler = styled.div.attrs({
	"aria-hidden": true,
})`
	${styles.effects.text.caption};
	position: fixed;
	position-anchor: --preview-grid;
	display: grid;
	color: ${c("fill-color-text-secondary")};
	font-size: 10px;
	text-align: center;
	transition: none;

	> * {
		contain: strict;
		overflow: clip;
		direction: ltr;
	}

	&.columns {
		right: anchor(right);
		bottom: anchor(top);
		left: anchor(left);
		grid-auto-flow: column;
		block-size: ${RULER_THICKNESS}px;

		> :not(:last-child) {
			border-inline-end: 1px solid ${c("stroke-color-divider-stroke-default")};
		}
	}

	&.rows {
		top: anchor(top);
		right: anchor(left);
		bottom: anchor(bottom);
		grid-auto-flow: row;
		inline-size: ${RULER_THICKNESS}px;

		> * {
			writing-mode: sideways-lr;
		}

		> :not(:last-child) {
			border-inline-start: 1px solid ${c("stroke-color-divider-stroke-default")};
		}
	}
`;

const TOOLTIP_OFFSET = 28;

const Multiply = styled.label.attrs({
	children: "×",
	role: "term",
	"aria-label": t.aria.timesOperator,
})`
	${styles.mixins.flexCenter()};
	inline-size: ${xWidth};
	padding-block-end: 3px;

	&.shadow {
		visibility: hidden;
	}
`;
// #endregion

export default function Grid() {
	const { columns: [columns, _setColumns], array, direction, fit, mirrorEdgesHFlip, mirrorEdgesVFlip, descending: [descending, setDescending], padding, spans: [spans, setSpans], columnWidths: [columnWidths, setColumnWidths], rowHeights: [rowHeights, setRowHeights], blanks: [blanks, setBlanks] } = useSelectConfig(c => c.track.grid);
	const setColumns = setStateInterceptor(_setColumns, (input: number) => clamp(input, 1, MAX_COL_ROW));
	// These properties were originally planned to put into the config, but now it is abandoned.
	// Originally, the user could customize the number of columns and rows independently, but I found that it is hard to implement technically.
	// If there are 25 cells and the user wants to place them into two rows, then is the final number of columns presented 13 columns or 24 columns?
	// Since the subsequent logical code has been written and not want to change, they are represented here as aliases.
	const rows = columns, setRows = setColumns;
	/** The column or row count when the array mode is square. */
	const radicand = Math.ceil(Math.sqrt(count));
	// Flow direction or writing mode.
	const horizontalDirection = direction[0].endsWith("tb"), verticalDirection = direction[0].startsWith("tb"), rtlDirection = direction[0].includes("rl");
	// array mode booleans.
	const square = array[0] === "square", fixedColumns = array[0] === "fixed" && horizontalDirection, fixedRows = array[0] === "fixed" && verticalDirection;
	const columnReadonly = square || fixedRows, rowReadonly = square || fixedColumns;
	const order = useMemo(() => descending ? "descending" : "ascending", [descending]);
	const id = "grid-view" + useId(), fieldAnchorName = `--${id}-field`;
	// Show fast fill float toolbar while editing column or row count text box?
	const [fastFillShown, setFastFillShown] = useState(false);
	const columnInputRef = useDomRef<"input">(), rowInputRef = useDomRef<"input">();
	const fixedColumnsOrFixedRows = t.track.grid[horizontalDirection ? "fixedColumns" : "fixedRows"];
	// Grid span helper.
	const { maxSameLineLength, finalCrossLineIndex, find: findSpan } = useMemo(() => gridSpanHelper(count, square ? radicand : fixedColumns ? columns : rows, square ? [] : spans, square ? [] : blanks), [square, count, fixedColumns, columns, rows, spans, blanks, radicand]);
	// The row count which is automatically calculated from the customized column count, and vice versa.
	const autoRows = finalCrossLineIndex, autoColumns = finalCrossLineIndex;
	// Show custom cell editor flyout (e.g. span, column width...).
	const [flyoutEditor, setFlyoutEditor] = useState<"span" | "width" | "height" | "blank">(), previousFlyoutEditor = useDeferredValue(flyoutEditor), _flyoutEditor = flyoutEditor || previousFlyoutEditor;
	// **(For span and blank operation only.)** Set highlight cell index, other cells will become translucent.
	const [highLightCellIndex, setHighLightCellIndex] = useState(-1);
	// The cell that being set in the custom cell editor flyout.
	const [flyoutEditorCell, setFlyoutEditorCell] = useState<[number, number]>();
	// The state properties of colspan and rowspan of the cell that being set in the custom cell editor flyout.
	const [spanX, spanY, setSpanX, setSpanY] = useMemo(() => {
		const getItem = (draft = spans) => {
			let x = 1, y = 1, itemIndex = -1;
			for (const [index, span] of draft.entries())
				if (span.sameLine === flyoutEditorCell?.[0] && span.crossLine === flyoutEditorCell[1]) {
					[x, y, itemIndex] = [span.sameLineSpan || 1, span.crossLineSpan || 1, index];
					break;
				}
			return { x, y, itemIndex };
		};
		const { x, y } = getItem();
		const setSpan = (value: React.SetStateAction<number>, axis: "column" | "row") => setSpans(produce(draft => {
			if (!flyoutEditorCell) return;
			const { x, y, itemIndex } = getItem(draft);
			if (typeof value === "function") value = value(axis === "column" ? x : y);
			const [newX, newY] = axis === "column" ? [value, y] : [x, value];
			if (~itemIndex)
				[draft[itemIndex].sameLineSpan, draft[itemIndex].crossLineSpan] = [newX, newY];
			else
				draft.push({
					sameLine: flyoutEditorCell[0],
					crossLine: flyoutEditorCell[1],
					sameLineSpan: newX,
					crossLineSpan: newY,
				});
		}));
		return [x, y, (v: React.SetStateAction<number>) => setSpan(v, "column"), (v: React.SetStateAction<number>) => setSpan(v, "row")] as const;
	}, [flyoutEditorCell, spans]);
	// The column width or row height that being set in the custom cell editor flyout.
	const [flyoutEditorColumnRow, setFlyoutEditorColumnRow] = useState<[number, "column" | "row"]>();
	// The state properties of column width or row height of column or row of the cell that being set in the custom cell editor flyout.
	const [columnRowValue, columnRowType, setColumnRow] = useMemo(() => {
		const items = flyoutEditorColumnRow?.[1] === "column" ? columnWidths : rowHeights;
		const getItem = (draft = items) => {
			const itemIndex = draft.findIndex(item => item.index === flyoutEditorColumnRow?.[0]);
			const { value, type } = draft[itemIndex] ?? { value: 1, type: "star" };
			return { currentValue: value, type, itemIndex };
		};
		const { currentValue, type } = getItem();
		const setColumnRow: {
			(value: React.SetStateAction<number>): void;
			(type: WebMessageEvents.GridUnitType): void;
		} = value => {
			if (!flyoutEditorColumnRow) return;
			const setState = flyoutEditorColumnRow[1] === "column" ? setColumnWidths : setRowHeights;
			setState(produce(draft => {
				const { currentValue, type, itemIndex } = getItem(draft); // Values outside are not updated.
				if (typeof value === "function") value = value(currentValue);
				const [newValue, newType] = typeof value === "number" ? [value, type] : [currentValue, value];
				if (~itemIndex)
					[draft[itemIndex].value, draft[itemIndex].type] = [newValue, newType];
				else
					draft.push({
						index: flyoutEditorColumnRow[0],
						value: newValue,
						type: newType,
					});
			}));
		};
		return [currentValue, type, setColumnRow] as const;
	}, [flyoutEditorColumnRow, columnWidths, rowHeights, count, columns, autoRows]);
	// The state property of insert blank of cell that being set in the custom cell editor flyout.
	const [blank, setBlank] = useMemo(() => {
		const getItem = (draft = blanks) => {
			let count = 0, itemIndex = -1;
			for (const [index, blank] of draft.entries())
				if (blank.sameLine === flyoutEditorCell?.[0] && blank.crossLine === flyoutEditorCell[1]) {
					[count, itemIndex] = [blank.sameLineSpan || 0, index];
					break;
				}
			return { count, itemIndex };
		};
		const { count } = getItem();
		const setBlank = (value: React.SetStateAction<number>) => setBlanks(produce(draft => {
			if (!flyoutEditorCell) return;
			const { count, itemIndex } = getItem(draft);
			if (typeof value === "function") value = value(count);
			if (~itemIndex)
				draft[itemIndex].sameLineSpan = value;
			else
				draft.push({
					sameLine: flyoutEditorCell[0],
					crossLine: flyoutEditorCell[1],
					sameLineSpan: value,
				});
		}));
		return [count, (v: React.SetStateAction<number>) => setBlank(v)] as const;
	}, [flyoutEditorCell, blanks]);
	// Get CSS grid properties style value.
	const { gridTemplateColumns, gridTemplateRows, rulerColumns, rulerRows } = useGridTemplateCss(square ? [] : columnWidths, square ? [] : rowHeights, square ? radicand : columns, square ? radicand : autoRows, verticalDirection, projectWidth, projectHeight, maxSameLineLength);
	const [showOperationRecordDialog, setShowOperationRecordDialog] = useState(false);
	// Selected operation record item.
	const [operationRecordSelection, setOperationRecordSelection] = useState<(["span" | "blank", WebMessageEvents.GridSpanItem] | ["column" | "row", WebMessageEvents.GridColumnWidthRowHeightItem])[]>([]);
	const [operationRecordFilter, setOperationRecordFilter] = useState<"all" | "span" | "columnWidth" | "rowHeight" | "blank">("all");
	const [operationRecordFilterSpan, operationRecordFilterColumnWidth, operationRecordFilterRowHeight, operationRecordFilterBlank] = useMemo(() => [operationRecordFilter.in("all", "span"), operationRecordFilter.in("all", "columnWidth"), operationRecordFilter.in("all", "rowHeight"), operationRecordFilter.in("all", "blank")], [operationRecordFilter]);
	// Check if current filter of the operation record is empty.
	const [isCurrentOperationRecordFilterItemEmpty, setIsCurrentOperationRecordFilterItemEmpty] = useState(false);
	// Check if current filter of the operation record is any item selected.
	const isCurrentOperationRecordFilterItemAnySelected = useMemo(() => !!operationRecordSelection.find(([specified]) => operationRecordFilterSpan && specified === "span" || operationRecordFilterColumnWidth && specified === "column" || operationRecordFilterRowHeight && specified === "row" || operationRecordFilterBlank && specified === "blank"), [operationRecordSelection, operationRecordFilterSpan, operationRecordFilterColumnWidth, operationRecordFilterRowHeight, operationRecordFilterBlank]);
	// Operation record filter badges.
	const operationRecordFilterBadgeCounts = { span: spans.length, columnWidth: columnWidths.length, rowHeight: rowHeights.length, blank: blanks.length } as Record<typeof operationRecordFilter, number>;
	operationRecordFilterBadgeCounts.all = sum(...Object.values(operationRecordFilterBadgeCounts));
	const [flipHRandomTimestamp, setFlipHRandomTimestamp] = useState(0), [flipVRandomTimestamp, setFlipVRandomTimestamp] = useState(0);

	useSetLayoutEnabledOnSave("grid", true);
	const setPageCommandBarDisabled = pageStore.useSetCommandBarDisabled();
	useEffect(() => { setPageCommandBarDisabled(!!flyoutEditor); }, [flyoutEditor]);

	const closeFlyoutEditor = () => setFlyoutEditor(undefined);
	useEventListener(window, "keydown", e => flyoutEditor && e.code === "Escape" && closeFlyoutEditor(), undefined, [flyoutEditor]);
	useEventListener(window, "blur", () => closeFlyoutEditor(), undefined, [flyoutEditor]);
	const resetRecords = () => { setSpans([]); setColumnWidths([]); setRowHeights([]); setBlanks([]); };

	function deleteSelection() {
		for (const selected of operationRecordSelection)
			if (operationRecordFilterColumnWidth && selected[0] === "column")
				setColumnWidths(produce(draft => draft.removeAt(draft.findIndex(({ index }) => index === selected[1].index))));
			else if (operationRecordFilterRowHeight && selected[0] === "row")
				setRowHeights(produce(draft => draft.removeAt(draft.findIndex(({ index }) => index === selected[1].index))));
			else if (operationRecordFilterSpan && selected[0] === "span")
				setSpans(produce(draft => draft.removeAt(draft.findIndex(({ sameLine, crossLine }) => sameLine === selected[1].sameLine && crossLine === selected[1].crossLine))));
			else if (operationRecordFilterBlank && selected[0] === "blank")
				setBlanks(produce(draft => draft.removeAt(draft.findIndex(({ sameLine, crossLine }) => sameLine === selected[1].sameLine && crossLine === selected[1].crossLine))));
		if (operationRecordFilter === "all")
			setOperationRecordSelection([]);
		else {
			if (operationRecordFilterColumnWidth)
				setOperationRecordSelection(draft => draft.filter(([specified]) => specified !== "column"));
			if (operationRecordFilterRowHeight)
				setOperationRecordSelection(draft => draft.filter(([specified]) => specified !== "row"));
			if (operationRecordFilterSpan)
				setOperationRecordSelection(draft => draft.filter(([specified]) => specified !== "span"));
			if (operationRecordFilterBlank)
				setOperationRecordSelection(draft => draft.filter(([specified]) => specified !== "blank"));
		}
	}

	function cleanUpInvalidOperationItems() {
		/**
		 * Check if a given `value` is either undefined or it is less than or equal to a specified number `leq`?
		 * @param value - The number to check, which may be `undefined`.
		 * @param leq - The number to compare against.
		 * @returns Is `value` undefined or less than or equal to `leq`?
		 */
		const undefOrLeq = (value: number | undefined, leq: number): value is number => value === undefined || value <= leq;
		const keyPool = new SerializeKeyedSet<object>();
		setSpans(produce(draft => {
			for (let i = draft.length - 1; i >= 0; i--) {
				const item = draft[i];
				if (keyPool.has(item) || undefOrLeq(item.sameLineSpan, 1) && undefOrLeq(item.crossLineSpan, 1)) draft.removeAt(i);
				else keyPool.add(item);
			}
		}));
		keyPool.clear();
		setColumnWidths(produce(draft => {
			for (let i = draft.length - 1; i >= 0; i--) {
				const item = draft[i];
				if (keyPool.has(item) || item.value === 1 && item.type === "star") draft.removeAt(i);
				else keyPool.add(item);
			}
		}));
		keyPool.clear();
		setRowHeights(produce(draft => {
			for (let i = draft.length - 1; i >= 0; i--) {
				const item = draft[i];
				if (keyPool.has(item) || item.value === 1 && item.type === "star") draft.removeAt(i);
				else keyPool.add(item);
			}
		}));
		keyPool.clear();
		setBlanks(produce(draft => {
			for (let i = draft.length - 1; i >= 0; i--) {
				const item = draft[i];
				if (keyPool.has(item) || undefOrLeq(item.sameLineSpan, 0)) draft.removeAt(i);
				else keyPool.add(item);
			}
		}));
	}

	function resetFlyoutEditor() {
		switch (flyoutEditor) {
			case "span":
				setSpanX(1);
				setSpanY(1);
				break;
			case "width":
			case "height":
				setColumnRow(1);
				setColumnRow("star");
				break;
			case "blank":
				setBlank(0);
				break;
			default:
				break;
		}
	}

	function focusDiffusion(e: TargetType, corners: ReturnType<typeof findSpan>["corners"]) {
		const borderRadius = Object.values(corners).every(corner => !corner) ? null :
			(["topLeft", "topRight", "bottomRight", "bottomLeft"] as const)
				.map(corner => corners[corner] ? PREVIEW_GRID_BORDER_RADIUS + "px" : 0).join(" ");
		makeFocusDiffusionEffect(e, { borderRadius });
	}

	return (
		<>
			<StyledContainerPreview>
				<div className="command-bar-wrapper">
					<CommandBar.Group>
						<CommandBar position="right" autoCollapse>
							<CommandBar.Item icon="approvals_app" onClick={() => { cleanUpInvalidOperationItems(); setShowOperationRecordDialog(true); }} caption={t.track.grid.operationRecord} altCaption={t({ context: "short" }).track.grid.operationRecord} aria-haspopup="dialog" />
							<hr />
							<CommandBar.Item
								icon={square ? "grid" : "grid_kanban_vertical"}
								caption={t.track.grid.array}
								hovering
								dirBasedIcon={!square && direction[0]}
								onClick={() => array[1](array => arrayTypes.nextItem(array))}
							>
								<ItemsView view="list" current={array}>
									{arrayTypes.map(option => (
										<ItemsView.Item
											id={option}
											key={option}
											icon={option === "square" ? "grid" : "grid_kanban_vertical"}
											dirBasedIcon={direction[0]}
											details={t.descriptions.track.grid[option === "square" ? option : horizontalDirection ? "fixedColumns" : "fixedRows"]}
										>
											{t.track.grid[option === "square" ? option : horizontalDirection ? "fixedColumns" : "fixedRows"]}
										</ItemsView.Item>
									))}
								</ItemsView>
							</CommandBar.Item>
							<CommandBar.Item
								icon="lr_tb"
								caption={t.track.grid.direction}
								hovering
								dirBasedIcon={direction[0]}
								onClick={() => direction[1](direction => directionTypes.nextItem(direction))}
							>
								<ItemsView view="list" current={direction}>
									{directionTypes.map(option => (
										<ItemsView.Item
											id={option}
											key={option}
											icon="lr_tb"
											dirBasedIcon={option}
											details={t.descriptions.track.grid.direction[new VariableName(option).camel]}
										>
											{t.track.grid.direction[new VariableName(option).camel]}
										</ItemsView.Item>
									))}
								</ItemsView>
							</CommandBar.Item>
							<CommandBar.Item icon={fit[0] === "contain" ? "letterbox" : "aspect_ratio"} caption={t.fit} details={t.descriptions.track.grid.fit} hovering onClick={() => fit[1](fit => fitTypes.nextItem(fit))}>
								<ItemsView view="list" current={fit}>
									{fitTypes.map(option => (
										<ItemsView.Item
											id={option}
											key={option}
											icon={option === "contain" ? "letterbox" : "aspect_ratio"}
											details={t.descriptions.track.grid.fit[option]}
										>
											{t.fit[option]}
										</ItemsView.Item>
									))}
								</ItemsView>
							</CommandBar.Item>
							<CommandBar.Item icon={order} caption={t[order]} details={t.descriptions.track.descending} onClick={() => setDescending(desc => !desc)} />
							<hr />
							{...(["h", "v"] as const).map(d => {
								const flipKey = `${d}Flip`, isH = d === "h";
								return (
									<CommandBar.Item
										key={flipKey}
										icon={`flip_${d}`}
										caption={t.track.grid.mirrorEdges + " - " + t.prve.effects[flipKey]}
										altCaption={t.prve.effects[flipKey]}
										details={t.descriptions.track.grid.mirrorEdges[flipKey]}
										hovering
									>
										<ItemsView view="list" current={isH ? mirrorEdgesHFlip : mirrorEdgesVFlip}>
											{(isH ? parityTypes : parityTypes_rowsFirst).map(option => (
												<ItemsView.Item
													id={option}
													key={option}
													icon={getParityIcon(option)}
													onClick={option === "random" ? () => (isH ? setFlipHRandomTimestamp : setFlipVRandomTimestamp)(Date.now()) : undefined}
												>
													{getParityText(option)}
												</ItemsView.Item>
											))}
										</ItemsView>
									</CommandBar.Item>
								);
							})}
						</CommandBar>
					</CommandBar.Group>
					<Button icon="arrow_reset" accent="critical" className="reset-btn" hidden={!flyoutEditor} onClick={resetFlyoutEditor}>{t.reset}</Button>
				</div>

				<PreviewGridContainer>
					<PreviewGrid
						style={{
							"--grid-template-count": Math.min(square ? radicand : fixedColumns ? columns : rows, maxSameLineLength),
							"--padding": padding[0] + "px",
							"--fit": fit[0],
							"--project-width": projectWidth,
							"--project-height": projectHeight,
							gridTemplateColumns,
							gridTemplateRows,
							anchorName: "--preview-grid",
						}}
						className={[fixedRows ? "row" : "column", { square }]}
						role="figure"
						aria-label={t.preview}
						dir={rtlDirection ? "rtl" : "ltr"}
					>
						{forMap(count, i => {
							let { colStart, colEnd, colSpan, rowStart, rowEnd, rowSpan, blankCol, blankRow, corners } = findSpan(i);
							if (verticalDirection)
								[colStart, colEnd, colSpan, blankCol, rowStart, rowEnd, rowSpan, blankRow, corners.topRight, corners.bottomLeft] =
								[rowStart, rowEnd, rowSpan, blankRow, colStart, colEnd, colSpan, blankCol, corners.bottomLeft, corners.topRight];
							if (rtlDirection)
								[corners.topLeft, corners.topRight, corners.bottomLeft, corners.bottomRight] =
								[corners.topRight, corners.topLeft, corners.bottomRight, corners.bottomLeft];
							const isSpanned = colSpan > 1 || rowSpan > 1;
							const thisCell = [colStart - 1, rowStart - 1].shouldReversed(verticalDirection) as TwoD;
							const blankCell = [blankCol, blankRow].shouldReversed(verticalDirection) as TwoD;
							const trackIndex = descending ? count - i : i + 1;
							return (
								<div
									className="padding-wrapper"
									key={`${colStart},${rowStart}`}
									style={{ gridArea: [rowStart, colStart, rowEnd + 1, colEnd + 1].join(" / ") }}
								>
									<div
										className={[{
											hFlip: matchParity(mirrorEdgesHFlip[0], colStart, rowStart, `${flipHRandomTimestamp.toString(36)},h`),
											vFlip: matchParity(mirrorEdgesVFlip[0], colStart, rowStart, `${flipVRandomTimestamp.toString(36)},v`),
											highlight:
												flyoutEditor?.in("span", "blank") && trackIndex === highLightCellIndex ||
												flyoutEditor === "width" && flyoutEditorColumnRow?.[1] === "column" && flyoutEditorColumnRow[0] === colEnd - 1 ||
												flyoutEditor === "height" && flyoutEditorColumnRow?.[1] === "row" && flyoutEditorColumnRow[0] === rowEnd - 1,
										}]}
										role="img"
										aria-label={t.descriptions.track.grid.previewAria({
											context: isSpanned ? "span" : undefined,
											trackIndex,
											columnIndex: colStart,
											rowIndex: rowStart,
											columnCount: columns,
											rowCount: autoRows,
											columnSpan: colSpan,
											rowSpan,
										})}
										data-index={trackIndex}
										data-column-start={colStart}
										data-column-end={colEnd}
										data-row-start={rowStart}
										data-row-end={rowEnd}
										onMouseDown={e => e.button === 2 && focusDiffusion(e, corners)}
										onContextMenu={createContextMenu(([
											...square ? [
												{ label: t.descriptions.track.grid.squareCannotUseTheseFeatures({ fixed: fixedColumnsOrFixedRows }) },
												{ kind: "separator" },
											] as const : [],
											{ label: t.menu.grid.columnWidth, onClick() { setFlyoutEditor("width"); setFlyoutEditorColumnRow([colEnd - 1, "column"]); } },
											{ label: t.menu.grid.rowHeight, onClick() { setFlyoutEditor("height"); setFlyoutEditorColumnRow([rowEnd - 1, "row"]); } },
											{ kind: "separator" },
											{ label: t.menu.grid.span, onClick() { setFlyoutEditor("span"); setFlyoutEditorCell(thisCell); setHighLightCellIndex(trackIndex); } },
											{ label: t.menu.grid.insertBlank, onClick() { setFlyoutEditor("blank"); setFlyoutEditorCell(blankCell); setHighLightCellIndex(trackIndex); } },
										] as const).map(item => ({ ...item, enabled: !square })))}
									/>
								</div>
							);
						})}
					</PreviewGrid>
					<Ruler className="columns" style={{ gridTemplateColumns }} dir={rtlDirection ? "rtl" : "ltr"}>{rulerColumns.map((value, i) => <div key={i}>{value}</div>)}</Ruler>
					<Ruler className="rows" style={{ gridTemplateRows }}>{rulerRows.map((value, i) => <div key={i}>{value}</div>)}</Ruler>
				</PreviewGridContainer>

				<div>
					<Determinant>
						<div>
							<Label id={id} htmlFor="column">{t(square ? radicand : columns).track.grid.column}</Label>
							<div />
							<Label id={id} htmlFor="row">{t(square ? radicand : rows).track.grid.row}</Label>
							{["column", "x", "row"].map(key => {
								if (key === "x") return <Multiply key={key} />;
								const isColumn = key === "column", ref = isColumn ? columnInputRef : rowInputRef, readonly = isColumn ? columnReadonly : rowReadonly;
								return (
									<EventInjector
										key={key}
										onFocusIn={() => { if (!readonly) { setFastFillShown(true); ref.current?.focus(); } }}
										onFocusOut={() => setFastFillShown(false)}
									>
										<TextBox.Number
											id={`${id}-${key}`}
											aria-labelledby={`${id}-${key}-label`}
											value={
												square ? [radicand] :
												isColumn ?
													columnReadonly ? [autoColumns] : [columns, setColumns] :
													rowReadonly ? [autoRows] : [rows, setRows]
											}
											min={1}
											max={MAX_COL_ROW}
											decimalPlaces={0}
											readOnly={readonly}
											style={readonly ? undefined : { anchorName: fieldAnchorName }}
											inputRef={ref}
										/>
									</EventInjector>
								);
							})}
						</div>
						<div>
							<div />
							<Label id={id} htmlFor="padding">{t(padding[0]).track.grid.padding}</Label>
							<Multiply className="shadow" />
							<Tooltip title={t.descriptions.track.grid.padding} placement="block-start" offset={TOOLTIP_OFFSET}>
								<TextBox.Number
									id={`${id}-padding`}
									aria-labelledby={`${id}-padding-label`}
									aria-description={t(padding[0]).track.grid.padding}
									value={padding}
									min={0}
									max={50}
									defaultValue={0}
									suffix={t.units.densityIndependentPixel}
								/>
							</Tooltip>
						</div>
					</Determinant>

					<Attrs hidden={!flyoutEditor}>
						<Mask onMouseDown={e => { stopEvent(e); closeFlyoutEditor(); }} />
						<FlyoutEditor>
							{_flyoutEditor === "span" ? (
								<div className="span">
									<Label id={id} htmlFor="colspan">{t.track.grid.columnSpan}</Label>
									<div />
									<Label id={id} htmlFor="rowspan">{t.track.grid.rowSpan}</Label>
									{(["colspan", "x", "rowspan"] as const).map(key => key === "x" ? <Multiply key={key} /> : (
										<TextBox.Number
											key={key}
											id={`${id}-${key}`}
											aria-labelledby={`${id}-${key}-label`}
											value={key === (verticalDirection ? "rowspan" : "colspan") ? [spanX, setSpanX] : [spanY, setSpanY]}
											min={1}
											max={100}
											decimalPlaces={0}
											defaultValue={1}
										/>
									))}
								</div>
							) : _flyoutEditor === "width" || _flyoutEditor === "height" ? (
								<div className="length">
									<Label id={id} htmlFor="length">{t.track.grid[_flyoutEditor === "width" ? "columnWidth" : "rowHeight"]}</Label>
									<TextBox.Number
										id={`${id}-length`}
										aria-labelledby={`${id}-length-label`}
										value={[columnRowValue, setColumnRow]}
										min={0}
										max={_flyoutEditor === "width" ? projectWidth : projectHeight}
										defaultValue={1}
										disabled={columnRowType === "auto"}
									/>
									{(["auto", "pixel", "star"] as const).map(unit => (
										<RadioButton
											key={unit}
											id={unit}
											value={[columnRowType, setColumnRow]}
											disabled={unit === "auto"} // Auto type is not supported now.
										>
											{getGridUnitTypeName(unit, columnRowValue)}
										</RadioButton>
									))}
								</div>
							) : _flyoutEditor === "blank" ? (
								<div className="length">
									<Label id={id} htmlFor="blank">{t(blank).track.grid.insertBlank}</Label>
									<TextBox.Number
										id={`${id}-blank`}
										aria-labelledby={`${id}-blank-label`}
										value={[blank, setBlank]}
										min={0}
										max={100}
										defaultValue={0}
										suffix={t.units.piece}
									/>
								</div>
							) : undefined}
						</FlyoutEditor>
					</Attrs>
				</div>
			</StyledContainerPreview>

			<Portal>
				<Flyout
					anchorName={fieldAnchorName}
					position="top"
					shown={[fastFillShown, setFastFillShown]}
					autoPadding="xy"
					portal={document.body}
					offset={TOOLTIP_OFFSET}
					autoFocus={false}
					onMouseDown={e => e.preventDefault()}
				>
					<Flyout.Item icon="edit_lightning" title={t.track.grid.fastFill} style={{ paddingInlineStart: "12px" }} />
					<div className="items">
						<FastFillOptions
							value={fixedRows ? [rows, setRows] : [columns, setColumns]}
							getInputRef={() => [columnInputRef, rowInputRef].find(el => el.current?.readOnly === false)}
							options={[
								{ id: "min", value: 1 },
								{ id: "max", value: MAX_COL_ROW },
								{ id: "square", value: radicand },
								{ id: "numberOfSelectedTracks", value: count },
								{ id: "transpose", value: fixedColumns ? autoRows : autoColumns, unselected: true },
							]}
						/>
					</div>
				</Flyout>
			</Portal>

			<ContentDialog
				shown={[showOperationRecordDialog, setShowOperationRecordDialog]}
				title={t.track.grid.operationRecord}
				buttons={close => (
					<>
						<Button onClick={resetRecords} disabled={isCurrentOperationRecordFilterItemEmpty}>{operationRecordFilter === "all" ? t.reset : t.resetThisPage}</Button>
						<Button onClick={deleteSelection} disabled={!isCurrentOperationRecordFilterItemAnySelected}>{t.deleteSelection}</Button>
						<Button autoFocus accent onClick={close}>{t.close}</Button>
					</>
				)}
			>
				<Filter current={[operationRecordFilter, setOperationRecordFilter]}>
					{(["all", "span", "columnWidth", "rowHeight", "blank"] as const).map(key => {
						const count = operationRecordFilterBadgeCounts[key];
						return <Filter.Item key={key} id={key} badge={count}>{key === "all" ? t(count).all : t(count).track.grid[key]}</Filter.Item>;
					})}
				</Filter>
				<ItemsView
					view="list"
					multiple
					current={[operationRecordSelection, setOperationRecordSelection]}
					emptyState={(
						<EmptyMessage
							icon={<ApprovalsAppIcon />}
							title={t.empty.operationRecord.title}
							details={t.empty.operationRecord.details({ fixed: fixedColumnsOrFixedRows })}
						/>
					)}
					selectAll
					onItemEmptyChange={setIsCurrentOperationRecordFilterItemEmpty}
				>
					{[
						...!operationRecordFilterSpan ? [] : spans.map(span => [["span", span] as const, {
							[t(1).track.grid.column]: (verticalDirection ? span.crossLine : span.sameLine) + 1,
							[t(1).track.grid.row]: (verticalDirection ? span.sameLine : span.crossLine) + 1,
							[t.track.grid.columnSpan]: verticalDirection ? span.crossLineSpan : span.sameLineSpan,
							[t.track.grid.rowSpan]: verticalDirection ? span.sameLineSpan : span.crossLineSpan,
						}] as const),
						...!operationRecordFilterColumnWidth ? [] : columnWidths.map(columnWidth => [["column", columnWidth] as const, {
							[t(1).track.grid.column]: columnWidth.index,
							[t.width]: columnWidth.value,
							[t.unit]: getGridUnitTypeName(columnWidth.type, columnWidth.value),
						}] as const),
						...!operationRecordFilterRowHeight ? [] : rowHeights.map(rowHeight => [["row", rowHeight] as const, {
							[t(1).track.grid.row]: rowHeight.index,
							[t.height]: rowHeight.value,
							[t.unit]: getGridUnitTypeName(rowHeight.type, rowHeight.value),
						}] as const),
						...!operationRecordFilterBlank ? [] : blanks.map(blank => [["blank", blank] as const, {
							[t(1).track.grid.column]: (verticalDirection ? blank.crossLine : blank.sameLine) + 1,
							[t(1).track.grid.row]: (verticalDirection ? blank.sameLine : blank.crossLine) + 1,
							[t(blank.sameLineSpan).track.grid.blank]: blank.sameLineSpan,
						}] as const),
					].map(([id, item]) => (
						<ItemsView.Item id={id} key={JSON.stringify(id)}>
							<OperationRecordItem>{item}</OperationRecordItem>
						</ItemsView.Item>
					))}
				</ItemsView>
			</ContentDialog>
		</>
	);
}

function FastFillOptions({ value: [currentValue, setCurrentValue], options, getInputRef }: {
	value: StatePropertyNonNull<number>;
	options: { id: string; value: number; unselected?: boolean }[];
	getInputRef(): MaybeRef<HTMLInputElement | undefined | null>;
}) {
	const focus = () => toValue(getInputRef())?.focus();
	return options.map(({ id, value, unselected }) => (
		<ToggleButton
			key={id}
			checked={[!(unselected || !(currentValue === value))]}
			minWidthUnbounded
			onPointerDown={() => { setCurrentValue(value); focus(); }}
			onPointerUp={focus}
			appearance="intense-hyperlink"
		>
			{t.track.grid[id]}
		</ToggleButton>
	));
}

function gridSpanHelper(count: number, thisLineLength: number, spans: WebMessageEvents.GridSpanItem[], blanks: WebMessageEvents.GridSpanItem[]) {
	const array: number[] = [];
	const spanMap = new Map<string, [sameLineSpan: number, crossLineSpan: number]>(); // Get colspan and rowspan from "x,y".
	const blankMap = new Map<string, number>(); // Get blank count from "x,y".
	const indexToBlankMap: [sameLine: number, crossLine: number, count: number][] = []; // Get blank x, y, and count from grid index.
	for (const { sameLine, crossLine, sameLineSpan, crossLineSpan } of spans)
		spanMap.set(`${sameLine},${crossLine}`, [sameLineSpan || 1, crossLineSpan || 1]);
	for (const { sameLine, crossLine, sameLineSpan } of blanks)
		blankMap.set(`${sameLine},${crossLine}`, sameLineSpan || 0);
	let x = 0, y = 0;
	const getIndex = (_x = x, _y = y) => _y * thisLineLength + _x;
	const nextIndex = () => {
		while (array[getIndex()] !== undefined)
			if (++x >= thisLineLength)
				x = (y++, 0);
	};
	for (let i = 0; i < count; i++) {
		let index = getIndex();
		let blank = blankMap.get(`${x},${y}`)!;
		if (!Number.isFinite(blank) || blank < 0) blank = 0; // `Number.isFinite` will exclude undefined from `blank`.
		indexToBlankMap[i] = [x, y, blank];
		if (blank > 0)
			while (blank--) {
				array[index] = -1;
				nextIndex();
				index = getIndex();
			}
		array[index] = i;
		const span = spanMap.get(`${x},${y}`);
		if (span)
			for (let _y = y; _y < y + span[1]; _y++)
				for (let _x = x; _x < Math.min(x + span[0], thisLineLength); _x++) {
					const newIndex = getIndex(_x, _y);
					if (array[newIndex] !== undefined && newIndex !== index) break;
					array[newIndex] = i;
				}
		nextIndex();
	}
	array.trimEnd([undefined!, -1]);
	const maxCrossLineLength = Math.ceil(array.length / thisLineLength);
	const array2d = Array.from({ length: maxCrossLineLength }, (_, i) => array.slice(i * thisLineLength, (i + 1) * thisLineLength));
	// console.table(array2d);
	const maxSameLineLength = Math.max(...array2d.map(i => i.findLastIndex(index => index !== undefined) + 1));
	// Do not name it `lastCrossLineIndex`, other developers may mistake it for `previousCrossLineIndex`.
	const finalCrossLineIndex = (array.findLastIndex(index => index !== undefined) / thisLineLength | 0) + 1;
	return {
		maxSameLineLength,
		finalCrossLineIndex,
		find(index: number) {
			if (index >= count) return {} as never;
			const first = array.indexOf(index), last = array.lastIndexOf(index);
			const sameLineStart = first % thisLineLength, sameLineEnd = last % thisLineLength, crossLineStart = first / thisLineLength | 0, crossLineEnd = last / thisLineLength | 0;
			const sameLineSpan = sameLineEnd - sameLineStart + 1, crossLineSpan = crossLineEnd - crossLineStart + 1;
			const blankEntry = indexToBlankMap[index];
			const corners = {
				topLeft: sameLineStart === 0 && crossLineStart === 0,
				topRight: sameLineEnd === maxSameLineLength - 1 && crossLineStart === 0,
				bottomLeft: sameLineStart === 0 && crossLineEnd === maxCrossLineLength - 1,
				bottomRight: sameLineEnd === maxSameLineLength - 1 && crossLineEnd === maxCrossLineLength - 1,
			};
			return {
				colStart: sameLineStart + 1,
				colEnd: sameLineEnd + 1,
				colSpan: sameLineSpan,
				rowStart: crossLineStart + 1,
				rowEnd: crossLineEnd + 1,
				rowSpan: crossLineSpan,
				blankCol: blankEntry[0],
				blankRow: blankEntry[1],
				blankCount: blankEntry[2],
				corners,
			};
		},
	};
}

function useGridTemplateCss(columnWidths: WebMessageEvents.GridColumnWidthRowHeightItem[], rowHeights: WebMessageEvents.GridColumnWidthRowHeightItem[], columns: number, rows: number, vertical: boolean, projectWidth: number, projectHeight: number, maxSameLineLength: number) {
	columns = Math.min(columns, maxSameLineLength);
	if (vertical) [columns, rows] = [rows, columns];
	const getValue = (value: number, type: WebMessageEvents.GridUnitType, specified: "width" | "height") =>
		type === "auto" ? "auto" : type === "star" ? value + "fr" : `calc(${value} / ${specified === "width" ? projectWidth : projectHeight} * 100cq${specified === "width" ? "w" : "h"})`;
	const getRulerValue = (value: number, type: WebMessageEvents.GridUnitType) =>
		type === "auto" ? t.auto : value === 1 && type === "star" ? ASTERISK : value + (type === "star" ? ASTERISK : type === "pixel" ? "px" : "");
	const toCssString = (items: string[]) => items.every(item => item === "1fr") ? undefined : items.join(" ");
	return useMemo(() => {
		const gridTemplateColumns = new Array<string>(columns).fill("1fr"), gridTemplateRows = new Array<string>(rows).fill("1fr");
		const rulerColumns = new Array<string>(columns).fill(ASTERISK), rulerRows = new Array<string>(rows).fill(ASTERISK);
		for (const { index, value, type } of columnWidths)
			if (index in gridTemplateColumns) {
				gridTemplateColumns[index] = getValue(value, type, "width");
				rulerColumns[index] = getRulerValue(value, type);
			}
		for (const { index, value, type } of rowHeights)
			if (index in gridTemplateRows) {
				gridTemplateRows[index] = getValue(value, type, "height");
				rulerRows[index] = getRulerValue(value, type);
			}
		return { gridTemplateColumns: toCssString(gridTemplateColumns), gridTemplateRows: toCssString(gridTemplateRows), rulerColumns, rulerRows };
	}, [columnWidths, rowHeights, columns, rows, vertical, projectWidth, projectHeight]);
}

const StyledOperationRecordItem = styled.div`
	display: grid;
	grid-auto-flow: column;
	grid-template-rows: repeat(2, auto);
	grid-template-columns: repeat(4, 1fr);

	.key {
		${styles.effects.text.caption};
	}

	.value {
		${styles.effects.text.subtitle};
	}

	.items-view-item .text:has(&) {
		inline-size: 100%;
	}

	.items-view-item:has(&) {
		padding-inline: 0;
	}
`;

function OperationRecordItem({ children = {} }: { children?: Record<string, Any> }) {
	return (
		<StyledOperationRecordItem>
			{Object.entries(children).map(([key, value]) => (
				<Fragment key={key}>
					<p className="key">{key}</p>
					<p className="value">{value}</p>
				</Fragment>
			))}
		</StyledOperationRecordItem>
	);
}
