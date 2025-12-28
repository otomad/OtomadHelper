// DELETE: Delete it after the PR is merged: https://github.com/DefinitelyTyped/DefinitelyTyped/pull/71187/files

export { };

declare module "react/jsx-runtime" {
	namespace JSX {
		interface IntrinsicElements {
			// MathML
			/** @deprecated */
			maction: React.MathMLProps<React.MathMLMActionAttributes>;
			math: React.MathMLProps<React.MathMLMathAttributes>;
			merror: React.MathMLProps<React.MathMLMErrorAttributes>;
			mfrac: React.MathMLProps<React.MathMLMFracAttributes>;
			mi: React.MathMLProps<React.MathMLMIAttributes>;
			mmultiscripts: React.MathMLProps<React.MathMLMMultiscriptsAttributes>;
			mn: React.MathMLProps<React.MathMLMNAttributes>;
			mo: React.MathMLProps<React.MathMLMOAttributes>;
			mover: React.MathMLProps<React.MathMLMOverAttributes>;
			mpadded: React.MathMLProps<React.MathMLMPaddedAttributes>;
			mphantom: React.MathMLProps<React.MathMLMPhantomAttributes>;
			mprescripts: React.MathMLProps<React.MathMLMPrescriptsAttributes>;
			mroot: React.MathMLProps<React.MathMLMRootAttributes>;
			mrow: React.MathMLProps<React.MathMLMRowAttributes>;
			ms: React.MathMLProps<React.MathMLMSAttributes>;
			mspace: React.MathMLProps<React.MathMLMSpaceAttributes>;
			msqrt: React.MathMLProps<React.MathMLMSqrtAttributes>;
			mstyle: React.MathMLProps<React.MathMLMStyleAttributes>;
			msub: React.MathMLProps<React.MathMLMSubAttributes>;
			msubsup: React.MathMLProps<React.MathMLMSubsupAttributes>;
			msup: React.MathMLProps<React.MathMLMSupAttributes>;
			mtable: React.MathMLProps<React.MathMLMTableAttributes>;
			mtd: React.MathMLProps<React.MathMLMTDAttributes>;
			mtext: React.MathMLProps<React.MathMLMTextAttributes>;
			mtr: React.MathMLProps<React.MathMLMTRAttributes>;
			munder: React.MathMLProps<React.MathMLMUnderAttributes>;
			munderover: React.MathMLProps<React.MathMLMUnderOverAttributes>;
			semantics: React.MathMLProps<React.MathMLSemanticsAttributes>;
			// MathML semantic annotations
			annotation: React.MathMLProps<React.MathMLAnnotationAttributes>;
			"annotation-xml": React.MathMLProps<React.MathMLAnnotationXmlAttributes>;
		}
	}

	interface MathMLElement extends Element { }

	namespace React {
		type MathMLProps<E extends MathMLAttributes> = globalThis.React.ClassAttributes<MathMLElement> & E;

		// https://developer.mozilla.org/en-US/docs/Web/MathML/Global_attributes
		interface MathMLAttributes extends globalThis.React.DOMAttributes<MathMLElement>, globalThis.React.AriaAttributes {
			className?: globalThis.React.HTMLAttributes<HTMLElement>["className"] | undefined;
			dir?: "ltr" | "rtl" | undefined;
			displaystyle?: "false" | "true" | undefined;
			href?: string | undefined;
			id?: string | undefined;
			nonce?: string | undefined;
			scriptlevel?: string | undefined;
			style?: CSSProperties | undefined;
			tabIndex?: number | undefined;
			// This attributes are considered legacy but still described in the specification:
			//
			// https://www.w3.org/TR/mathml-core/#legacy-mathml-style-attributes
			//
			mathbackground?: string | undefined;
			mathcolor?: string | undefined;
			mathsize?: string | undefined;
		}

		// MathML elements and attributes are described here:
		//
		// https://www.w3.org/TR/mathml-core/#mathml-elements-and-attributes
		//
		interface MathMLMActionAttributes extends MathMLAttributes {
			actiontype?: string | undefined;
			selection?: string | undefined;
		}
		interface MathMLMathAttributes extends MathMLAttributes {
			display?: "block" | "inline" | undefined;
		}
		interface MathMLMErrorAttributes extends MathMLAttributes { }
		interface MathMLMFracAttributes extends MathMLAttributes {
			linethickness?: string | undefined;
		}
		interface MathMLMIAttributes extends MathMLAttributes {
			mathvariant?: "normal" | undefined;
		}
		interface MathMLMMultiscriptsAttributes extends MathMLAttributes { }
		interface MathMLMNAttributes extends MathMLAttributes { }
		interface MathMLMOAttributes extends MathMLAttributes {
			/** This attribute is non-standard. */
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
			stretchy?: "false" | "true" | undefined;
			symmetric?: boolean | undefined;
		}
		interface MathMLMOverAttributes extends MathMLAttributes {
			accent?: boolean | undefined;
		}
		interface MathMLMPaddedAttributes extends MathMLAttributes {
			depth?: string | undefined;
			height?: string | undefined;
			lspace?: string | undefined;
			voffset?: string | undefined;
			width?: string | undefined;
		}
		interface MathMLMPhantomAttributes extends MathMLAttributes { }
		// Described in relation to <mmultiscripts /> here:
		//
		// https://www.w3.org/TR/mathml-core/#prescripts-and-tensor-indices-mmultiscripts
		//
		interface MathMLMPrescriptsAttributes extends MathMLAttributes { }
		interface MathMLMRootAttributes extends MathMLAttributes { }
		interface MathMLMRowAttributes extends MathMLAttributes { }
		interface MathMLMSAttributes extends MathMLAttributes {
			lquote?: string | undefined;
			rquote?: string | undefined;
		}
		interface MathMLMSpaceAttributes extends MathMLAttributes {
			depth?: string | undefined;
			height?: string | undefined;
			width?: string | undefined;
		}
		interface MathMLMSqrtAttributes extends MathMLAttributes { }
		interface MathMLMStyleAttributes extends MathMLAttributes { }
		interface MathMLMSubAttributes extends MathMLAttributes { }
		interface MathMLMSubsupAttributes extends MathMLAttributes { }
		interface MathMLMSupAttributes extends MathMLAttributes { }
		interface MathMLMTableAttributes extends MathMLAttributes {
			/** This attribute is non-standard. */
			align?: string | undefined;
			/** This attribute is non-standard. */
			columnalign?: string | undefined;
			/** This attribute is non-standard. */
			columnlines?: string | undefined;
			/** This attribute is non-standard. */
			columnspacing?: string | undefined;
			/** This attribute is non-standard. */
			frame?: "none" | "solid" | "dashed" | undefined;
			/** This attribute is non-standard. */
			framespacing?: string | undefined;
			/** This attribute is non-standard. */
			rowalign?: string | undefined;
			/** This attribute is non-standard. */
			rowlines?: string | undefined;
			/** This attribute is non-standard. */
			rowspacing?: string | undefined;
			/** This attribute is non-standard. */
			width?: string | undefined;
		}
		interface MathMLMTDAttributes extends MathMLAttributes {
			/** This attribute is non-standard. */
			columnalign?: "left" | "center" | "right" | undefined;
			columnspan?: number | string | undefined;
			/** This attribute is non-standard. */
			rowalign?: "axis" | "baseline" | "bottom" | "center" | "top" | undefined;
			rowspan?: number | string | undefined;
		}
		interface MathMLMTextAttributes extends MathMLAttributes { }
		interface MathMLMTRAttributes extends MathMLAttributes {
			/** This attribute is non-standard. */
			columnalign?: "left" | "center" | "right" | undefined;
			/** This attribute is non-standard. */
			rowalign?: "axis" | "baseline" | "bottom" | "center" | "top" | undefined;
		}
		interface MathMLMUnderAttributes extends MathMLAttributes {
			accentunder?: boolean | undefined;
		}
		interface MathMLMUnderOverAttributes extends MathMLAttributes {
			accent?: boolean | undefined;
			accentunder?: boolean | undefined;
		}
		/**
		 * @see https://w3c.github.io/mathml-core/#semantics-and-presentation
		 */
		interface MathMLSemanticsAttributes extends MathMLAttributes { }
		/**
		 * @see https://w3c.github.io/mathml-core/#semantics-and-presentation
		 */
		interface MathMLAnnotationAttributes extends MathMLAttributes {
			encoding?: string | undefined;
		}
		/**
		 * @see https://w3c.github.io/mathml-core/#semantics-and-presentation
		 */
		interface MathMLAnnotationXmlAttributes extends MathMLAttributes {
			encoding?: string | undefined;
		}
	}
}
