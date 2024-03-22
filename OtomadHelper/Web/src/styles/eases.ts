const eases = {
	// #region 基本
	/**
	 * ### 线性
	 * 以相同速度开始至结束的过渡效果 `t`。
	 */
	linear: "cubic-bezier(0, 0, 1, 1)",
	/**
	 * ### 助跑线性
	 * 与线性效果几乎相同 `1t`。
	 */
	linearApproach: "cubic-bezier(0.25, 0.25, 0.75, 0.75)",
	/**
	 * ### 缓动
	 * 以慢速开始，然后变快，然后慢速结束的过渡效果。
	 */
	ease: "cubic-bezier(0.25, 0.1, 0.25, 1)",
	// #endregion

	// #region 缓入
	/**
	 * ### 缓入
	 * 以慢速开始的过渡效果。
	 */
	easeIn: "cubic-bezier(0.42, 0, 1, 1)",
	/**
	 * ### 二次缓入
	 * 二次方的缓动 `t²`。
	 */
	easeInQuad: "cubic-bezier(0.55, 0.085, 0.68, 0.53)",
	/**
	 * ### 三次缓入
	 * 三次方的缓动 `t³`。
	 */
	easeInCubic: "cubic-bezier(0.55, 0.055, 0.675, 0.19)",
	/**
	 * ### 四次缓入
	 * 四次方的缓动 `t⁴`。
	 */
	easeInQuart: "cubic-bezier(0.895, 0.03, 0.685, 0.22)",
	/**
	 * ### 五次缓入
	 * 五次方的缓动 `t⁵`。
	 */
	easeInQuint: "cubic-bezier(0.755, 0.05, 0.855, 0.06)",
	/**
	 * ### 正弦缓入
	 * 正弦曲线的缓动 `sin(t)`。
	 */
	easeInSine: "cubic-bezier(0.47, 0, 0.745, 0.715)",
	/**
	 * ### 指数缓入
	 * 指数曲线的缓动 `2ᵗ`。
	 */
	easeInExpo: "cubic-bezier(0.95, 0.05, 0.795, 0.035)",
	/**
	 * ### 圆形缓入
	 * 圆形曲线的缓动 `√(1-t²)`。
	 */
	easeInCirc: "cubic-bezier(0.6, 0.04, 0.98, 0.335)",
	/**
	 * ### 急促回弹缓入
	 * 超过范围的三次方缓动 `(s+1)t³-st²`。
	 */
	easeInBack: "cubic-bezier(0.6, -0.28, 0.735, 0.045)",
	/**
	 * ### 平稳回弹缓入
	 * 超过范围后平稳结束的缓动 `(s+1)(t-1)³+s(t-1)²+1`。
	 */
	easeInBackSmooth: "cubic-bezier(0.36, 0, 0.66, -0.56)",
	/**
	 * ### 最大缓入
	 * 将锚点拉到头的缓动 `3t^⅔-2t`。
	 */
	easeInMax: "cubic-bezier(1, 0, 1, 1)",
	/**
	 * ### 平滑缓入
	 * 由设计师艾拉精心调整的平滑缓动参数。
	 */
	easeInSmooth: "cubic-bezier(0.6, 0, 1, 0.8)",
	/**
	 * ### 质感设计强调缓入
	 * 谷歌 Material Design 3 强调缓动，它捕捉了 Material Design 3 的表现风格。
	 */
	easeInMaterialEmphasized: "cubic-bezier(0.3, 0, 0.8, 0.15)",
	/**
	 * ### 质感设计标准缓入
	 * 谷歌 Material Design 3 标准缓动，用于简单、小型或以实用性为中心的过渡。
	 */
	easeInMaterialStandard: "cubic-bezier(0.3, 0, 1, 1)",
	/**
	 * ### 反弹缓入
	 * 如撞击地面时反弹般的缓动。
	 */
	easeInBounce: "linear(0, 0.012, 0.016, 0.012, 0, 0.027, 0.047, 0.059, 0.063, 0.059, 0.047, 0.027, 0 27.3%, 0.109 31.8%, 0.152, 0.188, 0.215, 0.234, 0.246, 0.25, 0.246, 0.234, 0.215, 0.188, 0.152, 0.109 59.1%, 0, 0.234, 0.438, 0.609, 0.75, 0.859 86.4%, 0.902, 0.938, 0.965, 0.984, 0.996, 1)",
	/**
	 * ### 弹跳缓入
	 * 如阻尼谐波运动般的缓动 `2⁻¹⁰ᵗsin[120°(10t-0.75)]+1`。
	 */
	easeInElastic: "linear(0, 0.002 13.3%, -0.006 27.8%, -0.001 31.9%, 0.015 39.3%, 0.016 42.5%, 0.012, 0.004 46.7%, -0.042 54.3%, -0.046 55.9% 57.3%, -0.036, -0.012 61.7%, 0.019 63.6%, 0.093 67.6%, 0.118 69.3%, 0.13, 0.131 72.2%, 0.123, 0.109 74.1%, 0.059 76%, -0.032 78.2%, -0.315 83.8%, -0.364 85.5%, -0.373, -0.37 87.1%, -0.346, -0.296 89.3%, -0.114 91.5%, 0.138 93.5%, 0.782 97.9%, 1)",
	// #endregion

	// #region 缓出
	/**
	 * ### 缓出
	 * 以慢速开始的过渡效果。
	 */
	easeOut: "cubic-bezier(0, 0, 0.58, 1)",
	/**
	 * ### 二次缓出
	 * 二次方的缓动 `t²`。
	 */
	easeOutQuad: "cubic-bezier(0.25, 0.46, 0.45, 0.94)",
	/**
	 * ### 三次缓出
	 * 三次方的缓动 `t³`。
	 */
	easeOutCubic: "cubic-bezier(0.215, 0.61, 0.355, 1)",
	/**
	 * ### 四次缓出
	 * 四次方的缓动 `t⁴`。
	 */
	easeOutQuart: "cubic-bezier(0.165, 0.84, 0.44, 1)",
	/**
	 * ### 五次缓出
	 * 五次方的缓动 `t⁵`。
	 */
	easeOutQuint: "cubic-bezier(0.23, 1, 0.32, 1)",
	/**
	 * ### 正弦缓出
	 * 正弦曲线的缓动 `sin(t)`。
	 */
	easeOutSine: "cubic-bezier(0.39, 0.575, 0.565, 1)",
	/**
	 * ### 指数缓出
	 * 指数曲线的缓动 `2ᵗ`。
	 */
	easeOutExpo: "cubic-bezier(0.19, 1, 0.22, 1)",
	/**
	 * ### 圆形缓出
	 * 圆形曲线的缓动 `√(1-t²)`。
	 */
	easeOutCirc: "cubic-bezier(0.075, 0.82, 0.165, 1)",
	/**
	 * ### 急促回弹缓出
	 * 超过范围的三次方缓动 `(s+1)t³-st²`。
	 */
	easeOutBack: "cubic-bezier(0.175, 0.885, 0.32, 1.275)",
	/**
	 * ### 平稳回弹缓出
	 * 超过范围后平稳结束的缓动 `(s+1)(t-1)³+s(t-1)²+1`。
	 */
	easeOutBackSmooth: "cubic-bezier(0.34, 1.56, 0.64, 1)",
	/**
	 * ### 最大缓出
	 * 将锚点拉到头的缓动 `3t^⅔-2t`。
	 */
	easeOutMax: "cubic-bezier(0, 0, 0, 1)",
	/**
	 * ### 平滑缓出
	 * 由设计师艾拉精心调整的平滑缓动参数。
	 */
	easeOutSmooth: "cubic-bezier(0.1, 0.9, 0.2, 1)",
	/**
	 * ### 流动设计强调缓出
	 * 微软 Windows 11 Fluent 2 中用于强调的缓动，如任务栏图标跳动等。
	 */
	easeOutFluentBack: "cubic-bezier(0.13, 1.62, 0, 0.92)",
	/**
	 * ### 质感设计强调缓出
	 * 谷歌 Material Design 3 强调缓动，它捕捉了 Material Design 3 的表现风格。
	 */
	easeOutMaterialEmphasized: "cubic-bezier(0.05, 0.7, 0.1, 1)",
	/**
	 * ### 质感设计标准缓出
	 * 谷歌 Material Design 3 标准缓动，用于简单、小型或以实用性为中心的过渡。
	 */
	easeOutMaterialStandard: "cubic-bezier(0, 0, 0, 1)",
	/**
	 * ### 弹簧缓出
	 * 如弹簧般的缓动。
	 */
	easeOutSpring: "linear(0, 0.009, 0.035 2.1%, 0.141, 0.281 6.7%, 0.723 12.9%, 0.938 16.7%, 1.017, 1.077, 1.121, 1.149 24.3%, 1.159, 1.163, 1.161, 1.154 29.9%, 1.129 32.8%, 1.051 39.6%, 1.017 43.1%, 0.991, 0.977 51%, 0.974 53.8%, 0.975 57.1%, 0.997 69.8%, 1.003 76.9%, 1.004 83.8%, 1)",
	/**
	 * ### 反弹缓出
	 * 如撞击地面时反弹般的缓动。
	 */
	easeOutBounce: "linear(0, 0.004, 0.016, 0.035, 0.063, 0.098, 0.141 13.6%, 0.25, 0.391, 0.563, 0.765, 1, 0.891 40.9%, 0.848, 0.813, 0.785, 0.766, 0.754, 0.75, 0.754, 0.766, 0.785, 0.813, 0.848, 0.891 68.2%, 1 72.7%, 0.973, 0.953, 0.941, 0.938, 0.941, 0.953, 0.973, 1, 0.988, 0.984, 0.988, 1)",
	/**
	 * ### 弹跳缓出
	 * 如阻尼谐波运动般的缓动 `2⁻¹⁰ᵗsin[120°(10t-0.75)]+1`。
	 */
	easeOutElastic: "linear(0, 0.218 2.1%, 0.862 6.5%, 1.114, 1.296 10.7%, 1.346, 1.37 12.9%, 1.373, 1.364 14.5%, 1.315 16.2%, 1.032 21.8%, 0.941 24%, 0.891 25.9%, 0.877, 0.869 27.8%, 0.87, 0.882 30.7%, 0.907 32.4%, 0.981 36.4%, 1.012 38.3%, 1.036, 1.046 42.7% 44.1%, 1.042 45.7%, 0.996 53.3%, 0.988, 0.984 57.5%, 0.985 60.7%, 1.001 68.1%, 1.006 72.2%, 0.998 86.7%, 1)",
	/**
	 * ### 反弹化的弹簧缓出
	 * 如弹簧般的缓动，但是反弹化。
	 */
	easeOutSpringBouncized: "linear(0, 0.008, 0.033 2%, 0.134, 0.272 6.5%, 0.76 13.5%, 0.896 15.9%, 1, 0.927, 0.876, 0.847 24.8%, 0.839, 0.837 27.2%, 0.842 29.2%, 0.859 31.5%, 0.961 40.7%, 1 45.3%, 0.988, 0.98, 0.975, 0.973 54.4%, 0.977 58.7%, 1 72.5%, 0.996 81.6%, 1)",
	/**
	 * ### 弹跳化的反弹缓出
	 * 如撞击地面时反弹般的缓动，但是弹跳化。
	 */
	easeOutBounceElasticized: "linear(0, 0.003, 0.013, 0.03, 0.053, 0.083, 0.12 12.6%, 0.213 16.8%, 0.356, 0.534, 0.749, 1 36.4%, 1.073, 1.134, 1.182, 1.217 47.9%, 1.241 51%, 1.247, 1.25, 1.249, 1.244 57.2%, 1.224 60.4%, 1.19, 1.141, 1.078, 1, 0.964, 0.943 79.1%, 0.938, 0.945, 0.966, 1 90.9%, 1.013 93.7%, 1.016, 1.014, 1.009, 1)",
	/**
	 * ### 反弹化的弹跳缓出
	 * 如阻尼谐波运动般的缓动 `2⁻¹⁰ᵗsin[120°(10t-0.75)]+1`，但是反弹化。
	 */
	easeOutElasticBouncized: "linear(0, 0.231 2.2%, 1 7.5%, 0.832, 0.717, 0.649 12%, 0.633, 0.627, 0.631, 0.644 14.9%, 0.695 16.5%, 1 22.5%, 0.941, 0.9, 0.876 27%, 0.87, 0.868 28.5%, 0.874 29.9%, 0.892 31.5%, 1 37.5%, 0.979, 0.965, 0.956, 0.953, 0.955 44.9%, 0.962 46.5%, 1 52.5%, 0.987, 0.984, 0.987 61.5%, 1, 0.994 73.5%, 1 82.5%, 0.998 88.5%, 1)",
	// #endregion

	// #region 缓入缓出
	/**
	 * ### 缓入缓出
	 * 以慢速开始的过渡效果。
	 */
	easeInOut: "cubic-bezier(0.42, 0, 0.58, 1)",
	/**
	 * ### 二次缓入缓出
	 * 二次方的缓动 `t²`。
	 */
	easeInOutQuad: "cubic-bezier(0.455, 0.03, 0.515, 0.955)",
	/**
	 * ### 三次缓入缓出
	 * 三次方的缓动 `t³`。
	 */
	easeInOutCubic: "cubic-bezier(0.645, 0.045, 0.355, 1)",
	/**
	 * ### 四次缓入缓出
	 * 四次方的缓动 `t⁴`。
	 */
	easeInOutQuart: "cubic-bezier(0.77, 0, 0.175, 1)",
	/**
	 * ### 五次缓入缓出
	 * 五次方的缓动 `t⁵`。
	 */
	easeInOutQuint: "cubic-bezier(0.86, 0, 0.07, 1)",
	/**
	 * ### 正弦缓入缓出
	 * 正弦曲线的缓动 `sin(t)`。
	 */
	easeInOutSine: "cubic-bezier(0.445, 0.05, 0.55, 0.95)",
	/**
	 * ### 指数缓入缓出
	 * 指数曲线的缓动 `2ᵗ`。
	 */
	easeInOutExpo: "cubic-bezier(1, 0, 0, 1)",
	/**
	 * ### 圆形缓入缓出
	 * 圆形曲线的缓动 `√(1-t²)`。
	 */
	easeInOutCirc: "cubic-bezier(0.785, 0.135, 0.15, 0.86)",
	/**
	 * ### 急促回弹缓入缓出
	 * 超过范围的三次方缓动 `(s+1)t³-st²`。
	 */
	easeInOutBack: "cubic-bezier(0.68, -0.55, 0.265, 1.55)",
	/**
	 * ### 平稳回弹缓入缓出
	 * 超过范围后平稳结束的缓动 `(s+1)(t-1)³+s(t-1)²+1`。
	 */
	easeInOutBackSmooth: "cubic-bezier(0.68, -0.6, 0.32, 1.6)",
	/**
	 * ### 最大缓入缓出
	 * 将锚点拉到头的缓动 `3t^⅔-2t`。
	 */
	easeInOutMax: "cubic-bezier(1, 0, 0, 1)",
	/**
	 * ### 平滑缓入缓出
	 * 由设计师艾拉精心调整的平滑缓动参数。
	 */
	easeInOutSmooth: "cubic-bezier(0.75, 0, 0, 1)",
	/**
	 * ### 流动设计点对点缓入缓出
	 * 微软 Windows 11 Fluent 2 中用于点对点的缓动，如窗口最大化、还原等。
	 */
	easeInOutFluent: "cubic-bezier(0.13, 1.62, 0, 0.92)",
	/**
	 * ### 质感设计强调缓入缓出
	 * 谷歌 Material Design 3 强调缓动，它捕捉了 Material Design 3 的表现风格。
	 */
	easeInOutMaterialEmphasized: "linear(0, 0.002, 0.01 3.6%, 0.034, 0.074 9.1%, 0.128 11.4%, 0.194 13.4%, 0.271 15%, 0.344 16.1%, 0.544, 0.66 20.6%, 0.717 22.4%, 0.765 24.6%, 0.808 27.3%, 0.845 30.4%, 0.883 35.1%, 0.916 40.6%, 0.942 47.2%, 0.963 55%, 0.979 64%, 0.991 74.4%, 0.998 86.4%, 1)",
	/**
	 * ### 质感设计标准缓入缓出
	 * 谷歌 Material Design 3 标准缓动，用于简单、小型或以实用性为中心的过渡。
	 */
	easeInOutMaterialStandard: "cubic-bezier(0.2, 0, 0, 1)",
	/**
	 * ### 反弹缓入缓出
	 * 如撞击地面时反弹般的缓动。
	 */
	easeInOutBounce: "linear(0, 0.006, 0.008, 0.006, 0 4.5%, 0.023 6.8%, 0.029, 0.031, 0.029, 0.023 11.4%, 0, 0.055, 0.094, 0.117 20.5%, 0.123, 0.125, 0.123, 0.117 25%, 0.094, 0.055, 0 31.8%, 0.137, 0.25, 0.342, 0.413 42.4%, 0.456, 0.485 46.8%, 0.497 48.7%, 0.505 51.9%, 0.527, 0.567, 0.625 59.1%, 0.695, 0.781 63.6%, 1 68.2%, 0.945, 0.906, 0.883 75%, 0.877, 0.875, 0.877, 0.883 79.5%, 0.906, 0.945, 1, 0.977 88.6%, 0.971, 0.969, 0.971, 0.977 93.2%, 1 95.5%, 0.994, 0.992, 0.994, 1)",
	/**
	 * ### 弹跳缓入缓出
	 * 如阻尼谐波运动般的缓动 `2⁻¹⁰ᵗsin[120°(10t-0.75)]+1`。
	 */
	easeInOutElastic: "linear(0, 0.001 8.5%, -0.005 19.2%, 0.002 22.4%, 0.023 27.8%, 0.024 30.2%, 0.014, -0.006 33.5%, -0.109 39.1%, -0.118, -0.115, -0.099, -0.07 43%, 0.03 44.8%, 0.93 54.7%, 1.045 56.4%, 1.082, 1.106 58.2%, 1.114, 1.118, 1.117, 1.112 60.7%, 1.092 62%, 1.005 66.5%, 0.986, 0.976 69.9%, 0.977 72.2%, 0.998 77.6%, 1.005 80.8%, 0.999 91.5%, 1)",
	// #endregion
};

export default eases;
