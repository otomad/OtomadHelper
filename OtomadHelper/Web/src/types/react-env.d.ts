import type _DndKitCore from "@dnd-kit/core";
import type CSSType from "csstype";
import type _Immer from "immer";
import type English from "locales/English";
import type _LottieWeb from "lottie-web";
import type React from "react";
import type _ReactTransitionGroup from "react-transition-group-fc";
import type * as ReactTransitionGroup from "react-transition-group-fc";
import type _StyledComponents from "styled-components";
import type { StoreApi, UseBoundStore } from "zustand";

declare module "react" {
	interface HTMLAttributes {
		disabled?: boolean;
	}
}

declare module "react/jsx-runtime" {
	import type React from "react";

	namespace JSX {
		interface IntrinsicElements {
			tt: IntrinsicElements["pre"];
			xmp: IntrinsicElements["pre"];
			selectedcontent: IntrinsicElements["section"];
			marquee: React.DetailedHTMLProps<React.HTMLAttributes<HTMLMarqueeElement>, HTMLMarqueeElement>;
		}
	}
}

declare module "styled-components" {
	export interface DefaultTheme extends ThemeType, WebMessageEvents.SystemConfig { }
}

declare module "valtio" {
	function useSnapshot<T extends object>(p: T): T;
}

declare module "i18next" {
	interface CustomTypeOptions {
		defaultNS: "javascript";
		resources: typeof English;
		allowObjectInHTMLChildren: false;
		strictKeyChecks: true;
	}
}

declare const genericElement: unique symbol;

declare global {
	/**
	 * React Hook style functional component type.
	 * @template TProps - Props of the component.
	 * @template TTagName - Inherit all Attrs from a native HTML element.
	 */
	export type FC<TProps = {}, TTagName extends string | null = null> = React.FC<FCP<TProps, TTagName>>;

	type GetTagFromElement<TElement> = {
		[Tag in keyof ElementTagNameMap]: ElementTagNameMap[Tag] extends TElement ? TElement extends ElementTagNameMap[Tag] ? Tag : never : never;
	}[keyof ElementTagNameMap];
	type GetAttributesFromTag<TTagName> = React.JSX.IntrinsicElements[TTagName];
	type GetAttributesFromElement<TElement> = GetAttributesFromTag<GetTagFromElement<TElement>>;

	/**
	 * Props type for React Hook style functional components.
	 * @template TProps - Props of the component.
	 * @template TTagName - Inherit all Attrs from a native HTML element.
	 */
	export type FCP<TProps = {}, TTagName extends string | Element | GenericElement | null = null> = Override<
		TTagName extends null ? PropsWithChildren :
		TTagName extends GenericElement ? GenericElementAttributes :
		TTagName extends string ? GetAttributesFromTag<TTagName> : GetAttributesFromElement<TTagName>, TProps>;

	/**
	 * Similar to HTMLElement, but the `ref` prop can accept anything.\
	 * Useful if you want to pass the type to multiple different elements.
	 * @version 19.0.0-React
	 * @example
	 * ```tsx
	 * export function MyComponent({ ...htmlAttrs }: FCP<{}, GenericElement>) {
	 *     return (
	 *         <div {...htmlAttrs}>
	 *             <button {...htmlAttrs} />
	 *         </div>
	 *     );
	 * }
	 * ```
	 */
	export type GenericElement = typeof genericElement;
	type GenericElementAttributes = React.HTMLAttributes<HTMLElement> & { ref?: MiscRef<any> };

	/**
	 * The type of setter function in React useState.
	 * @template T - Property type.
	 */
	export type SetState<T> = React.Dispatch<React.SetStateAction<T>> | ((value: T) => unknown);
	// ((value: T) => unknown) | ((value: (prevState: T) => unknown) => unknown);
	export type SetStateNarrow<T> = React.Dispatch<React.SetStateAction<T>>;

	/**
	 * The return value of useState, which contains a tuple representing the current value and the function that sets that value.
	 * Can be used to achieve bidirectional binding.
	 * @template T - Property type.
	 */
	export type StateProperty<T> = [get?: T, set?: SetState<T>];
	// | (T extends unknown[] ? never : T)
	/**
	 * The return value of useState, which contains a tuple representing the current value and the function that sets that value.
	 * Can be used to achieve bidirectional binding. But the property cannot be empty.
	 * @template T - Property type.
	 */
	export type StatePropertyNonNull<T> = [get: T, set: SetStateNarrow<T>];
	/**
	 * Add more functions to the StateProperty.
	 * @template T - State property type.
	 */
	export type StatePropertyPremium<T> = StateProperty<T> & {
		/**
		 * Subscribe the state property, if the value changed, the callback will be called.
		 */
		subscribe(callback: (value: unknown) => void): void;
		/**
		 * I don't know why `dnd-kit` and `valtio` aren't compatible with each other.
		 * So I convert the constructed state property to real React used state property.
		 */
		useState(): [get: T, set: SetStateNarrow<T>];
	};

	/**
	 * Get Props for the React component.
	 * @template TComponent - React functional component.
	 */
	export type PropsOf<TComponent> =
		TComponent extends keyof ElementTagNameMap ? FCP<{}, TComponent> :
		TComponent extends React.FC<infer P> ? P : never;

	/**
	 * Get React element instance for the React component.
	 * @template TComponent - React functional component.
	 */
	export type ReactElementOf<TComponent> = ReactElement<TComponent extends React.FC<infer TProps> ? TProps : unknown, TComponent>;

	/**
	 * Get React Element type for the React component.
	 * @template TComponent - React functional component.
	 */
	export type GetReactElementFromFC<TComponent> = ReactElement<PropsOf<TComponent>, TComponent>;

	/**
	 * Get React Element type for the HTML element.
	 * @template TTagName - HTML tag name.
	 */
	export type GetReactElementFromTag<TTagName extends keyof ElementTagNameMap> = FC<{}, TTagName>;

	/**
	 * Get the parameter types for Zustand store state.
	 * @template TStore - Zustand store object。
	 */
	export type ZustandState<TStore> = NonNull<TStore extends UseBoundStore<StoreApi<infer T>> ? T : never>;

	interface BaseEvent<T = Element> extends SyntheticEvent<T>, Event { }
	export type BaseEventHandler<T = Element> = EventHandler<BaseEvent<T>>;

	export type ElementTagNameMap = HTMLElementTagNameMap & Omit<SVGElementTagNameMap, "a"> & MathMLElementTagNameMap & HTMLElementDeprecatedTagNameMap;
	export type TagNameToElement<TTagName extends keyof ElementTagNameMap | Element> =
		TTagName extends keyof ElementTagNameMap ? ElementTagNameMap[TTagName] : TTagName;
	export type ForwardedRef<T> = T extends keyof ElementTagNameMap ? React.ForwardedRef<ElementTagNameMap[T]> : React.ForwardedRef<T>;
	export type AsTarget = keyof React.JSX.IntrinsicElements | React.ComponentType<PropsWithChildren>; // keyof JSX.IntrinsicElements ?≠ keyof ElementTagNameMap

	export type CSSTransitionProps = Partial<ReactTransitionGroup.CSSTransitionProps>;
	export type TransitionProps = Partial<ReactTransitionGroup.TransitionProps>;

	/** All available cursor type. */
	export type Cursor = ObtainLiterals<Exclude<CSSType.Property.Cursor, CSSType.Globals>>;

	export type { DropAnimationSideEffects } from "@dnd-kit/core";
	export type { PropertiesHyphen as CSSPropertiesHyphen, Property as CSSProperty } from "csstype";
	export type { Draft } from "immer";
	export type { AnimationItem } from "lottie-web";
	export type { AnimationEventHandler, CSSProperties, ChangeEventHandler, DependencyList, DragEventHandler, EventHandler, FocusEventHandler, FormEventHandler, KeyboardEventHandler, MouseEventHandler, PointerEventHandler, PropsWithChildren, ReactElement, ReactNode, Ref as MiscRef, RefObject, SyntheticEvent, UIEventHandler, WheelEventHandler, default as React } from "react";
	export type { CSSTransition as CssTransition, SwitchTransition, TransitionGroup } from "react-transition-group-fc"; // CSSTransition has the same name as a native class.
	export type { RuleSet, WebTarget } from "styled-components";
}
