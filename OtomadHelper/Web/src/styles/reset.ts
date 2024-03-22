/*
 * 在此处重置一切 CSS 默认的垃圾设置。
 */
export default css`
	@layer base {
		// 拜拜，丑陋的原生样式。
		button,
		input {
			all: unset;
			display: inline-block;
		}

		figure {
			all: unset;
			display: block;
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

		// 禁用样式覆盖。
		[disabled],
		:disabled {
			cursor: not-allowed;
			pointer-events: none;
			user-select: none;
		}

		// 惰性样式覆盖。
		[inert] {
			&,
			* {
				*,
				::before,
				::after {
					cursor: not-allowed;
					pointer-events: none !important;
					user-select: none;
				}
			}
		}

		// 隐藏样式覆盖 ID 选择器。
		[hidden] {
			display: none !important;
		}

		// 去除标题和段落中不应出现的边距。
		h1,
		h2,
		h3,
		h4,
		h5,
		h6,
		p {
			margin: 0;
		}

		// 全局超链接样式。
		a {
			color: ${c("accent-color")};
			text-decoration: none;
			border-radius: 3px;
			cursor: pointer;

			&:hover {
				opacity: 0.8;
			}

			&:active {
				opacity: 0.5;
			}
		}

		// 允许 SVG 元素中的形状超出其边界。
		svg,
		svg * {
			overflow: visible;
		}

		// 阻止图片被拖拽。
		img {
			-webkit-user-select: none;
			-moz-user-select: none;
			-ms-user-select: none;
			user-select: none;
			-webkit-user-drag: none;
			// stylelint-disable-next-line property-no-unknown
			user-drag: none;
		}
	}

	@media (forced-colors: active) or (prefers-contrast: more) {
		*,
		::before,
		::after {
			backdrop-filter: none !important;
		}
	}
`;
