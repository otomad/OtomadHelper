import type _DndKitCore from "@dnd-kit/core";
import type _Immer from "immer";
import type _LottieWeb from "lottie-web";
import type React from "react";
import type _ReactTransitionGroup from "react-transition-group";
import type * as ReactTransitionGroupCssTransition from "react-transition-group/CSSTransition";
import type * as ReactTransitionGroupTransition from "react-transition-group/Transition";
import type _StyledComponents from "styled-components";
import type { StoreApi, UseBoundStore } from "zustand";

declare module "react" {
	interface HTMLAttributes {
		disabled?: boolean;
		inert?: boolean;
	}
}

declare module "csstype" {
	interface Properties {
		/**
		 * **Custom properties** (sometimes referred to as **CSS variables** or
		 * **cascading variables**) are entities defined by CSS authors that contain
		 * specific values to be reused throughout a document. They are set using
		 * custom property notation (e.g., `--main-color: black;`) and are accessed
		 * using the `var()` function (e.g., `color: var(--main-color);`).
		 *
		 * @see https://developer.mozilla.org/docs/Web/CSS/Using_CSS_custom_properties
		 */
		[customProperty: `--${string}`]: string | number | undefined;
	}
}

declare module "valtio" {
	function useSnapshot<T extends object>(p: T): T;
}

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
	type GetAttributesFromTag<TTagName> =
		React.ReactDOM[TTagName] extends React.DetailedHTMLFactory<infer Attrs, any> ? Attrs :
		React.ReactDOM[TTagName] extends React.SVGFactory ? React.SVGAttributes<any> : never;
	type GetAttributesFromElement<TElement> = GetAttributesFromTag<GetTagFromElement<TElement>>;

	/**
	 * Props type for React Hook style functional components.
	 * @template TProps - Props of the component.
	 * @template TTagName - Inherit all Attrs from a native HTML element.
	 */
	export type FCP<TProps = {}, TTagName extends string | Element | null = null> = Override<
		TTagName extends null ? { children?: ReactNode } :
		TTagName extends string ? GetAttributesFromTag<TTagName> : GetAttributesFromElement<TTagName>, TProps>;

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
	export type PropsOf<TComponent> = TComponent extends React.FC<infer P> ? P : never;

	/**
	 * Get React Element type for the React component.
	 * @template TComponent - React functional component.
	 */
	export type GetReactElementFromFC<TComponent> = ReactElement<PropsOf<TComponent>, TComponent>;

	/**
	 * Get the parameter types for Zustand store state.
	 * @template TStore - Zustand store object。
	 */
	export type ZustandState<TStore> = NonNull<TStore extends UseBoundStore<StoreApi<infer T>> ? T : never>;

	interface BaseEvent<T = Element> extends React.SyntheticEvent<T>, Event { }
	export type BaseEventHandler<T = Element> = EventHandler<BaseEvent<T>>;

	export type ElementTagNameMap = HTMLElementTagNameMap & SVGElementTagNameMap & MathMLElementTagNameMap & HTMLElementDeprecatedTagNameMap;
	export type TagNameToElement<TTagName extends keyof ElementTagNameMap | Element> =
		TTagName extends keyof ElementTagNameMap ? ElementTagNameMap[TTagName] : TTagName;
	export type ForwardedRef<T> = T extends keyof ElementTagNameMap ? React.ForwardedRef<ElementTagNameMap[T]> : React.ForwardedRef<T>;

	export type CSSTransitionProps = Partial<ReactTransitionGroupCssTransition.CSSTransitionProps>;
	export type TransitionProps = Partial<ReactTransitionGroupTransition.TransitionProps>;

	export type { DropAnimationSideEffects } from "@dnd-kit/core";
	export type { Property as CSSProperty } from "csstype";
	export type { Draft } from "immer";
	export type { AnimationItem } from "lottie-web";
	export type { AnimationEventHandler, ChangeEventHandler, CSSProperties, DependencyList, DragEventHandler, EventHandler, FocusEventHandler, FormEventHandler, KeyboardEventHandler, MouseEventHandler, MutableRefObject, PointerEventHandler, default as React, ReactElement, ReactNode, RefObject, UIEventHandler, WheelEventHandler } from "react";
	export type { CssTransition, SwitchTransition, TransitionGroup } from "react-transition-group"; // CSSTransition has the same name as a native class.
	export type { WebTarget } from "styled-components";
}
