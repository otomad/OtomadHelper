const TEXT_MARGIN = [10, 8] as const;

const StyledPreviewLanguage = styled.div`
	position: relative;
	display: flex;
	flex-direction: column;
	justify-content: flex-end;
	align-items: flex-start;
	height: 100%;
	border-radius: inherit;

	.text {
		${styles.effects.text.subtitle};
		margin: ${TEXT_MARGIN[1]}px ${TEXT_MARGIN[0]}px;

		.items-view-item.selected & {
			color: ${c("accent-color")};
		}
	}

	.items-view-item.grid:has(&) {
		.text-part .title {
			padding-inline-start: ${TEXT_MARGIN[0] + 1}px;
		}

		&.selected .text-part .title {
			color: ${c("accent-color")};
		}
	}

	progress {
		height: 8px;

		.items-view-item.selected & {
			&::-webkit-progress-value {
				border-start-start-radius: 0;
				border-end-start-radius: 0;
			}

			&[value="100"]::-webkit-progress-value {
				border-radius: none;
			}
		}

		.items-view-item:not(.selected) &::-webkit-progress-value {
			background-color: ${c("fill-color-text-secondary")};
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
`;

const useApprovalProgresses = createStore<{
	proofreading: Map<string, number>;
	initial(language: string): void;
	set(language: string, progress: number): void;
}>()((set, get) => ({
	proofreading: new Map<string, number>(),
	initial(language) {
		const { proofreading, set: update } = get();
		if (proofreading.has(language)) return;
		update(language, -1);
		fetch(`https://img.shields.io/badge/dynamic/json?color=green&label=${language}&style=flat&logo=crowdin&query=%24.progress[?(@.data.languageId==%27${language}%27)].data.approvalProgress&url=https%3A%2F%2Fbadges.awesome-crowdin.com%2Fstats-16002405-661336.json`)
			.then(response => response.xml())
			.then(xml => xml.querySelector("title")?.textContent?.match(/\d+(\.\d+)?/)?.[0])
			.then(progress => update(language, progress != null ? +progress : 100));
	},
	set(language, progress) {
		set(prev => ({ proofreading: new Map(prev.proofreading).set(language, progress) }));
	},
}));

export default function PreviewLanguage({ language }: FCP<{
	/** The ISO language code. */
	language: string;
	children?: never;
}>) {
	const languageName = t({ lng: language }).metadata.name;
	const { initial: initialProofreading, proofreading } = useApprovalProgresses();
	initialProofreading(language);
	const progress = proofreading.get(language) ?? -1;
	const showProgressPercentage = progress >= 0 && progress < 100;

	return (
		<StyledPreviewLanguage lang={language}>
			<div className="text">{languageName}</div>
			<progress value={progress} max={100} />
			{showProgressPercentage && (
				<div className="approval-progress">
					<Icon name="logo/crowdin" />
					<span>{progress}%</span>
				</div>
			)}
		</StyledPreviewLanguage>
	);
}
