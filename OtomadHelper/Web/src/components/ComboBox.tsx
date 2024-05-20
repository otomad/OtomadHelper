import { StyledButton } from "./Button";

const StyledComboBox = styled(StyledButton)`
	min-height: 32px;
	padding: 4px 11px;

	.content {
		${styles.mixins.square("100%")};
		display: flex;
		gap: 8px;
		align-items: center;

		.text {
			${styles.effects.text.body};
			width: 100%;
		}

		.icon {
			flex-shrink: 0;
			font-size: 16px;
		}
	}

	select& option {
		background-color: ${c("background-color")};
	}
`;

export default function ComboBox<T extends string>({ options, current: [current, setCurrent] }: FCP<{
	/** The options of the combo box. */
	options: T[];
	/** The selected option of the combo box. */
	current: StateProperty<T>;
}>) {
	async function showComboBox(e: MouseEvent) {
		const rect = (e.currentTarget as HTMLButtonElement).getBoundingClientRect();
		const result = await bridges.bridge.showComboBox([rect.x, rect.y, rect.width, rect.height], current!, options);
		setCurrent?.(result as T);
	}

	if (window.isWebView)
		return (
			<StyledComboBox onClick={showComboBox}>
				<div className="content">
					<div className="text">{current}</div>
					<Icon name="chevron_down" />
				</div>
			</StyledComboBox>
		);
	else // fallback in a normal browser
		return (
			<StyledComboBox as="select" value={current} onChange={e => setCurrent?.(e.currentTarget.value as T)}>
				{options.map(option => <option key={option} value={option}>{option}</option>)}
			</StyledComboBox>
		);
}
