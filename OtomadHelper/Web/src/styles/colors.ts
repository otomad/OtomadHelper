/* eslint-disable @stylistic/quote-props */

const colors = {
	"background-color": ["#f3f3f3", "#202020", "Canvas"],
	"foreground-color": ["rgba(0, 0, 0, 0.9)", "rgb(255, 255, 255)", "CanvasText"], // fill-color-text-primary
	"accent-color": ["#005fb8", "#60cdff", "Highlight"],
	"colorization": ["#0078d4", "#0078d4", "transparent"],
	"fill-color-text-primary-solid": ["rgba(24, 24, 24)", "rgb(255, 255, 255)", "CanvasText"],
	"fill-color-text-secondary": ["rgba(0, 0, 0, 0.61)", "rgba(255, 255, 255, 0.79)", "CanvasText"],
	"fill-color-text-tertiary": ["rgba(0, 0, 0, 0.45)", "rgba(255, 255, 255, 0.54)", "CanvasText"],
	"fill-color-text-disabled": ["rgba(0, 0, 0, 0.36)", "rgba(255, 255, 255, 0.36)", "GrayText"],
	"fill-color-text-on-accent-primary": ["rgb(255, 255, 255)", "rgb(0, 0, 0)", "HighlightText"],
	"fill-color-text-on-accent-secondary": ["rgba(255, 255, 255, 0.7)", "rgba(0, 0, 0, 0.5)", "HighlightText"],
	"fill-color-text-on-accent-disabled": ["rgb(255, 255, 255)", "rgba(255, 255, 255, 0.53)", "HighlightText"],
	"fill-color-text-on-accent-selected-text": ["rgb(255, 255, 255)", "rgb(0, 0, 0)", "HighlightText"],
	"fill-color-control-transparent": ["rgba(255, 255, 255, 0)", "rgba(255, 255, 255, 0)"],
	"fill-color-control-default": ["rgba(255, 255, 255, 0.7)", "rgba(255, 255, 255, 0.06)", "ButtonFace"],
	"fill-color-control-secondary": ["rgba(249, 249, 249, 0.5)", "rgba(255, 255, 255, 0.08)", "ButtonFace"],
	"fill-color-control-tertiary": ["rgba(249, 249, 249, 0.3)", "rgba(255, 255, 255, 0.03)", "ButtonFace"],
	"fill-color-control-quarternary": ["rgba(243, 243, 243, 0.76)", "rgba(255, 255, 255, 0.06)", "ButtonFace"],
	"fill-color-control-input-active": ["rgb(255, 255, 255)", "rgba(30, 30, 30, 0.7)", "ButtonFace"],
	"fill-color-control-disabled": ["rgba(249, 249, 249, 0.3)", "rgba(255, 255, 255, 0.04)", "ButtonFace"],
	"fill-color-control-strong-default": ["rgba(0, 0, 0, 0.45)", "rgba(255, 255, 255, 0.54)", "InactiveBorder"],
	"fill-color-control-strong-disabled": ["rgba(0, 0, 0, 0.32)", "rgba(255, 255, 255, 0.25)", "ButtonFace"],
	"fill-color-subtle-transparent": ["rgba(255, 255, 255, 0)", "rgba(255, 255, 255, 0)"],
	"fill-color-subtle-secondary": ["rgba(0, 0, 0, 0.04)", "rgba(255, 255, 255, 0.06)", "ButtonFace"],
	"fill-color-subtle-tertiary": ["rgba(0, 0, 0, 0.02)", "rgba(255, 255, 255, 0.04)", "ButtonFace"],
	"fill-color-subtle-disabled": ["rgba(0, 0, 0, 0)", "rgba(255, 255, 255, 0)", "ButtonFace"],
	"fill-color-control-solid-default": ["rgb(255, 255, 255)", "rgb(69, 69, 69)", "ButtonFace"],
	"fill-color-control-solid-secondary": ["rgb(252, 252, 252)", "rgb(74, 74, 74)", "ButtonFace"],
	"fill-color-control-solid-tertiary": ["rgb(253, 253, 253)", "rgb(63, 63, 63)", "ButtonFace"],
	"fill-color-control-solid-disabled": ["rgb(253, 253, 253)", "rgb(65, 65, 65)", "ButtonFace"],
	"fill-color-control-alt-transparent": ["rgba(255, 255, 255, 0)", "rgba(255, 255, 255, 0)", "ButtonFace"],
	"fill-color-control-alt-secondary": ["rgba(0, 0, 0, 0.02)", "rgba(0, 0, 0, 0.1)", "ButtonFace"],
	"fill-color-control-alt-tertiary": ["rgba(0, 0, 0, 0.06)", "rgba(255, 255, 255, 0.04)", "ButtonFace"],
	"fill-color-control-alt-quarternary": ["rgba(0, 0, 0, 0.09)", "rgba(255, 255, 255, 0.07)", "ButtonFace"],
	"fill-color-control-alt-disabled": ["rgba(255, 255, 255, 0)", "rgba(255, 255, 255, 0)", "ButtonFace"],
	"fill-color-accent-disabled": ["rgba(0, 0, 0, 0.22)", "rgba(255, 255, 255, 0.16)", "GrayText"],
	"fill-color-system-critical": ["rgb(196, 43, 28)", "rgb(255, 153, 164)", "InfoText"],
	"fill-color-system-success": ["rgb(15, 123, 15)", "rgb(108, 203, 95)", "InfoText"],
	"fill-color-system-attention": ["rgb(0, 95, 183)", "rgb(96, 205, 255)", "InfoText"],
	"fill-color-system-caution": ["rgb(157, 93, 0)", "rgb(252, 225, 0)", "InfoText"],
	"fill-color-system-attention-background": ["rgba(246, 246, 246, 0.5)", "rgba(255, 255, 255, 0.03)", "InfoBackground"],
	"fill-color-system-success-background": ["rgb(223, 246, 221)", "rgb(57, 61, 27)", "InfoBackground"],
	"fill-color-system-caution-background": ["rgb(255, 244, 206)", "rgb(67, 53, 25)", "InfoBackground"],
	"fill-color-system-critical-background": ["rgb(253, 231, 233)", "rgb(68, 39, 38)", "InfoBackground"],
	"fill-color-system-neutral": ["rgba(0, 0, 0, 0.45)", "rgba(255, 255, 255, 0.54)", "ButtonFace"],
	"fill-color-system-neutral-background": ["rgba(0, 0, 0, 0.02)", "rgba(255, 255, 255, 0.03)", "ButtonText"],
	"fill-color-system-solid-neutral": ["rgb(138, 138, 138)", "rgb(157, 157, 157)", "ButtonFace"],
	"fill-color-system-solid-attention-background": ["rgb(247, 247, 247)", "rgb(46, 46, 46)", "InfoBackground"],
	"fill-color-system-solid-neutral-background": ["rgb(243, 243, 243)", "rgb(46, 46, 46)", "InfoBackground"],
	"fill-color-control-on-image-default": ["rgba(255, 255, 255, 0.79)", "rgba(28, 28, 28, 0.7)", "HighlightText"],
	"fill-color-control-on-image-secondary": ["rgb(243, 243, 243)", "rgb(26, 26, 26)", "HighlightText"],
	"fill-color-control-on-image-tertiary": ["rgb(235, 235, 235)", "rgb(19, 19, 19)", "HighlightText"],
	"fill-color-control-on-image-disabled": ["rgba(255, 255, 255, 0)", "rgba(30, 30, 30, 0)", "GrayText"],
	"stroke-color-control-stroke-default": ["rgba(0, 0, 0, 0.06)", "rgba(255, 255, 255, 0.07)", "ActiveBorder"],
	"stroke-color-control-stroke-secondary": ["rgba(0, 0, 0, 0.16)", "rgba(255, 255, 255, 0.09)", "ActiveBorder"],
	"stroke-color-control-stroke-secondary-on-default": ["rgba(0, 0, 0, 0.1)", "rgba(255, 255, 255, 0.02)", "ActiveBorder"],
	"stroke-color-control-stroke-tertiary": ["rgba(0, 0, 0, 0.13)", "rgba(255, 255, 255, 0.03)", "ActiveBorder"],
	"stroke-color-control-stroke-on-accent-default": ["rgba(255, 255, 255, 0.08)", "rgba(255, 255, 255, 0.08)", "ActiveBorder"],
	"stroke-color-control-stroke-on-accent-secondary": ["rgba(0, 0, 0, 0.4)", "rgba(0, 0, 0, 0.14)", "ActiveBorder"],
	"stroke-color-control-stroke-on-accent-tertiary": ["rgba(0, 0, 0, 0.22)", "rgba(0, 0, 0, 0.22)", "ActiveBorder"],
	"stroke-color-control-stroke-on-accent-disabled": ["rgba(0, 0, 0, 0.06)", "rgba(0, 0, 0, 0.2)", "ActiveBorder"],
	"stroke-color-control-stroke-for-strong-fill-when-on-image": ["rgba(255, 255, 255, 0.35)", "rgba(0, 0, 0, 0.42)", "ActiveBorder"],
	"stroke-color-control-strong-stroke-default": ["rgba(0, 0, 0, 0.61)", "rgba(255, 255, 255, 0.6)", "ActiveBorder"],
	"stroke-color-control-strong-stroke-disabled": ["rgba(0, 0, 0, 0.22)", "rgba(255, 255, 255, 0.16)", "ActiveBorder"],
	"stroke-color-card-stroke-default": ["rgba(0, 0, 0, 0.06)", "rgba(0, 0, 0, 0.1)", "ActiveBorder"],
	"stroke-color-card-stroke-default-solid": ["rgb(235, 235, 235)", "rgb(28, 28, 28)", "ActiveBorder"],
	"stroke-color-divider-stroke-default": ["rgba(0, 0, 0, 0.08)", "rgba(255, 255, 255, 0.08)", "ActiveBorder"],
	"stroke-color-surface-stroke-default": ["rgba(117, 117, 117, 0.4)", "rgba(117, 117, 117, 0.4)", "ActiveBorder"],
	"stroke-color-surface-stroke-flyout": ["rgba(0, 0, 0, 0.06)", "rgba(0, 0, 0, 0.2)", "ActiveBorder"],
	"stroke-color-focus-stroke-outer": ["rgba(0, 0, 0, 0.9)", "rgb(255, 255, 255)", "ActiveBorder"],
	"stroke-color-focus-stroke-inner": ["rgb(255, 255, 255)", "rgba(0, 0, 0, 0.7)", "ActiveBorder"],
	"background-fill-color-card-background-default": ["rgba(255, 255, 255, 0.7)", "rgba(255, 255, 255, 0.05)", "ButtonFace"],
	"background-fill-color-card-background-secondary": ["rgba(246, 246, 246, 0.5)", "rgba(255, 255, 255, 0.03)", "ButtonFace"],
	"background-fill-color-card-background-tertiary": ["rgb(255, 255, 255)", "rgba(255, 255, 255, 0.07)", "ButtonFace"],
	"background-fill-color-smoke-default": ["rgba(255, 255, 255, 0.5)", "rgba(0, 0, 0, 0.3)", "ButtonFace"],
	"background-fill-color-layer-default": ["rgba(255, 255, 255, 0.5)", "rgba(58, 58, 58, 0.3)", "ButtonFace"],
	"background-fill-color-layer-alt": ["rgb(255, 255, 255)", "rgba(255, 255, 255, 0.05)", "ButtonFace"],
	"background-fill-color-layer-alt-solid": ["rgb(255, 255, 255)", "rgb(44, 44, 44)", "ButtonFace"],
	"background-fill-color-layer-on-acrylic-default": ["rgba(255, 255, 255, 0.25)", "rgba(255, 255, 255, 0.04)", "ButtonFace"],
	"background-fill-color-layer-on-accent-acrylic-default": ["rgba(255, 255, 255, 0.25)", "rgba(255, 255, 255, 0.04)", "ButtonFace"],
	"background-fill-color-layer-on-mica-base-alt-default": ["rgba(255, 255, 255, 0.7)", "rgba(58, 58, 58, 0.45)", "ButtonFace"],
	"background-fill-color-layer-on-mica-base-alt-transparent": ["rgba(255, 255, 255, 0)", "rgba(0, 0, 0, 0)", "ButtonFace"],
	"background-fill-color-layer-on-mica-base-alt-secondary": ["rgba(0, 0, 0, 0.04)", "rgba(255, 255, 255, 0.06)", "ButtonFace"],
	"background-fill-color-layer-on-mica-base-alt-tertiary": ["rgb(249, 249, 249)", "rgb(44, 44, 44)", "ButtonFace"],
	"background-fill-color-acrylic-background-default": ["rgba(252, 252, 252, 0.85)", "rgba(44, 44, 44, 0.96)", "ButtonFace"],
	"background-fill-color-acrylic-background-base": ["rgba(243, 243, 243, 0.9)", "rgba(32, 32, 32, 0.96)", "ButtonFace"],
	"background-fill-color-mica-background-base": ["rgb(243, 243, 243)", "rgb(32, 32, 32)", "ButtonFace"],
	"background-fill-color-mica-background-base-alt": ["rgb(218, 218, 218)", "rgb(10, 10, 10)", "ButtonFace"],
	"background-fill-color-solid-background-base": ["rgb(243, 243, 243)", "rgb(32, 32, 32)", "ButtonFace"],
	"background-fill-color-solid-background-base-alt": ["rgb(218, 218, 218)", "rgb(10, 10, 10)", "ButtonFace"],
	"background-fill-color-solid-background-secondary": ["rgb(238, 238, 238)", "rgb(28, 28, 28)", "ButtonFace"],
	"background-fill-color-solid-background-tertiary": ["rgb(249, 249, 249)", "rgb(40, 40, 40)", "ButtonFace"],
	"background-fill-color-solid-background-quarternary": ["rgb(255, 255, 255)", "rgb(44, 44, 44)", "ButtonFace"],
	"background-fill-color-solid-background-quinary": ["rgb(253, 253, 253)", "rgb(51, 51, 51)", "ButtonFace"],
	"background-fill-color-solid-background-senary": ["rgb(255, 255, 255)", "rgb(55, 55, 55)", "ButtonFace"],
	"shadows-flyout": ["rgba(0, 0, 0, 0.14)", "rgba(0, 0, 0, 0.26)"],
	"pressed-text-opacity": ["0.6063", "0.786"],
	"disabled-text-opacity": ["0.3614", "0.3628"],
} satisfies Record<string, [string, string] | [string, string, SystemColors]>;

export type ColorNames = keyof typeof colors;
export default colors;
export function globalColors() {
	let css = "";
	for (let i = 0; i < 3; i++) {
		const selector = [':root[data-scheme="light"]', ":root", "@media (forced-colors: active) or (prefers-contrast: more) {\n:root, :root[data-scheme]"][i];
		css += selector + " {\n";
		for (const [key, values] of Object.entries(colors))
			if (values[i])
				css += `\t--${key}: ${values[i]};\n`;
		css += i === 2 ? "}\n}\n" : "}\n";
	}
	return css;
}

type SystemColors =
	"ActiveText" | "ButtonFace" | "ButtonText" | "Canvas" | "CanvasText" | "Field" | "FieldText" | "GrayText" | "Highlight" | "HighlightText" | "LinkText" | "VisitedText" | // Standard
	"AccentColor" | "AccentColorText" | "Mark" | "MarkText" | "ButtonBorder" | // Experimental
	"ActiveBorder" | "ActiveCaption" | "AppWorkspace" | "Background" | "ButtonHighlight" | "ButtonShadow" | "CaptionText" | "InactiveBorder" | "InactiveCaption" | "InactiveCaptionText" | "InfoBackground" | "InfoText" | "Menu" | "MenuText" | "Scrollbar" | "ThreeDDarkShadow" | "ThreeDFace" | "ThreeDHighlight" | "ThreeDLightShadow" | "ThreeDShadow" | "Window" | "WindowFrame" | "WindowText" | // Deprecated
	(string & {});
