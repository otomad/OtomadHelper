const curves = ["linear", "fast", "slow", "smooth", "sharp", "hold"] as const;

function CurveComboBox({ curve }: FCP<{
	curve: StateProperty<CurveType>;
}>) {
	return <ComboBox ids={curves} options={curves.map(curve => t.curve[curve])} icons={curves.map(curve => `curves/${curve}` as const)} current={curve} />;
}

export /* @internal */ default function ExpanderItemCurve({ curve }: FCP<{
	curve: StateProperty<CurveType>;
	children?: never;
}, "div">) {
	return (
		<Expander.Item title={t.curve} details={t.descriptions.curve} icon="curve">
			<CurveComboBox curve={curve} />
		</Expander.Item>
	);
}

const StyledCrossfadeCurveAction = styled.div`
	display: flex;
	gap: 8px;
	align-items: center;

	.combo-box {
		inline-size: 125px;
	}

	.synthetic-icon {
		display: grid;

		.icon {
			grid-area: 1 / 1;

			&:last-of-type {
				scale: 1 -1;
			}
		}
	}
`;

export /* @internal */ function ExpanderItemCrossfadeCurve({ curve: [[inCurve, outCurve], setCurve] }: FCP<{
	curve: StatePropertyNonNull<[CurveType, CurveType]>;
	children?: never;
}, "div">) {
	const setInCurve = (inCurve: CurveType) => setCurve(([, outCurve]) => [inCurve, outCurve]);
	const setOutCurve = (outCurve: CurveType) => setCurve(([inCurve]) => [inCurve, outCurve]);

	return ( // TODO: Update title and details text.
		<Expander.Item title={t.curve} details={t.descriptions.curve} icon="curve" wrapActionsWhenNarrow>
			<StyledCrossfadeCurveAction>
				<div className="synthetic-icon">
					<Icon name={`curves/${inCurve}`} />
					<Icon name={`curves/${outCurve}`} />
				</div>
				<CurveComboBox curve={[inCurve, setInCurve]} />
				<CurveComboBox curve={[outCurve, setOutCurve]} />
			</StyledCrossfadeCurveAction>
		</Expander.Item>
	);
}
