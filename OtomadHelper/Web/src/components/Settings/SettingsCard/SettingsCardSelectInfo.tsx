const StyledSettingsCardSelectInfo = styled.p`
	&:not(.invalid, .custom),
	&.accent {
		color: ${c("accent-color")} !important;
	}

	&.invalid,
	&.asterisk {
		color: ${c("fill-color-system-critical")} !important;
	}

	.badge {
		--size: 12px;
		margin-inline-end: 5px;
	}

	&:not(.invalid, .custom) .badge {
		background-color: ${c("accent-color")};
	}

	&:has(.contents:empty) {
		display: none !important;
	}

	&.success {
		color: ${c("fill-color-system-success")} !important;
	}

	&.warning {
		color: ${c("fill-color-system-caution")} !important;
	}
`;

export type SelectValidPropType = boolean | number | Status | BadgeRequiredArgs;

export /* @internal */ default function SettingsCardSelectInfo({ valid: _valid = true, children, className, ...htmlAttrs }: FCP<{
	/** Specifies whether the selection is valid if it's boolean, or the number of selection is not 0 if it's number. @default true */
	valid?: SelectValidPropType;
}, "p">) {
	const valid = typeof _valid === "string" ? [_valid] as BadgeRequiredArgs : _valid;
	const custom = Array.isArray(valid);
	return isRenderable(children) && (
		<StyledSettingsCardSelectInfo {...htmlAttrs} className={["details", "select-info", { invalid: !valid, custom }, custom && valid[0], className]}>
			{custom ?
				<Badge status={valid[0]} transitionOnAppear={false}>{valid[1]}</Badge> :
				<Badge status={valid ? "success" : "error"} colorOverride={valid ? "asterisk" : "error"} transitionOnAppear={false} />}
			<Contents><Preserves>{children}</Preserves></Contents>
		</StyledSettingsCardSelectInfo>
	);
}
