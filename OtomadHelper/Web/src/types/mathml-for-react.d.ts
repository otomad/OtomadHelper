// DELETE: Delete it after the PR is merged: https://github.com/DefinitelyTyped/DefinitelyTyped/pull/71187/files

export { };

declare module "react/jsx-runtime" {
	namespace JSX {
		interface IntrinsicElements {
			// MathML
			math: React.MathMLProps<React.MathMLMathAttributes, MathMLElement>;
			merror: React.MathMLProps<React.MathMLMerrorAttributes, MathMLElement>;
			mfrac: React.MathMLProps<React.MathMLMfracAttributes, MathMLElement>;
			mi: React.MathMLProps<React.MathMLMiAttributes, MathMLElement>;
			mmultiscripts: React.MathMLProps<React.MathMLMmultiscriptsAttributes, MathMLElement>;
			mn: React.MathMLProps<React.MathMLMnAttributes, MathMLElement>;
			mo: React.MathMLProps<React.MathMLMoAttributes, MathMLElement>;
			mover: React.MathMLProps<React.MathMLMoverAttributes, MathMLElement>;
			mpadded: React.MathMLProps<React.MathMLMpaddedAttributes, MathMLElement>;
			mphantom: React.MathMLProps<React.MathMLMphantomAttributes, MathMLElement>;
			mprescripts: React.MathMLProps<React.MathMLMprescriptsAttributes, MathMLElement>;
			mroot: React.MathMLProps<React.MathMLMrootAttributes, MathMLElement>;
			mrow: React.MathMLProps<React.MathMLMrowAttributes, MathMLElement>;
			ms: React.MathMLProps<React.MathMLMsAttributes, MathMLElement>;
			mspace: React.MathMLProps<React.MathMLMspaceAttributes, MathMLElement>;
			msqrt: React.MathMLProps<React.MathMLMsqrtAttributes, MathMLElement>;
			mstyle: React.MathMLProps<React.MathMLMstyleAttributes, MathMLElement>;
			msub: React.MathMLProps<React.MathMLMsubAttributes, MathMLElement>;
			msubsup: React.MathMLProps<React.MathMLMsubsupAttributes, MathMLElement>;
			msup: React.MathMLProps<React.MathMLMsupAttributes, MathMLElement>;
			mtable: React.MathMLProps<React.MathMLMtableAttributes, MathMLElement>;
			mtd: React.MathMLProps<React.MathMLMtdAttributes, MathMLElement>;
			mtext: React.MathMLProps<React.MathMLMtextAttributes, MathMLElement>;
			mtr: React.MathMLProps<React.MathMLMtrAttributes, MathMLElement>;
			munder: React.MathMLProps<React.MathMLMunderAttributes, MathMLElement>;
			munderover: React.MathMLProps<React.MathMLMunderoverAttributes, MathMLElement>;
			semantics: React.MathMLProps<React.MathMLSemanticsAttributes, MathMLElement>;
			// MathML semantic annotations
			annotation: React.MathMLProps<React.MathMLAnnotationAttributes, MathMLElement>;
			"annotation-xml": React.MathMLProps<React.MathMLAnnotationXmlAttributes, MathMLElement>;
		}
	}

	interface MathMLElement extends Element { }

	namespace React {
		type MathMLProps<E extends MathMLAttributes<T>, T> = React.ClassAttributes<T> & E;

		// https://developer.mozilla.org/en-US/docs/Web/MathML/Global_attributes
		interface MathMLAttributes<T> extends React.DOMAttributes<T> {
			className?: React.HTMLAttributes<T>["className"] | undefined;
			dir?: "ltr" | "rtl" | undefined;
			displaystyle?: boolean | undefined;
			href?: string | undefined;
			id?: string | undefined;
			mathbackground?: string | undefined;
			mathcolor?: string | undefined;
			mathsize?: string | undefined;
			nonce?: string | undefined;
			scriptlevel?: string | undefined;
			style?: CSSProperties | undefined;
			tabindex?: number | undefined;
		}

		// Individual MathML elements are described here:
		//
		// https://developer.mozilla.org/en-US/docs/Web/MathML/Element/math
		//
		interface MathMLMathAttributes extends MathMLAttributes<MathMLElement> {
			display?: "block" | "inline" | undefined;
		}
		interface MathMLMerrorAttributes extends MathMLAttributes<MathMLMerrorAttributes> { }
		interface MathMLMfracAttributes extends MathMLAttributes<MathMLElement> {
			linethickness?: string | undefined;
		}
		interface MathMLMiAttributes extends MathMLAttributes<MathMLElement> {
			mathvariant?: "normal" | undefined;
		}
		interface MathMLMmultiscriptsAttributes extends MathMLAttributes<MathMLElement> { }
		interface MathMLMnAttributes extends MathMLAttributes<MathMLElement> { }
		interface MathMLMoAttributes extends MathMLAttributes<MathMLElement> {
			/* This attribute is non-standard. */
			accent?: boolean | undefined;
			fence?: boolean | undefined;
			form?: "prefix" | "infix" | "postfix" | undefined;
			largeop?: boolean | undefined;
			lspace?: string | undefined;
			maxsize?: string | undefined;
			minsize?: string | undefined;
			movablelimits?: boolean | undefined;
			rspace?: string | undefined;
			separator?: boolean | undefined;
			stretchy?: Booleanish | undefined;
			symmetric?: boolean | undefined;
		}
		interface MathMLMoverAttributes extends MathMLAttributes<MathMLElement> {
			accent?: boolean | undefined;
		}
		interface MathMLMpaddedAttributes extends MathMLAttributes<MathMLElement> {
			depth?: string | undefined;
			height?: string | undefined;
			lspace?: string | undefined;
			voffset?: string | undefined;
			width?: string | undefined;
		}
		interface MathMLMphantomAttributes extends MathMLAttributes<MathMLElement> { }
		// Described in relation to <mmultiscripts /> here:
		//
		// https://developer.mozilla.org/en-US/docs/Web/MathML/Element/mmultiscripts#using_mprescripts
		//
		interface MathMLMprescriptsAttributes extends MathMLAttributes<MathMLElement> { }
		interface MathMLMrootAttributes extends MathMLAttributes<MathMLElement> { }
		interface MathMLMrowAttributes extends MathMLAttributes<MathMLElement> { }
		interface MathMLMsAttributes extends MathMLAttributes<MathMLElement> {
			lquote?: string | undefined;
			rquote?: string | undefined;
		}
		interface MathMLMspaceAttributes extends MathMLAttributes<MathMLElement> {
			depth?: string | undefined;
			height?: string | undefined;
			width?: string | undefined;
		}
		interface MathMLMsqrtAttributes extends MathMLAttributes<MathMLElement> { }
		interface MathMLMstyleAttributes extends MathMLAttributes<MathMLElement> { }
		interface MathMLMsubAttributes extends MathMLAttributes<MathMLElement> { }
		interface MathMLMsubsupAttributes extends MathMLAttributes<MathMLElement> { }
		interface MathMLMsupAttributes extends MathMLAttributes<MathMLElement> { }
		interface MathMLMtableAttributes extends MathMLAttributes<MathMLElement> {
			/* This attribute is non-standard. */
			align?: string | undefined;
			/* This attribute is non-standard. */
			columnalign?: string | undefined;
			/* This attribute is non-standard. */
			columnlines?: string | undefined;
			/* This attribute is non-standard. */
			columnspacing?: string | undefined;
			/* This attribute is non-standard. */
			frame?: "none" | "solid" | "dashed" | undefined;
			/* This attribute is non-standard. */
			framespacing?: string | undefined;
			/* This attribute is non-standard. */
			rowalign?: string | undefined;
			/* This attribute is non-standard. */
			rowlines?: string | undefined;
			/* This attribute is non-standard. */
			rowspacing?: string | undefined;
			/* This attribute is non-standard. */
			width?: string | undefined;
		}
		interface MathMLMtdAttributes extends MathMLAttributes<MathMLElement> {
			/* This attribute is non-standard. */
			columnalign?: "left" | "center" | "right" | undefined;
			columnspan?: number | string | undefined;
			/* This attribute is non-standard. */
			rowalign?: "axis" | "baseline" | "bottom" | "center" | "top" | undefined;
			rowspan?: number | string | undefined;
		}
		interface MathMLMtextAttributes extends MathMLAttributes<MathMLElement> { }
		interface MathMLMtrAttributes extends MathMLAttributes<MathMLElement> {
			/* This attribute is non-standard. */
			columnalign?: "left" | "center" | "right" | undefined;
			/* This attribute is non-standard. */
			rowalign?: "axis" | "baseline" | "bottom" | "center" | "top" | undefined;
		}
		interface MathMLMunderAttributes extends MathMLAttributes<MathMLElement> {
			accentunder?: boolean | undefined;
		}
		interface MathMLMunderoverAttributes extends MathMLAttributes<MathMLElement> {
			accent?: boolean | undefined;
			accentunder?: boolean | undefined;
		}
		/**
		 * @see https://w3c.github.io/mathml-core/#semantics-and-presentation
		 */
		interface MathMLSemanticsAttributes extends MathMLAttributes<MathMLElement> { }
		/**
		 * @see https://w3c.github.io/mathml-core/#semantics-and-presentation
		 */
		interface MathMLAnnotationAttributes extends MathMLAttributes<MathMLElement> {
			encoding?: string | undefined;
		}
		/**
		 * @see https://w3c.github.io/mathml-core/#semantics-and-presentation
		 */
		interface MathMLAnnotationXmlAttributes extends MathMLAttributes<MathMLElement> {
			encoding?: string | undefined;
		}
	}
}
