const curves = {
	all: ["linear", "fast", "slow", "smooth", "sharp", "hold"],
	holdLinearOnly: ["hold", "linear"],
	exceptHold: ["linear", "fast", "slow", "smooth", "sharp"],
} as const;

interface Props {
	/** Curve type. */
	curve: StateProperty<CurveType>;
	/**
	 * Constrain the available curve types.
	 * - "all": linear, fast, slow, smooth, sharp, hold;
	 * - "holdLinearOnly": hold, linear;
	 * - "exceptHold": linear, fast, slow, smooth, sharp.
	 * @default "all"
	 */
	subset?: keyof typeof curves;
	children?: never;
}

function CurveComboBox({ curve, subset = "all" }: Props) {
	const subsetCurves = curves[subset];
	return (
		<ComboBox
			ids={subsetCurves}
			options={subsetCurves.map(curve => t.curve[curve])}
			icons={subsetCurves.map(curve => `curves/${curve}` as const)}
			current={curve}
		/>
	);
}

export /* @internal */ default function ExpanderItemCurve({ curve, subset }: Props) {
	return (
		<Expander.Item title={t.curve.interpolation} details={t.descriptions.curve.interpolation} icon="curve">
			<CurveComboBox curve={curve} subset={subset} />
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
		margin-inline-end: 8px;

		.icon {
			grid-area: 1 / 1;

			&:last-of-type {
				scale: 1 -1;
			}
		}
	}
`;

export /* @internal */ function ExpanderItemCrossfadeCurve({ curve: [[multiplicandCurve, reciprocalCurve], setCurve], subset }: Override<Props, {
	/** Two curve types for creating crossfades. */
	curve: StatePropertyNonNull<CrossfadeCurveType>;
}>) {
	const setMultiplicandCurve = (multiplicandCurve: CurveType) => setCurve(([, reciprocalCurve]) => [multiplicandCurve, reciprocalCurve]);
	const setReciprocalCurve = (reciprocalCurve: CurveType) => setCurve(([multiplicandCurve]) => [multiplicandCurve, reciprocalCurve]);

	return (
		<Expander.Item title={t.curve.crossfade} details={t.descriptions.curve.crossfade} icon="curve" wrapActionsWhenNarrow>
			<StyledCrossfadeCurveAction>
				<div className="synthetic-icon">
					<Icon name={`curves/${multiplicandCurve}`} />
					<Icon name={`curves/${reciprocalCurve}`} />
				</div>
				<CurveComboBox curve={[multiplicandCurve, setMultiplicandCurve]} subset={subset} />
				<CurveComboBox curve={[reciprocalCurve, setReciprocalCurve]} subset={subset} />
			</StyledCrossfadeCurveAction>
		</Expander.Item>
	);
}
