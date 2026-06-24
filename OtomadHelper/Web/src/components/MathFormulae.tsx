namespace MathFormulae {
	export /* @internal */ function For39() {
		return (
			<math displaystyle="false" aria-label={t.aria.formulaFor39}>
				<semantics>
					<mrow>
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
					</mrow>
					<annotation-xml encoding="application/mathml-content+xml">
						<apply>
							<plusminus />
							<apply>
								<divide />
								<cn>12</cn>
								<apply>
									<log />
									<logbase>
										<cn>10</cn>
									</logbase>
									<cn>2</cn>
								</apply>
							</apply>
						</apply>
					</annotation-xml>
					<annotation encoding="application/x-tex">
						{String.raw`\pm\frac{12}{\lg{2}}\ \left(\approx\pm39.863137\right)`}
					</annotation>
					<annotation encoding="text/plain">
						{String.raw`±12/lg2 (≈±39.863137)`}
					</annotation>
				</semantics>
			</math>
		);
	}

	export /* @internal */ function For51() {
		return (
			<math displaystyle="false" aria-label={t.aria.formulaFor51}>
				<semantics>
					<mrow>
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
					</mrow>
					<annotation-xml encoding="application/mathml-content+xml">
						<apply>
							<in />
							<ci>Δp</ci>
							<interval closure="closed">
								<apply>
									<minus />
									<apply>
										<divide />
										<cn>12</cn>
										<apply>
											<log />
											<logbase>
												<cn>20</cn>
											</logbase>
											<cn>2</cn>
										</apply>
									</apply>
								</apply>
								<cn type="integer">24</cn>
							</interval>
						</apply>
					</annotation-xml>
					<annotation encoding="application/x-tex">
						{String.raw`\Delta p\in\left[-\frac{12}{\log_{20}{2}},+24\right]\ \left(\approx-51.863137\text{ – }+24\right)`}
					</annotation>
					<annotation encoding="text/plain">
						{String.raw`Δp∈[-12/log₂₀(2),+24] (≈-51.863137 – +24)`}
					</annotation>
				</semantics>
			</math>
		);
	}
}

export default MathFormulae;
