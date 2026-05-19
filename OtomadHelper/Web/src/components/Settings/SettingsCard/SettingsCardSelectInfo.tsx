const StyledSettingsCardSelectInfo = styled.p`
	&:not(.invalid, .custom) {
		color: ${c("accent-color")} !important;
	}

	&.invalid {
		color: ${c("fill-color-system-critical")} !important;
	}

	.badge {
		--size: 12px;
		margin-inline-end: 5px;
	}

	&:not(.invalid, .custom) .badge {
		background-color: ${c("accent-color")};
	}
`;

export /* @internal */ default function SettingsCardSelectInfo({ valid = true, children, ...htmlAttrs }: FCP<{
	/** Specifies whether the selection is valid if it's boolean, or the number of selection is not 0 if it's number. @default true */
	valid?: boolean | number | BadgeRequiredArgs;
}, "p">) {
	const custom = Array.isArray(valid);
	return children && (
		<StyledSettingsCardSelectInfo {...htmlAttrs} className={["details", "select-info", { invalid: !valid, custom }]}>
			{custom ?
				<Badge status={valid[0]} transitionOnAppear={false}>{valid[1]}</Badge> :
				<Badge status={valid ? "success" : "error"} colorOverride={valid ? "asterisk" : "error"} transitionOnAppear={false} />}
			<Preserves>{children}</Preserves>
		</StyledSettingsCardSelectInfo>
	);
}
