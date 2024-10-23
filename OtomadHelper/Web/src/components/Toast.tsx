const HEIGHT = 50;

const StyledToast = styled.div`
	position: fixed;
	inset-block-end: min(100px, 10dvh);
	inset-inline-start: min(100px, 20dvw);
	z-index: 90;
	block-size: ${HEIGHT}px;
	overflow: clip;
	background-color: ${c("background-fill-color-card-background-secondary")};
	border: 1px solid ${c("stroke-color-card-stroke-default")};
	border-radius: 4px;
	border-inline-start-width: 0;
	box-shadow: 0 8px 16px ${c("shadows-flyout")};
	backdrop-filter: blur(10px);
	animation:
		${keyframes`
			from {
				scale: 0;
			}
		`} 250ms ${eases.easeOutBackSmooth},
		${keyframes`
			from {
				border-radius: ${HEIGHT / 2}px;
			}
		`} 750ms ${eases.easeInOutMaterialEmphasized},
		${keyframes`
			from {
				inline-size: ${HEIGHT}px;
			}
		`} 750ms ${eases.easeInOutMaterialEmphasized} 250ms backwards,
		${keyframes`
			from {
				translate: 50px;
			}
		`} 750ms ${eases.easeOutMax};

	.base {
		position: relative;
		display: flex;
		column-gap: 13px;
		align-items: center;
		padding: 13px 16px;
		block-size: 100%;
		border: inherit;
		border-width: 0;
		border-radius: 4px;
		border-inline-start-width: 1px;

		p {
			white-space: nowrap;
		}

		.letter-by-letter:not(.disabled) span {
			animation: ${keyframes`
				from {
					opacity: 0;
					translate: 25px;
				}
			`} 500ms ${eases.easeOutMax} calc(var(--i, 0) * (100ms / var(--length, 7) * 7)) backwards;
		}
	}

	.progress {
		${styles.mixins.oval()};
		position: absolute;
		inset-block-start: calc((100% - var(--progress) * 1%) / 2);
		inset-inline-start: 0;
		block-size: calc(var(--progress) * 1%);
		inline-size: 3px;
		background-color: ${c("accent-color")};
	}

	&.hidden {
		animation:
			${keyframes`
				to {
					scale: 0;
				}
			`} 150ms ${eases.easeInMax} 250ms forwards,
			${keyframes`
				to {
					border-radius: ${HEIGHT / 2}px;
				}
			`} 650ms ${eases.easeInOutMaterialEmphasized} 150ms forwards,
			${keyframes`
				to {
					inline-size: ${HEIGHT}px;
				}
			`} 500ms ${eases.easeInOutMaterialEmphasized} forwards,
			${keyframes`
				to {
					translate: -50px;
				}
			`} 650ms ${eases.easeInMaterialStandard} forwards;

		.letter-by-letter span {
			translate: calc(var(--i) * -0.5ex);
			opacity: 0;
			filter: blur(2px);
		}

		.badge {
			animation: ${keyframes`
				to {
					scale: 2.5;
				}
			`} 150ms ${eases.easeInMax} 250ms forwards;
		}
	}
`;

export default function Toast() {
	const [text, setText] = useState("");
	const [shown, setShown] = useState(false);
	const [timestamp, setTimestamp] = useState(0);
	const [cooldown, setCooldown] = useState(0);
	const [disableLetterByLetter, setDisableLetterByLetter] = useState(false);
	const [isInitialed, setIsInitialed] = useState(false);
	const DURATION = 2000, UPDATE_COOLDOWN_INTERVAL = 10;

	useInterval(() => {
		if (!shown) return;
		setCooldown(cooldown => cooldown + UPDATE_COOLDOWN_INTERVAL / DURATION * 100);
		if (cooldown >= 100) {
			setShown(false);
			setDisableLetterByLetter(false);
		}
	}, UPDATE_COOLDOWN_INTERVAL);

	useListen("app:toast", text => {
		setIsInitialed(true);
		setDisableLetterByLetter(shown);
		setText(text);
		setCooldown(0);
		setShown(true);
		setTimestamp(Date.now());
	});

	if (!isInitialed) return;

	return (
		<Portal>
			<StyledToast className={{ hidden: !shown }} style={{ "--progress": cooldown }} onPointerMove={() => setCooldown(100)}>
				<div className="progress" />
				<div className="base">
					<Badge status="info" />
					<LetterByLetter key={timestamp} className={{ disabled: disableLetterByLetter }}>{text}</LetterByLetter>
				</div>
			</StyledToast>
		</Portal>
	);
}
