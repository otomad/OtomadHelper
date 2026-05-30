namespace MathFormulae {
	export /* @internal */ function For39() {
		return (
			<math aria-label={t.aria.formulaFor39}>
				<mrow>
					<mo>±</mo>
					<mfrac>
						<mn>12</mn>
						<mrow>
							<mrow>
								<mi>lg</mi>
								<mspace width="0.1667em" />
							</mrow>
							<mn>2</mn>
						</mrow>
					</mfrac>
				</mrow>
				<mspace width="0.75ch" />
				<mrow>
					<mo form="prefix" stretchy="false">(</mo>
					<mo>≈</mo>
					<mo form="prefix" stretchy="false">±</mo>
					<mn>39.863137</mn>
					<mo form="postfix" stretchy="false">)</mo>
				</mrow>
			</math>
		);
	}

	export /* @internal */ function For51() {
		return (
			<math aria-label={t.aria.formulaFor51}>
				<mrow>
					<mi mathvariant="normal">Δ</mi>
					<mi>p</mi>
				</mrow>
				<mo>∈</mo>
				<mrow>
					<mo fence="true" form="prefix" stretchy="true">[</mo>
					<mo form="prefix" stretchy="false">−</mo>
					<mfrac>
						<mn>12</mn>
						<mrow>
							<msub>
								<mi>log</mi>
								<mn>20</mn>
							</msub>
							<mn>2</mn>
						</mrow>
					</mfrac>
					<mo separator="true" stretchy="true">,</mo>
					<mo form="prefix" stretchy="false">+</mo>
					<mn>24</mn>
					<mo fence="true" form="postfix" stretchy="true">]</mo>
				</mrow>
				<mspace width="0.75ch" />
				<mrow>
					<mo fence="true" form="prefix" stretchy="true">(</mo>
					<mo>≈</mo>
					<mo form="prefix" stretchy="false">−</mo>
					<mn>51.863137</mn>
					<mspace width="0.5ch" />
					<mtext>{t.rangeDash}</mtext>
					<mspace width="0.5ch" />
					<mo form="prefix" stretchy="false">+</mo>
					<mn>24</mn>
					<mo fence="true" form="postfix" stretchy="true">)</mo>
				</mrow>
			</math>
		);
	}
}

export default MathFormulae;
