/*
 * 在此处重置一切 CSS 默认的垃圾设置。
 */
export default css`
	@layer base {
		// 拜拜，丑陋的按钮样式。
		button {
			margin: 0;
			padding: 0;
			color: inherit;
			font-family: inherit;
			font-size: inherit;
			background: none;
			border: none;
			outline: none;
			cursor: default;
			appearance: none;
		}

		// 合计着图片和视频默认居然是行内元素，导致 block 下方的空隙就是你造成的是吧？
		img,
		video,
		picture {
			display: block;
			vertical-align: bottom;
		}

		// 标签的鼠标光标应该使用继承的样式。
		label {
			cursor: inherit;
		}

		[disabled],
		:disabled {
			cursor: not-allowed;
			pointer-events: none;
		}

		h1,
		h2,
		h3,
		h4,
		h5,
		h6,
		p {
			margin: 0;
		}

		a {
			color: ${c("accent-color")};
			text-decoration: none;
			cursor: pointer;

			&:hover {
				opacity: 0.8;
			}

			&:active {
				opacity: 0.5;
			}
		}

		svg,
		svg * {
			overflow: visible;
		}
	}
`;
