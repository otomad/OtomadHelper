export default function OptionalBold({ bold = true, style, ...htmlAttrs }: FCP<{
	/** Activate the bold? @default true */
	bold?: boolean;
}, "strong">) {
	return <strong style={{ ...style, fontWeight: bold ? "bold" : "normal" }} {...htmlAttrs} />;
}
