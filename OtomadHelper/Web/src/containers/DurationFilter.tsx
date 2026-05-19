import { defaultDurationFilter } from "helpers/default-configs";

export /* @internal */ const durationFilterUnits = ["beat", "second"] as const;

const StyledDurationFilter = styled.div`
	display: grid;
	grid-template-columns: auto auto 200px auto;
	gap: 8px;
	justify-content: center;
	align-items: center;

	.logic-gate,
	.units {
		grid-row: span 2;
	}

	.logic-gate {
		${styles.text.bodyLarge};
		min-inline-size: 3em;
		margin-inline-end: 2px;
		text-align: end;
	}

	.button.compare {
		min-inline-size: unset;

		.text {
			margin-block-start: -3px;
		}
	}
`;

const StyledDurationFilterWrapper = styled(Expander.ChildWrapper)`
	display: grid;
	grid-template-columns: 1fr auto 1fr;
	gap: 8px;
	place-items: center end;

	> :nth-child(1) {
		grid-column: 2;
	}

	> :nth-child(2) {
		grid-column: 3;
	}

	@container page (width < 684px) {
		display: flex;
		flex-direction: column;
		justify-content: revert;

		> :nth-child(2) {
			align-self: end;
		}
	}
`;

const StyledDurationFilterPreview = styled(Expander.ChildWrapper)`
	p {
		${styles.effects.textAlignCenterCjk};

		&.caption {
			${styles.text.caption};
		}

		&.formula {
			${styles.text.body};

			math {
				${styles.text.bodyLarge};
				margin-inline: 1ex;
			}
		}
	}
`;

export default function DurationFilter({ filter, target: _target }: {
	filter: StatePropertyNonNull<Config.DurationFilter>;
	target: "note" | "clip";
}) {
	if (_target == null)
		throw new RangeError("You have not provide the `target` property in `DurationFilter`");
	const { hideUseTips } = useSnapshot(configStore.settings);
	const { min: min_, max: max_, minEqual: minEqual_, maxEqual: maxEqual_, unit: _unit } = deconstructState(filter);
	const min = min_[0], max = max_[0], minEqual = minEqual_[0], maxEqual = maxEqual_[0];
	const hasMin = Number.isFinite(min), hasMax = Number.isFinite(max);
	const tO = tAlias.aria.operators;
	const oneValue = !hasMin || !hasMax;
	const unit = (plural: number) => t.units[_unit[0]]({ count: plural, context: "full" }), target = t(11)[_target];
	const caption = useMemo(() => {
		if (isAllPassed(filter[0]))
			return tO.allPassed;
		if (oneValue) {
			const value = hasMin ? min : max;
			return tO.keepDuration_oneValue({
				target, unit: unit(value), value,
				compare: hasMin ? minEqual ? tO.geq : tO.gt : maxEqual ? tO.leq : tO.lt,
			});
		} else if (min === max)
			return tO.keepDuration_oneValue({
				target, unit: unit(min), value: min,
				compare: minEqual ? tO.eq : tO.neq,
			});
		else
			return tO.keepDuration_twoValues((() => {
				const options: Parameters<typeof tO.keepDuration_twoValues>[0] = {
					target, unit1: unit(min), unit2: unit(max),
					value1: min, value2: max,
					compare1: minEqual ? tO.geq : tO.gt,
					compare2: maxEqual ? tO.leq : tO.lt,
					logic: min <= max ? tO.and : tO.or,
				};
				if (min > max) {
					[options.unit1, options.unit2] = [options.unit2, options.unit1];
					[options.value1, options.value2] = [options.value2, options.value1];
					[options.compare1, options.compare2] = [options.compare2, options.compare1];
				}
				return options;
			})());
	}, [min, max, minEqual, maxEqual, _unit[0]]);

	function reset() {
		filter[1]({ ...defaultDurationFilter });
	}

	return (
		<>
			<StyledDurationFilterPreview>
				<p className="formula" aria-hidden>
					{tO.keepDuration}
					<RangeFormula filter={filter[0]} />
					{unit(2).toLocaleLowerCase()}
				</p>
				<p className="caption">{caption}</p>
			</StyledDurationFilterPreview>
			<StyledDurationFilterWrapper>
				<StyledDurationFilter>
					<p className="logic-gate">
						{
							oneValue ? "" :
							min < max ? tO.and :
							min > max ? tO.or :
							minEqual && maxEqual ? tO.and : tO.or
						}
					</p>
					<Tooltip title={tO.comparisonOperator} placement="block-start" disabled={hideUseTips}>
						<Button className="compare" onClick={() => minEqual_[1](equal => !equal)}>{minEqual ? "≥" : ">"}</Button>
					</Tooltip>
					<TextBox.Number value={min_} decimalPlaces={3} min={0} required={false} placeholder="−∞" />
					<ComboBox className="units" current={_unit} ids={durationFilterUnits} options={durationFilterUnits.map(unit => t(11).units[unit])} />
					<Tooltip title={tO.comparisonOperator} placement="block-end" disabled={hideUseTips}>
						<Button className="compare" onClick={() => maxEqual_[1](equal => !equal)}>{maxEqual ? "≤" : "<"}</Button>
					</Tooltip>
					<TextBox.Number value={max_} decimalPlaces={3} min={0} required={false} placeholder="+∞" />
				</StyledDurationFilter>
				<Button icon="arrow_reset" accent="critical" subtle extruded onClick={reset}>{t.reset}</Button>
			</StyledDurationFilterWrapper>
		</>
	);
}

function isAllPassed({ min, max, minEqual, maxEqual }: Config.DurationFilter) {
	const hasMin = Number.isFinite(min), hasMax = Number.isFinite(max);
	return !hasMin && !hasMax || min === max && minEqual !== maxEqual;
}

DurationFilter.isAllPassed = isAllPassed;

function RangeFormula({ filter }: { filter: Config.DurationFilter }) {
	const ARGUMENT = "x";
	const { min, max, minEqual, maxEqual } = filter;
	const hasMin = Number.isFinite(min), hasMax = Number.isFinite(max);

	const predicate = (() => {
		if (isAllPassed(filter))
			return <mo>⊤</mo>;
		if (!hasMin || !hasMax)
			return (
				<>
					<mi>{ARGUMENT}</mi>
					<mo>{hasMin ? minEqual ? "≥" : ">" : maxEqual ? "≤" : "<"}</mo>
					<mn>{hasMin ? min : max}</mn>
				</>
			);
		else if (min === max)
			return (
				<>
					<mi>{ARGUMENT}</mi>
					<mo>{minEqual ? "=" : "≠"}</mo>
					<mn>{min}</mn>
				</>
			);
		else if (min < max)
			return (
				<>
					<mn>{min}</mn>
					<mo>{minEqual ? "≤" : "<"}</mo>
					<mi>{ARGUMENT}</mi>
					<mo>{maxEqual ? "≤" : "<"}</mo>
					<mn>{max}</mn>
				</>
			);
		else
			return (
				<>
					<mi>{ARGUMENT}</mi>
					<mo>{maxEqual ? "≤" : "<"}</mo>
					<mn>{max}</mn>
					<mo>∨</mo>
					<mi>{ARGUMENT}</mi>
					<mo>{minEqual ? "≥" : ">"}</mo>
					<mn>{min}</mn>
				</>
			);
	})();

	return (
		<math>
			<mrow>
				<mo form="prefix" stretchy="false">{"{"}</mo>
				<mi>{ARGUMENT}</mi>
				<mo>∈</mo>
				<msub>
					<mi>ℝ</mi>
					<mrow>
						<mo lspace="0em" rspace="0em">≥</mo>
						<mn>0</mn>
					</mrow>
				</msub>
				<mo lspace="0.22em" rspace="0.22em" stretchy="false">|</mo>
				{predicate}
				<mo form="postfix" stretchy="false">{"}"}</mo>
			</mrow>
		</math>
	);
}
