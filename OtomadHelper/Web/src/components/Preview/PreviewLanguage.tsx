import links from "helpers/links";

const TEXT_MARGIN = [10, 8] as const;

const StyledPreviewLanguage = styled.div`
	position: relative;
	display: flex;
	flex-direction: column;
	justify-content: end;
	align-items: start;
	height: 100%;
	border-radius: inherit;

	.text {
		${styles.effects.text.subtitle};
		margin: ${TEXT_MARGIN[1]}px ${TEXT_MARGIN[0]}px;
		text-align: start;

		.items-view-item.selected & {
			color: ${c("accent-color")};
		}
	}

	.items-view-item.grid:has(&) {
		.text-part .text {
			padding-inline-start: ${TEXT_MARGIN[0] + 1}px;
		}

		&.selected .text-part .title {
			color: ${c("accent-color")};
		}
	}

	progress {
		height: 8px;

		.items-view-item.selected & {
			${progressFinishedPart(css`
				border-start-start-radius: 0;
				border-end-start-radius: 0;
			`)}

			&[value="100"] {
				${progressFinishedPart(css`
					border-radius: 0;
				`)}
			}
		}

		.items-view-item:not(.selected) & {
			${progressFinishedPart(css`
				background-color: ${c("fill-color-text-secondary")};
			`)}
		}
	}

	.approval-progress {
		position: absolute;
		inset-block-start: ${TEXT_MARGIN[1]}px;
		inset-inline-end: ${TEXT_MARGIN[0]}px;
		display: flex;
		gap: 4px;
		align-items: center;

		.icon {
			font-size: 16px;
		}
	}

	.shading-icon {
		position: absolute;
		inset-block-start: -8px;
		inset-inline-start: -8px;
		z-index: -1;
		font-size: 64px;
		opacity: 0.3;
	}
`;

const approvalProgresses = atomWithStorageAndImmer("translationProgress", new Map<string, number>());
approvalProgresses.onMount = setProgress => {
	fetch(import.meta.env.DEV ? "/api/crowdin" : links.crowdin.badgeApi)
		.then(response => response.json())
		.then(data => {
			setProgress(draft => draft.set("en", 100));
			(data.progress as AnyObject[]).forEach(({ data }) => {
				const { id } = data.language;
				const progress = parseFloat(data.approvalProgress);
				setProgress(draft => draft.set(id, progress));
			});
		}).catch(noop);
};

export default function PreviewLanguage({ language, showProgress = true }: FCP<{
	/** The ISO language code. */
	language: string;
	/** Show translation approval progress? @default true */
	showProgress?: boolean;
	children?: never;
}>) {
	const allLanguages = useLanguageTags();
	const languageName = (allLanguages.includes(language) ?
		t({ lng: language }).metadata.name :
		getLocaleName(language, language)).toTitleCase();
	const [progresses] = useAtom(approvalProgresses);
	const progress = progresses.get(language) ?? -1;
	const showProgressPercentage = progress >= 0 && progress < 100;

	return (
		<StyledPreviewLanguage lang={language}>
			<Icon className="shading-icon" name="globe_40" />
			<div className="text">{languageName}</div>
			{showProgress && (
				<>
					<progress value={progress} max={100} aria-hidden />
					{showProgressPercentage && (
						<div className="approval-progress">
							<Icon name="logo/crowdin" />
							<span>{progress}%</span>
						</div>
					)}
				</>
			)}
		</StyledPreviewLanguage>
	);
}
